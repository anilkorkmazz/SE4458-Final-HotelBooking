using System.ComponentModel.DataAnnotations;

namespace HotelService.DTOs
{
    public class RoomReservationSummaryDto
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public int Capacity { get; set; }
        public int ReservedCount { get; set; }

        public string RoomNumber { get; set; } = string.Empty;     
        public string HotelName { get; set; } = string.Empty;     
    }
}
