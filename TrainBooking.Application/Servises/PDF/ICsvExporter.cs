using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Application.Servises.PDF
{
    public interface ICsvExporter
    {
        Task<byte[]> ExportToCsv<T>(IEnumerable<T> collection);
    }
}
