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
        public static async Task<Alert?> CancellDrugs(ulong doctorId, string protocolId, IList<string>? oldDrugsKeys, string locale)
        {
            if (Emc is null)
                await Initialize();

            try
            {
                //тут отмена драгов
                await Emc.ReplaceDrugs(doctorId, protocolId, oldDrugsKeys, null, null);
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
                return new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(805, locale),
                    Message = "EMC.CancellProtocol: " + ex.Message,
                    Sticky = true
                };
            }

            return null;
        }
    }
}
