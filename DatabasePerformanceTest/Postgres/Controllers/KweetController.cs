using Microsoft.AspNetCore.Mvc;
using Postgres.Models;
using Postgres.Services;

namespace Postgres.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KweetController : ControllerBase
{
    private readonly KweetService _kweetService;

    public KweetController(KweetService kweetService) =>
        _kweetService = kweetService;

    [HttpGet]
    public async Task<List<KweetDto>> Get()
    {
        var kweets = await _kweetService.GetAsync();
        return kweets.Select(kweet => new KweetDto(id: kweet.Id, user: kweet.User, message: kweet.Message)).ToList();
    }

    [HttpGet("{id:length(36)}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var kweet = await _kweetService.GetAsync(id);

            if (kweet is null)
            {
                return NotFound();
            }

            return Ok(new KweetDto(id: kweet.Id, user: kweet.User, message: kweet.Message));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(KweetDto kweet)
    {
        try
        {
            await _kweetService.CreateAsync(new Kweet(id: kweet.Id, user: kweet.User, message: kweet.Message));

            return CreatedAtAction(nameof(Get), new { id = kweet.Id }, kweet);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(KweetDto kweet)
    {
        try
        {
            await _kweetService.UpdateAsync(new Kweet(id: kweet.Id, user: kweet.User, message: kweet.Message));

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }

    [HttpDelete("{id:length(36)}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var kweet = await _kweetService.GetAsync(id);

            if (kweet is null)
            {
                return NotFound();
            }

            await _kweetService.RemoveAsync(id);

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
}