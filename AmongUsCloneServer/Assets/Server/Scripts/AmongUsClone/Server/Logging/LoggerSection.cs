namespace AmongUsClone.Server.Logging
{
    // I do not use enums, because c# forbids enum inheritance, which is very much needed for the shared Logger class
    public static class LoggerSection
    {
        public const string Initialization = "Initialization";
        public const string Network = "Network";
        public const string Connection = "Connection";
        public const string GameSnapshots = "GameSnapshots";
        public const string PlayerColors = "PlayerColors";
        public const string Interactions = "Interactions";
        public const string AdminPanelViewing = "AdminPanelViewing";
        public const string SecurityPanelViewing = "SecurityPanelViewing";
    }
}
