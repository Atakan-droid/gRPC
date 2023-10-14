namespace ToDoGrpc.Models;

public class ToDoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Desc { get; set; }
    public string ToDtoStatus { get; set; } = "NEW";
}