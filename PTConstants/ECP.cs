using System;
using System.Collections.Generic;
using System.Text;

namespace PTConstants
{
    /// <summary>
    /// Все что относится к ЭКФ
    /// </summary>
    public class ECP
    {
        /// <summary>
        /// Номер сервиса
        /// </summary>
        public const string Service = "108";
        /// <summary>
        /// Номер старый сервиса
        /// </summary>
        public const string OldService = "4";
        /// <summary>
        /// Ключ для ЭКФ Web (технический API) (f37d7a0f2bdc4464 старый)
        /// </summary>
        public static string Key= "f6e285df1acfda4e";
        /// <summary>
        /// Id папки с концептами(Интерфейс ЭКФ -> Названия кнопок)
        /// </summary>
        public const string Buttons = "710000055";
        /// <summary>
        /// Id папки с концептами(Интерфейс ЭКФ -> Вспомогательный текст)
        /// </summary>
        public const string SupportText = "710000056";
        /// <summary>
        /// Id папки с концептами(кратность приема)
        /// </summary>
        public const string Freq = "260001853";
        /// <summary>
        /// Id папки с концептами(длительность приема)
        /// </summary>
        public const string Duration = "260001905";
        /// <summary>
        /// Id папки с концептами текстов варнингов
        /// </summary>
        public const string Caution = "710000100";
        /// <summary>
        /// Id(uint) папки с концептами текстов варнингов
        /// </summary>
        public const uint Cautionuint = 710000100;
        /// <summary>
        /// хз что это и для чего нужно, вроде что то типа кеша
        /// </summary>
        public static HashSet<uint> Exclusion = new HashSet<uint>();
        /// <summary>
        /// Адрес ЭКФа
        /// </summary>
        public static string Domain;
    }
}
