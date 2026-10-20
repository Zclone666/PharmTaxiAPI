using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UMKBCoreAPI.Interfaces;
using UMKBCoreAPI.Models.Api.Requests.Umkb;

namespace CacheTextMessage
{
    public static class Cache
    {
        public static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        //текстовые сообщения ошибок
        private const string MessageError = "3970071";
        //текстовые сообщения
        private const string Message = "9070071";

        //Dictionary<локализация, Dictionary<id текста, сам текст>>
        private static readonly Dictionary<string, Dictionary<ulong, string>> _locales = new Dictionary<string, Dictionary<ulong, string>>();

        public static Task Initialize(List<string> locales, IUmkbApiInterface UmkbApi)
        {
            foreach (var local in locales)
            {
                if (string.IsNullOrWhiteSpace(local)) continue;

                locker.EnterWriteLock();

                try
                {
                    string locale = local.ToLower();
                    if (!_locales.ContainsKey(locale))
                    {
                        var response = UmkbApi.Umkb.GetSemantic(new RequestGetSemantic()
                        {
                            LibId = new List<string>() { Message, MessageError },
                            Lib = new List<ushort>() { 71 },
                            Level = new List<ushort>() { 1050, 1051 },
                            Deep = 10,
                            Locale = locale,
                            IsForcedDefaultLocale = true,
                            Route = 0
                        });

                        if (response.Alert != null && response.Alert.Code != "200")
                            throw new Exception(response.Alert.Message);

                        Dictionary<ulong, string> localeItems = new Dictionary<ulong, string>();

                        foreach (var item in response.Graph)
                        {
                            if (item.Type == 1) { continue; }

                            var id = item.IdB / 10000;

                            if (!localeItems.ContainsKey(id) && response.Names.ContainsKey(item.IdB))
                                localeItems.Add(id, response.Names[item.IdB]);
                        }
                        _locales.Add(locale, localeItems);
                    }
                }
                finally { locker.ExitWriteLock(); }
            }

            return Task.CompletedTask;
        }

        public static string GetMessage(uint Id, string locale)
        {
            locker.EnterReadLock();

            try
            {
                if (string.IsNullOrWhiteSpace(locale) || !_locales.ContainsKey(locale)) { locale = "ru"; }

                if (_locales.ContainsKey(locale) && _locales[locale].ContainsKey(Id))
                {
                    return _locales[locale][Id];
                }

                return $"Text {Id} not found";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally { locker.ExitReadLock(); }
        }
    }
}
