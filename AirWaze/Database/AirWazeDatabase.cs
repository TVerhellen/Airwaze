using AirWaze.Database.Design;
using AirWaze.Entities;

namespace AirWaze.Database
{
    public class AirWazeDatabase : IAirWazeDatabase
    {
        private readonly AirWazeDbContext _dbContext;

        public AirWazeDatabase(AirWazeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddFlight(Flight flight)
        {
            throw new NotImplementedException();
        }

        public int AddTicket(Ticket ticket)
        {
            _dbContext.Tickets.Add(ticket);
            return _dbContext.SaveChanges();
        }

        public Flight GetFlightByNr(string nr)
        {
            throw new NotImplementedException();
        }

        public List<Flight> GetFlights()
        {
            throw new NotImplementedException();
        }

        public List<Ticket> GetTicketByFlight(Flight flight)
        {
            var query = from ticket in _dbContext.Tickets
                        where ticket.CurrentFlight == flight
                        select ticket;
            return query.ToList();
        }

        public Ticket GetTicketByNr(string nr)
        {
            var query = from ticket in _dbContext.Tickets
                        where ticket.TicketNr == nr
                        select ticket;
            return query.FirstOrDefault();
        }

        public List<Ticket> GetTickets()
        {
            return _dbContext.Tickets.ToList();
        }

        public List<Ticket> GetTicketsByUser(User user)
        {
            var query = from ticket in _dbContext.Tickets
                        where ticket.CurrentUser == user
                        select ticket;
            return query.ToList();
        }

        public void RemoveFlight(Flight flight)
        {
            throw new NotImplementedException();
        }

        public int RemoveTicket(Ticket ticket)
        {
            _dbContext.Tickets.Remove(ticket);
            return _dbContext.SaveChanges();
        }

        public void UpdateFlight(Flight flight)
        {
            throw new NotImplementedException();
        }

        public int UpdateTicket(Ticket ticket)
        {
            _dbContext.Tickets.Update(ticket);
            return _dbContext.SaveChanges();
        }
    }
}
