using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace AzureLearningApp.Pages;

public class IndexModel : PageModel
{
    public List<Course> Courses = new List<Course>();
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;
    

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void OnGet()
    {
        string connectionString =_configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
        var sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();
        var sqlcommand =new SqlCommand("SELECT CourseId,CourseName,Rating FROM Course", sqlConnection);
        using(SqlDataReader sqlDataReader = sqlcommand.ExecuteReader())
        {   
            while(sqlDataReader.Read())
            {
                  Courses.Add(new Course(){
                    CourseId = Int32.Parse(sqlDataReader["CourseId"].ToString()),
                    CourseName = sqlDataReader["CourseName"].ToString(),
                    Rating = Decimal.Parse(sqlDataReader["Rating"].ToString())
                  });                  
            }
        }
    }
}
