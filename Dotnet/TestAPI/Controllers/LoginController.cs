using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = AuthenticateUser(model.Username, model.Password);

            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(new { Token = token });
            }

            return BadRequest(new { message = "Nom d'utilisateur ou mot de passe incorrect" });
        }

        private User AuthenticateUser(string username, string password)
        {
            string connectionString = Configuration["connectionString"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Requête SQL pour récupérer l'utilisateur correspondant aux informations d'identification saisies
                string query = "SELECT Id, Username, Password, Email FROM [test].[dbo].[Users] WHERE Username = @Username AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Si l'utilisateur est trouvé, créer un objet User et le renvoyer
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                Email = reader.GetString(3)
                            };
                        }
                    }
                }
            }

            // Si aucun utilisateur n'est trouvé, renvoyer null
            return null;
        }


        private string GenerateToken(User user)
        {
            // Clé secrète utilisée pour signer le jeton
            string secretKey = Configuration["secretKey"];

            // Créer les revendications (claims) pour le jeton
            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Username", user.Username),
                new Claim("Password", user.Password),
                new Claim("Email", user.Email),
            };

            // Créer un objet SymmetricSecurityKey avec la clé secrète
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Créer un objet SigningCredentials avec la clé et l'algorithme de signature
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Créer un objet JwtSecurityToken avec les revendications, la date d'expiration et les SigningCredentials
            var token = new JwtSecurityToken(
                issuer: "votre_issuer",
                audience: "votre_audience",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            // Générer le jeton sous forme de chaîne de caractères
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class User
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }

        }
    }
}
