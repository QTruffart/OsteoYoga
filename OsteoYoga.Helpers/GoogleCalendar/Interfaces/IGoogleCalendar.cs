using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;

namespace OsteoYoga.Helper.GoogleCalendar.Interfaces
{
    public interface IGoogleCalendar
    {
        CalendarService Authenticate();
    }
}
 