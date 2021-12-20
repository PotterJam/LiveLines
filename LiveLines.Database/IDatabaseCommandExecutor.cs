using System;
using System.Data;
using System.Threading.Tasks;

namespace LiveLines.Database
{
    public interface IDatabaseCommandExecutor
    {
        Task ExecuteCommand(Func<IDbCommand, Task> action);
        Task<T> ExecuteCommand<T>(Func<IDbCommand, Task<T>> action);
    }
}