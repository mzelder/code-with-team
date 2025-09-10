namespace api.Models
{
    public class UserLanguage
    {
        public int Id { get; set; }
        public int UserSelectionId { get; set; }
        public UserSelection UserSelection { get; set; }
        public int ProgrammingLanguageId { get; set; }
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
    }
}
