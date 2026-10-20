using EMC.CoreLib.Entities.Exceptions;
using EMC.CoreLib.Exceptions;
using EMC.ECPIntegration.Models;
using System;
using CacheTextMessage;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Tele = Telegram.Singleton;
using UMKBCoreAPI.Models.Api.Responses;
using static SMMeasurer.Singleton;
using EMC.Models;
using SMTelegram;

namespace EMC
{
    public static partial class Singleton
    {
        public async static Task<MISResponse> UpsertMIS(MISData args)
        {
            if (Emc is null)
                await Initialize();


#if DEBUG
            var a = System.Text.Json.JsonSerializer.Serialize(args, new System.Text.Json.JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
#endif

            using var metricUpsertMIS = MeasurerProvider?.GetHistogram("ECP_EMC_UpsertMIS");
            try
            {
                var data = await Emc.UpsertMIS(args);

#if DEBUG
                var a1 = System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
#endif
                return new MISResponse()
                {
                    UserId = args.UserId,
                    DoctorId = data.DoctorId,
                    DoctorCode = data.DoctorCode,
                    DoctorName = data.DoctorName,
                    ClinicId = data.ClinicId,
                    ClinicCode = data.ClinicCode,
                    ClinicName = data.ClinicName,
                    DepartmentId = data.DepartmentId,
                    DepartmentCode = data.DepartmentCode,
                    DepartmentName = data.DepartmentName,
                    Patient = data.Patient,
                    Lang = data.Lang,
                };
            }
            catch (MongoDBException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"MongoDB для ЕМС недоступна.{Environment.NewLine}{ex.Message}");

                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "500",
                        Level = "error",
                        Title = Cache.GetMessage(1462, args.Lang),
                        Sticky = true
                    }
                };
            }
            catch (TarantoolException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"Tarantool для ЕМС недоступен.{Environment.NewLine}{ex.Message}");

                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "500",
                        Level = "error",
                        Title = Cache.GetMessage(1462, args.Lang),
                        Sticky = true
                    }
                };
            }
            catch (BadPatinentException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1330, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadDoctorException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1441, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadClinicException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1442, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadDepartmentException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1443, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadUserIdException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1444, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadPatinentsEmailException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "406",
                        Level = "error",
                        Title = Cache.GetMessage(1244, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "500",
                        Level = "error",
                        Title = Cache.GetMessage(805, args.Lang),
                        Message = "EMC.UpsertMIS: " + ex.Message,
                        Sticky = true
                    }
                };
            }
        }
    }
}
