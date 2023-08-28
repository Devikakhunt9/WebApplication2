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

        public IActionResult LOC_StateList(SearchModel sr)
        {
            SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_State_SerchByStateCodeOrStateName";
            sqlCommand.Parameters.AddWithValue("@StateName", sr.StateName);
            sqlCommand.Parameters.AddWithValue("@StateCode", sr.StateCode);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            return View(dataTable);
            //return View();
        }

        public IActionResult LOC_StateAddEdit(int StateID)
        {
            StateAddDropdown(StateID);
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
                //cmd.CommandType = CommandType. 
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

        public IActionResult StateAddDropdown(int StateID)
        {
            SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_SelectByPK";
            cmd.Parameters.AddWithValue("@StateID", StateID);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            LOC_StateModel stateModel = new LOC_StateModel();
            foreach(DataRow row in dataTable.Rows)
            {
                stateModel.StateID = int.Parse(row["StateID"].ToString());
                stateModel.StateName = row["StateName"].ToString();
                stateModel.StateCode = row["StateCode"].ToString();
                stateModel.CountryID = int.Parse(row["CountryID"].ToString());
            }
            SqlCommand sqlCommand2 = sqlConnection.CreateCommand();
            sqlCommand2.CommandType = CommandType.StoredProcedure;
            sqlCommand2.CommandText = "PR_LOC_Country_SelectAll";
            SqlDataReader sqlDataReader = sqlCommand2.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(sqlDataReader);

            List<LOC_CountryModel> li = new List<LOC_CountryModel>();    
            foreach(DataRow dr in dataTable1.Rows)
            {
                LOC_CountryModel lOC_CountryModel = new LOC_CountryModel();
                lOC_CountryModel.CountryID = int.Parse(dr["CountryID"].ToString());
                lOC_CountryModel.CountryName = dr["CountryName"].ToString();
                li.Add(lOC_CountryModel);

            }
            ViewBag.LI = li;
            sqlConnection.Close();
            return View(stateModel);

        }

        public IActionResult Serach(SearchModel sr) 
        {
            SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_State_SerchByStateCodeOrStateName";
            sqlCommand.Parameters.AddWithValue("@StateName", sr.StateName);
            sqlCommand.Parameters.AddWithValue("@StateCode", sr.StateCode);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            return View("Serach", dataTable);
        }
    }
}
