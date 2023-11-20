using Grpc.Core;
using GrpcServerApp.Data;
using Microsoft.EntityFrameworkCore;

namespace GrpcServerApp.Services;

public class ToDoService (DataContext context) : ToDoGrpcService.ToDoGrpcServiceBase
{
    public override async Task<ListToDo> GetAllToDo(EmptyRequest emptyRequest, ServerCallContext serverCallContext)
    {
        var listToDo = new ListToDo();
        var toDos = await context.ToDos.Select(t => new ToDo()
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description
        }).ToListAsync();
        listToDo.AllToDos.AddRange(toDos);
        return await Task.FromResult(listToDo);
    }

    public override async Task<ToDo> GetById(IdRequest idRequest, ServerCallContext serverCallContext)
    {
        var toDo = await context.ToDos.FindAsync(idRequest.Id);
        if (toDo == null) throw new RpcException(new Status(StatusCode.NotFound, "ToDo not found!"));
        return await Task.FromResult(new ToDo()
        {
            Id = toDo.Id,
            Title = toDo.Title,
            Description = toDo.Description
        });
    }

    public override async Task<ToDo> CreateToDo(AddToDo addToDo, ServerCallContext serverCallContext)
    {
        var newToDo = new ToDo()
        {
            Title = addToDo.Title,
            Description = addToDo.Description
        };
        await context.ToDos.AddAsync(newToDo);
        await context.SaveChangesAsync();
        return await Task.FromResult(new ToDo()
        {
            Id = newToDo.Id,
            Title = newToDo.Title,
            Description = newToDo.Description
        });
    }

    public override async Task<ToDo> UpdateToDo(ToDo newToDo, ServerCallContext serverCallContext)
    {
        var toDo = await context.ToDos.FindAsync(newToDo.Id);
        if (toDo == null) throw new RpcException(new Status(StatusCode.NotFound, "ToDo not found!"));
        toDo.Title = newToDo.Title;
        toDo.Description = newToDo.Description;
        await context.SaveChangesAsync();
        return await Task.FromResult(new ToDo()
        {
            Id = toDo.Id,
            Title = toDo.Title,
            Description = toDo.Description
        });
    }

    public override async Task<BooleanResponse> DeleteToDo(IdRequest idRequest, ServerCallContext serverCallContext)
    {
        var toDo = await context.ToDos.FindAsync(idRequest.Id);
        if (toDo == null) throw new RpcException(new Status(StatusCode.NotFound, "ToDo not found!"));
        context.ToDos.Remove(toDo);
        await context.SaveChangesAsync();
        return await Task.FromResult(new BooleanResponse()
        {
            Boolean = true
        });
    }
}