using System;
using System.Collections.Generic;

namespace TrainBooking.Domain.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CardNumber { get; set; } = null!;

    public string CardDate { get; set; } = null!;

    public string CardCvv { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
