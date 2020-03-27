using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace musicDBManagementSystem.project.repository {
    public class DataBaseRepository {
        private readonly SqlConnection connection;
        
        /// <summary>
        /// Open connection to DataBase with given specifications
        /// </summary>
        /// <param name="server"> Name of Server </param>
        /// <param name="database"> Name of DataBase </param>
        /// <param name="user"> Name of User </param>
        /// <param name="password"> Password - not stored </param>
        public DataBaseRepository(string server, string database, string user, string password) {
            connection = new SqlConnection($"Server={server};Database={database};User Id={user};Password={password}");
            connection.Open();
            /*
            Console.WriteLine("Successfully connected to db");
            Console.WriteLine(connection.ConnectionTimeout);
            Console.WriteLine(connection.ConnectionString);
            */
        }

        /// <summary>
        /// Return SqlDataAdapter of given query
        /// </summary>
        /// <param name="select"> Columns to select </param>
        /// <param name="tableName"> Table to select from </param>
        /// <param name="where"> Condition - Can be null todo </param>
        /// <returns type="SqlDataAdapeter"></returns>
        public DataRowCollection selectFromTableWhere(string select, string tableName, string where) {
            string commandText = $"select {select} from {tableName}";
            if (where != null) {
                commandText += $" where {where}";
            }
            //Console.WriteLine($"\n\t\tSCRIPT:\n{commandText}");
            DataSet dataSet = new DataSet(tableName);
            SqlDataAdapter adapter = new SqlDataAdapter(commandText, this.connection);
            adapter.Fill(dataSet, tableName);
            return dataSet.Tables[tableName].Rows;
        }

        /// <summary>
        /// Execute insert statement
        /// </summary>
        /// <param name="tableName"> Name of table to insert into </param>
        /// <param name="of"> List of columns of table - can be null</param>
        /// <param name="values"> List of values to insert like (value1, value2, value3) </param>
        /// <returns type="int"> Number of affected rows </returns>
        public int insertIntoTableValues(string tableName, string of, List<string> values) {
            string commandText = $"insert into {tableName + of}";
            for (int index = 0; index < values.Count - 1; index += 1) {
                commandText += $" values {values[index]}, ";
            } commandText += $" values {values[values.Count - 1]};";
            
            SqlCommand command = new SqlCommand(commandText, this.connection);
            //SqlDataAdapter adapter = new SqlDataAdapter($"select * from {tableName}", this.connection)
            //    {InsertCommand = new SqlCommand(commandText, this.connection)};
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute update statement
        /// </summary>
        /// <param name="tableName"> Name of table to update </param>
        /// <param name="set"> Columns to update like column1 = value2, column2 = value2 </param>
        /// <param name="where"> Condition - Can be null </param>
        /// <returns type="int"> Number of affected rows </returns>
        public int updateTableSetWhere(string tableName, string set, string where) {
            string commandText = $"update {tableName} set {set}";
            if (where != null) {
                commandText += $" where {where}";
            }
            SqlCommand command = new SqlCommand(commandText, this.connection);
            //SqlDataAdapter adapter = new SqlDataAdapter($"select * from {tableName}", this.connection)
            //    {UpdateCommand = new SqlCommand(commandText, this.connection)};
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute delete statement
        /// </summary>
        /// <param name="tableName"> Name of table to delete from </param>
        /// <param name="where"> Condition - Can be null </param>
        /// <returns type="int"> Number of affected rows </returns>
        public int deleteFromTableWhere(string tableName, string where) {
            string commandText = $"delete from {tableName}";
            if (where != null) {
                commandText += $" where {where}";
            }
            SqlCommand command = new SqlCommand(commandText, this.connection);
            //SqlDataAdapter adapter = new SqlDataAdapter($"select * from {tableName}", this.connection)
            //    {DeleteCommand = new SqlCommand(commandText, this.connection)};
            return command.ExecuteNonQuery();
        }
        
        /// <summary>
        /// Close connection to DataBase
        /// Must be called before End Of Program
        /// </summary>
        public void close() {
            this.connection.Close();
        }
    }
}