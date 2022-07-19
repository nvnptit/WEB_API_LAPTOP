﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace WEB_API_LAPTOP.Helper
{
    public class SQLHelper
    {
        private readonly String connectionString;
        public SQLHelper(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public SqlConnection Connection()
        {

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        public DataTable SelectQuery(string strSQL)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            cn.Close();
            return dt;
        }

        public int ExcuteStamentQuery(string strSQL)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            int rs = 0;
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(strSQL, cn);
                rs = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            cn.Close();
            return rs;
        }

        public DataTable ExecuteQuery(string spName, List<SqlParameter> listpara)
        {
            DataTable dt = new DataTable();
            SqlConnection con = Connection();
            try
            {
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                if (spName != null)
                {
                    foreach (SqlParameter para in listpara)
                    {
                        command.Parameters.Add(para);
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            con.Close();
            return dt;
        }

        public int ExecuteNoneQuery(string spName, List<SqlParameter> listpara)
        {
            int n = -1;
            SqlConnection con = Connection();
            try
            {
                SqlCommand command = new SqlCommand(spName, con);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                if (listpara != null)
                {
                    foreach (SqlParameter para in listpara)
                        command.Parameters.Add(para);
                }
                n = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            con.Close();
            return n;
        }

        public DataSet ExcuteQueryDataSet(string sp, List<SqlParameter> listpara)
        {
            DataSet dts = new DataSet();
            SqlConnection con = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand(sp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                if (listpara != null)
                {
                    foreach (SqlParameter para in listpara)
                        cmd.Parameters.Add(para);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            con.Close();
            return dts;
        }
        public DataTable ExecuteString(string sql, List<SqlParameter> listpara = null)
        {
            DataTable dt = new DataTable();
            SqlConnection con = Connection();
            try
            {
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                if (sql != null && listpara != null)
                {
                    foreach (SqlParameter para in listpara)
                    {
                        command.Parameters.Add(para);
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            con.Close();
            return dt;
        }
    }
}
