using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Role
    {
       public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category{ get; set; }
    }
}
