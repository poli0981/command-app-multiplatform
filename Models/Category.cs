using System.Collections.Generic;

namespace CommandApp.Models;

public class Category
{
    public string Name { get; set; } = string.Empty;
    public List<Command> Commands { get; set; } = new(); 
}