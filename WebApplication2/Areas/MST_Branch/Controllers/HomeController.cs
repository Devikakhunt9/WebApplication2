using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication2.Areas.MST_Branch.Models;

namespace WebApplication2.Areas.MST_Branch.Controllers
{
    [Area("MST_Branch")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: HomeController
        public ActionResult Index()
        {
            SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Branch_SelectAll";
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sqlDataReader);
            return View(dt);
        }

       

        

        // POST: HomeController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(int BranchID)
        {
            ViewBag.Data = BranchID;
            
                if(BranchID != 0)
                {

                    ViewBag.Data = BranchID;
                    string connectionStr = this._configuration.GetConnectionString("myConnectionStr");
                    DataTable dt = new DataTable();
                    SqlConnection conn = new SqlConnection(connectionStr);
                    conn.Open();
                    SqlCommand objcmd = conn.CreateCommand();
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "PR_Branch_SelectByPK";
                    objcmd.Parameters.AddWithValue("BranchId", BranchID);
                    SqlDataReader objDataReader = objcmd.ExecuteReader();
                    dt.Load(objDataReader);
                    conn.Close();
                    MST_BranchModel MB = new MST_BranchModel
                    {
                        BranchID = (int)dt.Rows[0]["BranchID"],
                        BranchName = (string)dt.Rows[0]["BranchName"],
                        BranchCode = (string)dt.Rows[0]["BranchCode"]
                    };
                    return View(MB);
                }
            else
            {
                return View();
            }
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int BranchID, IFormCollection collection)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "PR_Branch_DeleteByPK";
                sqlCommand.Parameters.AddWithValue("@BranchId", BranchID);
                sqlCommand.ExecuteReader();
                sqlConnection.Close();
                return RedirectToAction("Index");
               
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Save(MST_BranchModel branch)
        {
                SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
            if (branch.BranchID != 0)
            {
                cmd.CommandText = "PR_Branch_UpdateBy_PK";
                cmd.Parameters.AddWithValue("BranchId", branch.BranchID);
                
            }
            else
            {
                cmd.CommandText = "PR_Branch_Insert";
               
            }
            cmd.Parameters.AddWithValue("BranchName", branch.BranchName);
            cmd.Parameters.AddWithValue("BranchCode", branch.BranchCode);
            cmd.ExecuteReader();
            return RedirectToAction("Index");
        }
    }
}
