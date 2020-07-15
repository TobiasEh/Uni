using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.Interfaces.AdministrationController;
using Sopro.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sopro.Persistence.PersBooking
{
    public class BookingRepository : IBookingRepository
    {
        public List<IBooking> import()
        {
            return new List<IBooking>();
        }

        public void export(List<IBooking> list)
        {
           
        }
    }
}
