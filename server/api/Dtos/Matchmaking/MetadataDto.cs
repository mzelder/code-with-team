using api.Models;

namespace api.Dtos.Matchmaking
{
    public class MetadataDto
    {
        public List<CategoryDto> Categories { get; set; }
        public List<RoleDto> Roles{ get; set; }
        public List<ProgrammingLanguageDto> ProgrammingLanguages{ get; set; }
    }
}
