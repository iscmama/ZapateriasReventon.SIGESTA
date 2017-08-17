﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ZapateriasReventon.SIGESTA.Main.Data
{
    public class SQLHelper
    {
        public static DataTable ExecuteStoredProcedure(string nameStoredProcedure, ParameterIn[] arrayParametersIn = null)
        {
            DataTable table = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SIGESTADB"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(nameStoredProcedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (arrayParametersIn != null)
                        {
                            foreach (ParameterIn parameter in arrayParametersIn)
                            {
                                cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
                            }
                        }

                        con.Open();

                        using (var da = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            da.Fill(table);
                        }
                    }
                }

                return table;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}