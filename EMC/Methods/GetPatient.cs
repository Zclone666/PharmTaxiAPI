using EMC.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using static EMC.CoreLib.Data.EMCId;
using Tele = Telegram.Singleton;
using UMKBCoreAPI.Models.Api.Responses;
using CacheTextMessage;
using EMC.Models;
using SMTelegram;

namespace EMC
{
    public static partial class Singleton
    {
        public async static Task<ResultPatient> GetPatient(string id, string locale)
        {
            ulong.TryParse(id, out var _id);
            return await GetPatient(_id, locale);
        }

        public async static Task<ResultPatient> GetPatient(ulong id, string locale)
        {
            if (Emc is null)
                await Initialize();

            ResultPatient response = new ResultPatient();

            try
            {
                response.Patient = await Emc.GetPatient(id, false);

#if DEBUG
                string a = System.Text.Json.JsonSerializer.Serialize(response.Patient, new System.Text.Json.JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
#endif

                if (response.Patient is null)
                {
                    response.Alert = new Alert()
                    {
                        Code = "404",
                        Level = "error",
                        Title = "EMC.GetPatient: " + Cache.GetMessage(1328, locale)
                    };
                }
            }
            catch (MongoDBException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"MongoDB для ЕМС недоступна.{Environment.NewLine}{ex.Message}");

                response.Alert = new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(1462, locale),
                    Sticky = true

                };
            }
            catch (TarantoolException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"Tarantool для ЕМС недоступен.{Environment.NewLine}{ex.Message}");

                response.Alert = new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(1462, locale),
                    Sticky = true

                };
            }
            catch (BadIdException)
            {
                response.Alert = new Alert()
                {
                    Code = "400",
                    Level = "error",
                    Title = Cache.GetMessage(1329, locale)
                };
            }
            catch (Exception ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, ex);

                response.Alert = new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(1329, locale),
                    Message = "EMC.GetPatient: " + ex.Message,
                    Sticky = true
                };
            }

            return response;
        }

        public static async Task<ResultPatient> GetPatient(string code, ulong userId, string locale)
        {
            if (Emc is null)
                await Initialize();

            ResultPatient response = new ResultPatient();

            try
            {
                response.Patient = await Emc.GetPatient(code, userId);

#if DEBUG
                string a = System.Text.Json.JsonSerializer.Serialize(response.Patient, new System.Text.Json.JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
#endif

                if (response.Patient is null)
                {
                    response.Alert = new Alert()
                    {
                        Code = "404",
                        Level = "error",
                        Title = "EMC.GetPatient: " + Cache.GetMessage(1328, locale)
                    };
                }
            }
            catch (MongoDBException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"MongoDB для ЕМС недоступна.{Environment.NewLine}{ex.Message}");

                response.Alert = new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(1462, locale),
                    Sticky = true

                };
            }
            catch (TarantoolException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"Tarantool для ЕМС недоступен.{Environment.NewLine}{ex.Message}");

                response.Alert = new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(1462, locale),
                    Sticky = true

                };
            }
            catch (BadIdException)
            {
                response.Alert = new Alert()
                {
                    Code = "400",
                    Level = "error",
                    Title = Cache.GetMessage(1329, locale)
                };
            }
            catch (Exception ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, ex);

                response.Alert = new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(1329, locale),
                    Message = "EMC.GetPatient: " + ex.Message,
                    Sticky = true
                };
            }

            return response;
        }
    }
}
