using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication2.Areas.LOC_Country.Models;

namespace WebApplication2.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    
    public class LOC_CountryController : Controller
    {
        private readonly IConfiguration _configuration;
        public LOC_CountryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LOC_CountryList()
        {
            string connectionStr = this._configuration.GetConnectionString("myConnectionStr");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionStr);
            conn.Open();
            SqlCommand objcmd = conn.CreateCommand();
            objcmd.CommandType = CommandType.StoredProcedure;
            objcmd.CommandText = "PR_LOC_Country_SelectAll";
            SqlDataReader dataReader = objcmd.ExecuteReader();
            dt.Load(dataReader);
            return View("LOC_CountryList",dt);
        }
        public IActionResult LOC_CountryAddEdit(int CountryID)
        {

            if (CountryID != 0)
            {

                ViewBag.Data = CountryID;
                string connectionStr = this._configuration.GetConnectionString("myConnectionStr");
                DataTable dt = new DataTable();
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand objcmd = conn.CreateCommand();
                objcmd.CommandType = CommandType.StoredProcedure;
                objcmd.CommandText = "PR_Country_SelectByPK";
                objcmd.Parameters.AddWithValue("CountryID", CountryID);
                SqlDataReader objDataReader = objcmd.ExecuteReader();
                dt.Load(objDataReader);
                conn.Close();
                LOC_CountryModel LC = new LOC_CountryModel
                {
                    CountryID = (int)dt.Rows[0]["CountryID"],
                    CountryName = (string)dt.Rows[0]["CountryName"],
                    CountryCode = (string)dt.Rows[0]["CountryCode"]
                };
                return View(LC);
            }
            else
            {
                return View();
            }
        }
        public IActionResult Save(LOC_CountryModel CountryModel)
        {
            string connectionStr = this._configuration.GetConnectionString("myConnectionStr");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionStr);
            conn.Open();
            SqlCommand objcmd = conn.CreateCommand();
            objcmd.CommandType = CommandType.StoredProcedure;
            if(CountryModel.CountryID != 0)
            {
                objcmd.CommandText = "PR_Country_Update";
                objcmd.Parameters.AddWithValue("@CountryID",CountryModel.CountryID);
            }
            else
            {
                objcmd.CommandText = "PR_Country_Insert";
            }
            
            objcmd.Parameters.AddWithValue("@CountryName", CountryModel.CountryName);
            objcmd.Parameters.AddWithValue("@Countrycode", CountryModel.CountryCode);
            objcmd.ExecuteReader();
            conn.Close();
            return RedirectToAction("LOC_CountryList");
        }


        public IActionResult Delete(int CountryID)
        {
            string connectionStr = this._configuration.GetConnectionString("myConnectionStr");
            SqlConnection conn = new SqlConnection(connectionStr);
            conn.Open();
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_Delete";
            objCmd.Parameters.AddWithValue("@CountryID", CountryID);
            objCmd.ExecuteReader();
            conn.Close();
            return RedirectToAction("LOC_CountryList");
        }
    }
}
