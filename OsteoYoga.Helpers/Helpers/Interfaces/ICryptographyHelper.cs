namespace OsteoYoga.Helper.Helpers.Interfaces
{
    public interface ICryptographyHelper
    {
        string Encrypt(string password);
        string Decrypt(string encryptPassword);
    }
}
