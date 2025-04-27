using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.DTOs;

namespace TrainBooking.Application.Servises
{
    public interface ITrainStructureService
    {
        Task<TrainStructureDto> GetTrainStructureAsync(string trainNumber);
        Task<TrainStructureDto> GetTrainStructureWithAvalibleSeatsAsync(int scheduleId);
    }
}
