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
        public List<Booking> bookinglist { get; set; }
        [MaxFileSize(1 * 1024 * 1024, ErrorMessage ="test")]
        [AllowedExtensions(new String[] { ".json"}, ErrorMessage = "Wrong extension")]
        public IFormFile importedBookings { set; get; }
    }
}
