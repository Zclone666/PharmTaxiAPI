using CacheTextMessage;
using EMC.CoreLib.Exceptions;
using EMC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tele = Telegram.Singleton;
using UMKBCoreAPI.Models.Api.Responses;
using SMTelegram;

namespace EMC
{
    public static partial class Singleton
    {
        public static async Task<Alert?> CancellProtocol(string protocol, ulong autour, string locale)
        {
            if (Emc is null)
                await Initialize();

            try
            {
                await Emc.CancellProtocol(protocol, autour);
            }
            catch (MongoDBException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"MongoDB для ЕМС недоступна.{Environment.NewLine}{ex.Message}");

                return new Alert()
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

                return new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(1462, locale),
                    Sticky = true
                };
            }
            catch (Exception ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, ex);

                return new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(1329, locale),
                    Message = "EMC.GetPatient: " + ex.Message,
                    Sticky = true
                };
            }

            return null;
        }
    }
}
