using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Sopro.Interfaces.AdministrationSimulation;
using System.Data;
using Org.BouncyCastle.Utilities;

namespace Sopro.Models.Simulation
{
    public class Generator : IGenerator
    {
        [Range(0,1)]
        public double probabilityVIP { get; set; }
        [Range(0, 1)]
        public double probabilityGUEST { get; set; }

        public Generator()
        {
            probabilityGUEST = 0.05;
            probabilityVIP = 0.05;
        }
        public List<Booking> generateBookings(Scenario scenario) 
        {
            
            //TimeSpan Simulator.tickLength = new TimeSpan(0,0,1,0);

            Console.WriteLine("duration in days : {0}", (scenario.duration * Simulator.tickLength.TotalSeconds) / (24 * 60 * 60));

            if ((scenario.duration * Simulator.tickLength.TotalSeconds) / (24 * 60 * 60) < 1) return nggguu(scenario);

            double totSeconds = scenario.duration * Simulator.tickLength.TotalSeconds;
            double totDays = totSeconds / (24*60*60);    
            int duration = (int)totDays;
            double leftover = totDays % 1;
            List<Booking> bookingList = new List<Booking>();
            for(int i = 0; i < duration; i++) //days
            {
               bool exists = false;
                if (scenario.rushhours.Count != 0)
                {
                    int bpdC = scenario.bookingCountPerDay;
                    DateTime minRHstart = new DateTime();
                    foreach (Rushhour rh in scenario.rushhours.Where(x => x.start.Day == scenario.start.AddDays(i).Day))
                    {
                        exists = true;
                        minRHstart = minRHstart == new DateTime() ? rh.start : minRHstart > rh.start ? rh.start : minRHstart;
                        var startTimes = rh.run();
                        if (rh.bookings > bpdC)
                        {
                            startTimes.RemoveRange(bpdC - 1 < 0 ? 0 : bpdC - 1, rh.run().Count -bpdC);
                            
                        }
                        if (startTimes.Count != 0)
                        foreach (DateTime start in startTimes)
                        {
                            int r = new Random().Next(scenario.vehicles.Count);
                            bookingList.Add(
                                new Booking
                                {
                                    capacity = scenario.vehicles[r].capacity,
                                    plugs = scenario.vehicles[r].plugs,
                                    socEnd = scenario.vehicles[r].socEnd,
                                    socStart = scenario.vehicles[r].socStart,
                                    user = "megarandombookinggeneratorduud",
                                    startTime = start,
                                    endTime = start.AddHours(new Random().Next(1, 8)),
                                    station = null,
                                    active = false,
                                    priority = setPrio(),
                                    location = scenario.location
                                }
                                ); ;
                            bpdC--;
                        };
                    }
                    if (bpdC != 0 && exists == true)
                    {
                        List<Booking> linBList = new List<Booking>(); ;
                        LinearDist(linBList, scenario, i);
                        linBList.RemoveRange(linBList.FindIndex(x => x.startTime >= minRHstart), scenario.bookingCountPerDay-bpdC);
                        bookingList.AddRange(linBList);
                    }
                    if (exists == false) LinearDist(bookingList, scenario, i);
                        

                }
                else
                {
                    LinearDist(bookingList,scenario,i);
                }
            }
            if (leftover != 0 && leftover * 24 * 60 >= (24 * 60) / scenario.bookingCountPerDay)
            {
                bookingList.AddRange(nggguu(scenario));
            }    

            return bookingList;
        }

        private List<Booking> nggguu(Scenario scenario)
        {
            List<Booking> bookingList = new List<Booking>();
            
            TimeSpan Simulator.tickLength = new TimeSpan(0, 0, 1);
            bool exists = false;
            double leftover = (scenario.duration * Simulator.tickLength.TotalSeconds / 24 * 60 * 60) % 1;
            int bpdC = (int)(scenario.bookingCountPerDay * leftover);
            //if (bpdC == 0) return bookingList;
            if (scenario.rushhours.FindAll(x => x.start >= scenario.start && x.start <= scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds)).Count != 0)
            {
                Console.WriteLine("true");
                DateTime minRHstart = new DateTime();
                foreach (Rushhour rh in scenario.rushhours.Where(x => x.start >= scenario.start.AddDays((int)(scenario.duration * Simulator.tickLength.TotalSeconds / 24 * 60 * 60)) && x.start <= scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds)))
                {
                    exists = true;
                    minRHstart = minRHstart == new DateTime() ? rh.start : minRHstart > rh.start ? rh.start : minRHstart;
                    List<DateTime> startTimes = rh.run();
                    if (rh.end > scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds))
                    {
                        startTimes.RemoveAll(x => x > scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds));
                    }
                    if (startTimes.Count != 0)
                        foreach (DateTime start in startTimes)
                        {
                            int r = new Random().Next(scenario.vehicles.Count);
                            bookingList.Add(
                                new Booking
                                {
                                    capacity = scenario.vehicles[r].capacity,
                                    plugs = scenario.vehicles[r].plugs,
                                    socEnd = scenario.vehicles[r].socEnd,
                                    socStart = scenario.vehicles[r].socStart,
                                    user = "megarandombookinggeneratorduud",
                                    startTime = start,
                                    endTime = start.AddHours(new Random().Next(1, 8)),
                                    station = null,
                                    active = false,
                                    priority = setPrio(),
                                    location = scenario.location
                                }
                                ); ;
                            bpdC--;
                            if (bpdC <= 0) break;
                        };
                    if (bpdC <= 0) break;
                }
                if (bpdC < 0)
                {
                    List<Booking> linBList = new List<Booking>(); ;
                    LinearDist(linBList, scenario, 0);
                    linBList.RemoveRange(linBList.FindIndex(x => x.startTime >= minRHstart), scenario.bookingCountPerDay - Math.Abs(bpdC));
                    bookingList.AddRange(linBList);
                }
            }
            else
            {
                LinearDist(bookingList, scenario, 0);
                bookingList.RemoveAll(x => x.startTime > scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds) && x.startTime < scenario.start.AddDays((int)(scenario.duration * Simulator.tickLength.TotalSeconds / 24 * 60 * 60)));
                bookingList.RemoveRange(bookingList.Count-1,bookingList.Count-bpdC);
            }
            

            return bookingList;
        }

        private UserType setPrio()
        {
            double prob = new Random().NextDouble();
            if (prob > probabilityVIP + probabilityGUEST)
            {
                return UserType.EMPLOYEE;
            }
            else
            {
                if (prob > probabilityGUEST)
                {
                    return UserType.VIP;
                }
                else
                {
                    return UserType.GUEST;
                }
            }
        }

        public void LinearDist(List<Booking> bookingList, Scenario scenario, int i)
        {
            int r = new Random().Next(scenario.vehicles.Count);

            for (int j = 0; j < scenario.bookingCountPerDay; j++)
            {
                bookingList.Add(new Booking
                {
                    capacity = scenario.vehicles[r].capacity,
                    plugs = scenario.vehicles[r].plugs,
                    socEnd = scenario.vehicles[r].socEnd,
                    socStart = scenario.vehicles[r].socStart,
                    user = "",
                    startTime = scenario.start.AddDays(i).AddMinutes(((24 * 60) / scenario.bookingCountPerDay)*j),
                    endTime = scenario.start.AddDays(i).AddHours(j).AddHours(new Random().Next(1, 8)),
                    station = null,
                    active = false,
                    priority = setPrio(),
                    location = scenario.location
                }
                );
         
            }
        }
        

    }
}
