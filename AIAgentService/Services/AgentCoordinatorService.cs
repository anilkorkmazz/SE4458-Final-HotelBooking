using AIAgentService.DTOs;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using HotelService.DTOs;

public class AgentCoordinatorService : IAgentCoordinatorService
{
    private readonly IAgentService _agentService;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AgentCoordinatorService(IAgentService agentService, HttpClient httpClient, IConfiguration config)
    {
        _agentService = agentService;
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> HandleMessageAsync(string userMessage)
    {
        var intentDto = await _agentService.AnalyzeMessageAsync(userMessage);

        if (intentDto == null || string.IsNullOrWhiteSpace(intentDto.Intent))
            return "I couldn't understand your message. Please try again with more detail.";

        var hotelApiBaseUrl = _config["HotelService:BaseUrl"] ?? "http://localhost:5035";
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // 🏨 BOOKING INTENT
        if (intentDto.Intent == "BookHotel")
        {
            // 1. Arama yap
            var searchParams = new Dictionary<string, string>
            {
                { "Location", intentDto.Location ?? "" },
                { "StartDate", intentDto.StartDate?.ToString("yyyy-MM-dd") ?? "" },
                { "EndDate", intentDto.EndDate?.ToString("yyyy-MM-dd") ?? "" },
                { "PeopleCount", intentDto.PeopleCount.ToString() },
                { "Page", "1" },
                { "PageSize", "10" }
            };

            var searchUrl = $"{hotelApiBaseUrl}/api/v1/Room/search?{string.Join("&", searchParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"))}";

            var searchResponse = await _httpClient.GetAsync(searchUrl);
            if (!searchResponse.IsSuccessStatusCode)
                return "Sorry, I couldn't fetch available rooms.";

            var searchResult = await searchResponse.Content.ReadFromJsonAsync<PagedResult<RoomDto>>(options);
            if (searchResult == null || searchResult.Items.Count == 0)
                return "No rooms found for your booking request.";

            var matchingRoom = searchResult.Items
                .FirstOrDefault(r => string.Equals(r.Hotel?.Name, intentDto.HotelName, StringComparison.OrdinalIgnoreCase)
                                  && r.Capacity >= intentDto.PeopleCount);

            if (matchingRoom == null)
                return $"No available room found at {intentDto.HotelName} in {intentDto.Location}.";

            // 2. Rezervasyon yap
            var reservationDto = new ReservationRequestDto
            {
                RoomId = matchingRoom.Id,
                Username = intentDto.Username ?? "guest",
                StartDate = intentDto.StartDate ?? DateTime.UtcNow,
                EndDate = intentDto.EndDate ?? DateTime.UtcNow.AddDays(1),
                PeopleCount = intentDto.PeopleCount
            };

            var reservationUrl = $"{hotelApiBaseUrl}/api/v1/Reservation";
            var bookingResponse = await _httpClient.PostAsJsonAsync(reservationUrl, reservationDto);

            if (!bookingResponse.IsSuccessStatusCode)
            {
                var errorContent = await bookingResponse.Content.ReadAsStringAsync();
                return $"Booking failed. Server response: {errorContent}";
            }

            return $"✅ Booking successful!\n🏨 {matchingRoom.Hotel?.Name} in {matchingRoom.Hotel?.Location}\n🛏️ Room {matchingRoom.RoomNumber}\n📅 {reservationDto.StartDate:yyyy-MM-dd} → {reservationDto.EndDate:yyyy-MM-dd}\n👥 Guests: {reservationDto.PeopleCount}";
        }

        // 🔍 SEARCH INTENT
        if (intentDto.Intent == "SearchHotel")
        {
            var queryParams = new Dictionary<string, string>
            {
                { "Location", intentDto.Location },
                { "StartDate", intentDto.StartDate?.ToString("yyyy-MM-dd") ?? string.Empty },
                { "EndDate", intentDto.EndDate?.ToString("yyyy-MM-dd") ?? string.Empty },
                { "PeopleCount", intentDto.PeopleCount.ToString() },
                { "Page", "1" },
                { "PageSize", "10" }
            };

            var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            var requestUrl = $"{hotelApiBaseUrl}/api/v1/Room/search?{queryString}";

            var response = await _httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
                return "Sorry, I couldn't fetch hotel data at the moment.";

            var result = await response.Content.ReadFromJsonAsync<PagedResult<RoomDto>>(options);
            if (result == null || result.Items.Count == 0)
                return "No available rooms found for your request.";

            var sb = new StringBuilder();
            sb.AppendLine("Here are the available rooms:\n");

            foreach (var room in result.Items)
            {
                sb.AppendLine($"🏨 Hotel: {room.Hotel?.Name}");
                sb.AppendLine($"📍 Location: {room.Hotel?.Location}");
                sb.AppendLine($"🛏️ Room: {room.RoomNumber} | Capacity: {room.Capacity}");
                //sb.AppendLine($"💰 Price per night: {room.PricePerNight}₺");
                sb.AppendLine($"💰 Price per night: {room.PricePerNight.ToString("0.##")}₺");
                sb.AppendLine($"📅 Available: {room.AvailableFrom:yyyy-MM-dd} → {room.AvailableTo:yyyy-MM-dd}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        return "Sorry, I don't support this type of request yet.";
    }
}
