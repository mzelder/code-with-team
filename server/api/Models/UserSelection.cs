namespace api.Models
{
    public class UserSelection
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<UserLanguage> UserLanguages { get; set; }
        public ICollection<LobbyQueue> LobbyQueues { get; set; }
    }
}
