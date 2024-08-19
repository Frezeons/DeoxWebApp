using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DeoxWebApp.Models
{
    public class DataLayer
    {
        private static SqlConnection _connection = new SqlConnection("Data Source=800166NB\\MSSQLSERVER01;Initial Catalog=UserInfo;Integrated Security=True;");

        public DataSet Select(string szCommand, Dictionary<string, object> parameters = null)
        {
            try
            {
                DataSet dataSet = new DataSet();

                using (SqlCommand command = new SqlCommand(szCommand, _connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    _connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataSet);
                    }

                    _connection.Close();
                }

                return dataSet;
            }
            catch (Exception ex)
            {
                _connection.Close();
                return null;
            }
        }

        public bool Delete(string query, string paramName, object value)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue(paramName, value);

                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    _connection.Close();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                _connection.Close();
                return false;
            }
        }

        public bool Insert(string szCommand, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(szCommand, _connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    _connection.Close();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Hata ile karşılaşılırsa loglanabilir veya handle edilebilir
                _connection.Close();

                return false;
            }
        }

        public bool Update(string szCommand, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(szCommand, _connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    _connection.Close();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                _connection.Close();

                // Hata ile karşılaşılırsa loglanabilir veya handle edilebilir
                return false;
            }
        }

    }
}

