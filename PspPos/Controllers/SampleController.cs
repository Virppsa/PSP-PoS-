//using AutoMapper;
//using PspPos.Infrastructure;
//using PspPos.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace PspPos.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class SampleController : ControllerBase
//    {

//        private readonly ISampleService _sampleService;
//        private readonly IMapper _mapper;

//        public SampleController(ISampleService sampleService, IMapper mapper)
//        {
//            _sampleService = sampleService;
//            _mapper = mapper;
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<Sample>> GetSample(int id)
//        {
//            var sample = await _sampleService.Get(id);
//            return Ok(sample);
//        }

//        [HttpGet]
//        public async Task<ActionResult<List<Sample>>> GetAllSamples()
//        {
//            return Ok(await _sampleService.GetAll());
//        }

//        [HttpPost]
//        public async Task<ActionResult<Sample>> CreateSample(SamplePostModel sample)
//        {
//            var createdSample = _mapper.Map<Sample>(sample);
//            await _sampleService.Add(createdSample);
//            await _sampleService.SaveAll();

//            return Ok(_mapper.Map<SampleViewModel>(createdSample));
//        }

//        [HttpPut]
//        public async Task<ActionResult<Sample>> UpdateSample(Sample sample)
//        {
//            var sampleToUpdate = await _sampleService.Get(sample.Id);
//            if (sampleToUpdate == null)
//            {
//                return NotFound();
//            }
//            //_mapper.Map(sample, sampleToUpdate);
//            sampleToUpdate.Location = sample.Location;
//            sampleToUpdate.Date = sample.Date;
//            await _sampleService.SaveAll();
//            return Ok(sampleToUpdate);
//        }

//        [HttpDelete("{id}")]
//        public async Task<ActionResult<Sample>> DeleteSample(int id)
//        {
//            return Ok(await _sampleService.Delete(id));
//        }
//    }
//}