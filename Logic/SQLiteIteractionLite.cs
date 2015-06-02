/*   
 *  WebQA
 *  WebQA Server
 *  Copyright (C) Fuks Alexander 2013-2015
 *  
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *  
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
 *  
 *  Fuks Alexander, hereby disclaims all copyright
 *  interest in the program "WebQA"
 *  (which makes passes at compilers)
 *  written by Alexander Fuks.
 * 
 *  Alexander Fuks, 06 November 2013.
 */

using System;
using System.IO;
using System.Data.SQLite;
using System.Data;
using WebQA.Logic;

namespace WebQA
{
    /// <summary>
    /// Provides ability for connection and performing queries to SQLite databases
    /// </summary>
    internal static class SQLiteIteractionLite
    {
        #region variables

        /// <summary>
        /// Connection string
        /// </summary>
        public static string ConnectionString
        {
            get { return Connect.ConnectionString; }
            set { Disconnect(); Connect.ConnectionString = value; }
        }

        /// <summary>
        /// Last error message
        /// </summary>
        public static string LastErrorMessage { get; private set; }

        /// <summary>
        /// Error message from last query operation
        /// </summary>
        public static string LastQueryErrorMessage { get; private set; }

        /// <summary>
        /// Is tracing enabled
        /// </summary>
        public static bool TraceEnabled { get { return _traceenabled; } }

        /// <summary>
        /// Is connection should be kept open
        /// </summary>
        public static bool KeepOpen { get { return _keepopen; } }

        private static bool _traceenabled;
        private static bool _keepopen;
        private static readonly SQLiteConnection Connect = new SQLiteConnection();
        #endregion

        #region Methods_TestConnection

        /// <summary>
        /// Perform connection test
        /// </summary>
        /// <returns>True if connection can be established false if failed</returns>
        public static bool TestConnection()
        {
            return TestConnection(ConnectionString, false, null);
        }

        /// <summary>
        /// Perform connection test
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>True if connection can be established false if failed</returns>
        public static bool TestConnection(string connectionString)
        {
            return TestConnection(connectionString, false, null);
        }

        /// <summary>
        /// Perform connection test
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="saveIfSuccess">Save new connection string if success</param>
        /// <returns>True if connection can be established false if failed</returns>
        public static bool TestConnection(string connectionString, bool saveIfSuccess)
        {
            return TestConnection(connectionString, saveIfSuccess, null);
        }

        /// <summary>
        /// Perform connection test
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="saveIfSuccess">Save new connection string if success</param>
        /// <param name="isKeepOpen">Keep open connection open if connection will be established</param>
        /// <returns>True if connection can be established false if failed</returns>
        public static bool TestConnection(string connectionString, bool saveIfSuccess, bool? isKeepOpen)
        {
            using (var connect = new SQLiteConnection(connectionString))
            {
                try
                {
                    LastQueryErrorMessage = "";
                    connect.Open();
                    connect.Close();
                    if (saveIfSuccess) ConnectionString = connectionString;
                    if (isKeepOpen != null) _keepopen = (bool)isKeepOpen;
                    return true;
                }
                catch (Exception ex)
                {
                    RegisterError(ex.Message);
                    return false;
                }
            }
        }
        #endregion

        /// <summary>
        /// Check is connection is active now
        /// </summary>
        /// <returns>True if active, false if not</returns>
        public static bool IsActiveConnection()
        {
            return ((Connect.State == ConnectionState.Open) || (Connect.State == ConnectionState.Executing) || (Connect.State == ConnectionState.Fetching));
        }

        /// <summary>
        /// Close connection
        /// </summary>
        public static void Disconnect()
        {
            Connect.Close();
        }
        
        /// <summary>
        /// Switch on/off tracing
        /// </summary>
        /// <param name="enable">Enable tracing?</param>
        public static void SetTrace(bool enable)
        {
            _traceenabled = enable;
        }

        private static void AddTrace(string query)
        {
            try
            {
                Trace.Add(
                    string.Concat(
                    "SQL WARNING: ",
                    LastErrorMessage,
                    "   Query: ",
                    query),
                     Trace.Color.Red);
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message;
                _traceenabled = false;
            }
        }

        private static void RegisterError(string Message)
        {
            LastErrorMessage = Message;
            LastQueryErrorMessage = Message;
        }

