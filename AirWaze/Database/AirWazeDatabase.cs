﻿using AirWaze.Database.Design;
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
            _dbContext.SaveChanges();
        }

        public void AddFlight(Flight flight)
        {
            _dbContext.Flights.Add(flight);
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
            return _dbContext.SaveChanges();
        }

        public Flight GetFlightByNr(string nr)
        {
            return _dbContext.Flights
                .Include(x => x.CurrentPlane)
                .Include(x => x.CurrentPlane.CurrentAirline)
                .SingleOrDefault(flight => flight.FlightNr.Equals(nr));
        }

        public List<Flight> GetFlights()
        {
            return _dbContext.Flights
                .Include(x => x.CurrentPlane)
                .Include(x => x.CurrentPlane.CurrentAirline)
                .ToList();
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
            throw new NotImplementedException();
        }

        public Gate GetGateByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public void AddGate(Gate flight)
        {
            throw new NotImplementedException();
        }

        public List<Runway> GetRunways()
        {
            throw new NotImplementedException();
        }

        public Runway GetRuwaysByID(string nr)
        {
            throw new NotImplementedException();
        }

        public void AddRunway(Runway runway)
        {
            throw new NotImplementedException();
        }
    }
}
