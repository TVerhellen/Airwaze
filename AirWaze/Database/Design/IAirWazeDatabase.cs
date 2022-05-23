using AirWaze.Entities;

namespace AirWaze.Database.Design
{
    public interface IAirWazeDatabase
    {

       
        List<Flight> GetFlights();
      

        Flight GetFlightByNr(string nr);

        List<Flight> GetFlightsByDate(DateTime date, int range);

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

        List<User> GetUsers();

        User FindUserByID(Guid id);

        void AddUser(User user);

        void UpdateUser(User user);

        void RemoveUser(User user);
        public List<Ticket> GetTicketsByFlight(string flightnr);

        List<Ticket> GetTickets();

        //List<Ticket> GetTicketByFlight(Flight flight);

        List<Ticket> GetTicketsByUser(User user);

        Ticket GetTicketByNr(string nr);

        int AddTicket(Ticket ticket);

        int UpdateTicket(Ticket ticket);

        int RemoveTicket(Ticket ticket);

        List<Gate>GetGates();

        Gate GetGateByNr(int nr);

        void AddGate(Gate flight);

        List<Runway> GetRunways();

        Runway GetRunwayByNr(int nr);

        void AddRunway(Runway runway);
        List<Destination> GetDestinations();
        Destination GetDestinationByID(int id);
        void AddDestination(Destination destination);
    }
}
