namespace DemoSignalR;

public static class Helpers
{
    public static void ExecuteSafe(Func<bool> whileCondition, Action exec, int timeoutMinutes)
    {
        var          count        = 0;
        const double pollEverySec = 0.5;
        const int    waitTime     = (int) (pollEverySec * 1000);
        while (whileCondition.Invoke() && count < timeoutMinutes * 60)
        {
            Thread.Sleep(waitTime);
            count++;
        }

        exec.Invoke();
    }
}