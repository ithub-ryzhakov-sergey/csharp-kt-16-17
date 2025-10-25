namespace App.Topics.UsingAndDispose.T1_UsingTryFinally;

internal static class UsingTryFinallyRunner
{
    // Должно: с использованием using вызвать Open/DoWork/Dispose и логировать в порядке OPEN, WORK, DISPOSE.
    // При throwInWork == true — сымитировать исключение в рабочей секции, но гарантировать вызов Dispose.
    public static void RunWithUsing(ILog log, bool throwInWork = false)
    {
        // TODO: реализовать через using; порядок логов должен совпадать с RunWithTryFinally
        throw new NotImplementedException();
    }

    // Должно: вручную развернуть логику как try/finally с тем же порядком логов.
    public static void RunWithTryFinally(ILog log, bool throwInWork = false)
    {
        // TODO: реализовать через try/finally; порядок логов должен совпадать с RunWithUsing
        throw new NotImplementedException();
    }
}