        #region SELECT DATA

        /// <summary>
        /// Perform SELECT query
        /// </summary>
        /// <param name="query">Query statement</param>
        /// <param name="args">Query arguments</param>
        /// <returns>All found table</returns>
        public static DataTable SelectTable(string query, params SQLiteParameter[] args)
        {
            var table = new DataTable();
            try
            {
                LastQueryErrorMessage = "";
                if (Connect.State == ConnectionState.Closed) Connect.Open();
                var adapter = new SQLiteDataAdapter(query, Connect);
                if (args != null)
                {
                    foreach (var param in args)
                    {
                        adapter.SelectCommand.Parameters.Add(param);
                    }
                }
                adapter.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                RegisterError(ex.Message);
                if (_traceenabled) AddTrace(query);
                return null;
            }
            finally
            {
                if (!_keepopen) Connect.Close();
            }

        }

        /// <summary>
        /// Perform SELECT query
        /// </summary>
        /// <param name="query">Query statement</param>
        /// <param name="args">Query arguments</param>
        /// <returns>1st found row</returns>
        public static DataRow SelectRow(string query, params SQLiteParameter[] args)
        {
            var table = SelectTable(query, args);
            return table != null && table.Rows.Count == 1 ? table.Rows[0] : null;
        }

        /// <summary>
        /// Perform SELECT query
        /// </summary>
        /// <param name="query">Query statement</param>
        /// <param name="args">Query arguments</param>
        /// <returns>1st found column</returns>
        public static DataColumn SelectColumn(string query, params SQLiteParameter[] args)
        {
            var table = SelectTable(query, args);
            return table != null && table.Columns.Count == 1 ? table.Columns[0] : null;
        }

        /// <summary>
        /// Perform SELECT query
        /// </summary>
        /// <typeparam name="TReturnType">Expected data type</typeparam>
        /// <param name="query">Query statement</param>
        /// <param name="args">Query arguments</param>
        /// <returns>1st cell in 1st row of 1st column</returns>
        public static TReturnType SelectCell<TReturnType>(string query, params SQLiteParameter[] args)
        {
            var table = SelectTable(query, args);
            if (table == null || table.Rows.Count != 1 || table.Rows[0].ItemArray[0].GetType() != typeof(TReturnType))
            {
                throw new InvalidOperationException(string.Concat("Wrong query or type of cell is not equal type of ReturnType. Return type is equals ", table.Rows[0].ItemArray[0].GetType().ToString()));
            }
            return (TReturnType)table.Rows[0].ItemArray[0];
        }

        /// <summary>
        /// Perform SELECT query
        /// </summary>
        /// <typeparam name="TReturnType">Expected data type</typeparam>
        /// <param name="query">Query statement</param>
        /// <param name="DefReturnValue">Default value that was return by method</param>
        /// <param name="args">Query arguments</param>
        /// <returns>1st cell in 1st row of 1st column</returns>
        public static TReturnType SelectCell<TReturnType>(string query, TReturnType DefReturnValue = default(TReturnType), params SQLiteParameter[] args)
        {
            var table = SelectTable(query, args);
            if (table == null || table.Rows.Count == 0 || table.Rows[0].ItemArray[0].GetType() != typeof(TReturnType))
            {
                return DefReturnValue;
            }
            return (TReturnType)table.Rows[0].ItemArray[0];
        }

        #endregion

        #region CHANGE DATA

        /// <summary>
        /// Perform INSERT,UPDATE,DELETE etc queries
        /// </summary>
        /// <param name="query">Query statement</param>
        /// <param name="args">Query arguments</param>
        /// <returns>Number of affected rows</returns>
        public static int ChangeData(string query, params SQLiteParameter[] args)
        {
            try
            {
                LastQueryErrorMessage = "";
                if (Connect.State == ConnectionState.Closed) Connect.Open();
                var command = new SQLiteCommand(query, Connect);
                if (args != null)
                {
                    foreach (var param in args)
                    {
                        command.Parameters.Add(param);
                    }
                }
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                RegisterError(ex.Message);
                if (_traceenabled) AddTrace(query);
                return -1;
            }
            finally
            {
                if (!_keepopen) Connect.Close();
            }
        }
        #endregion
    }
}