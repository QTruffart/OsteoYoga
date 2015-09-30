using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO.Abstracts
{
    public class GoogleRepository : IGoogleRepository<Event>
    {
        private CalendarService service;
        static readonly string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Calendar API Quickstart";

        public virtual CalendarService Service
        {
            get
            {
                if (service == null)
                {
                    UserCredential credential;

                    var assembly = Assembly.GetExecutingAssembly();
                   
                    using (var resStream = assembly.GetManifestResourceStream("OsteoYoga.Repository.DAO.Json.client_secret.json"))
                    {
                        byte[] buffer = new byte[resStream.Length];
                        resStream.Read(buffer, 0, buffer.Length);
                        // TODO: use the buffer that was read
                        using (var stream = new MemoryStream(buffer))
                        {
                            string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                            credPath = Path.Combine(credPath, ".credentials");

                            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                               GoogleClientSecrets.Load(stream).Secrets,
                              Scopes,
                              "user",
                              CancellationToken.None,
                              new FileDataStore(credPath, true)).Result;
                        }

                        // Create Calendar Service.
                        service = new CalendarService(new BaseClientService.Initializer()
                        {
                            HttpClientInitializer = credential,
                            ApplicationName = ApplicationName,
                        });
                        
                    }
                }
                return service;
            }
            set { service = value; }
        }


        public virtual Event Save(Date date, string summary, string description, PratictionerOffice office)
        {
            Event.RemindersData remaRemindersData = new Event.RemindersData
            {
                Overrides = new List<EventReminder> {
                    new EventReminder
                    {
                        Method = "email",
                        Minutes = office.Reminder
                    },
                     new EventReminder
                    {
                        Method = "popup",
                        Minutes = office.Reminder
                    }
                },
                UseDefault = false
            };

            Event eventToAdd = new Event
            {
                Description = description,
                Summary = summary,
                Created = DateTime.Now,
                Start = new EventDateTime { DateTime = date.BeginTime },
                End = new EventDateTime { DateTime = date.BeginTime.AddMinutes(date.Duration.Value) },
                Reminders = remaRemindersData,
                Location = date.Office.Adress,
                Attendees = new List<EventAttendee> { new EventAttendee() { Email = date.Patient.Mail, DisplayName = date.Patient.FullName, ResponseStatus = "needsAction" } },
            };
            return Service.Events.Insert(eventToAdd, "primary").Execute();
        }

        public Event Update(string eventId, Date date, string summary, string description, PratictionerOffice pratictionerOffice)
        {
            Event toUpdate = Service.Events.Get("primary", eventId).Execute();
            EventDateTime beginTime = new EventDateTime {DateTime = date.BeginTime};
            EventDateTime endTime = new EventDateTime {DateTime = date.BeginTime.AddMinutes(date.Duration.Value) };
            toUpdate.Start = beginTime;
            toUpdate.End = endTime;
            toUpdate.Summary = summary;
            toUpdate.Description = description;
            toUpdate.Location = date.Office.Adress;
            toUpdate.Attendees = new List<EventAttendee> { new EventAttendee() { Email = date.Patient.Mail, DisplayName = date.Patient.FullName } };

            return Service.Events.Update(toUpdate, "primary", eventId).Execute();
        }

        public virtual void Delete(string toDelete)
        {
            Service.Events.Delete("primary", toDelete).Execute();
        }

        public virtual Event GetById(string id)
        {
            return Service.Events.Get("primary", id).Execute();
        }

        public virtual IList<Event> GetAll()
        {
            return  Service.Events.List("primary").Execute().Items;
        }

        public IList<Event> GetAllForPractionerInterval(PratictionerOffice pratictionerOffice)
        {
            EventsResource.ListRequest request = Service.Events.List("primary");
            
            request.TimeMin = pratictionerOffice.MinDateInterval;
            request.TimeMax = pratictionerOffice.MaxDateInterval;
            
            return request.Execute().Items;
        }
    }
}
