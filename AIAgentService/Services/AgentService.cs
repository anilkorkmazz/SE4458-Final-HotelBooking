using AIAgentService.DTOs;
using System.Text;
using System.Text.Json;

public class AgentService : IAgentService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AgentService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<HotelIntentDto> AnalyzeMessageAsync(string userMessage)
    {
        var apiKey = _config["OpenAI:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new Exception("OpenAI API key is missing in configuration.");

        var systemPrompt = """
        You are an AI assistant that extracts hotel-related intent from user input.
        You can return either:
        - "SearchHotel": when the user wants to **search** for available rooms.
        - "BookHotel": when the user wants to **book** a specific hotel.

        Return only JSON in this format:

        {
        "Intent": "BookHotel",
        "Username": "koray",
        "Location": "Maldives",
        "HotelName": "Ocean Breeze Resort",
        "StartDate": "2025-07-15",
        "EndDate": "2025-07-20",
        "PeopleCount": 2
        }

        Rules:
        - For SearchHotel, "HotelName" can be empty.
        - If the username is stated in the message, include it as "Username". Otherwise, leave it empty.
        - Extract values from the message dynamically — do not hardcode.
        - If no year is given, assume 2025.
        - Only return valid JSON. No explanation or extra text.
        """;



        var payload = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userMessage }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var resultJson = await response.Content.ReadAsStringAsync();

        // Güvenli JSON parse
        var doc = JsonDocument.Parse(resultJson);
        if (doc.RootElement.TryGetProperty("choices", out var choices) &&
            choices.GetArrayLength() > 0 &&
            choices[0].TryGetProperty("message", out var message) &&
            message.TryGetProperty("content", out var contentElement))
        {
            var content = contentElement.GetString();

            if (string.IsNullOrWhiteSpace(content))
                throw new Exception("OpenAI response content is empty.");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<HotelIntentDto>(content, options)
                ?? throw new Exception("Failed to deserialize intent.");
        }

        throw new Exception("OpenAI response format is invalid.");
    }
}
