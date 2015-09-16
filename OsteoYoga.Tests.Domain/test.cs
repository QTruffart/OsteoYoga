using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OsteoYoga.Tests.Domain
{
    [TestClass]
    public class Tests
    {

        static readonly string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Calendar API Quickstart";

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            UserCredential credential;

            using (var stream = new FileStream("client_secret.json", FileMode.Open,FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                   GoogleClientSecrets.Load(stream).Secrets,
                  Scopes,
                  "user",
                  CancellationToken.None,
                  new FileDataStore(credPath, true)).Result;

                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Calendar Service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            DateTime begin = DateTime.Now.Add(new TimeSpan(1, 1, 0));
            DateTime end = DateTime.Now.Add(new TimeSpan(1, 1, 0));
            EventDateTime beginEventDate = new EventDateTime() { DateTime = begin };
            EventDateTime endEventDate = new EventDateTime() { DateTime = end };
            Event event1 = new Event
            {
                Description = "TEST QUT2 " + DateTime.Now.ToLongTimeString(),
                Summary = "TEST QUT2 " + DateTime.Now.ToLongTimeString(),
                Created = DateTime.Now,
                Start = beginEventDate,
                End = endEventDate
            };

            EventsResource.InsertRequest request = service.Events.Insert(event1, "primary");
            request.Execute();
        }
    }
}