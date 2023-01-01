public interface IDataProvider
{
    double GetData();
}

public class DataProvider : IDataProvider
{
    public virtual double GetData()
    {
        //код для доступу до диску або бази даних
        return 42.0;
    }
    public double UseGetData()
    {
        return GetData() * 2;
    }
}

