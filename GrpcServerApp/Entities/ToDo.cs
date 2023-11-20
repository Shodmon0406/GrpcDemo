namespace GrpcServerApp.Entities;

public class ToDo
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}