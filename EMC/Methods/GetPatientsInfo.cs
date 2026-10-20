using CacheTextMessage;
using EMC.CoreLib.Exceptions;
using EMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tele = Telegram.Singleton;
using UMKBCoreAPI.Models.Api.Responses;
using SMTelegram;

namespace EMC
{
    public static partial class Singleton
    {
        public static async Task<PatientsSearchResponse> GetPatientsInfo(IEnumerable<ulong> Ids, string locale)
        {
            if (Emc is null)
                await Initialize();

            PatientsSearchResponse response = new PatientsSearchResponse();
            try
            {
                var patients = await Emc.GetPatientsInfo(Ids.ToHashSet(), false);

                if (patients != null)
                {
                    foreach (var patient in patients)
                    {
                        response.patients.Add(new PatientSearch() { id = patient.Id.ToString(), name = patient.Name, email = patient.Email, birthdate = patient.Birthdate });
                    }
                }
            }
            catch (MongoDBException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"MongoDB для ЕМС недоступна.{Environment.NewLine}{ex.Message}");

                response.alert = new Alert()
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

                response.alert = new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(1462, locale),
                    Sticky = true

                };
            }
            catch (Exception ex)
            {
                response.alert = new Alert()
                {
                    Code = "500",
                    Level = "error",
                    Title = Cache.GetMessage(1329, locale),
                    Message = "GetPatient: " + ex.Message,
                    Sticky = true
                };
            }

            return response;
        }
    }
}
