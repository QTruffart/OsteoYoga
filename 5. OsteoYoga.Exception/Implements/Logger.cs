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
        ILog Log { get; set; }
        int Level { get; set; }

        public Logger(Type origin, bool direct)
        {
            // Si on est en appel direct on est au niveau 2 de la stack trace sinon on passe par l'exception handler et on est au niveau 3
            Level = direct ? 2 : 3;
            Log = LogManager.GetLogger(origin);
        }
        public void Error(string message, System.Exception exception)
        {
            Log.Error(string.Format("Method -> \"{0}\" : \"{1}\"", GetCalledMethod(), message), exception);
        }
        public void Info(string message)
        {
            Log.Info(string.Format("Method -> \"{0}\" : \"{1}\"", GetCalledMethod(), message));
        }
        public void Warning(string message, System.Exception exception)
        {
            Log.Warn(string.Format("Method -> \"{0}\" : \"{1}\"", GetCalledMethod(), message), exception);
        }
        public void Fatal(string message, System.Exception exception)
        {
            Log.Fatal(string.Format("Method -> \"{0}\" : \"{1}\"", GetCalledMethod(), message), exception);
        }
        public void Debug(string message)
        {
            Log.Debug(string.Format("Method -> \"{0}\" : \"{1}\"", GetCalledMethod(), message));
        }
        public void Error(string message, System.Exception exception, string method)
        {
            Log.Error(string.Format("Method -> \"{0}\" : \"{1}\"", method, message), exception);
        }
        public void Info(string message, string method)
        {
            Log.Info(string.Format("Method -> \"{0}\" : \"{1}\"", method, message));
        }
        public void Warning(string message, System.Exception exception, string method)
        {
            Log.Warn(string.Format("Method -> \"{0}\" : \"{1}\"", method, message), exception);
        }
        public void Fatal(string message, System.Exception exception, string method)
        {
            Log.Fatal(string.Format("Method -> \"{0}\" : \"{1}\"", method, message), exception);
        }
        public void Debug(string message, string method)
        {
            Log.Debug(string.Format("Method -> \"{0}\" : \"{1}\"", method, message));
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
