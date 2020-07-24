﻿using Sopro.CustomValidationAttributes;
using Sopro.Interfaces;
using Sopro.Interfaces.ControllerSimulation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sopro.Models.Simulation
{
    public class Scenario : IScenario
    {
        public string id { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int duration { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int bookingCountPerDay { get; set; }
        [Required]
        [ListMinLength(1)]
        public List<Vehicle> vehicles { get; set; }
        [Required]
        public List<Rushhour> rushhours { get; set; }
        public DateTime start { get; set; }
        [Required]
        public ILocation location { get; set; }

        public Scenario()
        {
            id = Guid.NewGuid().ToString();
        }

        public bool addVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return false;
            }
            vehicles.Add(vehicle);
            return true;
        }
        public bool deleteVehicle(Vehicle vehicle)
        {
            if (!vehicles.Any())
                return false;
            if (!vehicles.Contains(vehicle))
                return false;
            vehicles.Remove(vehicle);
            return true;
        }

        public bool addRushhour(Rushhour rushhour)
        {
            if (rushhour == null)
                return false;
            if (rushhours.Contains(rushhour))
                return false;
            rushhours.Add(rushhour);
            return true;
        }

        public bool deleteRushhour(Rushhour rushhour)
        {
            if (!rushhours.Any())
                return false;
            if (!rushhours.Contains(rushhour))
                return false;
            rushhours.Remove(rushhour);
            return true;
        }
    }
}