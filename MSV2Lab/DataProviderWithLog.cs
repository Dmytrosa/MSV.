public class DataProviderWithLog : DataProvider
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG]: {message}");
    }

    public override double GetData()
    {
        Log("Getting data...");
        return base.GetData();
    }
}
