using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyCompany.NameProject.Application.Common.Exceptions;
using MyCompany.NameProject.Application.Common.Interfaces;
using MyCompany.NameProject.Domain.Entities.Weather;
using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.Infrastructure.Persistence;
using MyCompany.NameProject.Infrastructure.Persistence.Common.Transactions;
using MyCompany.NameProject.Infrastructure.Persistence.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyCompany.NameProject.Infrastructure.Weather
{
    public class WeatherHistoryRepository : IWeatherHistoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<WeatherHistoryRepository> _logger;
        private readonly IMapper<WeatherHistoryEntity, WeatherHistory> _rawToDomainWeatherHistoryMapper;
        private readonly IMapper<WeatherHistory, WeatherHistoryEntity> _domainToRawWeatherHistoryMapper;
        private readonly IEnricher<WeatherHistoryEntity> _rawWeatherHistoryEnricher;

        public WeatherHistoryRepository(
            ApplicationDbContext dbContext,
            ILogger<WeatherHistoryRepository> logger,
            IMapper<WeatherHistoryEntity, WeatherHistory> rawToDomainWeatherHistoryMapper,
            IMapper<WeatherHistory, WeatherHistoryEntity> domainToRawWeatherHistoryMapper,
            IEnricher<WeatherHistoryEntity> rawWeatherHistoryEnricher)
        {
            _dbContext = dbContext;
            _logger = logger;
            _rawToDomainWeatherHistoryMapper = rawToDomainWeatherHistoryMapper;
            _domainToRawWeatherHistoryMapper = domainToRawWeatherHistoryMapper;
            _rawWeatherHistoryEnricher = rawWeatherHistoryEnricher;
        }

        public async Task<WeatherHistory> GetWeatherHistoryAsync(
            Guid id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.WeatherHistories
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return _rawToDomainWeatherHistoryMapper.Map(entity);
        }

        public async Task<WeatherHistory> CreateWeatherHistoryAsync(
            WeatherHistory weatherHistory, CancellationToken cancellationToken)
        {
            if (weatherHistory == null)
                throw new ArgumentNullException(nameof(weatherHistory));
            if (weatherHistory.Id != Guid.Empty)
                throw new ArgumentException(null, nameof(weatherHistory));

            var entity = _domainToRawWeatherHistoryMapper.Map(weatherHistory);
            var entityTracker = await _dbContext.WeatherHistories.AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var createdEntityId = entityTracker.Entity.Id;
            var createdEntity = await _dbContext.WeatherHistories
                .FirstOrDefaultAsync(p => p.Id == createdEntityId, cancellationToken);

            return _rawToDomainWeatherHistoryMapper.Map(createdEntity);
        }

        public async Task UpdateWeatherHistoryAsync(
            WeatherHistory weatherHistory, CancellationToken cancellationToken)
        {
            if (weatherHistory == null)
                throw new ArgumentNullException(nameof(weatherHistory));

            var tokenId = weatherHistory.Id;
            if (tokenId == Guid.Empty)
                throw new ArgumentException(null, nameof(weatherHistory));

            using (var transaction = await _dbContext.Database.BeginOrUseTransactionAsync(cancellationToken))
            {
                var entity = await _dbContext.WeatherHistories
                    .FirstOrDefaultAsync(p => p.Id == tokenId, cancellationToken);
                if (entity == null)
                    throw new NotFoundException($"WeatherHistory with id: \"{tokenId}\" not found");

                var entityData = _domainToRawWeatherHistoryMapper.Map(weatherHistory);
                _rawWeatherHistoryEnricher.Enrich(entity, entityData);
                _dbContext.WeatherHistories.Update(entity);

                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
        }

        public async Task<bool> DeleteWeatherHistoryAsync(
            Guid id, CancellationToken cancellationToken)
        {
            using (var transaction = await _dbContext.Database.BeginOrUseTransactionAsync(cancellationToken))
            {
                var token = await _dbContext.WeatherHistories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
                if (token == null)
                    throw new NotFoundException($"WeatherHistory with id: \"{id}\" not found");

                try
                {
                    _dbContext.WeatherHistories.Remove(token);

                    var res = await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    return res > 0;
                }
                catch (Exception e)
                {
                    _logger?.LogError(e, $"Error on deletion record with id: {id}");
                    return false;
                }
            }
        }
    }
}
