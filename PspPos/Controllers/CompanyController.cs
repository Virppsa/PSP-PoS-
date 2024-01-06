using AutoMapper;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.AspNetCore.Mvc;

namespace PspPos.Controllers
{
    [ApiController]
    [Route("cinematic/[controller]")]
    public class CompanyController : ControllerBase
    {

        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Company>>> GetAllCompanies()
        {
            return Ok(await _companyService.GetAll());
        }

        [HttpPost]
        public async Task<ActionResult<Company>> CreateCompany(CompanyPostModel company)
        {
            var createdCompany = _mapper.Map<Company>(company);
            await _companyService.Add(createdCompany);

            return Ok(_mapper.Map<CompanyViewModel>(createdCompany));
        }
        [HttpGet("{companyId}")]
        public async Task<ActionResult<Company>> GetCompany(int companyId)
        {
            var company = await _companyService.Get(companyId);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpPut("{companyId}")]
        public async Task<ActionResult<Company>> UpdateCompany(int companyId, CompanyPostModel company)
        {
            var companyUpdateWith = _mapper.Map<Company>(company);
            companyUpdateWith.Id = companyId;

            var updatedCompany = await _companyService.Update(companyUpdateWith);

            if (updatedCompany == null)
            {
                return NotFound();
            }

            return Ok(updatedCompany);
        }

        [HttpDelete("{companyId}")]
        public async Task<ActionResult> DeleteCompany(int companyId)
        {
            bool deleted = await _companyService.Delete(companyId);
            if (deleted == false)
            {
                return NotFound();
            }
            else { 
                return NoContent();
            }
        }
    }
}