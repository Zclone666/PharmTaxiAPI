using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models
{
    /// <summary>
    /// Параметры развертки графа
    /// </summary>
    public class SettingGraph
    {
        public string key; // лицензионный ключ
        public string login; // логин пользователя
        public string authkey; // динамический ключ активации
        public List<ushort> tool;  // список id инструментов для развертки
        public List<ushort> lib; // Список номеров разделов в которых осуществляется поиск (в режиме поиска по классификаторам и по таблицам первообразных концептов, указывается только один раздел)
        public string processing; // название шаблона процесса выполнения поиска (например, диагностика)
        public int binarization = 0; // режим бинаризации концепта (0 - деление концепта выключено, 1 - включено)
        public List<ushort> level; // список базовых id типов концептов для развертки
        public List<string> libid; // список с индексами (сквазной - bigid) концептов 
        public List<List<string>> libids; // список списка с индексами (сквазной - bigid) концептов
        public short route; // базовое направление развертки
        public List<short> routs; // список с направлениями связей для каждого типа связи level, соответственно
        public short deep; // глубина развертки графа
        public HashSet<ushort> SelectLevelb; // Список типов концептов правого плеча (levelb), в рамках которых будет отфильтрован результат
        public string text; // текст с названиеями концептов
        public List<string> texts; // список текстов с названиеями концептов, если список не пуст, то параметр text игнорируется
        public bool valmore; // включает дополнительные занчения, например value_b и value_c (2 - value_b, 3 - value_c, 5 - value_b и value_c)
        public string locale; // фильтр локали (языка).
        public bool isForcedDefaultLocale = false; // флаг для принудительного получения имён, при отсуствия фильтрующей локали
        public HashSet<ulong> SelectLongIDB = null; // Фильтр по idb(LongID) полю.
        public Dictionary<ulong, List<ulong>> SelectLongIDBByIDA = null; // Фильтр по idb(LongID) полю.
        public HashSet<ushort> InvisibleLevelb; // Фильтр невидимых записей по Levelb полю.
        public UMKBRequests.NextArm NextArm; // NextArm параметр для выбора хождения по графу в рамках одного запроса. Варианты параметра: Right - от правого плеча, Сenter - от ребра, RightCenter - от правого плеча и ребра
        public HashSet<ushort> IgnoreLevelb = null; // Фильтр для игнорирования по Levelb полю.
        public HashSet<ushort> IgnoreLib_B = null; // Фильтр для игнорирования по Lib_b полю.
        public HashSet<ulong> IgnoreLongIDB = null; // Фильтр для игнорирования по idb(LongID) полю.
    }
}
