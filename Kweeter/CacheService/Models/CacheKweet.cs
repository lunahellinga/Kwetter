using System;
using System.Collections.Generic;
using Domain.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CacheService.Models;

public class CacheKweet
{
    [BsonId] public new string KweetId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string KweetText { get; set; }
    public List<string> Tags { get; set; }
    public List<string> MentionedUsers { get; set; }
    public List<string> ContentLinks { get; set; }
    public DateTime CreatedAt { get; set; }
}