using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using OsteoYoga.Helper.GoogleCalendar.Interfaces;

namespace OsteoYoga.Helper.GoogleCalendar.Implements
{
    public class GoogleCalendar : IGoogleCalendar
    {
        const string ClientSecretJson = "client_secret.json";
        readonly string[] scopes = { CalendarService.Scope.Calendar };
        string user;
        const string ApplicationName = "OsteoYoga";

        public CalendarService Authenticate()
        {
            UserCredential credential;
            using (var stream = new FileStream(ClientSecretJson, FileMode.Open,
               FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials");

                user = "user";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
               GoogleClientSecrets.Load(stream).Secrets,
              scopes,
              user,
              CancellationToken.None,
              new FileDataStore(credPath, true)).Result;

            }

            // Create Calendar Service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }

    }
}
