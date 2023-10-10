using System.Collections.Specialized;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using FirstApi_AdoNet.Controllers;
using FirstApi_AdoNet.Models;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace FirstApi_AdoNet.DAL
{
    public class StudentDAL
    {
        private readonly string _config;

        public StudentDAL(IConfiguration config) 
        {
            _config = config.GetConnectionString("addcon");
        }

        public DataTable GetData(string SpName, NameValueCollection NV)
        {

            SqlConnection con = new SqlConnection();
            string dbTyper = "";
            try
            {
                con.ConnectionString = _config;
                DataTable dt = new DataTable();
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = SpName;
                cmd.CommandTimeout = 999999999;


                if (NV != null)
                {
                    //New code implemented for retrieving data
                    for (int i = 0; i < NV.Count; i++)
                    {
                        string[] arraySplit = NV.Keys[i].Split('-');

                        if (arraySplit.Length > 2)
                        {
                            dbTyper = "SqlDbType." + arraySplit[1].ToString() + "," + arraySplit[2].ToString();

                            if (NV[i].ToString() == "NULL")
                            {
                                cmd.Parameters.AddWithValue(arraySplit[0].ToString(), dbTyper).Value = DBNull.Value;
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(arraySplit[0].ToString(), dbTyper).Value = NV[i].ToString();
                            }
                        }
                        else
                        {
                            dbTyper = "SqlDbType." + arraySplit[1].ToString();

                            if (NV[i].ToString() == "NULL")
                            {
                                cmd.Parameters.AddWithValue(arraySplit[0].ToString(), dbTyper).Value = DBNull.Value;
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(arraySplit[0].ToString(), dbTyper).Value = NV[i].ToString();
                            }
                        }
                    }
                }

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);

                con.Close();
                da.Dispose();

                return dt;
            }
            catch (Exception ex)
            {
                //SaveErrorLogs(System.Reflection.MethodBase.GetCurrentMethod(), ex);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public DataTable GetStudentById(string SpName, NameValueCollection NV ,int studentId)
        {
            SqlConnection con = new SqlConnection(_config);
            DataTable dt = new DataTable();

            try
            {
                con.ConnectionString = _config;
                DataTable dts = new DataTable();
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = SpName;
                cmd.CommandTimeout = 999999999;

                // Add a parameter for studentId
                SqlParameter param = new SqlParameter("@StudentId", SqlDbType.Int);
                param.Value = studentId;
                cmd.Parameters.Add(param);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                // Handle the exception or log it.
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return dt;
        }


        public DataTable CreateStudent(string SpName, NameValueCollection NV, string Sname)
        {
            SqlConnection con = new SqlConnection(_config);
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = SpName;
                cmd.CommandTimeout = 999999999;

                // Add a parameter for student name
                cmd.Parameters.AddWithValue("@Sname", Sname);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // Handle the exception or log it.
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return dt;
        }
       




    }
}
