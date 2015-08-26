namespace _5.OsteoYoga.Exception.Interfaces
{
    public interface ILogger 
    {
        void Error(string message, System.Exception exception);
        void Info(string message);
        void Warning(string message, System.Exception exception);
        void Fatal(string message, System.Exception exception);
        void Debug(string message);
    }
}
