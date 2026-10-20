using CacheTextMessage;
using ECPLib.Common.Patient.Models;
using EMC.CoreLib.Entities.Exceptions;
using EMC.CoreLib.Exceptions;
using EMC.Models;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Tele = Telegram.Singleton;
using UMKBCoreAPI.Models.Api.Responses;
using static SMMeasurer.Singleton;
using SMTelegram;

namespace EMC
{
    public static partial class Singleton
    {
        public static async Task<ResultPatient> UpsertPatient(UpsertPatient args)
        {
#if DEBUG
            string a = System.Text.Json.JsonSerializer.Serialize(args, new System.Text.Json.JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
#endif

            if (args.clinicId is null)
            {
                return await UpsertPatient(args.patient, args.userId, args.doctorId, args.doctorName, null, null, null, null, args.active_language);
            }
            else if (args.departmentId is null)
            {
                return await UpsertPatient(args.patient, args.userId, args.doctorId, args.doctorName, args.clinicId, args.clinicName, null, null, args.active_language);
            }
            else
            {
                return await UpsertPatient(args.patient, args.userId, args.doctorId, args.doctorName, args.clinicId, args.clinicName, args.departmentId, args.departmentName, args.active_language);
            }
        }

        private static async Task<ResultPatient> UpsertPatient(Patient patient, ulong userId, ulong doctorId, string doctorName, ulong? clinicId = null, string? clinicName = null, ulong? departmentId = null, string? departmentName = null, string locale = "ru")
        {
            if (Emc is null)
                await Initialize();

            try
            {
                using var metricEMCUpsertPatient = MeasurerProvider?.GetHistogram("ECP_EMC_UpsertPatient");
                var patientUpdate = await Emc.UpsertPatient(patient, userId, doctorId, doctorName, clinicId, clinicName, departmentId, departmentName, locale);

                return new ResultPatient() { Patient = patientUpdate };
            }
            catch (MongoDBException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"MongoDB для ЕМС недоступна.{Environment.NewLine}{ex.Message}");

                return new ResultPatient()
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

                return new ResultPatient()
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
                return new ResultPatient()
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
            catch (BadDoctorException ex)
            {
                return new ResultPatient()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1441, locale),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadClinicException ex)
            {
                return new ResultPatient()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1442, locale),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadDepartmentException ex)
            {
                return new ResultPatient()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1443, locale),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadUserIdException ex)
            {
                return new ResultPatient()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1444, locale),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadPatinentsEmailException ex)
            {
                return new ResultPatient()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1244, locale),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultPatient()
                {
                    Alert = new Alert()
                    {
                        Code = "500",
                        Level = "error",
                        Title = Cache.GetMessage(805, locale),
                        Message = "EMC.UpsertPatient: " + ex.Message,
                        Sticky = true
                    }
                };
            }
        }
    }
}
