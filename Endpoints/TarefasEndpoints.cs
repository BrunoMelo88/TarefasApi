using Dapper.Contrib.Extensions;
using TarefasApi.Data;
using static TarefasApi.Data.TarefaContext;

namespace TarefasApi.Endpoints
{
    public static class TarefasEndpoints
    {
        public static void MapTarefasEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => $"Bem-vindo a API Tarefas - {DateTime.Now}");

            app.MapGet("/tarefas", async (GetConnection connectionGetter) =>
            {
                using var con = await connectionGetter();
                var tarefas = con.GetAll<Tarefa>().ToList();
                if (tarefas is null) { return Results.NotFound(); }
                return Results.Ok(tarefas);
            });

            app.MapGet("/tarefas/{id}", async (GetConnection connectionGetter, int id) =>
            {
                using var con = await connectionGetter();
                var tarefa = con.Get<Tarefa>(id);
                if (tarefa is null) { return Results.NotFound(); }
                return Results.Ok(tarefa);
            });

            app.MapPost("/tarefas", async (GetConnection connectionGetter, Tarefa Tarefa) =>
            {
                using var con = await connectionGetter();
                var id = con.Insert(Tarefa);
                return Results.Created($"/tarefas/{id}", Tarefa);

            });

            app.MapPut("/tarefas", async (GetConnection connectionGetter, Tarefa tarefa) =>
            {
                if (tarefa is null)
                    return Results.BadRequest("Dados inválidos");
                using var con = await connectionGetter();
                con.Update(tarefa);
                return Results.Ok(tarefa);
            });

            app.MapDelete("/terefas/{id}", async (GetConnection connectionGetter, int id) =>
            {
                using var con = await connectionGetter();
                var deleted = con.Get<Tarefa>(id);
                if (deleted is null) { return Results.NotFound("Registro não encontrado."); }
                con.Delete(deleted);
                return Results.Ok(deleted);
            });
        }
    }
}
