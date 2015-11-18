using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using _5.OsteoYoga.Exception.Interfaces;

namespace _5.OsteoYoga.Exception.Implements
{
    public class Logger :  ILogger
    {
        ILog Log { get; }
        int Level { get; }

        public Logger(Type origin, bool direct)
        {
            // Si on est en appel direct on est au niveau 2 de la stack trace sinon on passe par l'exception handler et on est au niveau 3
            Level = direct ? 2 : 3;
            Log = LogManager.GetLogger(origin);
        }
        public void Error(string message, System.Exception exception)
        {
            Log.Error($"Method -> \"{GetCalledMethod()}\" : \"{message}\"", exception);
        }
        public void Info(string message)
        {
            Log.Info($"Method -> \"{GetCalledMethod()}\" : \"{message}\"");
        }
        public void Warning(string message, System.Exception exception)
        {
            Log.Warn($"Method -> \"{GetCalledMethod()}\" : \"{message}\"", exception);
        }
        public void Fatal(string message, System.Exception exception)
        {
            Log.Fatal($"Method -> \"{GetCalledMethod()}\" : \"{message}\"", exception);
        }
        public void Debug(string message)
        {
            Log.Debug($"Method -> \"{GetCalledMethod()}\" : \"{message}\"");
        }
        public void Error(string message, System.Exception exception, string method)
        {
            Log.Error($"Method -> \"{method}\" : \"{message}\"", exception);
        }
        public void Info(string message, string method)
        {
            Log.Info($"Method -> \"{method}\" : \"{message}\"");
        }
        public void Warning(string message, System.Exception exception, string method)
        {
            Log.Warn($"Method -> \"{method}\" : \"{message}\"", exception);
        }
        public void Fatal(string message, System.Exception exception, string method)
        {
            Log.Fatal($"Method -> \"{method}\" : \"{message}\"", exception);
        }
        public void Debug(string message, string method)
        {
            Log.Debug($"Method -> \"{method}\" : \"{message}\"");
        }
        private string GetCalledMethod()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();
            string methodName = string.Empty;
            if (stackFrames != null && stackFrames.Count() > Level)
            {
                StackFrame frame = stackFrames[Level];
                MethodBase method = frame.GetMethod();
                methodName = method.Name;
            }
            return methodName;
        }
    }
}
