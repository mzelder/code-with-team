namespace api.Services.Interfaces
{
    public interface IGreptileReviewService
    {
        Task<string> ReviewCodeAsync(string repoName);
    }
}
