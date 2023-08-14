using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication2.Areas.LOC_Country.Models;
using WebApplication2.Areas.LOC_State.Models;

namespace WebApplication2.Areas.LOC_State.Controllers
{
    [Area("LOC_State")]
    public class LOC_StateController : Controller
    {
        private readonly IConfiguration _configuration;
        public LOC_StateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LOC_StateList()
        {
            string connectionString = this._configuration.GetConnectionString("myConnectionStr");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_SelectAll";
            SqlDataReader dataReader = cmd.ExecuteReader();
            dt.Load(dataReader);
            return View("LOC_StateList", dt);
            //return View();
        }

        public IActionResult LOC_StateAddEdit(int StateID)
        {
            if (StateID != 0)
            {
                Console.WriteLine("EDIt called"); 
                //databse connection for update
                //ViewBag.Data = LOC_StateModel.CountryID;
                DataTable dataTable = new DataTable();
                SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.CommandText = "PR_State_SelectByPK";
                cmd.Parameters.AddWithValue("StateID", StateID);
                SqlDataReader dataReader = cmd.ExecuteReader();
                dataTable.Load(dataReader);
                conn.Close();
                LOC_StateModel  SC = new LOC_StateModel 
                {
                    StateID = (int)dataTable.Rows[0]["StateID"],
                    CountryID = (int)dataTable.Rows[0]["CountryID"],
                    //CountryName = (string)dataTable.Rows[0]["CountryName"],
                    StateCode = (string)dataTable.Rows[0]["StateCode"],
                    StateName = (string)dataTable.Rows[0]["StateName"],
                };
                return View(SC);
            }
            else
            {
                //query execute for insert
                return View();
            }
            
        }
        
        public IActionResult Save(LOC_StateModel stateModel)
        {
            //submit btn for add and update
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if(stateModel.StateID != 0) 
            {
                cmd.CommandText = "PR_state_UpdateByPK";
                cmd.Parameters.AddWithValue("StateID", stateModel.StateID);
            }
            else
            {
                cmd.CommandText = "PR_State_Insert";
            }
            cmd.Parameters.AddWithValue("StateName", stateModel.StateName);
            cmd.Parameters.AddWithValue("StateCode", stateModel.StateCode);
            cmd.Parameters.AddWithValue("CountryID", stateModel.CountryID);
            cmd.ExecuteReader();
            conn.Close();
            return RedirectToAction("LOC_StateList");
        }

        public IActionResult Delete(int StateID)
        {
            
            Console.WriteLine("Delete methd called");
            SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_DeleteByPK";
            cmd.Parameters.AddWithValue("StateID", StateID);
            cmd.ExecuteReader();
            conn.Close();
            return RedirectToAction("LOC_StateList");
        }
    }
}
