using Grpc.Net.Client;
using GrpcServerApp;

using var channel = GrpcChannel.ForAddress("http://localhost:5235");

var client = new ToDoGrpcService.ToDoGrpcServiceClient(channel);

Console.Write("Title: ");
var title = Console.ReadLine();
Console.Write("Description: ");
var description = Console.ReadLine();
var addToto = new AddToDo()
{
    Title = title,
    Description = description
};
var response = await client.CreateToDoAsync(addToto);
Console.WriteLine($"{response.Id} {response.Title} {response.Description}");

var allToDo = await client.GetAllToDoAsync(new EmptyRequest());
foreach (var toDo in allToDo.AllToDos)
{
    Console.WriteLine($"{toDo.Id} {toDo.Title} {toDo.Description}");
}