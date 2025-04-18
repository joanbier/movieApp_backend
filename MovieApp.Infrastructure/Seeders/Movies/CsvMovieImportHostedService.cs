using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieApp.Domain.Abstractions;
using MovieApp.Infrastructure.Context;

namespace MovieApp.Infrastructure.Seeders.Movies;

public class CsvMovieImportHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CsvMovieImportHostedService> _logger;

    public CsvMovieImportHostedService(
        IServiceProvider serviceProvider,
        ILogger<CsvMovieImportHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<MovieAppDbContext>();
        var importer = scope.ServiceProvider.GetRequiredService<ICsvMovieImporter>();

        if (!dbContext.Movies.Any())
        {
            _logger.LogInformation("Database is empty. Starting CSV movie import...");

            var filePath = Path.Combine(AppContext.BaseDirectory, "Seeders", "Movies", "movies.csv");

            if (!File.Exists(filePath))
            {
                _logger.LogWarning("CSV file not found at path: {Path}", filePath);
                return;
            }

            await importer.ImportFromCsvAsync(filePath);
            _logger.LogInformation("Movie import completed.");
        }
        else
        {
            _logger.LogInformation("Database already contains movies. Skipping CSV import.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}