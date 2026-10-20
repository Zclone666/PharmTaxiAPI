using CacheTextMessage;
using ECPLib.Common.Patient.Models;
using EMC.CoreLib.Entities.Exceptions;
using EMC.CoreLib.Exceptions;
using static SMMeasurer.Singleton;
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
        public static async Task<CheckLog?> AddProtocol(CheckLog checkLog, string locale)
        {
            if (Emc is null)
                await Initialize();

            try
            {
                using var metricEMCAddProtocol = MeasurerProvider?.GetHistogram("ECP_EMC_AddProtocol");

                await Emc.AddProtocol(checkLog);

                checkLog.alert = null;
                return checkLog;
            }
            catch (MongoDBException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"MongoDB для ЕМС недоступна.{Environment.NewLine}{ex.Message}");

                return new CheckLog()
                {
                    alert = new Alert()
                    {
                        Code = "500",
                        Level = "error",
                        Title = Cache.GetMessage(1462, locale),
                        Sticky = true
                    }
                };
            }
            catch (TarantoolException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"Tarantool для ЕМС недоступен.{Environment.NewLine}{ex.Message}");

                return new CheckLog()
                {
                    alert = new Alert()
                    {
                        Code = "500",
                        Level = "error",
                        Title = Cache.GetMessage(1462, locale),
                        Sticky = true
                    }
                };
            }
            catch (BadPatinentException ex)
            {
                return new CheckLog()
                {
                    alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1330, locale),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new CheckLog()
                {
                    alert = new Alert()
                    {
                        Code = "500",
                        Level = "error",
                        Title = "ServerError",
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
        }
    }
}
