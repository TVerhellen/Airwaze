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
            throw new NotImplementedException();
        }

        public List<Flight> GetFlights()
        {
            throw new NotImplementedException();
        }

        public List<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }

        public void RemoveFlight(Flight flight)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(User user)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public void UpdateFlight(Flight flight)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }
    }
}
