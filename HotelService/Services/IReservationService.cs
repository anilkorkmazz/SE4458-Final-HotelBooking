using HotelService.DTOs;

namespace HotelService.Services
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> BookReservationAsync(ReservationRequestDto dto);
    }
}
