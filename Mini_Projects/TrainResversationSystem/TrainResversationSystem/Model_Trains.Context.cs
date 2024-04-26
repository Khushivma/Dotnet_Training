﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainResversationSystem
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TrainRerversationSystemEntities01 : DbContext
    {
        public TrainRerversationSystemEntities01()
            : base("name=TrainRerversationSystemEntities01")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminDetail> AdminDetails { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Cancellation> Cancellations { get; set; }
        public virtual DbSet<Train> Trains { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual int InsertBookingAndUpdateTrainWithDates(string passengerName, Nullable<int> trainNo, string @class, Nullable<System.DateTime> date_of_Travel, Nullable<int> totalseats, ObjectParameter totalamount)
        {
            var passengerNameParameter = passengerName != null ?
                new ObjectParameter("PassengerName", passengerName) :
                new ObjectParameter("PassengerName", typeof(string));
    
            var trainNoParameter = trainNo.HasValue ?
                new ObjectParameter("TrainNo", trainNo) :
                new ObjectParameter("TrainNo", typeof(int));
    
            var classParameter = @class != null ?
                new ObjectParameter("Class", @class) :
                new ObjectParameter("Class", typeof(string));
    
            var date_of_TravelParameter = date_of_Travel.HasValue ?
                new ObjectParameter("Date_of_Travel", date_of_Travel) :
                new ObjectParameter("Date_of_Travel", typeof(System.DateTime));
    
            var totalseatsParameter = totalseats.HasValue ?
                new ObjectParameter("Totalseats", totalseats) :
                new ObjectParameter("Totalseats", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertBookingAndUpdateTrainWithDates", passengerNameParameter, trainNoParameter, classParameter, date_of_TravelParameter, totalseatsParameter, totalamount);
        }
    
        public virtual int InsertCancellationAndUpdateTrainWithRefund(Nullable<int> cancel_id, Nullable<int> booking_ID, string passengerName, string @class, Nullable<System.DateTime> cancel_Date, Nullable<int> no_of_Seats, string remarks)
        {
            var cancel_idParameter = cancel_id.HasValue ?
                new ObjectParameter("Cancel_id", cancel_id) :
                new ObjectParameter("Cancel_id", typeof(int));
    
            var booking_IDParameter = booking_ID.HasValue ?
                new ObjectParameter("Booking_ID", booking_ID) :
                new ObjectParameter("Booking_ID", typeof(int));
    
            var passengerNameParameter = passengerName != null ?
                new ObjectParameter("PassengerName", passengerName) :
                new ObjectParameter("PassengerName", typeof(string));
    
            var classParameter = @class != null ?
                new ObjectParameter("Class", @class) :
                new ObjectParameter("Class", typeof(string));
    
            var cancel_DateParameter = cancel_Date.HasValue ?
                new ObjectParameter("Cancel_Date", cancel_Date) :
                new ObjectParameter("Cancel_Date", typeof(System.DateTime));
    
            var no_of_SeatsParameter = no_of_Seats.HasValue ?
                new ObjectParameter("No_of_Seats", no_of_Seats) :
                new ObjectParameter("No_of_Seats", typeof(int));
    
            var remarksParameter = remarks != null ?
                new ObjectParameter("Remarks", remarks) :
                new ObjectParameter("Remarks", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertCancellationAndUpdateTrainWithRefund", cancel_idParameter, booking_IDParameter, passengerNameParameter, classParameter, cancel_DateParameter, no_of_SeatsParameter, remarksParameter);
        }
    
        public virtual ObjectResult<string> RegisterUser(string username, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("RegisterUser", usernameParameter, passwordParameter);
        }
    
        public virtual int UpdateTrainStatus(Nullable<int> trainNo, string newStatus)
        {
            var trainNoParameter = trainNo.HasValue ?
                new ObjectParameter("TrainNo", trainNo) :
                new ObjectParameter("TrainNo", typeof(int));
    
            var newStatusParameter = newStatus != null ?
                new ObjectParameter("NewStatus", newStatus) :
                new ObjectParameter("NewStatus", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateTrainStatus", trainNoParameter, newStatusParameter);
        }
    
        public virtual ObjectResult<string> UserLogin(string username, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("UserLogin", usernameParameter, passwordParameter);
        }
    }
}