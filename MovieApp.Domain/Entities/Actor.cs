namespace MovieApp.Domain.Entities;

public class Actor : Entity
{
    public string Name { get; set; }
    
    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
}