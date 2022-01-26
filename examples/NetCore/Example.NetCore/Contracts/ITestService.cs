namespace Example.NetCore.Contracts
{
    public interface ITestService : IBaseService
    {
        void Hello(string name);
    }
}