using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Domain.Entities
{
    public class RoutePopularityReportEntetity
    {
        public string Number { get; set; } = null!; // Номер потяга
        public string Direction { get; set; } = null!; // Напрямок (станція відправлення – станція прибуття)
        public int NumberOfTrips { get; set; } // Кількість поїздок
        public int NumberOfTicketsSold { get; set; } // Кількість проданих квитків
        public decimal AverageOccupancyPercentage { get; set; } // Середня завантаженість (%)
    }


    public class RevenueReportEntity
    {
        public DateTime Date { get; set; }  // Дата покупки квитка
        public int TicketsSold { get; set; }  // Кількість проданих квитків
        public decimal Revenue { get; set; }  // Загальний дохід
        public string MostPopularTrain { get; set; }  // Найпопулярніший потяг
    }
}
