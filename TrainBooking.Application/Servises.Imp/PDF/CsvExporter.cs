using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Application.Servises.PDF;

namespace TrainBooking.Application.Servises.Imp.PDF
{
    public class CsvExporter : ICsvExporter
    {
        public async Task<byte[]> ExportToCsv<T>(IEnumerable<T> collection)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var csvContent = new StringBuilder();

           
            var headers = properties.Select(p => p.Name);
            csvContent.AppendLine(string.Join(",", headers));

            // Дані з колекції
            foreach (var item in collection)
            {
                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item)?.ToString();
                    return value?.Contains(",") == true ? $"\"{value}\"" : value;
                });

                csvContent.AppendLine(string.Join(",", values));
            }

            var utf8WithBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
            return utf8WithBom.GetBytes(csvContent.ToString());
        }

    }
}
