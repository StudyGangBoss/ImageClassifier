namespace Domain;

public class UserInfo : IDEntity
{
    public Guid Id { get; set; }
    public string TelegramName { get; set; }
    public UserRole UserRole { get; set; }
}