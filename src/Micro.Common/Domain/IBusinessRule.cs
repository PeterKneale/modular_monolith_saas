namespace Micro.Common.Domain;

public interface IBusinessRule
{
    string Message { get; }
    bool IsBroken();
}