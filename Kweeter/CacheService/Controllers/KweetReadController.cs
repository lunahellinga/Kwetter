using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CacheService.Models;
using CacheService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CacheService.Controllers;

[ApiController]
[Route("get")]
public class KweetReadController : ControllerBase
{
    private readonly IKweetService _kweetService;

    public KweetReadController(IKweetService kweetService)
    {
        _kweetService = kweetService;
    }


    [HttpGet("/by-id")]
    public async Task<ActionResult<CacheKweet>> GetKweet(Guid id)
    {
        try
        {
            return Ok(await _kweetService.GetKweet(id));
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Error getting kweet {KweetId}", id);
            return StatusCode(500, $"Error getting kweet {id}");
        }
    }

    [HttpGet("/recent")]
    public async Task<ActionResult<IList<CacheKweet>>> GetRecentKweets()
    {
        try
        {
            return Ok(await _kweetService.GetRecentKweets());
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Error getting recent kweets");
            return StatusCode(500, "Error getting recent kweets");
        }
    }

    [HttpGet("/by-user")]
    public async Task<ActionResult<IList<CacheKweet>>> GetKweetsByUsername(string username)
    {
        try
        {
            return Ok(await _kweetService.GetKweetsByUsername(username));
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Error getting kweets for {Username}", username);
            return StatusCode(500, $"Error getting kweets for {username}");
        }
    }

    [HttpGet("/for-tag")]
    public async Task<ActionResult<IList<CacheKweet>>> GetKweetsByTag(string tag)
    {
        try
        {
            return Ok(await _kweetService.GetKweetsByTag(tag));
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Error getting kweets for tag {Tag}", tag);
            return StatusCode(500, $"Error getting kweets for tag {tag}");
        }
    }
}