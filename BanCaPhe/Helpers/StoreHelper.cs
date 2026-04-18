using Dapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace BanCaPhe.Models
{
    public static class StoreHelper
    {
    
        private static readonly ConcurrentDictionary<string, List<string>> _paramCache 
            = new ConcurrentDictionary<string, List<string>>();

        private static DynamicParameters MapParameters(string storeName, object? param)
        {
            if (param is DynamicParameters dp) return dp;
            
            var dynamicParams = new DynamicParameters();
            if (param == null) return dynamicParams;

       
            if (!_paramCache.TryGetValue(storeName, out var storeParams))
            {
                storeParams = DiscoverParameters(storeName);
                _paramCache.TryAdd(storeName, storeParams);
            }

           
            var properties = param.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
               
                string paramName = "@" + prop.Name;
                if (storeParams.Any(p => string.Equals(p, paramName, StringComparison.OrdinalIgnoreCase)))
                {
                    dynamicParams.Add(prop.Name, prop.GetValue(param));
                }
            }

            return dynamicParams;
        }

        private static List<string> DiscoverParameters(string storeName)
        {
            var paramsList = new List<string>();
            try
            {
                using (var conn = DoUongDbConnection.GetConnection() as SqlConnection)
                {
                    if (conn == null) return paramsList;

                    using (var cmd = new SqlCommand(storeName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        SqlCommandBuilder.DeriveParameters(cmd);
                        foreach (SqlParameter p in cmd.Parameters)
                        {
                            paramsList.Add(p.ParameterName);
                        }
                    }
                }
            }
            catch { }
            return paramsList;
        }

        public static int Execute(string storeName, object? param = null)
        {
            using (IDbConnection conn = DoUongDbConnection.GetConnection())
            {
                var processedParam = MapParameters(storeName, param);
                return conn.Execute(storeName, processedParam, commandType: CommandType.StoredProcedure);
            }
        }

        public static List<T> QueryList<T>(string storeName, object? param = null)
        {
            using (IDbConnection conn = DoUongDbConnection.GetConnection())
            {
                var processedParam = MapParameters(storeName, param);
                return conn.Query<T>(storeName, processedParam, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static T? QueryFirstOrDefault<T>(string storeName, object? param = null)
        {
            using (IDbConnection conn = DoUongDbConnection.GetConnection())
            {
                var processedParam = MapParameters(storeName, param);
                return conn.QueryFirstOrDefault<T>(storeName, processedParam, commandType: CommandType.StoredProcedure);
            }
        }

        public static T ExecuteScalar<T>(string storeName, object? param = null)
        {
            using (IDbConnection conn = DoUongDbConnection.GetConnection())
            {
                var processedParam = MapParameters(storeName, param);
                return conn.ExecuteScalar<T>(storeName, processedParam, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
