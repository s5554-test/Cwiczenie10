﻿using Cwiczenie7.DTOs.Requests;
using Cwiczenie7.DTOs.Responses;
using Cwiczenie7.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace Cwiczenie7.Services
{
    public class SqlServerStudentsDbService : IStudentDbService
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s5554;Integrated Security=True";
        public IEnumerable<EnrollStudentResponse> EnrollStudent(EnrollStudentRequest request)
        {

            var list = new List<EnrollStudentResponse>();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {

                com.Connection = con;

                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;

                try
                {

                    com.CommandText = "SELECT IdStudy from Studies WHERE Name=IT";
                    com.Parameters.AddWithValue("StudiesName", request.StudiesName);
                    com.ExecuteNonQuery();
                    SqlDataReader exr = com.ExecuteReader();

                    var st = new Student();
                    var rs = new EnrollStudentResponse();
                    if (!exr.Read())
                    {

                        tran.Rollback();
                    }
                    String studiesname = exr["StudiesName"].ToString();
                    if (studiesname.Equals("IT"))
                    {

                        com.CommandText = "INSERT INTO Student(IndexNumber,FirstName,LastName,BirthDate) VALUES(@IndexNumber,@FirstName,@LastName,@BirthDate)";
                        com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                        com.Parameters.AddWithValue("FirstName", request.FirstName);
                        com.Parameters.AddWithValue("LastName", request.LastName);
                        com.Parameters.AddWithValue("BirthDate", request.BirthDate);
                        com.ExecuteNonQuery();

                        tran.Commit();
                        rs.IndexNumber = request.IndexNumber;
                        rs.FirstName = request.FirstName;
                        rs.LastName = request.LastName;
                        rs.BirthDate = request.BirthDate;
                        list.Add(rs);


                    }
                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                }
                con.Close();
            }




            return list;
        }


        public IEnumerable<EnrollStudentResponse> PromoteStudents(int semester, string studiesname)
        {
            var list = new List<EnrollStudentResponse>();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {

                com.Connection = con;

                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;

                try
                {

                    com.CommandText = "SELECT IdEnrollment from Enrollment INNER JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy WHERE Semester=@Semester ";
                    com.Parameters.AddWithValue("StudiesName", studiesname);
                    com.Parameters.AddWithValue("Semester", semester);
                    com.ExecuteNonQuery();
                    SqlDataReader exr = com.ExecuteReader();

                    var st = new Student();
                    var rs = new EnrollStudentResponse();
                    if (!exr.Read())
                    {

                        tran.Rollback();
                    }


                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                }
                con.Close();
            }




            return list;
        }

        public IEnumerable<Student> GetStudents()
        {

            var db = new s5554Context();

            var students = db.Student.ToList();

            return students;
        }

        public Student GetStudent(string id)
        {
            //var list = new List<Student>();
            var st = new Student();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "SELECT * FROM Student WHERE IndexNumber = @IndexNumber";
                com.Parameters.AddWithValue("IndexNumber", id);
                com.Connection = con;

                con.Open();
                SqlDataReader exr = com.ExecuteReader();

                if (exr.Read())
                {


                    st.IndexNumber = exr["IndexNumber"].ToString();
                    st.FirstName = exr["FirstName"].ToString();
                    st.LastName = exr["LastName"].ToString();
                    st.BirthDate = Convert.ToDateTime(exr["BirthDate"]);

                }
                con.Close();


            }
            return st;

        }

        public Student UpdateStudent(string id, string lastname)
        {
            var db = new s5554Context();

            // update

            var st1 = db.Student.Find(id);
            st1.LastName = lastname;

            db.SaveChanges();

            return st1;
        }

        public Student RemoveStudent(string id)
        {
            var db = new s5554Context();

            var s = new Student
            {
                IndexNumber = id
            };
            db.Attach(s);
            db.Remove(s);

            db.SaveChanges();

            return s;
        }


    }



}
