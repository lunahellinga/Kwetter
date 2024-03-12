using Microsoft.AspNetCore.Mvc;
using Neo4j.Models;
using Neo4j.Services;

namespace Neo4j.Controllers;

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
        return kweets.Select(kweet =>
            new KweetDto(id: Guid.Parse(kweet.Id), user: Guid.Parse(kweet.User), message: kweet.Message)).ToList();
    }

    [HttpGet("{id:length(36)}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var kweet = await _kweetService.GetAsync(id.ToString());

        if (kweet is null)
        {
            return NotFound();
        }

        return Ok(new KweetDto(id: Guid.Parse(kweet.Id), user: Guid.Parse(kweet.User), message: kweet.Message));
    }

    [HttpPost]
    public async Task<IActionResult> Post(KweetDto kweet)
    {
        await _kweetService.CreateAsync(new Kweet(id: kweet.Id, user: kweet.User, message: kweet.Message));

        return CreatedAtAction(nameof(Get), new { id = kweet.Id }, kweet);
    }

    [HttpPut]
    public async Task<IActionResult> Update(KweetDto kweet)
    {
        await _kweetService.UpdateAsync(new Kweet(id: kweet.Id, user: kweet.User, message: kweet.Message));

        return Ok();
    }

    [HttpDelete("{id:length(36)}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var kweet = await _kweetService.GetAsync(id.ToString());

        if (kweet is null)
        {
            return NotFound();
        }

        await _kweetService.RemoveAsync(id.ToString());

        return Ok();
    }
}