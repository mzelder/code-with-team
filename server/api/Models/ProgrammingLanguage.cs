namespace api.Models
{
    public class ProgrammingLanguage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<UserLanguage> UserLanguages { get; set; }
    }
}
