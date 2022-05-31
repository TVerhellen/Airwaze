using System.ComponentModel.DataAnnotations;
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

        public string FillSeatDiagram(int seatsToAdd)
        {
            if(SeatDiagram != null)
            {
                int seatCounter = 0;
                int rowCounter = 0;
                bool isFound = false;
                for(int i = 0; i < SeatDiagram.Length; i++)
                {
                    switch (SeatDiagram[i])
                    {
                        case '0': //free seat
                            SeatDiagram = SeatDiagram.Remove(i, 1).Insert(i, "1");
                            isFound = true;
                            break;
                        case '-': //break between rows
                            seatCounter = 0;
                            rowCounter++;
                            break;
                        default: //any other option
                            seatCounter++;
                            break;
                    }
                    if (isFound) //empty seat
                    {
                        break;
                    }
                }
                string seatNr = "";
                seatNr += (rowCounter+1).ToString();
                switch (seatCounter)
                {
                    case 0:
                        seatNr += "A";
                        break;
                    case 1:
                        seatNr += "B";
                        break;
                    case 2:
                        seatNr += "C";
                        break;
                    case 3:
                        seatNr += "D";
                        break;
                    case 4:
                        seatNr += "E";
                        break;
                    case 5:
                        seatNr += "F";
                        break;
                    case 6:
                        seatNr += "G";
                        break;
                    case 7:
                        seatNr += "H";
                        break;
                }
                return seatNr;
            }
            return "15B";
        }
    }
}
