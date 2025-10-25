namespace App.Topics.UsingAndDispose.T1_UsingTryFinally;

// Простой лог, чтобы проверять порядок действий в тестах
internal interface ILog
{
    void Write(string message);
}

// Набор констант для стабильных проверок в тестах
internal static class LogEvents
{
    public const string Open = "OPEN";
    public const string Work = "WORK";
    public const string Dispose = "DISPOSE";
    public const string Error = "ERROR";
}
