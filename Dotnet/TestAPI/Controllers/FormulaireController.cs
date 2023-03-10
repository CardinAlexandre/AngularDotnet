using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Web.Http;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormulaireController : ControllerBase
    {

        private readonly IConfiguration Configuration;

        public FormulaireController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post([FromBody] FormulaireData data)
        {

            string connectionString = Configuration["connectionString"];
            string query = "INSERT INTO [test].[dbo].[Person] (nom, prenom) VALUES (@nom, @prenom)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nom", data.Nom);
                    command.Parameters.AddWithValue("@prenom", data.Prenom);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                BadRequest(e.Message);
            }

            return Ok(new { success = true, message = "Données enregistrées avec succès", data = data });

        }

        [HttpGet]
        public IActionResult Get()
        {
            string connectionString = Configuration["connectionString"];
            string query = "SELECT id, nom, prenom FROM [test].[dbo].[Person]";
            List<FormulaireData> formDataList = new List<FormulaireData>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string nom = reader.GetString(1);
                        string prenom = reader.GetString(2);
                        FormulaireData formData = new FormulaireData { id = id.ToString(), Nom = nom, Prenom = prenom };
                        formDataList.Add(formData);
                    }
                }
            }
            catch (Exception e)
            {
                BadRequest(e.Message);
            }

            return Ok(formDataList);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // requête SQL de suppression
            string connectionString = Configuration["connectionString"];
            string deleteQuery = "DELETE FROM [test].[dbo].[Person] WHERE id = @id";
            try
            {
                // création de la connexion à la base de données
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // création de la commande SQL
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        // ajout du paramètre d'ID à la commande
                        command.Parameters.AddWithValue("@id", id);

                        // ouverture de la connexion à la base de données
                        connection.Open();

                        // exécution de la commande SQL
                        int rowsAffected = command.ExecuteNonQuery();

                        // vérification du nombre de lignes affectées
                        if (rowsAffected == 0)
                        {
                            return NotFound(new { success = false, message = "L'id n'existe pas : " + id, id = id });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
            return Ok(new { success = true, message = "Element supprimé de la liste : " + id, id = id });
        }
    }

    public class FormulaireData
    {
        public string? id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
    }
}