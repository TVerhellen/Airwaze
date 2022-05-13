using AirWaze.Entities;

namespace AirWaze.Database.Design
{
    public interface IAirWazeDatabase
    {
        List<Flight> GetFlights();

        Flight GetFlightByNr(string nr);

        void AddFlight(Flight flight);

        void UpdateFlight(Flight flight);

        void RemoveFlight(Flight flight);
    }
}
