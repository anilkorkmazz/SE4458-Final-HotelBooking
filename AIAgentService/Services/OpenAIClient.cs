using OpenAI_API;
using OpenAI_API.Chat;

namespace AIAgentService.Services
{
    public class OpenAIClient
    {
        private readonly OpenAIAPI _api;

        public OpenAIClient(IConfiguration config)
        {
            var key = config["OpenAI:ApiKey"];
            _api = new OpenAIAPI(key);
        }

        public async Task<string> GetIntentJsonAsync(string userMessage)
        {
            var chat = _api.Chat.CreateConversation();

            chat.AppendSystemMessage(@"
You are an AI that extracts booking intent from hotel queries. Return ONLY valid JSON with:
{""intent"":""BookHotel"", ""location"":""..."", ""start_date"":""..."", ""end_date"":""..."", ""people_count"":...}
");

            chat.AppendUserInput(userMessage);

            var response = await chat.GetResponseFromChatbotAsync();
            return response;
        }
    }
}
