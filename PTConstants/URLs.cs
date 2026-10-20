using System;
using System.Collections.Generic;
using System.Text;

namespace PTConstants
{
    /// <summary>
    /// Тут собраны все URL
    /// </summary>
    public class URLs
    {
        /// <summary>
        /// какая то не понятная штука
        /// </summary>
        public const string UPCReqAddress = "https://api.upcitemdb.com/prod/trial/lookup";
        /// <summary>
        /// какая то не понятная штука
        /// </summary>
        public static string PrescrInfoShort = $"{eRpPrescript}/api/prescription/short/list";
        /// <summary>
        /// какая то не понятная штука
        /// </summary>
        public static string PrescrInfoFull = $"{eRpPrescript}/api/prescription/list";
        /// <summary>
        /// какая то не понятная штука
        /// </summary>
        public static string PrescRelease = $"{eRpPrescript}/api/release";
        /// <summary>
        /// какая то не понятная штука
        /// </summary>
        public const string eRpPrescript = "https://e-rp.ru/PrescriptionDev";

        #region ЭКФ
        /// <summary>
        /// Адрес бека ЭКФ
        /// </summary>
        public static string DomainECP = ECP.Domain;
        /// <summary>
        /// Адрес API elements
        /// </summary>
        public static string Elements = $"{DomainECP}/info/elements";
        /// <summary>
        /// Адрес API UIText
        /// </summary>
        public static string UIText = $"{DomainECP}/info/UIText";
        /// <summary>
        /// Адрес API Dosages
        /// </summary>
        public static string Dosages = $"{DomainECP}/drugs/Dosages";
        /// <summary>
        /// Адрес API buttonTexts
        /// </summary>
        public static string ButtonTexts = $"{DomainECP}/info/buttonTexts";
        /// <summary>
        /// Адрес API Schedule
        /// </summary>
        public static string Schedule = $"{DomainECP}/info/Schedule";
        /// <summary>
        /// Адрес API UnitsPharm
        /// </summary>
        public static string UnitsPharm = $"{DomainECP}/tools/UnitsPharm";
        /// <summary>
        /// Адрес API UnitsPrimary
        /// </summary>
        public static string UnitsPrimary = $"{DomainECP}/tools/UnitsPrimary";
        /// <summary>
        /// Адрес API FPDosages
        /// </summary>
        public static string FPDosages = $"{DomainECP}/drugs/FPDosage";
        /// <summary>
        /// Адрес API CreateDosage
        /// </summary>
        public static string CreateDosage = $"{DomainECP}/drugs/CreateDosage";
        /// <summary>
        /// Адрес API Diseases
        /// </summary>
        public static string Diseases = $"{DomainECP}/Search/Diseases";
        /// <summary>
        /// Адрес API Allergy
        /// </summary>
        public static string Allergy = $"{DomainECP}/Search/Allergy";
        /// <summary>
        /// Адрес API Info
        /// </summary>
        public static string Info = $"{DomainECP}/drugs/Info";
        /// <summary>
        /// Адрес API Warnings
        /// </summary>
        public static string Warnings = $"{DomainECP}/Check/Warnings";
        /// <summary>
        /// Адрес API Actual
        /// </summary>
        public static string Actual = $"{DomainECP}/Prescript/Actual";
        /// <summary>
        /// Адрес API Recipes
        /// </summary>
        public static string Recipes = $"{DomainECP}/Prescript/Recipes";
        /// <summary>
        /// Адрес API RecipesCore
        /// </summary>
        public static string RecipesCore = $"{DomainECP}/Prescript/RecipeCore";
        /// <summary>
        /// Адрес API для получения печатной формы рецепта
        /// </summary>
        public static string RecipeByCode = $"{DomainECP}/prescript/recipe?code=";
        #endregion

        #region Ядро
        /// <summary>
        /// Адрес бека Ядра
        /// </summary>
        public static string DomainCore = "https://cs.socmedica.com";
        /// <summary>
        /// Адрес API для получения информации о пользователе по Email
        /// </summary>
        public static string GetUserInfoByEmail = $"{DomainCore}/api/user/GetUserInfoByEmail";
        /// <summary>
        /// Адрес API для авторизация пользователя
        /// </summary>
        public static string Authorization = $"{DomainCore}/api/user/Authorization";
        /// <summary>
        /// Адрес API для проверки ид пользователя, вроде, не уверен
        /// </summary>
        public static string CheckRighstUserId = $"{DomainCore}/api/umkb/CheckRightsUserid";
        /// <summary>
        /// Адрес API для запроса ссылки на сброс пароля, вроде, не уверен
        /// </summary>
        public static string PassRecover = $"{DomainCore}/api/user/Recovery";
        /// <summary>
        /// Адрес API для подтверждения почты(нет ссылок)
        /// </summary>
        public static string ConfirmEmail = $"{DomainCore}/api/registration/ConfirmEmail";
        /// <summary>
        /// Адрес API для приглашения пользователя
        /// </summary>
        public static string Invite = $"{DomainCore}/api/invitations/Invite";
        /// <summary>
        /// Адрес API для подтверждения приглашения
        /// </summary>
        public static string ConfirmInvite = $"{DomainCore}/api/invitations/ConfirmInviteAuthkey";
        /// <summary>
        /// Адрес API для регистрации пользователя пользователем
        /// </summary>
        public static string RegisterUserByUser = $"{DomainCore}/api/registration/RegisterUserByUser";
        /// <summary>
        /// Адрес API для создания пользователя из гостя
        /// </summary>
        public static string CreateUserFromGuestURL = $"{DomainCore}/api/guest/RegisterUserFromGuest";
        /// <summary>
        /// Адрес API для регистрации пользователя, вроде, не уверен
        /// </summary>
        public static string Registration = $"{DomainCore}/api/registration/send";
        /// <summary>
        /// Адрес API для создания гостевого пользователя
        /// </summary>
        public static string CreateGuestUserURL = $"{DomainCore}/api/guest/Create";
        /// <summary>
        /// Адрес API для генерации пользователя по authkey (другим пользователем)
        /// </summary>
        public static string GenerateUserByAuthkey = $"{DomainCore}/api/user/GenerateUserByAuthkey";
        /// <summary>
        /// Адрес API для добавления сервиса пользователю по его Authkey
        /// </summary>
        public static string AddServiceByAuthkey = $"{DomainCore}/api/user/AddServiceByAuthkey";
        /// <summary>
        /// Адрес API для получения записи названий концепта из списка, обращения к БД
        /// </summary>
        public static string GetNamesDB = $"{DomainCore}/api/umkb/GetNamesDB";
        /// <summary>
        /// Адрес API для моделирования (добавления, изменения и удаления) данных в UMKB
        /// </summary>
        public static string Modelling = $"{DomainCore}/api/umkb/Modeling";
        /// <summary>
        /// Адрес API для получения параметров концепта в формате varchar по фильтру
        /// </summary>
        public static string GetVarcharWhere = $"{DomainCore}/API/UMKB/GetVarcharWhere";
        #endregion
    }
}
