using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;

namespace PRDXC.Core.Modules.Database
{
    public class Database
    {
        public string Connection { get; set; }

        public Database(string con)
        {
            Connection = con;
        }

        public async Task Query(string query)
        {
            try
            {
                var Connection = new MySqlConnection(this.Connection);
                await Connection.OpenAsync();
                await new MySqlCommand(query, Connection).ExecuteNonQueryAsync();
                await Connection.CloseAsync();
                await Connection.DisposeAsync();
            }
            catch (Exception ex)
            {
                await Resource.Logger.Write(ex.Message, Logger.LogType.Error);
            }
        }

        public async Task<DataTable> QueryResult(string query)
        {
            try
            {
                var Connection = new MySqlConnection(this.Connection);
                var result = new DataTable();
                await Connection.OpenAsync();
                var cmd = new MySqlCommand(query, Connection);
                await cmd.ExecuteNonQueryAsync();
                new MySqlDataAdapter(cmd).Fill(result);
                await Connection.CloseAsync();
                await Connection.DisposeAsync();
                return result;
            }
            catch (Exception ex)
            {
                await Resource.Logger.Write(ex.Message, Logger.LogType.Error);
            }
            return null;
        }

        public async Task Query(MySqlCommand cmd)
        {
            try
            {
                var Connection = new MySqlConnection(this.Connection);
                cmd.Connection = Connection;
                await Connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await Connection.CloseAsync();
                await Connection.DisposeAsync();
            }
            catch (Exception ex)
            {
                await Resource.Logger.Write(ex.Message, Logger.LogType.Error);
            }
        }

        public async Task<DataTable> QueryResult(MySqlCommand cmd)
        {
            try
            {
                var Connection = new MySqlConnection(this.Connection);
                cmd.Connection = Connection;
                var result = new DataTable();
                await Connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                new MySqlDataAdapter(cmd).Fill(result);
                await Connection.CloseAsync();
                await Connection.DisposeAsync();
                return result;
            }
            catch (Exception ex)
            {
                await Resource.Logger.Write(ex.Message, Logger.LogType.Error);
            }
            return null;
        }
    }
}
