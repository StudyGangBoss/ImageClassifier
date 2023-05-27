using Ardalis.Specification;

namespace Infrastructure;

public interface IReadRepository<T>:IReadRepositoryBase<T> where T:class
{
}