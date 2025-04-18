using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;
using MovieApp.Infrastructure.Context;

namespace MovieApp.Infrastructure.Seeders.Movies;

internal class CsvMovieImporter : ICsvMovieImporter
{
    
    private readonly IMovieRepository _movieRepository;
    private readonly IActorRepository _actorRepository; 
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CsvMovieImporter> _logger;

    public CsvMovieImporter(IMovieRepository movieRepository, IActorRepository actorRepository, IUnitOfWork unitOfWork,  ILogger<CsvMovieImporter> logger)
    {
        _movieRepository = movieRepository;
        _actorRepository = actorRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task ImportFromCsvAsync(string filePath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        };
        
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);
        var records = csv.GetRecords<MovieCsvDto>().ToList();
        
        var allActors = await _actorRepository.GetAllAsync();
        var actorCache = allActors.ToDictionary(a => a.Name.Trim(), StringComparer.OrdinalIgnoreCase);
        
        foreach (var dto in records)
        {
            var actorNames = new[] { dto.Star1, dto.Star2, dto.Star3, dto.Star4 }
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct()
                .ToList();

            var movieActors = new List<MovieActor>();

            foreach (var actorName in actorNames)
            {
                if (!actorCache.TryGetValue(actorName, out var actor))
                {
                    actor = new Actor { Name = actorName };
                    _actorRepository.Add(actor);
                    actorCache[actorName] = actor;
                }
                movieActors.Add(new MovieActor { Actor = actor });
            }

            if (!decimal.TryParse(dto.Gross?.Replace(",", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out var gross))
                gross = 0;

            bool movieExists = await _movieRepository.IsAlreadyExistAsync(dto.Title, dto.Director);

            if (movieExists)
            {
                _logger.LogInformation("Movie {Title} already exists.", dto.Title);
                continue;
            }
         

            var movie = new Movie
            {
                Title = dto.Title,
                ReleasedYear = dto.ReleasedYear,
                Rating = dto.Rating,
                Director = dto.Director,
                Description = dto.Description,
                Genre = dto.Genre,
                PosterLink = dto.PosterLink,
                Certificate = dto.Certificate,
                Runtime = dto.Runtime,
                Gross = gross,
                MovieActors = movieActors
            };

            _movieRepository.Add(movie);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}