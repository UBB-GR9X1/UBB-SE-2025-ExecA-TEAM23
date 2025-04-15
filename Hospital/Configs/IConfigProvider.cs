namespace Hospital.Configs
{
    /// <summary>
    /// Interface for configuration provider
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// Gets the database connection string
        /// </summary>
        /// <returns>Database connection string</returns>
        string GetDatabaseConnection();
    }
}