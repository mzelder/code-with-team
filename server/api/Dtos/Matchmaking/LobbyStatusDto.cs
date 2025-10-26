namespace api.Dtos.Matchmaking
{
    public class LobbyStatusDto
    {
        public bool Found { get; set; }
        public int? LobbyId { get; set; }
        public List<LobbyMemberDto>? Members { get; set; }
        public string? RepositoryUrl { get; set; }
    }
}
