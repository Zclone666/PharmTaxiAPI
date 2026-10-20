using Newtonsoft.Json;
using PharmTaxiAdm.Models.User;
using PharmTaxiAPI.Models;
using System;
using System.Threading.Tasks;
using PTConstants;

namespace PharmTaxiAPI.Methods
{
    public class UserFromG
    {
        public static CreateUserFromGuestResponse CreateUserFromGuest(UserFromGuest args)
        {

            try
            {
                //var authCheckReturn = Global.CheckAuth(args.authkey);
                //if (authCheckReturn.code is "400")
                //	return Content(JsonConvert.SerializeObject(authCheckReturn));
                AlertUserID uid = new AlertUserID();
                AuthKeyByLicKey auf = new AuthKeyByLicKey();
                args.key = UMKB.CreateUserFromGuest;
                args.application = "5";
                string OldUserID;
                AlertUserID AlForOldUID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(args.authkey, "5");
                OldUserID = AlForOldUID.userid.ToString();
                if (String.IsNullOrEmpty(args.email) || String.IsNullOrEmpty(args.firstName) || String.IsNullOrEmpty(args.lastName) || String.IsNullOrEmpty(args.authkey) || String.IsNullOrEmpty(args.password))
                    return new CreateUserFromGuestResponse()
                    {
                        alert = (new Alert()
                        {
                            code = "400",
                            level = "danger",
                            message = "Не указаны обязательные параметры",
                            title = "Ошибка",
                            sticky = false
                        }),
                        authkey = null,
                        user_id = 0
                    };
                //uid = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(args.authkey, "5");
                //if(uid.alert.code=="403") return Content(JsonConvert.SerializeObject(uid));
                CreateUserFromGuestResponse cufg = JsonConvert.DeserializeObject<CreateUserFromGuestResponse>(
                    UMKBRequests.WebUtils.Post(URLs.CreateUserFromGuestURL, JsonConvert.SerializeObject(args), "application/json").Answer);
                if (cufg.alert.code == "Forbidden") cufg.alert.code = "403";
                if (cufg.alert.message == "Пользователь с указанным адресом уже зарегистрирован.")
                {

                    string authX = PharmTaxiAdm.Methods.User.UserMethods.Authorize(args.email, args.password, "5");
                    AuthK resX = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthK>(authX);
                    cufg.authkey = resX.authkey;
                    cufg.alert = resX.alert;
                    cufg.user_id = 0;
                    //cufg.alert.message = "Регистрация успешно пройдена!";
                    if (resX.alert.code != "200") return cufg;
                    else
                    {
                        cufg.authkey = resX.authkey;
                        uid = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(cufg.authkey, "5");
                        cufg.user_id = uid.userid;
                        Task.Run(()=>SMReceipt.Global.Methods.Mongo.ChangeUserId(OldUserID, uid.userid.ToString()));
                        Task.Run(()=>SMReceipt.Global.Methods.Mongo.ChangeUserIdFakeOrder(OldUserID, uid.userid.ToString()));
                        if (uid.alert.code == "200")
                        {
                            cufg.alert.code = uid.alert.code;
                            //cufg.alert.level = uid.alert.level;
                            cufg.alert.message = resX.alert.title;
                            cufg.alert.title = uid.alert.title;
                        }
                        else
                        {
                            cufg.alert.code = uid.alert.code;
                            cufg.alert.level = uid.alert.level;
                            cufg.alert.message = uid.alert.message;
                            cufg.alert.title = uid.alert.title;
                        }
                        return cufg;
                    }
                }
                if (cufg.alert.message == "Неверный authkey.")
                {
                    return cufg;
                }
                if (cufg.alert.message == "Регистрация прошла успешно. На ваш адрес отправлена ссылка для подтверждения регистрации" || cufg.alert.message == "На указанный вами адрес, ранее была отправлена ссылка для подтверждения регистрации." || cufg.alert.message== "Адрес ожидает подтверждения. На указанный email, ранее была отправлена ссылка для подтверждения регистрации.")
                {

                    auf.email = args.email;
                    if (RegisterWorkaround.AutoConfirmEmailTF(auf))
                        cufg.alert.message = "Регистрация прошла успешно.";
                    else
                    {
                        bool TEST = false;
                        UserInfoReq req = new UserInfoReq() { email = auf.email, key = UMKB.GetUserInfoByEmail };
                        UserInfoResp resp = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoResp>(UMKBRequests.WebUtils.Post(URLs.GetUserInfoByEmail, Newtonsoft.Json.JsonConvert.SerializeObject(req), "application/json").Answer);
                        if (resp.userInfo is null)
                        {
                            var tr = PharmTaxiAdm.Methods.User.UserMethods.Authorize2(args.email, args.password, "5");
                            AuthKUserId atr = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthKUserId>(tr);
                            TEST = RegisterWorkaround.AutoConfirmEmailTF(atr.userid, auf.email);
                        }
                        else TEST = RegisterWorkaround.AutoConfirmEmailTF(resp.userInfo.user_id, auf.email);
                        if (TEST)
                        {

                            cufg.user_id = resp.userInfo.user_id;
                            string authY = PharmTaxiAdm.Methods.User.UserMethods.Authorize(args.email, args.password, "5");
                            AuthK resY = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthK>(authY);
                            cufg.alert.message = "Регистрация прошла успешно.";
                        }
                    }
                }
                if (cufg.alert.message == "Заявка на подключение сервиса ожидает подтверждения. На указанный email, ранее была отправлена ссылка для активации сервиса.")
                {
                    auf.email = args.email;
                    if (RegisterWorkaround.AutoConfirmEmailTF(auf))
                        cufg.alert.message = "Регистрация прошла успешно.";
                    else
                    {
                        bool TEST=false;
                        UserInfoReq req = new UserInfoReq() { email = auf.email, key = UMKB.GetUserInfoByEmail };
                        UserInfoResp resp = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoResp>(UMKBRequests.WebUtils.Post(URLs.GetUserInfoByEmail, Newtonsoft.Json.JsonConvert.SerializeObject(req), "application/json").Answer);
                        if(resp.userInfo is null)
                        {
                            var tr = PharmTaxiAdm.Methods.User.UserMethods.Authorize2(args.email, args.password, "5");
                            AuthKUserId atr = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthKUserId>(tr);
                            TEST = RegisterWorkaround.AutoConfirmEmailTF(atr.userid, auf.email);
                        }
                        else TEST = RegisterWorkaround.AutoConfirmEmailTF(resp.userInfo.user_id, auf.email);
                        if (TEST)
                        {

                            cufg.user_id = resp.userInfo.user_id;
                            string authY = PharmTaxiAdm.Methods.User.UserMethods.Authorize(args.email, args.password, "5");
                            AuthK resY = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthK>(authY);
                            cufg.alert.message = "Регистрация прошла успешно.";
                        }
                    }
                }
                //AuthKeyByLicKey auf = new AuthKeyByLicKey();
                //auf.email = args.email;
                //RegisterWorkaround.AutoConfirmEmail(auf);
                string auth = PharmTaxiAdm.Methods.User.UserMethods.Authorize(args.email, args.password, "5");
                AuthK res = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthK>(auth);
                if (res.alert.message == "Адрес уже зарегистрирован. Заявка на подключение сервиса ожидает подтверждения. На ваш адрес отправлена ссылка для активации сервиса")
                {
                    auf.email = args.email;
                    bool TEST = RegisterWorkaround.AutoConfirmEmailTF(auf);
                    if (!TEST)
                    {
                        UserInfoReq req = new UserInfoReq() { email = auf.email, key = UMKB.GetUserInfoByEmail };
                        UserInfoResp resp = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoResp>(UMKBRequests.WebUtils.Post(URLs.GetUserInfoByEmail, Newtonsoft.Json.JsonConvert.SerializeObject(req), "application/json").Answer);
                        TEST = RegisterWorkaround.AutoConfirmEmailTF(resp.userInfo.user_id, auf.email);
                        if (TEST)
                        {

                            cufg.user_id = resp.userInfo.user_id;
                            auth = PharmTaxiAdm.Methods.User.UserMethods.Authorize(args.email, args.password, "5");
                            res = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthK>(auth);

                        }
                    }
                }
                //cufg.alert.message = "Регистрация успешно пройдена!";
                cufg.authkey = res.authkey;
                cufg.alert = res.alert;
                cufg.user_id = 0;
                if (res.alert.code != "200") return cufg;
                cufg.authkey = res.authkey;
                uid = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(cufg.authkey, "5");
                cufg.user_id = uid.userid;
                if (uid.alert.code == "200")
                {
                    cufg.alert.code = uid.alert.code;
                    cufg.alert.level = uid.alert.level;
                    cufg.alert.message = uid.alert.message;
                    cufg.alert.title = uid.alert.title;
                }
                else
                {
                    cufg.alert.code = uid.alert.code;
                    cufg.alert.level = uid.alert.level;
                    cufg.alert.message = uid.alert.message;
                    cufg.alert.title = uid.alert.title;
                }
                return cufg;
            }
            catch
            {
                return new CreateUserFromGuestResponse()
                {
                    alert = new Alert()
                    {
                        code = "400",
                        level = "danger",
                        message = "Ошибка",
                        title = "Ошибка",
                        sticky = false
                    },
                    user_id = 0,
                    authkey = null
                };
            }
        }
    }
}
