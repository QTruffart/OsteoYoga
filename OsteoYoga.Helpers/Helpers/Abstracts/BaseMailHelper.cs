
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
                string contentID = "contentID";
                mail.Subject = Subject;
                mail.IsBodyHtml = true;

                mail.Body = Razor.Parse(FileToString(), model);

                mail.To.Add(new MailAddress(((Contact)model).Mail));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["MailSender"]);

                AlternateView View = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");
                LinkedResource resource;
               
                resource = new LinkedResource(@"./FileResources/Template/Image/logo.png", "image/png");
                resource.ContentId = item.ImagePlaceHolde;
                View.LinkedResources.Add(resource);
                string ImageTag = "<img src=cid:" + item.ImagePlaceHolde + "width='" + item.width + "' and height='" + item.height + "px'/></p>";
                mail.Body = mail.Body.Replace(item.ImagePlaceHolde, ImageTag);
                mail.AlternateViews.Add(View);
                }

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Body, null, MediaTypeNames.Text.Html);

                LinkedResource imagelink = new LinkedResource(@"./FileResources/Template/Image/logo.png", "image/png");


                imagelink.TransferEncoding = TransferEncoding.Base64;
                htmlView.LinkedResources.Add(imagelink);

                mail.AlternateViews.Add(htmlView);
                using (
                    var smtp = new SmtpClient(ConfigurationManager.AppSettings["MailServer"],
                        int.Parse(ConfigurationManager.AppSettings["MailPort"])))
                {
                    smtp.Send(mail);
                }
            }
        }

        public class EmbedImages
        {
            public EmbedImages(string _ImagePlaceHolde, string _ImagePath, string _width, string _height)
            {
                this.ImagePlaceHolde = _ImagePlaceHolde;
                this.ImagePath = _ImagePath;
                this.width = _width;
                this.height = _height;
            }
            public string ImagePlaceHolde { get; set; }
            public string ImagePath { get; set; }
            public string width { get; set; }
            public string height { get; set; }
        }

    }
}
