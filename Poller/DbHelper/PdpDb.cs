using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using NServiceBus.Logging;

namespace Poller.DbHelper
{
    public sealed class PdpDb : IDisposable
    {
        private readonly SqlConnection _dbConnection;
        private readonly ILog _logger;

        public PdpDb(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
            _logger = LogManager.GetLogger(GetType());
        }

        public void Dispose()
        {
            _dbConnection?.Close();
            _dbConnection?.Dispose();
        }

        /// <summary>
        /// Query list of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetListAsync<T>(string sql, object @params = null)
        {
            if (await OpenConnectionAsync())
            {
                return await _dbConnection.QueryAsync<T>(sql, @params);
            }

            return null;
        }

        public async Task<int> UpdateStatusAsync(string sql, object @params = null)
        {
            if (await OpenConnectionAsync())
            {
                return await _dbConnection.ExecuteAsync(sql, @params);
            }

            return -1;
        }

        private async Task<bool> OpenConnectionAsync()
        {
            try
            {
                if (_dbConnection.State != ConnectionState.Open)
                {
                    await _dbConnection.OpenAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Open db connection error.", ex);
                return false;
            }
        }
    }
}
