﻿namespace Mongo.Data;

public class MongoDbSettings : IMongoDbSettings
{
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
    public string ConnectionString { get; set; }
}