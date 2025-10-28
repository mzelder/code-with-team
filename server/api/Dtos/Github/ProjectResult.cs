using Octokit.GraphQL;

namespace api.Dtos.Github
{
    public class ProjectResult
    {
        public ID Id { get; set; }
        public string Url { get; set; }
    }
}
