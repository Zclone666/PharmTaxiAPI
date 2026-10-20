using CacheTextMessage;
using ECPLib.Common.Patient.Models;
using EMC.CoreLib.Exceptions;
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
        public async static Task<CheckLog?> GetProtocol(string id, string local)
        {
            if (Emc is null)
                await Initialize();

            try
            {
                var result = await Emc.GetProtocol(id);

                if (result != null) 
                {
                    result.alert = null;
                    return result; 
                }
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
                        Title = Cache.GetMessage(1462, local),
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
                        Title = Cache.GetMessage(1462, local),
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
                        Title = Cache.GetMessage(805, local),
                        Message = "EMC.GetProtocol: " + ex.Message,
                        Sticky = true
                    }
                };
            }

            return null;
        }
    }
}
