using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.Models.Communication;

namespace Sopro.Controllers
{
    public class CommunicationController : Controller
    {
        private List<Message> messages = Messenger.messages;

        public IActionResult Index()
        {
            var email = this.HttpContext.Session.GetString("email");
            List<Message> userMessage = new List<Message>();
            foreach (Message item in messages)
                if (item.email == email)
                {
                    userMessage.Add(item);
                }
            if (userMessage.Exists(e => e.read == false)) {
                HttpContext.Session.SetInt32("message", 1);
            }
            return View(userMessage);
        }

        public IActionResult Read(string id)
        {
            Message message = messages.Find(e => e.id.Equals(id));
            int index = messages.IndexOf(message);
            messages.RemoveAt(index);


            message.read = true;
            messages.Insert(index, message);
            HttpContext.Session.SetInt32("message", 0);
            
            return RedirectToAction("Index");
        }
    }
}
