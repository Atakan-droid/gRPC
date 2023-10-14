using Grpc.Core;
using TodoGrpc;
using ToDoGrpc.Data;
using ToDoGrpc.Models;

namespace ToDoGrpc.Services;

public class ToDoService : ToDo.ToDoBase
{
    private readonly AppDbContext _context;

    public ToDoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateToDoResponse> CreateToDoItem(CreateToDoRequest request, ServerCallContext context)
    {
        if (request.Title == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Title cannot be null"));

        if (request.Description == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Description cannot be null"));

        var toDoItem = new ToDoItem
        {
            Title = request.Title,
            Desc = request.Description
        };
        _context.ToDoItems.Add(toDoItem);
        await _context.SaveChangesAsync();
        return new CreateToDoResponse
        {
            Id = toDoItem.Id,
        };
    }

    public override Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
    {
        var toDoItem = _context.ToDoItems.FirstOrDefault(x => x.Id == request.Id);
        if (toDoItem == null)
            throw new RpcException(new Status(StatusCode.NotFound, "ToDo item not found"));

        toDoItem.Title = request.Title;
        toDoItem.Desc = request.Description;
        _context.ToDoItems.Update(toDoItem);
        _context.SaveChanges();
        return Task.FromResult(new UpdateToDoResponse
        {
            Id = toDoItem.Id,
        });
    }

    public override Task<DeleteToDoResponse> DeleteToDo(DeleteToDoRequest request, ServerCallContext context)
    {
        var toDoItem = _context.ToDoItems.FirstOrDefault(x => x.Id == request.Id);
        if (toDoItem == null)
            throw new RpcException(new Status(StatusCode.NotFound, "ToDo item not found"));

        _context.ToDoItems.Remove(toDoItem);
        _context.SaveChanges();
        return Task.FromResult(new DeleteToDoResponse
        {
            Id = toDoItem.Id,
        });
    }

    public override Task<GetToDoResponse> GetToDo(GetToDoRequest request, ServerCallContext context)
    {
        var toDoItem = _context.ToDoItems.FirstOrDefault(x => x.Id == request.Id);
        if (toDoItem == null)
            throw new RpcException(new Status(StatusCode.NotFound, "ToDo item not found"));

        return Task.FromResult(new GetToDoResponse
        {
            Id = toDoItem.Id,
            Title = toDoItem.Title,
            Description = toDoItem.Desc,
        });
    }

    public override Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
    {
        var toDoItems = _context.ToDoItems.ToList();
        var response = new GetAllResponse();
        foreach (var toDoItem in toDoItems)
        {
            response.ToDos.Add(new GetToDoResponse
            {
                Id = toDoItem.Id,
                Title = toDoItem.Title,
                Description = toDoItem.Desc,
            });
        }

        return Task.FromResult(response);
    }
}