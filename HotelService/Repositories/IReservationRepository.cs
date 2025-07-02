using HotelService.Models;

namespace HotelService.Repositories
{
    public interface IReservationRepository
    {
        Task<Reservation> AddAsync(Reservation reservation);
    }
}
