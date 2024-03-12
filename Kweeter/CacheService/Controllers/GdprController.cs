using System;
using System.Threading.Tasks;
using CacheService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CacheService.Controllers;

[ApiController]
[Route("gdpr")]
public class GdprController
{
    private readonly IGdprService _gdprService;

    public GdprController(IGdprService gdprService)
    {
        _gdprService = gdprService;
    }

    [HttpDelete]
    public async Task<IActionResult> Anonymize(Guid userId)
    {
        try
        {
            await _gdprService.Anonymize(userId);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Anonymize failed");
            return new BadRequestResult();
        }

        return new OkResult();
    }
}