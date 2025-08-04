namespace EmployeeManagement.Api
{
    public static class Settings
    {
        public static IConfiguration? Configuration { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static string GetEnvVars(string env)
        {
            return Environment.GetEnvironmentVariable(env) ?? string.Empty;
        }

        public static string GetConnectionString()
        {
            string databaseServer = GetEnvVars("DATABASE__SERVER");
            string databaseName = GetEnvVars("DATABASE__NAME");
            string databasePort = GetEnvVars("DATABASE__PORT");
            string databaseUser = GetEnvVars("DATABASE__USER");
            string databasePass = GetEnvVars("DATABASE__PASSWORD");

            if (Configuration == null)
                throw new InvalidOperationException("Configuration has not been initialized.");

            string? conectString = Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(conectString))
                throw new InvalidOperationException("Connection string 'mysql' not found in configuration.");

            conectString = string
            .Format(
                conectString,
                databaseServer,
                databasePort,
                databaseName,
                databaseUser,
                databasePass
            );

            return conectString;
        }
    }
}