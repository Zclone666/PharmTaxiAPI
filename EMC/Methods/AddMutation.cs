using System;
using System.Collections.Generic;
using System.Text;
using UMKBCoreAPI.Models.Api.Responses;
using EMC.ECPIntegration.Models.Mutations;
using System.Threading.Tasks;
using EMC.CoreLib.Entities.Exceptions;
using EMC.CoreLib.Exceptions;
using CacheTextMessage;
using ECPLib.Common.Patient.Models;
using SMTelegram;
using Tele = Telegram.Singleton;

namespace EMC
{
    public static partial class Singleton
    {
        public static async Task<Alert?> AddMutation(ulong patientId, Dictionary<string, Mutation> mutations, string locale, bool replace = false)
        {
            if (Emc is null)
                await Initialize();

            try
            {
                ModelMutations emcMutations = new ModelMutations() { Mutations = mutations };

                await Emc.AddMutations(patientId, emcMutations, replace);
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
            catch (BadPatinentException ex)
            {
                return new Alert()
                {
                    Code = "400",
                    Level = "error",
                    Title = Cache.GetMessage(1330, locale),
                    Message = ex.Message,
                    Sticky = true
                };
            }
            catch (Exception ex)
            {
                return new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = "ServerError",
                    Message = ex.Message,
                    Sticky = true
                };
            }
            return null;
        }
    }
}
