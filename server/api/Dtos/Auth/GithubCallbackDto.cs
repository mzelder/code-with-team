using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Auth
{
    public class GithubCallbackDto
    {
        [Required]
        public string Code { get; set; }
    }
}