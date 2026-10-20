using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using PTConstants;
using System.Collections.Generic;
using System.Text;
using SMTelegram;
using Telegram;
using UMKBCoreAPI.Api.Keys;
using UMKBUtils;

namespace PharmTaxiAPI
{
    public class Startup
    {
        MainKeys keys = new MainKeys();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            PTConstants.EMC.UseMetrics = bool.Parse(configuration["EMC:UseMetrics"]);
            PTConstants.EMC.TarantoolConnectionString = configuration["EMC:TarantoolConnectionString"];
            PTConstants.EMC.MongoDbConnectionString = configuration["EMC:MongoDbConnectionString"];
            ECP.Key = configuration["ECP:Key"];
            ECP.Domain = configuration["ECP:Domain"];
            Mongo.MongoDB = configuration["Mongo:MongoDB"];
            MySQL.FPPriceConString = configuration["MySQL:FPPriceConString"];
            MySQL.UMKBConString = configuration["MySQL:UMKBConString"];
            Crypto.Password = configuration["Crypto:Password"];
            Crypto.Сryptographer = new BigIdCryptography(Crypto.Password);

#if !DEBUG
            Singleton.Initialize(configuration["Telegram:TokenBot"], configuration["Telegram:ChatPT"], configuration["Telegram:ChatSMDevOps"], configuration["Telegram:ChatSMUMKB"]);
#endif

            #region Ключи УМКБ
            //keys.Anj.SearchConcept = ;
            //keys.Anj.SearchConceptList = ;

            //keys.Billing.GetFunctionsApplication = ;

            //keys.Umkb.GetBelList = ;
            //keys.Umkb.GetBelWhere = ;
            //keys.Umkb.GetCode = ;
            //keys.Umkb.GetDate = ;
            //keys.Umkb.GetDescriptionsDB = ;
            //keys.Umkb.GetDouble = ;
            //keys.Umkb.GetIdByCode = ;
            //keys.Umkb.GetName = ;
            //keys.Umkb.GetNameDB = ;
            //keys.Umkb.GetNameDetailsDB = ;
            keys.Umkb.GetSemantic = configuration["UMKBKeys:GetSemantic"];
            //keys.Umkb.GetValue = ;
            //keys.Umkb.GetVarcharDB = ;
            //keys.Umkb.GetVarcharWhere = ;
            //keys.Umkb.Modeling = ;

            //keys.Registration.RegisterUserByUser = ECPConstants.Keys.RegistrUserByUser;
            //keys.User.GetUserInfoByEmail = ECPConstants.Keys.PatientInfoForEmail;
            //keys.Pars.MeaningObjects = ECPConstants.Keys.ParserUni;
            //keys.Ecp.GetFPDescriptions = ECPConstants.Keys.DescriptionFP;
            #endregion
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            List<Exception> errors = new List<Exception>();

            try
            {
                Controllers.ProcessesController.IsStarted = true;
                services.AddCors(options => { options.AddPolicy(Cors.AllowAll.Name, Cors.AllowAll.Policy); });
                services.AddCors(options => options.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));
                services.AddControllers();
                //services.AddControllers().AddNewtonsoftJson();
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                // Register the Swagger generator, defining 1 or more Swagger documents
                services.AddSwaggerGen();
                services.AddSwaggerGen(options => { options.CustomSchemaIds(type => type.ToString()); options.EnableAnnotations(); });
                var directory = Directory.GetCurrentDirectory();
                if (!string.IsNullOrEmpty(directory))
                {
                    var descriptions = Directory.EnumerateFiles(directory, "*.xml", SearchOption.AllDirectories).ToList();
                    if (descriptions != null) { foreach (var item in descriptions) { services.AddSwaggerGen(options => { options.IncludeXmlComments(item); }); } }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                //Установка defaut(через Web) реализации интерфейсов для запросов к UMKBCoreAPI
                if (UMKB.CoreApi is null) { UMKB.CoreApi = new UMKBCoreAPI.Api.UmkbApiWeb(keys, URLs.DomainCore); }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                CacheTextMessage.Cache.Initialize(new List<string>() { "ru" }, UMKB.CoreApi);
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                PharmTaxiAdm.Services.ExternalServices.DBInit();
                PharmTaxiAdm.Services.ExternalServices.DBForStupidFrontInit();
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                SMBarcode.Configuration.Setup(
                   MySQL.UMKBConString,
                   UMKB.TableGet,
                   UMKB.Graph,
                   UMKB.Concept,
                   new SMBarcode.Configuration.SetupBase(),
                   Crypto.Сryptographer,
                   UMKB.GraphList,
                   UMKB.ConceptList,
                   PTConstants.Constants.GS1_user,
                   PTConstants.Constants.GS1_pass,
                   UMKB.AbdParse,
                   MySQL.FPPriceConString,
                   UMKB.Modelling,
#if !DEBUG
                   false
#else
                   false
#endif
                );
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                SMBarcode.Configuration.UMKB_MYSQL_CONNECTION_STRING = MySQL.UMKBConString;
                SMBarcode.Configuration.getNames = UMKB.GetNamesAndFields;
                SMBarcode.Configuration.AbdParseKeyN = UMKB.AbdWr;
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                AUpload.Config.Setup(
                    MySQL.UMKBConString,
                    UMKB.AbdParse,
                    UMKB.TableGet,
                    UMKB.Graph,
                    UMKB.Concept,
                    Crypto.Сryptographer,
                    UMKB.GraphList,
                    UMKB.ConceptList,
                    null,
                    UMKB.Recognize
                );
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                AUpload.Config.MongoDb = Mongo.MongoDB;
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                EMC.Singleton.Initialize().Wait();

                EMCLib.EMC.EMC_instanse.InitBox(Mongo.MongoDB, UMKB.CoreApi);
                EMCLib.EMC.EMC_instanse.Intit();
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.Lics, Mongo.SKOLDBName);
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            try
            {
                if (PTConstants.EMC.UseMetrics) { SMMeasurer.Singleton.Initialize(); }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            if (errors.Count > 0)
            {
                StringBuilder text = new StringBuilder();

                foreach (var error in errors)
                {
                    text.Append(error + Environment.NewLine);
                }

                Message_StatusInit += text.ToString();

                Singleton.Notification?.SendTextAndFile(Notification.Type.Server, Notification.Level.Error, "Server started with errors", text.ToString(), "ServerStarErrors.txt").Wait();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("Content-Type", "application/json");
                await next();
            });

            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseAuthorization();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseSwagger();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Набор сообщений об успехе или неудаче процессов при старте сервера
        /// </summary>
        public static string Message_StatusInit { get; set; } = string.Empty;

        public enum Server
        {
            Development,
            Production
        }

        public static Server GetServer()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                return Server.Production;
            }
            return Server.Development;
        }
    }
}
