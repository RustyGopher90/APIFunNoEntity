using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace APIFunNoEntity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ILogger<PeopleController> _logger;

        private MyConfig _myconifg;
        public PeopleController(MyConfig myconfig, ILogger<PeopleController> logger)
        {
            _myconifg = myconfig;
            _logger = logger;
        }

        [HttpGet]
        public  IActionResult Get()
        {
            IAsyncEnumerable<People> peeps = AllPeople();
            return Ok(peeps);    
        }

        private async IAsyncEnumerable<People> AllPeople()
        {
            await using var connection = new MySqlConnection(_myconifg.ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT FirstName, LastName, DOB, Email FROM people;", connection);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())          
                yield return new People
                {
                    FirstName = reader.GetString(0),
                    LastName = reader.GetString(1),
                    DOB = reader.GetDateTime(2),
                    Email = reader.GetString(3)
                };                        
        }

        [HttpGet("{firstname}/{lastname}")]
        public IActionResult GetSinglePerson(string firstname, string lastname)
        {
            IAsyncEnumerable<People> peeps = SinglePerson(firstname, lastname);
            return Ok(peeps);
        }

        private async IAsyncEnumerable<People> SinglePerson(string firstname, string lastname)
        {
            await using var connection = new MySqlConnection("Server=127.0.0.1;User ID=app_people_api;Password=P$5TR^7ytdc@#1f5$3;Database=api");
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT FirstName, LastName, DOB, Email FROM people where Firstname like @firstname and LastName like @lastname;", connection);
            command.Parameters.AddWithValue("@firstname", "%" + firstname + "%");
            command.Parameters.AddWithValue("@lastname", "%" + lastname + "%");
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                yield return new People
                {
                    FirstName = reader.GetString(0),
                    LastName = reader.GetString(1),
                    DOB = reader.GetDateTime(2),
                    Email = reader.GetString(3)
                };
        }
    }
}
