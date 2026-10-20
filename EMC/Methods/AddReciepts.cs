using CacheTextMessage;
using ECPLib.Common.Recipes.Models;
using EMC.CoreLib.Entities.Exceptions;
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
        public static async Task<Alert?> AddReciepts(string protocolId, string drugKey, ModelRecipeBase recipe, string locale)
        {
            if (Emc is null)
                await Initialize();

            try
            {
                await Emc.AddReciepts(protocolId, drugKey, new List<ModelRecipeBase>(1) { recipe });
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
            catch (NotFoundException ex)
            {
                return new Alert()
                {
                    Code = "400",
                    Level = "error",
                    Title = Cache.GetMessage(1331, locale),
                    Message = "InsertToProtocols: " + ex.Message,
                };
            }
            catch (Exception ex)
            {
                return new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(807, locale),
                    Message = "InsertToProtocols: " + ex.Message,
                    Sticky = true
                };
            }

            return null;
        }
    }
}
