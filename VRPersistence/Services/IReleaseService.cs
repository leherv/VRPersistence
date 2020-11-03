﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using VRPersistence.BO;

namespace VRPersistence.Services
{
    public interface IReleaseService
    {
        Task<Result<IEnumerable<Release>>> GetNotNotified(string mediaName);
        Task<List<Result>> AddReleases(IEnumerable<Release> release);
        Task<Result> AddRelease(Release release);
        Task<List<Result>> SetNotified(IEnumerable<SetNotified> setNotified);
    }
}