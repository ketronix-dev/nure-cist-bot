namespace NureCistBot.Classes
{
    public class DbChat
    {
        public long Id { get; set; }
        public bool IsPrivate { get; set; }
        public long CistId { get; set; }
        public string? CistName { get; set; }
        public string? ChatName { get; set; }
        // If this chat is private
        public string? ChatLastName { get; set; }
        public string? ChatNickname { get; set; }
    }
}