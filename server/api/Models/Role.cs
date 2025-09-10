using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public ICollection<UserSelection> UserSelections { get; set; }
    }
}
