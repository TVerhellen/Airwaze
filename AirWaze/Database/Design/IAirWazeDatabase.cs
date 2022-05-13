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

        List<User> GetUsers();

        User FindUserByID(Guid id);

        void AddUser(User user);

        void UpdateUser(User user);

        void RemoveUser(User user);
    }
}
