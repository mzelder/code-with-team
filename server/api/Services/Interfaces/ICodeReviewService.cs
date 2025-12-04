using api.Models;
using Microsoft.Graph.Reports.AuthenticationMethods.UsersRegisteredByFeatureWithIncludedUserTypesWithIncludedUserRoles;

namespace api.Services.Interfaces
{
    public interface ICodeReviewService
    {
        Task GenerateLobbySummaryAsync(Lobby lobby, CancellationToken ct = default);
    }
}
