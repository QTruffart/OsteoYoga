
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using OsteoYoga.Domain.Models;
using OsteoYoga.Domain.Models.Interface;
using OsteoYoga.Helper.Helpers.Interfaces;
using RazorEngine;

namespace OsteoYoga.Helper.Helpers.Abstracts
{
    public class BaseMailHelper : IMailHelper
    {
        public virtual string TemplatePath { get; }
        public virtual string Subject { get; }

        protected string FileToString()
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(TemplatePath)))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }


        public void SendMail(IMailModel model)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.Subject = Subject;
                mail.IsBodyHtml = true;

                mail.Body = Razor.Parse(FileToString(), model);

                mail.To.Add(new MailAddress(((Contact)model).Mail));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["MailSender"]);

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(mail.Body, null,
                    MediaTypeNames.Text.Html);
                mail.AlternateViews.Add(avHtml);

                using (
                    var smtp = new SmtpClient(ConfigurationManager.AppSettings["MailServer"],
                        int.Parse(ConfigurationManager.AppSettings["MailPort"])))
                {
                    smtp.Send(mail);
                }
            }
        }
    }
}
