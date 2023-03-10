using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateAccountController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public CreateAccountController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreationUtilisateur data)
        {

            string connectionString = Configuration["connectionString"]; 
            string query = "INSERT INTO [test].[dbo].[Users] (Username, Password, Email) VALUES (@Username, @Password, @Email)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", data.Username);
                    command.Parameters.AddWithValue("@Password", data.Password);
                    command.Parameters.AddWithValue("@Email", data.Email);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new { success = true, message = "Utilisateur créé avec succés", data = data });

        }
        public class CreationUtilisateur
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }
    }
}
