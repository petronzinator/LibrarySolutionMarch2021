using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class StatusController : ControllerBase
    {
        // GET /status\
        [HttpGet("status")]
        public StatusResponse GetTheStatus()
        {
            return new StatusResponse
            {
                Message = "Everything is going great. Thanks for asking!",
                LastChecked = DateTime.Now
            };
        }

        // GET /customer/13
        // GET /customers/(anything that is an integer)
        [HttpGet("customers/{customerId:int}")]
        public ActionResult GetInfoAboutCustomer(int customerID)
        {
            return Ok($"Getting info about customer {customerID}");
        }


        [HttpGet("blogs/{year:int}/{month:int:min(1):max(12)}/{day:int}")]
        public ActionResult GetBlogPosts(int year, int month, int day)
        {
            if(day < 1 || day > 31)
            {
                NotFound();
            }
            return Ok($"Getting blogs for {month}-{day}-{year}");
        }

        // GET /employees?department=DEV
        [HttpGet("employees")]
        public ActionResult GetEmployees([FromQuery] string department= "All")
        {
            var response = new GetEmployeesResponse
            {
                Data = new List<string> { "Joe", "Sue", "Mary" },
                Department = department
            };
            return Ok(response);
        }

        [HttpGet("whoami")]
        public ActionResult WhoAmI([FromHeader(Name = "User-Agent")] string userAgent)
        {
            return Ok($"I have no idea, but you are running {userAgent}");
        }

        [HttpPost("employees")]
        public ActionResult Hire([FromBody] PostEmployeeRequest request)
        {
            return Ok($"Hiring {request.Name} in {request.Department} for {request.StartingSalary:c}");
        }
    }

    public class PostEmployeeRequest
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal StartingSalary { get; set; }
    }

    public class GetEmployeesResponse
    {
        public List<string> Data { get; set; }
        public string Department { get; set; }
    }

    public class StatusResponse
    {
        public string Message { get; set; }
        public DateTime LastChecked { get; set; }
    }
}
