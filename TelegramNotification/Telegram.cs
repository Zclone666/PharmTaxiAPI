using System;
using PTConstants;
using System.Threading.Tasks;
using SMTelegram;

namespace Telegram
{
    public static class Singleton
    {
        public static Notification Notification { get; set; }

        public static Task Initialize(string botToken, string projectChat, string devOpsChat, string UMKBChat)
        {
            Notification = new Notification(botToken, projectChat,devOpsChat,UMKBChat);

            return Task.CompletedTask;
        }
    }
}
