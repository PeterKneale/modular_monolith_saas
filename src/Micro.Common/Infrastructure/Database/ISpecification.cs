namespace Micro.Common.Infrastructure.Database;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T item);
}