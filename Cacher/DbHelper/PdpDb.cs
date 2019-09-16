using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NServiceBus.Logging;
using Poller.Messages.Models;

namespace Cacher.DbHelper
{
    public sealed class PdpDb : IDisposable
    {
        private readonly MySqlConnection _mySqlConnection;
        private readonly ILog _logger;

        public PdpDb(string connectionString)
        {
            _mySqlConnection = new MySqlConnection(connectionString);
            _logger = LogManager.GetLogger<PdpDb>();
        }

        /// <summary>
        /// insert item to db.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<int> Insert(Customer customer)
        {
            using (var command = _mySqlConnection.CreateCommand())
            {
                command.CommandText = $@"INSERT INTO t_customer (id, customer_name, customer_type) SELECT
	                                        ?id,
	                                        ?customer_name,
	                                        ?customer_type
                                        FROM
	                                        DUAL
                                        WHERE
	                                        NOT EXISTS (
		                                        SELECT
			                                        id
		                                        FROM
			                                        t_customer
		                                        WHERE
			                                        id = ?id
	                                        )";
                command.Parameters.AddWithValue("id", customer.Id);
                command.Parameters.AddWithValue("customer_name", customer.CustomerName);
                command.Parameters.AddWithValue("customer_type", customer.CustomerType);
                if (await OpenConnectionAsync())
                {
                    return await Task.Run(() => command.ExecuteNonQuery());
                }
            }

            return -1;
        }

        private async Task<bool> OpenConnectionAsync()
        {
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Mysql connection error.", ex);
                return false;
            }
        }

        public void Dispose()
        {
            _mySqlConnection?.Close();
            _mySqlConnection?.Dispose();
        }
    }
}
