//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Train
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Train()
        {
            this.Bookings = new HashSet<Booking>();
            this.Cancellations = new HashSet<Cancellation>();
        }
    
        public int TrainNo { get; set; }
        public string TrainName { get; set; }
        public System.TimeSpan Departure_Time { get; set; }
        public System.TimeSpan Arrival_Time { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Class { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cancellation> Cancellations { get; set; }
    }
}