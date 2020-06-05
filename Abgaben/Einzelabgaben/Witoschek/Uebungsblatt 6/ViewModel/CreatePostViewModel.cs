using Blatt03.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blatt03.ViewModel
{
    public class CreatePostViewModel
    {
        public List<Booking> bookings { get; set; }
        public IFormFile importedBookings { set; get; }
    }
}
