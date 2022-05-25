﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirWaze.Entities
{
    public class Flight
    {
        [Key]
        public int FlightID { get; set; }
        public string FlightNr { get; set; }
        public Plane? CurrentPlane { get; set; }
        public bool CurrentPlaneConfirmed { get; set; }

        public DateTime Departure { get; set; }
        public Destination Destination { get; set; }
        public Gate CurrentGate { get; set; }
        public Runway CurrentRunway { get; set; }
        public int Status { get; set; }
        /* 
          0 = Generated, Not Approved
          1 = Approved
          2 = Delayed
          3 = Boarding
          4 = Departed
          5 = Arrived
          6 = Cancelled
          7 = Completed (Plane has returned)
         */
        public string? SeatDiagram { get; set; }

        public override string ToString()
        {
            return $"{Destination} {Departure}";
        }
    }
}
