using AirWaze.Database.Design;
using AirWaze.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirWaze.Database
{
    public class AirWazeDatabase : IAirWazeDatabase
    {

        private readonly AirWazeDbContext _dbContext;

        public AirWazeDatabase(AirWazeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddAirline(Airline airline)
        {
            _dbContext.Airlines.Add(airline);
            //_dbContext.Entry(airline.CurrentPlanes).State = EntityState.Unchanged;
            _dbContext.SaveChanges();
        }

        public void AddFlight(Flight flight)
        {
            _dbContext.Flights.Add(flight);
            _dbContext.Entry(flight.CurrentPlane).State = EntityState.Unchanged;
            _dbContext.Entry(flight.CurrentPlane.CurrentAirline).State = EntityState.Unchanged;
            _dbContext.Entry(flight.CurrentGate).State = EntityState.Unchanged;
            _dbContext.Entry(flight.CurrentRunway).State = EntityState.Unchanged;
            _dbContext.SaveChanges();
        }

        public void AddPlane(Plane plane)
        {

            _dbContext.Airlines.Remove(_dbContext.Airlines.FirstOrDefault(x => x.AirlineID == plane.CurrentAirline.AirlineID));
            _dbContext.Airlines.Update(plane.CurrentAirline);
            _dbContext.Planes.Add(plane);
            _dbContext.SaveChanges();
        }

        public Airline GetAirlineByID(Guid ID)
        {
            return _dbContext.Airlines.SingleOrDefault(airline => airline.AirlineID == ID);
        }

        public List<Airline> GetAirlines()
        {
            return _dbContext.Airlines.ToList();
        }

        public void AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public User FindUserByID(Guid id)
        {
            return _dbContext.Users.SingleOrDefault(user => user.UserID.Equals(id));
        }

        public int AddTicket(Ticket ticket)
        {
            _dbContext.Tickets.Add(ticket);
            _dbContext.Entry(ticket.CurrentUser).State = EntityState.Unchanged;
            _dbContext.Entry(ticket.CurrentFlight).State = EntityState.Unchanged;
            _dbContext.Entry(ticket.CurrentFlight.CurrentPlane).State = EntityState.Unchanged;
            _dbContext.Entry(ticket.CurrentFlight.CurrentPlane.CurrentAirline).State = EntityState.Unchanged;
            return _dbContext.SaveChanges();
        }

        public Flight GetFlightByNr(string nr)
        {
            return _dbContext.Flights
                .AsNoTracking()
                .Include(x => x.CurrentPlane)
                .Include(x => x.CurrentPlane.CurrentAirline)
                .Include(x => x.CurrentGate)
                .Include(x => x.CurrentRunway)
                .SingleOrDefault(flight => flight.FlightNr.Equals(nr));
        }

        public List<Flight> GetFlightsByDate(DateTime date, int range)
        {
            DateTime earliest = date;
            earliest.AddDays(-range);
            var query = from flight in _dbContext.Flights
                        where flight.Departure >= earliest
                        select flight;
            return query
                .Include(x => x.CurrentPlane)
                .Include(x => x.CurrentPlane.CurrentAirline)
                .ToList();
        }

        public List<Flight> GetFlights()
        {
            return _dbContext.Flights
                .AsNoTracking()
                .Include(x => x.CurrentPlane)
                .Include(x => x.CurrentPlane.CurrentAirline)
                .Include(x => x.CurrentGate)
                .Include(x => x.CurrentRunway)
                .ToList();
        }

        public Plane GetPlaneByNr(string nr)
        {
            return _dbContext.Planes.SingleOrDefault(plane => plane.PlaneNr == nr);
        }

        public List<Plane> GetPlanes()
        {
            return _dbContext.Planes.AsNoTracking().Include(x => x.CurrentAirline).ToList();
        }
        public List<Ticket> GetTicketsByFlight(string flightnr)
        {
            var query = from ticket in _dbContext.Tickets
                        where ticket.CurrentFlight.FlightNr == flightnr
                        select ticket;
            return query
                .Include(x => x.CurrentFlight)
                .Include(x => x.CurrentUser)
                .ToList();

        }

        public void RemoveAirline(Airline airline)
        {
            _dbContext.Airlines.Remove(airline);
            _dbContext.SaveChanges();
        }

        public List<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }

        //public List<Ticket> GetTicketByFlight(Flight flight)
        //{
        //    var query = from ticket in _dbContext.Tickets
        //                where ticket.CurrentFlight == flight
        //                select ticket;
        //    return query.ToList();
        //}

        public Ticket GetTicketByNr(string nr)
        {
            var query = from ticket in _dbContext.Tickets
                        where ticket.TicketNr == nr
                        select ticket;
            return query.Include(x => x.CurrentUser).Include(x => x.CurrentFlight).FirstOrDefault();
        }

        public List<Ticket> GetTickets()
        {
            return _dbContext.Tickets.Include(x => x.CurrentUser).Include(x => x.CurrentFlight).ToList();
        }

        public List<Ticket> GetTicketsByUser(User user)
        {
            var query = from ticket in _dbContext.Tickets
                        where ticket.CurrentUser == user
                        select ticket;
            return query
                .Include(x => x.CurrentUser)
                .Include(x => x.CurrentFlight)
                .Include(x => x.CurrentFlight.CurrentPlane)
                .Include(x => x.CurrentFlight.CurrentPlane.CurrentAirline)
                .ToList();
        }

        public void RemoveFlight(Flight flight)
        {
            _dbContext.Flights.Remove(flight);
            _dbContext.SaveChanges();
        }

        public void RemovePlane(Plane plane)
        {
            _dbContext.Planes.Remove(_dbContext.Planes.FirstOrDefault(x => x.PlaneNr == plane.PlaneNr));
            _dbContext.SaveChanges();
        }

        public void UpdateAirline(Airline airline)
        {

            _dbContext.Airlines.Remove(_dbContext.Airlines.FirstOrDefault(x => x.AirlineID == airline.AirlineID));
            _dbContext.Airlines.Add(airline);
            _dbContext.SaveChanges();
        }

        public void RemoveUser(User user)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public int RemoveTicket(Ticket ticket)
        {
            _dbContext.Tickets.Remove(ticket);
            return _dbContext.SaveChanges();
        }

        public void UpdateFlight(Flight flight)
        {
            _dbContext.Flights.Update(flight);
            _dbContext.SaveChanges();
        }

        public void UpdatePlane(Plane plane)
        {
            _dbContext.Planes.Remove(_dbContext.Planes.FirstOrDefault(x => x.PlaneID == plane.PlaneID));
            _dbContext.Planes.Add(plane);
            _dbContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }

        public int UpdateTicket(Ticket ticket)
        {
            _dbContext.Tickets.Update(ticket);
            return _dbContext.SaveChanges();
        }

        public List<Gate> GetGates()
        {
            return _dbContext.Gates.ToList();
        }

        public void AddGate(Gate gate)
        {
            throw new NotImplementedException();
        }

        public List<Runway> GetRunways()
        {
            return _dbContext.Runways.ToList();
        }

        public void AddRunway(Runway runway)
        {
            throw new NotImplementedException();
        }

        public Gate GetGateByNr(int nr)
        {
            return _dbContext.Gates
                .SingleOrDefault(gate => gate.Number.Equals(nr));
        }

        public Runway GetRunwayByNr(int nr)
        {
            return _dbContext.Runways
               .SingleOrDefault(runway => runway.Number.Equals(nr));
        }
    } 
}
