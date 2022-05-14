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
        public void AddAirline(Airline airline)
        {
            _dbContext.Airlines.Add(airline);
            _dbContext.SaveChanges();
        }

        public void AddFlight(Flight flight)
        {
            _dbContext.Flights.Add(flight);
            _dbContext.SaveChanges();
        }

        public void AddPlane(Plane plane)
        {
            ;
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

        public Flight GetFlightByNr(string nr)
        {
            return _dbContext.Flights.SingleOrDefault(flight => flight.FlightNr.Equals(nr));
        }

        public List<Flight> GetFlights()
        {
            return _dbContext.Flights.ToList();
        }

        public Plane GetPlaneByNr(string nr)
        {
            return _dbContext.Planes.SingleOrDefault(plane => plane.PlaneNr == nr);
        }

        public List<Plane> GetPlanes()
        {
            return _dbContext.Planes.ToList();
        }
        public List<Ticket> GetTicketsByFlight(string flightnr)
        {

            List<Ticket> ticketList = new List<Ticket>();
            return ticketList;
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
          
            _dbContext.Airlines.Update(airline);
            _dbContext.SaveChanges();
        }

        public void RemoveUser(User user)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public void UpdateFlight(Flight flight)
        {
            _dbContext.Flights.Update(flight);
            _dbContext.SaveChanges();
        }

        public void UpdatePlane(Plane plane)
        {


            _dbContext.Airlines.Update(plane.CurrentAirline);
            _dbContext.Planes.Update(plane);
            _dbContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }
    }
}
