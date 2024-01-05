using Microsoft.AspNetCore.Mvc;

namespace PspPos.Controllers;

[ApiController]
[Route("cinematic/[controller]")]
public class CompanyController : ControllerBase
{

//    private readonly ICompanyService _companyService;
//    private readonly IMapper _mapper;

//    public CompanyController(ICompanyService companyService, IMapper mapper)
//    {
//        _companyService = companyService;
//        _mapper = mapper;
//    }

//    [HttpGet]
//    public async Task<ActionResult<List<Company>>> GetAllCompanies()
//    {
//        return Ok(await _companyService.GetAll());
//    }

//    [HttpPost]
//    public async Task<ActionResult<Company>> CreateCompany(CompanyPostModel company)
//    {
//        var createdCompany = _mapper.Map<Company>(company);
//        await _companyService.Add(createdCompany);
//        await _companyService.SaveAll();

//        return Ok(_mapper.Map<CompanyViewModel>(createdCompany));
//    }
//    [HttpGet("{companyId}")]
//    public async Task<ActionResult<Company>> GetCompany(int companyId)
//    {
//        var company = await _companyService.Get(companyId);
//        return Ok(company);
//    }

//    [HttpPut("{companyId}")]
//    public async Task<ActionResult<Company>> UpdateCompany(int companyId, CompanyPostModel company)
//    {
//        var companyToUpdate = await _companyService.Get(companyId);
//        if (companyToUpdate == null)
//        {
//            return NotFound();
//        }
//        //_mapper.Map(sample, sampleToUpdate);
//        companyToUpdate.Name = company.Name;
//        companyToUpdate.Email = company.Email;
//        await _companyService.SaveAll();
//        return Ok(companyToUpdate);
//    }

//    [HttpDelete("{companyId}")]
//    public async Task<ActionResult> DeleteCompany(int companyId)
//    {
//        var deleted = await _companyService.Delete(companyId);
//        if (deleted != null)
//        {
//            return NoContent();
//        }
//        else { 
//            return NotFound();
//        }
//    }
//}
}