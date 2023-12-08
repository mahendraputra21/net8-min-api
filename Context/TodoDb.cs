using Microsoft.EntityFrameworkCore;
using net8_aot_api.Entity;

namespace net8_aot_api.Context
{
    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
    }
}
