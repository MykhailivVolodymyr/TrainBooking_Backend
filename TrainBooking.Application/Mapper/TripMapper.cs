using TrainBooking.Application.DTOs;
using TrainBooking.Domain.Models;

public static class TripMapper
{
    public static Trip MapToTrip(TripDto tripDto)
    {
        if (tripDto == null)
        {
            throw new ArgumentNullException(nameof(tripDto));
        }

        return new Trip
        {
            TrainId = tripDto.TrainId,
            DepartureTime = tripDto.DepartureTime,
            ArrivalTime = tripDto.ArrivalTime,
            ScheduleId = tripDto.SheduleId 
        };
    }
}
