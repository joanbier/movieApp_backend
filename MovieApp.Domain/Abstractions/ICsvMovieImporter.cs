namespace MovieApp.Domain.Abstractions;

public interface ICsvMovieImporter
{
    Task ImportFromCsvAsync(string filePath);
}