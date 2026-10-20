using CacheTextMessage;
using EMC.CoreLib.Entities.Exceptions;
using EMC.CoreLib.Exceptions;
using EMC.ECPIntegration.Models.Mutations;
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
        public static async Task<ResponseMutations?> GetMutation(ulong patientId, string locale)
        {
            if (Emc is null)
                await Initialize();

            try
            {
                var mutations = await Emc.GetMutations(patientId);

                if(mutations is null || mutations.Mutations is null || mutations.Mutations.Count == 0)
                {
                    return new ResponseMutations()
                    {
                        Alert = new Alert()
                        {
                            Code = "400",
                            Level = "error",
                            Message = Cache.GetMessage(1566, locale),
                            Sticky = true
                        }
                    };
                }

                return new ResponseMutations() { Mutations = mutations.Mutations };
            }
            catch (MongoDBException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"MongoDB для ЕМС недоступна.{Environment.NewLine}{ex.Message}");

                return new ResponseMutations()
                {
                    Alert = new Alert()
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

                return new ResponseMutations()
                {
                    Alert = new Alert()
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
                return new ResponseMutations()
                {
                    Alert = new Alert()
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
                return new ResponseMutations()
                {
                    Alert = new Alert()
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
