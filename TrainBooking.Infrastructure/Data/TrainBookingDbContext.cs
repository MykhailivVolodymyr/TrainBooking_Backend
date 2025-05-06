using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TrainBooking.Domain.Models;

namespace TrainBooking.Infrastructure.Data;

public partial class TrainBookingDbContext : DbContext
{
    public TrainBookingDbContext()
    {
    }

    public TrainBookingDbContext(DbContextOptions<TrainBookingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carriage> Carriages { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<RouteStation> RouteStations { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<SchedulePattern> SchedulePatterns { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Train> Trains { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-1BTCT19;Database=TrainBookingDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carriage>(entity =>
        {
            entity.HasKey(e => e.CarriageId).HasName("PK__Carriage__17FE2DD0FDCF172A");

            entity.ToTable("Carriage");

            entity.Property(e => e.CarriageType).HasMaxLength(50);

            entity.HasOne(d => d.Train).WithMany(p => p.Carriages)
                .HasForeignKey(d => d.TrainId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Carriage__TrainI__4AB81AF0");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A38697B1143");

            entity.ToTable("Payment");

            entity.Property(e => e.CardCvv)
                .HasMaxLength(3)
                .HasColumnName("CardCVV");
            entity.Property(e => e.CardDate).HasMaxLength(5);
            entity.Property(e => e.CardNumber).HasMaxLength(16);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PK__Route__80979B4D90088008");

            entity.ToTable("Route");

            entity.HasIndex(e => new { e.StartStationId, e.EndStationId, e.VariantName }, "UQ_Route_StartEndVariant").IsUnique();

            entity.Property(e => e.VariantName).HasMaxLength(200);

            entity.HasOne(d => d.EndStation).WithMany(p => p.RouteEndStations)
                .HasForeignKey(d => d.EndStationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Route__EndStatio__4222D4EF");

            entity.HasOne(d => d.StartStation).WithMany(p => p.RouteStartStations)
                .HasForeignKey(d => d.StartStationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Route__StartStat__412EB0B6");
        });

        modelBuilder.Entity<RouteStation>(entity =>
        {
            entity.HasKey(e => new { e.RouteId, e.StationId }).HasName("PK__RouteSta__4E9A1126D7D673A4");

            entity.ToTable("RouteStation");

            entity.HasIndex(e => new { e.RouteId, e.StationId }, "idx_route_station");

            entity.HasOne(d => d.Route).WithMany(p => p.RouteStations)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("FK__RouteStat__Route__66603565");

            entity.HasOne(d => d.Station).WithMany(p => p.RouteStations)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK__RouteStat__Stati__6754599E");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B4998049185");

            entity.ToTable("Schedule");

            entity.HasOne(d => d.Train).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TrainId)
                .HasConstraintName("FK__Schedule__TrainI__534D60F1");
        });

        modelBuilder.Entity<SchedulePattern>(entity =>
        {
            entity.HasKey(e => e.PatternId).HasName("PK__Schedule__0A631B5288392C16");

            entity.ToTable("SchedulePattern");

            entity.Property(e => e.DayParity).HasMaxLength(10);
            entity.Property(e => e.DaysOfWeek).HasMaxLength(15);
            entity.Property(e => e.FrequencyType).HasMaxLength(50);

            entity.HasOne(d => d.Train).WithMany(p => p.SchedulePatterns)
                .HasForeignKey(d => d.TrainId)
                .HasConstraintName("FK__ScheduleP__Train__1EA48E88");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PK__Seat__311713F35CA86FED");

            entity.ToTable("Seat");

            entity.Property(e => e.SeatType).HasMaxLength(50);

            entity.HasOne(d => d.Carriage).WithMany(p => p.Seats)
                .HasForeignKey(d => d.CarriageId)
                .HasConstraintName("FK__Seat__CarriageId__4F7CD00D");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.StationId).HasName("PK__Station__E0D8A6BD0307EB5E");

            entity.ToTable("Station");

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.StationName).HasMaxLength(200);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__712CC607B06F1D7F");

            entity.ToTable("Ticket");

            entity.HasIndex(e => e.SeatId, "IX_Ticket_SeatId");

            entity.HasIndex(e => e.UserId, "idx_ticket_user");

            entity.Property(e => e.TicketPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Payment).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Ticket__PaymentI__6383C8BA");

            //entity.HasOne(d => d.Schedule).WithMany(p => p.Tickets)
            //    .HasForeignKey(d => d.ScheduleId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_Ticket_Schedule");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__SeatId__5DCAEF64");

            entity.HasOne(d => d.Trip).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__TripId__5EBF139D");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__UserId__5CD6CB2B");
        });

        modelBuilder.Entity<Train>(entity =>
        {
            entity.HasKey(e => e.TrainId).HasName("PK__Train__8ED2723A20113DE3");

            entity.ToTable("Train");

            entity.HasIndex(e => e.Number, "UQ__Train__78A1A19D2C2E07DA").IsUnique();

            entity.Property(e => e.Number).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Route).WithMany(p => p.Trains)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Train__RouteId__45F365D3");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.TripId).HasName("PK__Trip__51DC713E2F7223F2");

            entity.ToTable("Trip");

            entity.Property(e => e.ArrivalTime).HasColumnType("datetime");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");

            entity.HasOne(d => d.EndStation).WithMany(p => p.TripEndStations)
                .HasForeignKey(d => d.EndStationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trip__EndStation__59063A47");

            entity.HasOne(d => d.StartStation).WithMany(p => p.TripStartStations)
                .HasForeignKey(d => d.StartStationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trip__StartStati__5812160E");

            entity.HasOne(d => d.Train).WithMany(p => p.Trips)
                .HasForeignKey(d => d.TrainId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Trip__TrainId__5629CD9C");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Trips)
               .HasForeignKey(d => d.ScheduleId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Ticket_Schedule__");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CBD8C199D");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D105341A7E32C7").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasDefaultValue("User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
