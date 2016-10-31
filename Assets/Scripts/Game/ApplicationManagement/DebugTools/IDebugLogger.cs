namespace ApplicationManagement.DebugTools {
    public interface IDebugLogger {
        void LogInfo(params string[] infoLog);
        void LogWarning(params string[] warningLog);
        void LogError(int severity, params string[] errorLog);
    }
}