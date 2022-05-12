using AirWaze.Entities;

namespace AirWaze.Database.Design
{
    public interface IFlightDatabase
    {
        List<Flight> GetFlights()
        {
            List<Flight> flights = new List<Flight>();

            // TODO: get list flights from database

            return flights;
        }

        Flight GetFlightByNr(string nr)
        {
            Flight flight = null;

            // TODO: get flight from database

            return flight;
        }

        void AddFlight(Flight flight)
        {
            // TODO: add flight to database
        }

        void UpdateFlight(Flight flight)
        {
            // TODO: update flight in database
        }

        void RemoveFlight(Flight flight)
        {
            // TODO: remove flight from database
        }
    }
}
