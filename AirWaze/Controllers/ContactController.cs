using AirWaze.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AirWaze.Controllers
{
    public class ContactController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Message = "Contact form";

            return View();
        }

        [HttpPost]
        public ActionResult Index(ContactViewModel e)
        {
            if (ModelState.IsValid)
            {

                StringBuilder message = new StringBuilder();
                MailAddress from = new MailAddress(e.Email.ToString());
                message.Append("Name: " + e.Name + "\n");
                message.Append("Email: " + e.Email + "\n");
                message.Append("Subject: " + e.Subject + "\n\n");
                message.Append(e.Message);

                MailMessage mail = new MailMessage();

                SmtpClient smtp = new SmtpClient();

                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;

                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("contact.airwaze@gmail.com", "Admin123?123");

                smtp.Credentials = credentials;
                smtp.EnableSsl = true;

                mail.From = from;
                mail.To.Add("contact.airwaze@gmail.com");
                mail.Subject = "Contact from " + e.Name;
                mail.Body = message.ToString();

                smtp.Send(mail);
            }
            return View(e);
        }       

    }
}

