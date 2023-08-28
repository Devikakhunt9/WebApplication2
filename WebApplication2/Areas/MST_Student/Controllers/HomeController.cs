﻿using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication2.Areas.MST_Student.Models;

namespace WebApplication2.Areas.MST_Student.Controllers
{
    [Area("MST_Student")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        public IActionResult Index()
        {
            SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Student_SelectAll";
            SqlDataReader sqlDataReader=  cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sqlDataReader);
            conn.Close();

            return View(dt);
        }

        public IActionResult Delete(int StudentId)
        {
            SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText= "PR_Student_DeleteByPK";
            cmd.Parameters.AddWithValue("StudentId", StudentId);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        public IActionResult Add(int StudentId)
        {
            ViewBag.Data = StudentId;
            if (StudentId != 0)
            {
                SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Student_SelectByPK";
                cmd.Parameters.AddWithValue("StudentId", StudentId);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);
                sqlConnection.Close();
                MST_StudentModel mST = new MST_StudentModel()
                {
                    StudentName = (string)dt.Rows[0]["StudentName"],
                    Email = (string)dt.Rows[0]["Email"],
                    BranchID = (int)dt.Rows[0]["BranchID"],
                    CityID = (int)dt.Rows[0]["CityID"],
                    MobileNoFather = (string)dt.Rows[0]["MobileNoFather"],
                    MobileNoStudent = (string)dt.Rows[0]["MobileNoStudent"],
                    Address = (string)dt.Rows[0]["Address"],
                    BirthDate = (DateTime)dt.Rows[0]["BirthDate"],
                    IsActive = (bool)dt.Rows[0]["IsActive"],
                    Gender = (string)dt.Rows[0]["Gender"],
                    Password = (string)dt.Rows[0]["Password"]
                };
                return View(mST);
            }
            
            return View();
        }

        public IActionResult Save(MST_StudentModel student)
        {
           
            SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("myConnectionStr"));
            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if(student.StudentID != 0)
            {
                cmd.CommandText = "PR_Student_UpdateByPK";
                cmd.Parameters.AddWithValue("StudentId", student.StudentID);
            }
            else
            {
                cmd.CommandText = "PR_Student_Insert";
            }
            cmd.Parameters.AddWithValue("BranchId", student.BranchID);
            cmd.Parameters.AddWithValue("CityId", student.CityID);
            cmd.Parameters.AddWithValue("StudentName", student.StudentName);
            cmd.Parameters.AddWithValue("MobileNoStudent", student.MobileNoStudent);
            cmd.Parameters.AddWithValue("Email", student.Email);
            cmd.Parameters.AddWithValue("MobileNoFather", student.MobileNoFather);
            cmd.Parameters.AddWithValue("Address", student.Address);
            cmd.Parameters.AddWithValue("BirthDate", student.BirthDate);
            cmd.Parameters.AddWithValue("IsActive", student.IsActive);
            cmd.Parameters.AddWithValue("Gender", student.Gender);
            cmd.Parameters.AddWithValue("Password", student.Password);
            cmd.ExecuteReader();
            sqlConnection.Close();
            return RedirectToAction("Index");
        }
    }
}
