using CacheTextMessage;
using EMC.CoreLib.Entities.Exceptions;
using EMC.CoreLib.Exceptions;
using EMC.ECPIntegration.Models;
using EMC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tele = Telegram.Singleton;
using UMKBCoreAPI.Models.Api.Responses;
using static SMMeasurer.Singleton;
using SMTelegram;

namespace EMC
{
    public static partial class Singleton
    {
        public async static Task<MISResponse> GetMIS(MISData args)
        {
            if (Emc is null)
                await Initialize();

            using var metricGetMIS = MeasurerProvider?.GetHistogram("ECP_EMC_GetMIS");
            try
            {
                var data = await Emc.GetMIS(args.UserId, args.Patient.miscode, args.DoctorCode, args.ClinicCode, args.DepartmentCode, args.Lang);

                if (!string.IsNullOrWhiteSpace(data.ClinicCode) && !data.ClinicId.HasValue)
                {
                    return new MISResponse()
                    {
                        Alert = new Alert()
                        {
                            Code = "406",
                            Level = "error",
                            Title = Cache.GetMessage(819, args.Lang),
                            Message = Cache.GetMessage(1436, args.Lang),
                            Sticky = true
                        }
                    };
                }

                if (!string.IsNullOrWhiteSpace(data.DoctorCode) && !data.DoctorId.HasValue)
                {
                    return new MISResponse()
                    {
                        Alert = new Alert()
                        {
                            Code = "406",
                            Level = "error",
                            Title = Cache.GetMessage(819, args.Lang),
                            Message = Cache.GetMessage(1321, args.Lang),
                            Sticky = true
                        }
                    };
                }

                if (!string.IsNullOrWhiteSpace(data.DepartmentCode) && !data.DepartmentId.HasValue)
                {
                    return new MISResponse()
                    {
                        Alert = new Alert()
                        {
                            Code = "406",
                            Level = "error",
                            Title = Cache.GetMessage(819, args.Lang),
                            Message = Cache.GetMessage(1437, args.Lang),
                            Sticky = true
                        }
                    };
                }

                if (data.Patient is null)
                {
                    return new MISResponse()
                    {
                        Alert = new Alert()
                        {
                            Code = "406",
                            Level = "error",
                            Title = Cache.GetMessage(819, args.Lang),
                            Message = Cache.GetMessage(1328, args.Lang),
                            Sticky = true
                        }
                    };
                }

                return new MISResponse()
                {
                    UserId = args.UserId,
                    DoctorId = data.DoctorId,
                    DoctorCode = data.DoctorCode,
                    DoctorName = data.DoctorName,
                    ClinicId = data.ClinicId,
                    ClinicCode = data.ClinicCode,
                    ClinicName = data.ClinicName,
                    DepartmentId = data.DepartmentId,
                    DepartmentCode = data.DepartmentCode,
                    DepartmentName = data.DepartmentName,
                    Patient = data.Patient,
                    Lang = data.Lang,
                };
            }
            catch (MongoDBException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"MongoDB для ЕМС недоступна.{Environment.NewLine}{ex.Message}");

                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "500",
                        Level = "error",
                        Title = Cache.GetMessage(1462, args.Lang),
                        Sticky = true
                    }
                };
            }
            catch (TarantoolException ex)
            {
                await Tele.Notification?.SendText(Notification.Type.Service, Notification.Level.Error, $"Tarantool для ЕМС недоступен.{Environment.NewLine}{ex.Message}");

                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "500",
                        Level = "error",
                        Title = Cache.GetMessage(1462, args.Lang),
                        Sticky = true
                    }
                };
            }
            catch (BadPatinentException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1330, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadDoctorException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1441, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadClinicException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1442, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadDepartmentException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1443, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadUserIdException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1444, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (BadPatinentsEmailException ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "400",
                        Level = "error",
                        Title = Cache.GetMessage(1244, args.Lang),
                        Message = ex.Message,
                        Sticky = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new MISResponse()
                {
                    Alert = new Alert()
                    {
                        Code = "500",
                        Level = "error",
                        Title = Cache.GetMessage(805, args.Lang),
                        Message = "EMC.GetMIS: " + ex.Message,
                        Sticky = true
                    }
                };
            }
        }
    }
}
