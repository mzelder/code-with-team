using api.Models;

namespace api.Dtos.Matchmaking
{
    public class ChoosedOptionsDto
    {
       
        public int? CategoryId { get; set; }
        public int? RoleId { get; set; }
        public List<int>? ProgrammingLanguageIds { get; set; }
    }
}
