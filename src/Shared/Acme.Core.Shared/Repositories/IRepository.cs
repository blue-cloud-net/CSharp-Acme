namespace Acme.Repositories;

/// <summary>
/// 仓储基接口
/// </summary>
/// <typeparam name="T">聚合根类型</typeparam>
public interface IRepository<T> where T : Domain.IAggregateRoot
{
}
