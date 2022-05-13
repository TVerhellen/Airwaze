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
        
        List<Plane> GetPlanes();

        Plane GetPlaneByNr(string nr);

        void AddPlane(Plane plane);

        void UpdatePlane(Plane plane);

        void RemovePlane(Plane plane);

        List<Airline> GetAirlines();

        Airline GetAirlineByID(Guid id);
        
        void AddAirline(Airline airline);

        void UpdateAirline(Airline airline);

        void RemoveAirline(Airline airline);
    }
}
