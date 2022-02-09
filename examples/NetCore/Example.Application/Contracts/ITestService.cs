using Example.Application.Contracts.Base;

namespace Example.Application.Contracts
{
    public interface ITestService : IBaseService
    {
        void Hello(string name);
    }
}