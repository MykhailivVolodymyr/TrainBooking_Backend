using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Application.DTOs
{
    public class TrainStructureDto
    {
        public string TrainNumber { get; set; }
        public List<CarriageDto> Carriages { get; set; }
    }

    public class CarriageDto
    {
        public int CarriageId { get; set; }
        public string CarriageType { get; set; }
        public int Capacity { get; set; }
        public List<SeatDto> Seats { get; set; }
    }

    public class SeatDto
    {
        public int SeatId { get; set; }
        public int SeatNumber { get; set; }
        public string SeatType { get; set; }
    }

}
