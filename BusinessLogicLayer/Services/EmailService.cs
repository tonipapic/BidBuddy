using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{   
    /// <summary>
    /// <remarks>Toni Papić</remarks>
    /// </summary>
    public class EmailService
    {
        private string userEmail=AuthenticationService.LoggedUser.Email;
        private SmtpClient smtpclient;
        public EmailService()
        {
            smtpclient = new SmtpClient("smtp.gmail.com", 587)
            {

                Credentials = new NetworkCredential("testrwakolegij@gmail.com", "mtvx wpvu iksv zdjp"),
                EnableSsl = true,
            };
        }


        /// <summary>
        /// Sends email to any email address with given subject and body
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void SendEmail(string to = "", string subject = "Uspješno dodana ponuda", string body = "")
        {
            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress("testrwakolegij@gmail.com"),
                Subject = subject,
                Body = body,
                To = { new MailAddress(to == "" ? userEmail:to) },
                IsBodyHtml = true
            };
            smtpclient.Send(mailMessage);
        }




    }
}