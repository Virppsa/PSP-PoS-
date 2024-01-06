using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PspPos.Infrastructure;
using PspPos.Models;
using PspPos.Services;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PspPos.Controllers
{
    [ApiController]
    [Route("cinematic/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("/{companyID}/users")]
        public async Task<ActionResult<List<User>>> GetCompanyUsers([Required] Guid companyID)
        {
            try
            {
                var users = await _userService.GetUsersByCompanyIdAsync(companyID);
                if (users == null || users.Count == 0)
                {
                    return NotFound($"No users found for company ID: {companyID}");
                }
                return Ok(users);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database" + e);
            }
        }

        [HttpGet("/{companyID}/users/{userID}")]
        public async Task<ActionResult<User>> GetCompanyUserByUserID([Required] Guid companyID, [Required] Guid userID)
        {
            try
            {
                var user = await _userService.GetUserByCompanyAndUserID(companyID, userID);
                if (user == null)
                {
                    return NotFound($"No users found for company ID: {companyID} and with user ID: {userID}");
                }
                return Ok(user);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database" + e);
            }
        }

        [HttpPost("/{companyID}/users")]
        public async Task<ActionResult> CreateUser([Required] Guid companyID, [Required] UserPostModel userArgs)
        {
            try
            {
                return Ok(await _userService.AddUser(userArgs));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error creating user" + e);
            }
        }

        [HttpPut("/{companyID}/users/{userID}")]
        public async Task<ActionResult<User>> UpdateUser([Required] Guid companyID, [Required] Guid userID, [Required] UserPostModel userArgs)
        {
            try
            {
                return Ok(await _userService.UpdateUser(userID, companyID, userArgs));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error updating user" + e);
            }
        }

        [HttpDelete("/{companyID}/users/{userID}")]
        public async Task<ActionResult> DeleteUser([Required] Guid companyID, [Required] Guid userID)
        {
            try
            {
                await _userService.DeleteUser(userID, companyID);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error deleting user" + e);
            }
        }

    }
}
