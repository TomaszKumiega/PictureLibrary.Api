﻿using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Repositories;

public interface IAuthorizationDataRepository : IRepository<AuthorizationData>
{
    public Task<AuthorizationData> UpsertForUser(AuthorizationData entity);
    public AuthorizationData? GetByUserId(ObjectId userId);
}