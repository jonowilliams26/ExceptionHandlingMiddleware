using FiltersTest.Models;
using System.Threading.Tasks;

namespace FiltersTest.Services
{
    public interface ITodoService
    {
        Task<int> Add(Todo todo);
        Task Delete(int id);
    }

    public class TodoService : ITodoService
    {
        public Task<int> Add(Todo todo) => Task.FromResult(1);
        public Task Delete(int id) => Task.CompletedTask;
    }
}
