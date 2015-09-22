using System.Net;
using System.Net.Mail;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Helper
{
    public class Email
    {
        readonly MailAddress fromAddress = new MailAddress("quentin.truffart@gmail.com", "Administration osteoyoga.fr");
        private const string Subject = "Demande de rendez-vous au ";
        private static Email _instance;
        private const string SmtpOsteoyogaFr = "smtp.osteoyoga.fr";
        private const string FormatDate = "dd/MM/yyyy";
        private const int Port = 587;
        private bool ssl = true;

        public static Email Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public static Email GetInstance()
        {
            return _instance ?? (_instance = new Email());
        }

        //public virtual void SendForPatientPropose(Dates date, string serverAddress)
        //{
        //    var toAddress = new MailAddress(date.Patient.Mail, date.Patient.FullName);

        //    string body = string.Format("<html><head></head><body>Bonjour, <br /><br />Votre demande a été proposée. Vous devez cliquer sur le lien suivant pour valider votre rendez-vous : <br /><a href='{0}/RendezVous/Validate?id={1}'>Lien Ici !</a> <br /><br />" + "Resumé du rendez-vous : " + "Dates : {2}<br />" + "Horaire : {3} <br />" + "Patient : {4} <br /><br /> Cordialement,<br />Nicolas Truffart</body></html>", serverAddress, date.ConfirmationId, date.Day.ToString("dd/MM/yyyy"), date.WorkTimeSlot, date.Patient);

        //    var smtp = new SmtpClient
        //    {
        //        Host = SmtpOsteoyogaFr,
        //        Port = Port,
        //        EnableSsl = ssl,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromAddress.Address, Constants.GetInstance().PassMail)
        //    };
        //    using (var message = new MailMessage(fromAddress, toAddress)
        //    {
        //        Subject = Subject + date.Day.ToString(FormatDate),
        //        Body = body,
        //        IsBodyHtml = true
        //    })
        //    {
        //        smtp.Send(message);
        //    }
        //}

        //public virtual void SendForPatientValidation(Dates date)
        //{
        //    var toAddress = new MailAddress(date.Patient.Mail, date.Patient.FullName);

        //    string body = string.Format("<html><head></head><body>Bonjour, <br /><br />Votre demande de rendez-vous a été validée. <br /><br />" + "Resumé du rendez-vous : " + "Dates : {0}<br />" + "Horaire : {1} <br />" + "Patient : {2} <br /><br /> Cordialement,<br />Nicolas Truffart</body></html>", date.Day.ToString("dd/MM/yyyy"), date.WorkTimeSlot, date.Patient);

        //    var smtp = new SmtpClient
        //    {
        //        Host = SmtpOsteoyogaFr,
        //        Port = Port,
        //        EnableSsl = ssl,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromAddress.Address, Constants.GetInstance().PassMail)
        //    };
        //    using (var message = new MailMessage(fromAddress, toAddress)
        //    {
        //        Subject = Subject + date.Day.ToString(FormatDate),
        //        Body = body,
        //        IsBodyHtml = true
        //    })
        //    {
        //        smtp.Send(message);
        //    }
        //}

        //public virtual void SendForAdminPropose(Dates date)
        //{
        //    var toAddress = new MailAddress(Constants.GetInstance().MailNico, Constants.GetInstance().NomNico);

        //    string body = string.Format("<html><head></head><body>Bonjour, <br /><br />Une demande de rendez-vous a été proposée.<br /> En attente de validation du patient <br /><br />" + "Resumé du rendez-vous : " + "Dates : {0}<br />" + "Horaire : {1} <br />" + "Patient : {2} </body></html>", date.Day.ToString("dd/MM/yyyy"), date.WorkTimeSlot, date.Patient);

        //    var smtp = new SmtpClient
        //    {
        //        Host = SmtpOsteoyogaFr,
        //        Port = Port,
        //        EnableSsl = ssl,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromAddress.Address, Constants.GetInstance().PassMail)
        //    };
        //    using (var message = new MailMessage(fromAddress, toAddress)
        //    {
        //        Subject = Subject + date.Day.ToString(FormatDate),
        //        Body = body,
        //        IsBodyHtml = true
        //    })
        //    {
        //        smtp.Send(message);
        //    }
        //}

        //public virtual void SendForAdminValidation(Dates date)
        //{
        //    var toAddress = new MailAddress(Constants.GetInstance().MailNico, Constants.GetInstance().NomNico);
            

        //    string body = string.Format("<html><head></head><body>Bonjour, <br /><br />La demande de rendez-vous a été validée <br /><br />" + "Resumé du rendez-vous : " + "Dates : {0}<br />" + "Horaire : {1} <br />" + "Patient : {2}</body></html>", date.Day.ToString("dd/MM/yyyy"), date.WorkTimeSlot, date.Patient);

        //    var smtp = new SmtpClient
        //    {
        //        Host = SmtpOsteoyogaFr,
        //        Port = Port,
        //        EnableSsl = ssl,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromAddress.Address, Constants.GetInstance().PassMail)
        //    };
        //    using (var message = new MailMessage(fromAddress, toAddress)
        //    {
        //        Subject = Subject + date.Day.ToString(FormatDate),
        //        Body = body,
        //        IsBodyHtml = true
        //    })
        //    {
        //        smtp.Send(message);
        //    }
        //}
    }
}
