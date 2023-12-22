using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dentistry_API.Models;
namespace Dentistry_API.Controllers
{
    public class LoginController : Controller
    {
        private readonly DentistryContext _context;

        public LoginController(DentistryContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials model)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(u => u.EmailPatient == model.Email && u.PasswordPatient == model.Password);

            var employee = await _context.Employees
               .FirstOrDefaultAsync(u => u.EmailEmployee == model.Email && u.PasswordEmployee == model.Password);

            if (patient == null)
            {
                if (employee == null)
                {

                    return Unauthorized();
                }
                else
                {
                    return Ok(employee);
                }

            }

            return Ok(patient);
        }




    }
}
