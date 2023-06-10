using LiteDB;
using NureCistBot.Classes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NureCistBot.BackendServices
{
    public class Database
    {
        private static LiteDatabase db = new LiteDatabase(@"Data/Database.db");

        public static void AddGroup(Group group, Message message)
        {
            var collection = db.GetCollection<DbChat>("Chats");
            if (message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup)
            {
                var NewChat = new DbChat
                {
                    Id = message.Chat.Id,
                    IsPrivate = false,
                    CistId = group.Id,
                    CistName = group.Name,
                    ChatName = message.Chat.Title,
                    ChatNickname = message.Chat.Username
                };
                collection.Insert(NewChat);
            }
            else if (message.Chat.Type == ChatType.Private)
            {
                var NewChat = new DbChat
                {
                    Id = message.Chat.Id,
                    IsPrivate = true,
                    CistId = group.Id,
                    CistName = group.Name,
                    ChatName = message.Chat.FirstName,
                    ChatLastName = message.Chat.LastName,
                    ChatNickname = message.Chat.Username
                };
                collection.Insert(NewChat);
            }
        }
        public static void BlockGroup(DbChat chat)
        {
            var collection = db.GetCollection<DbChat>("BlockedChats");
            collection.Insert(chat);
        }

        public static bool CheckGroup(long Id)
        {
            var collection = db.GetCollection<DbChat>("Chats");
            if (collection.Count(x => x.Id == Id) > 0)
            {
                return true;
            }
            return false;
        }
        public static bool IsBlocked(long Id)
        {
            var collection = db.GetCollection<DbChat>("BlockedChats");
            if (collection.Count(x => x.Id == Id) > 0)
            {
                return true;
            }
            return false;
        }

        public static DbChat GetGroup(long Id)
        {
            var collection = db.GetCollection<DbChat>("Chats");
            return collection.FindOne(x => x.Id == Id);
        }
        public static List<DbChat> GetGroups()
        {
            var collection = db.GetCollection<DbChat>("Chats");
            return collection.FindAll().ToList();
        }

        public static void UpdateGroup(Message message, Group group)
        {
            var collection = db.GetCollection<DbChat>("Chats");
            var groupToUpdate = collection.FindOne(x => x.Id == message.Chat.Id);

            if (message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup)
            {
                groupToUpdate.CistId = group.Id;
                groupToUpdate.CistName = group.Name;
                groupToUpdate.ChatName = message.Chat.Title;
            }
            else if (message.Chat.Type == ChatType.Private)
            {
                groupToUpdate.CistId = group.Id;
                groupToUpdate.CistName = group.Name;
                groupToUpdate.ChatName = message.Chat.FirstName;
            }

            collection.Update(groupToUpdate);
        }
    }
}