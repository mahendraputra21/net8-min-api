using Microsoft.EntityFrameworkCore;
using net8_aot_api.Context;
using net8_aot_api.Entity;

internal class Program
{
    private static void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
        });
    }

    static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);
        builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
        ConfigureServices(builder.Services);
        var app = builder.Build();


        //Endpoints
        app.MapGet("/todoitems", async (TodoDb db) =>
                await db.Todos.ToListAsync());

        app.MapGet("/todoitems/complete", async (TodoDb db) =>
            await db.Todos.Where(t => t.IsComplete).ToListAsync());

        app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
            await db.Todos.FindAsync(id)
                is Todo todo
                    ? Results.Ok(todo)
                    : Results.NotFound());

        app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
        {
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            return Results.Created($"/todoitems/{todo.Id}", todo);
        });

        app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
        {
            var todo = await db.Todos.FindAsync(id);

            if (todo is null) return Results.NotFound();

            todo.Name = inputTodo.Name;
            todo.IsComplete = inputTodo.IsComplete;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
        {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }

            return Results.NotFound();
        });

        app.Run();
    }
}
