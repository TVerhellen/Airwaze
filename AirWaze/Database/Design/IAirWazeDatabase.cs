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

        List<Ticket> GetTickets();

        List<Ticket> GetTicketByFlight(Flight flight);

        List<Ticket> GetTicketsByUser(User user);

        Ticket GetTicketByNr(string nr);

        int AddTicket(Ticket ticket);

        int UpdateTicket(Ticket ticket);

        int RemoveTicket(Ticket ticket);
    }
}
