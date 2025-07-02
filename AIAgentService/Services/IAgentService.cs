// IAgentService.cs
using AIAgentService.DTOs;

public interface IAgentService
{
    Task<HotelIntentDto> AnalyzeMessageAsync(string userMessage);
}
