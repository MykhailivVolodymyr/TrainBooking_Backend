﻿using System;
using System.Collections.Generic;

namespace TrainBooking.Domain.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int UserId { get; set; }

    public int SeatId { get; set; }

    public int TripId { get; set; }

    public int? PaymentId { get; set; }

    public decimal TicketPrice { get; set; }

    public int ScheduleId { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;

    public virtual Trip Trip { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
