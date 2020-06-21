﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class BookingController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        List<Booking> bookings = new List<Booking>();

        public BookingController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public BookingController()
        { 
        }

        public IActionResult Index()
        {
            var cacheKey = "bookings";
            _memoryCache.TryGetValue(cacheKey, out bookings);

            return View(bookings);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Booking());
        }

        [HttpPost]
        public IActionResult Post(Booking booking)
        {
            var cacheKey = "bookings";

            if (!_memoryCache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<Booking>();
            }

            bookings.Add(booking);
            _memoryCache.Set(cacheKey, bookings);
            return View("Index", bookings);
        }
        public IActionResult Auswertung()
        {
            var cacheKey = "bookings";
            if (!_memoryCache.TryGetValue(cacheKey, out bookings))
            {
                return View();
            }

            List<ConnectorTypeEvaluationViewModel> evaluationData = new List<ConnectorTypeEvaluationViewModel>();

            int[] plugs = new int[Enum.GetNames(typeof(ConnectorType)).Length];
            foreach (Booking b in bookings)
            {
                plugs[(int)(b.connectorType)]++;
            }

            int total = plugs.Sum();

            foreach (ConnectorType c in Enum.GetValues(typeof(ConnectorType)))
            {
                evaluationData.Add(new ConnectorTypeEvaluationViewModel()
                {
                    connectorType = c,
                    percentageBooking = calcpercentage(plugs, c, total)
                }); ;
            } 
            

            return View(evaluationData);
        }
        public double calcpercentage(int[] plugs, ConnectorType c, int total)
        {
            //return Math.Round((double)plugs[(int)c] / (double)total * 100, 2);
            return onlycalc(plugs[(int)c], total);
        }

        public double onlycalc(int plugs, int total)
        {
            return Math.Round((double)plugs / (double)total * 100, 2);
        }
        
    }

}
