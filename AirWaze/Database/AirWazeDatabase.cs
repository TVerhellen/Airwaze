﻿using AirWaze.Database.Design;
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
            throw new NotImplementedException();
        }

        public void AddPlane(Plane plane)
        {
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

        public Flight GetFlightByNr(string nr)
        {
            throw new NotImplementedException();
        }

        public List<Flight> GetFlights()
        {
            throw new NotImplementedException();
        }

        public Plane GetPlaneByNr(string nr)
        {
            return _dbContext.Planes.SingleOrDefault(plane => plane.PlaneNr == nr);
        }

        public List<Plane> GetPlanes()
        {
            return _dbContext.Planes.ToList();
        }

        public void RemoveAirline(Airline airline)
        {
            _dbContext.Airlines.Remove(_dbContext.Airlines.FirstOrDefault(x => x.AirlineID == airline.AirlineID));           
            _dbContext.SaveChanges();
        }

        public void RemoveFlight(Flight flight)
        {
            throw new NotImplementedException();
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

        public void UpdateFlight(Flight flight)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlane(Plane plane)
        {
            _dbContext.Planes.Update(plane);
            _dbContext.SaveChanges();
        }
    }
}
