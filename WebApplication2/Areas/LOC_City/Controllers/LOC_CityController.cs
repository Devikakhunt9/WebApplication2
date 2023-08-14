    using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication2.Areas.LOC_City.Models;

namespace WebApplication2.Areas.LOC_City.Controllers
{
        [Area("LOC_City")]
    public class LOC_CityController : Controller
    {
        private readonly IConfiguration _configuration;
        public LOC_CityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LOC_CityList()
        {
            
            SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable(); 
            dt.Load(reader);
            return View("LOC_CityList",dt);
        }
        public IActionResult LOC_CityAddEdit(int CityID)
        {
            //PR_City_Insert
            //PR_City_UpdateByPK
            if (CityID != 0)
            {
                SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
                DataTable dt = new DataTable();
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_City_SelectByPK";
                cmd.Parameters.AddWithValue("CityID", CityID);
                SqlDataReader dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
                sqlConnection.Close();
                LOC_CityModel CC = new LOC_CityModel
                {
                    CityID = (int)dt.Rows[0]["CityID"],
                    CityName = (string)dt.Rows[0]["CityName"],
                    CityCode = (string)dt.Rows[0]["CityCode"],
                    StateID = (int)dt.Rows[0]["StateID"],
                    CountryID = (int)dt.Rows[0]["CountryID"],
                    //CountryName = (string)dataTable.Rows[0]["CountryName"],
                    //= (string)dt.Rows[0]["StateCode"],
                    StateName = (string)dt.Rows[0]["StateName"],
                    CountryName = (string)dt.Rows[0]["CountryName"]
                };
                return View(CC);
            }
            else
            {
                return View();
            }
        }

        public IActionResult Save(LOC_CityModel cityModel)
        {
           
            SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            DataTable dt = new DataTable();
            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if(cityModel.CityID !=0)
            {
                cmd.CommandText = "PR_City_UpdateByPK";
                cmd.Parameters.AddWithValue("CityID",cityModel.CityID);
            }
            else
            {
                cmd.CommandText = "PR_City_Insert";
            }
            cmd.Parameters.AddWithValue("CityName", cityModel.CityName);
            cmd.Parameters.AddWithValue("CityCode", cityModel.CityCode);
            cmd.Parameters.AddWithValue("StateID", cityModel.StateID);
            cmd.Parameters.AddWithValue("CountryID", cityModel.CountryID);
            cmd.ExecuteReader();
            sqlConnection.Close();
            return RedirectToAction("LOC_CityList");
        }

        public IActionResult Delete(int CityID)
        {
            SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand(); 
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_DeleteByPK";
            cmd.Parameters.AddWithValue("CityID", CityID);
            cmd.ExecuteReader();
            sqlConnection.Close();
            return RedirectToAction("LOC_CityList");
        }
    }
}
