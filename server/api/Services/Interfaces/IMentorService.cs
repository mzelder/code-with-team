using api.Dtos.Mentor;

namespace api.Services.Interfaces
{
    public interface IMentorService
    {
        Task<MentorDto> GetMentorStatusAsync(int userId);
        Task<MentorQuestionDto> GetMentorQuestionsAsync();
        Task SubmitMentorFormAsync(int userId, MentorFormDto mentorFormDto);
        Task AcceptMentorApplicationAsync(int mentorFormId);
    }
}
