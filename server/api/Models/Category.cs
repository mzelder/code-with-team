namespace api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<UserSelection> UserSelections { get; set; }
    }
}
