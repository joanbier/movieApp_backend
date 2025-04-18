using System.Net;

namespace MovieApp.Domain.Exceptions;

public abstract class MovieAppException : Exception
{
    public abstract HttpStatusCode StatusCode { get; }
    
    public MovieAppException(string message) : base(message)
    {
        
    }
}