using PspPos.Models;
using Microsoft.AspNetCore.Mvc;
using PspPos.Data;

namespace PspPos.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly SampleContext _sampleContext;
    private int _debug_sample_count = 0;

    public SampleController(SampleContext sampleContext)
    {
        _sampleContext = sampleContext;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sample>> GetSample(int id)
    {
        var sample = await _sampleContext.GetSampleAsync(id);
        return Ok(sample);
    }

    [HttpGet]
    public async Task<ActionResult<List<Sample>>> GetAllSamples()
    {
        var samples = await _sampleContext.GetAllSamplesAsync();
        return Ok(samples);
    }

    [HttpPost]
    public async Task<ActionResult<Sample>> CreateSample(SamplePostModel sample)
    {
        var createdSample = new Sample(_debug_sample_count, sample);
        await _sampleContext.AddSampleAsync(createdSample);

        return Ok(createdSample);
    }

    [HttpPut]
    public async Task<ActionResult<Sample>> UpdateSample(Sample sample)
    {
        return Ok(await _sampleContext.UpdateSampleAsync(sample));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSample(int id)
    {
        await _sampleContext.DeleteSampleAsync(id);
        return Ok();
    }
}