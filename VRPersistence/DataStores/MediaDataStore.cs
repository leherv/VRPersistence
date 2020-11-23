﻿using System.Linq;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VRPersistence.DAO;

namespace VRPersistence.DataStores
{
    public class MediaDataStore : IMediaDataStore
    {
        private readonly VRPersistenceDbContext _dbContext;
        private readonly ILogger<MediaDataStore> _logger;

        public MediaDataStore(ILogger<MediaDataStore> logger, VRPersistenceDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        
        public Result<Media> GetMedia(string mediaName)
        {
            var media = _dbContext.Media.Where(m => m.MediaName.ToLower().Equals(mediaName.ToLower()))
                .Take(1)
                .ToList();
            return media.Count == 1
                ? Result.Success(media[0])
                : Result.Failure<Media>($"There are {media.Count.ToString()} media with mediaName {mediaName}.");
        }
    }
}