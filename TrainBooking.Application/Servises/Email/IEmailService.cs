﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Application.Servises.Email
{
    public interface IEmailService
    {
        public Task SendTicketEmailAsync(string token, List<(byte[] pdf, string fileName)> attachments, string body);
    }
}
