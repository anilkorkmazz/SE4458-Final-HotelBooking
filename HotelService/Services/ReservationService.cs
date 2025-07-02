using HotelService.DTOs;
using HotelService.Models;
using HotelService.Queue;
using HotelService.Queue.Messages;
using HotelService.Repositories;

namespace HotelService.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly RabbitMQPublisher _publisher;

        public ReservationService(
            IReservationRepository reservationRepo,
            IRoomRepository roomRepo,
            RabbitMQPublisher publisher)
        {
            _reservationRepo = reservationRepo;
            _roomRepo = roomRepo;
            _publisher = publisher;
        }

        public async Task<ReservationResponseDto> BookReservationAsync(ReservationRequestDto dto)
        {
            var room = await _roomRepo.GetByIdAsync(dto.RoomId);
            if (room == null)
            {
                throw new Exception("Room not found.");
            }

            //  Kapasite kontrolü
             if (room.Capacity < dto.PeopleCount)
                throw new Exception("Not enough capacity for this room.");

            //  Kapasite azaltma
            room.Capacity -= dto.PeopleCount;
            await _roomRepo.UpdateAsync(room.Id, room);

            var reservation = new Reservation
            {
                RoomId = dto.RoomId,
                Username = dto.Username,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                PeopleCount = dto.PeopleCount
            };

            var created = await _reservationRepo.AddAsync(reservation);

            //  RabbitMQ kuyruğuna mesaj gönder
            var message = new ReservationMessage
            {
                RoomId = created.RoomId,
                PeopleCount = created.PeopleCount,
                StartDate = created.StartDate,
                EndDate = created.EndDate
            };

            _publisher.Publish(message);

            return new ReservationResponseDto
            {
                Id = created.Id,
                RoomId = created.RoomId,
                Username = created.Username,
                StartDate = created.StartDate,
                EndDate = created.EndDate,
                PeopleCount = created.PeopleCount
            };
        }
    }
}
