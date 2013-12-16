using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DatabaseApplication.Database
{
    class SqliteDatabase : IDisposable
    {
        private static object lockObj = new Object();

        private List<string> statementList;

        private string dbConnection;
        private string dbPlacement;

        private bool connected = false;

        private bool disposed;

        public bool Connected
        {
            get { return this.connected; }
            set
            {
                if (this.connected != value)
                {
                    this.connected = value;
                }
            }
        }

        public SqliteDatabase(string user)
        {
            statementList = new List<string>();

            dbPlacement = Calamity.Properties.Settings.Default.pathToDatabase + user + "\\playlist.sqlite";

            if (!dbPlacement.Equals(""))
            {
                dbConnection = "Data Source=" + dbPlacement + ";Version=3;";
            }

            connected = true;
            disposed = false;
        }

        public string DatabasePlacement
        {
            get { return this.dbPlacement; }
        }

        public string DatabaseConnection
        {
            get { return this.dbConnection; }
        }

        public void SQLiteDatabaseWithOptions(Dictionary<string, string> connectionOpts)
        {
            String str = "";
            foreach (KeyValuePair<string, string> row in connectionOpts)
            {
                str += String.Format("{0}={1}; ", row.Key, row.Value);
            }
            str = str.Trim().Substring(0, str.Length - 1);
            dbConnection = str;
        }

        public DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
                {
                    cnn.Open();

                    using (SQLiteCommand mycommand = new SQLiteCommand(sql, cnn))
                    {
                        using (SQLiteDataReader reader = mycommand.ExecuteReader())
                        {
                            dt.Load(reader);
                            reader.Close();
                        }
                    }
                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : " + sql + " // Message :" + e.Message);
                throw new Exception(e.Message);
            }

            return dt;
        }

        public int ExecuteNonQuery(string sql)
        {
            int rowsUpdated = 0;
            try
            {
                using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
                {
                    cnn.Open();

                    using (SQLiteCommand mycommand = new SQLiteCommand(sql, cnn))
                    {
                        rowsUpdated = mycommand.ExecuteNonQuery();
                    }
                    cnn.Close();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error : " + sql + " // Message :" + ex.Message);
            }


            return rowsUpdated;
        }

        public string ExecuteScalar(string sql)
        {
            object value = null;

            using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();

                using (SQLiteCommand mycommand = new SQLiteCommand(sql, cnn))
                {
                    value = mycommand.ExecuteScalar();
                }
                cnn.Close();
            }

            if (value != null)
            {
                return value.ToString();
            }

            return "";
        }

        public string ExecuteScalar(string sql, SQLiteConnection cnn)
        {
            object value = null;

            using (SQLiteCommand mycommand = new SQLiteCommand(sql, cnn))
            {
                value = mycommand.ExecuteScalar();
            }

            if (value != null)
            {
                return value.ToString();
            }

            return "";
        }

        public bool Update(String tableName, Dictionary<string, string> data, String where)
        {
            String vals = "";
            Boolean returnCode = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<string, string> val in data)
                {
                    vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                }
                vals = vals.Substring(0, vals.Length - 1);
            }
            try
            {
                this.ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        public bool Update(String tableName, Dictionary<string, MyValue> data, String where)
        {
            String vals = "";
            Boolean returnCode = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<string, MyValue> val in data)
                {
                    if (val.Value.Value == "null" || val.Value.Value == "")
                    {
                        vals += String.Format(" {0},", "null");
                    }
                    else
                    {
                        switch (val.Value.Type)
                        {
                            case DbType.Int64:
                                vals += String.Format(" {0} = {1},", val.Key.ToString(), val.Value.Value);
                                break;
                            case DbType.Int32:
                                vals += String.Format(" {0} = {1},", val.Key.ToString(), val.Value.Value);
                                break;
                            case DbType.Int16:
                                vals += String.Format(" {0} = {1},", val.Key.ToString(), val.Value.Value);
                                break;
                            case DbType.DateTime:
                                vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.Value);
                                break;
                            case DbType.String:
                                vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.Value);
                                break;
                            case DbType.Double:
                                vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.Value.Replace(',', '.'));
                                break;
                            default:
                                vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.Value);
                                break;
                        }
                    }
                }
            }
            vals = vals.Substring(0, vals.Length - 1);
            try
            {
                this.ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        public bool Delete(String tableName, String where)
        {
            Boolean returnCode = true;
            try
            {
                this.ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where));
            }
            catch (Exception fail)
            {
                returnCode = false;
            }
            return returnCode;
        }

        public bool Insert(String tableName, Dictionary<string, string> data)
        {
            String columns = "";
            String values = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<string, string> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());
                values += String.Format(" '{0}',", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                this.ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
            }
            catch (Exception fail)
            {
                returnCode = false;
            }
            return returnCode;
        }

        public bool Insert(String tableName, Dictionary<string, MyValue> data)
        {
            String columns = "";
            String values = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<string, MyValue> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());

                if (val.Value.Value == "null" || val.Value.Value == "")
                {
                    values += String.Format(" {0},", "null");
                }
                else
                {
                    switch (val.Value.Type)
                    {
                        case DbType.Int64:
                            values += String.Format(" {0},", val.Value.Value);
                            break;
                        case DbType.Int32:
                            values += String.Format(" {0},", val.Value.Value);
                            break;
                        case DbType.Int16:
                            values += String.Format(" {0},", val.Value.Value);
                            break;
                        case DbType.DateTime:
                            values += String.Format(" '{0}',", val.Value.Value);
                            break;
                        case DbType.String:
                            values += String.Format(" '{0}',", val.Value.Value);
                            break;
                        case DbType.Double:
                            values += String.Format(" '{0}',", val.Value.Value.Replace(',', '.'));
                            break;
                        default:
                            values += String.Format(" '{0}',", val.Value.Value);
                            break;
                    }
                }
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                this.ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
            }
            catch (Exception fail)
            {
                returnCode = false;
            }
            return returnCode;
        }

        public bool ClearDB()
        {
            DataTable tables;
            try
            {
                tables = this.GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");
                foreach (DataRow table in tables.Rows)
                {
                    this.ClearTable(table["NAME"].ToString());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ClearTable(String table)
        {
            try
            {
                this.ExecuteNonQuery(String.Format("delete from {0};", table));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GenerateInsertStatement(String tableName, Dictionary<string, MyValue> data)
        {
            String columns = "";
            String values = "";

            foreach (KeyValuePair<string, MyValue> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());

                if (val.Value.Value == "null" || val.Value.Value == "")
                {
                    values += String.Format(" {0},", "null");
                }
                else
                {
                    switch (val.Value.Type)
                    {
                        case DbType.Int64:
                            values += String.Format(" {0},", val.Value.Value);
                            break;
                        case DbType.Int32:
                            values += String.Format(" {0},", val.Value.Value);
                            break;
                        case DbType.Int16:
                            values += String.Format(" {0},", val.Value.Value);
                            break;
                        case DbType.DateTime:
                            values += String.Format(" '{0}',", val.Value.Value);
                            break;
                        case DbType.String:
                            values += String.Format(" '{0}',", val.Value.Value);
                            break;
                        case DbType.Double:
                            values += String.Format(" '{0}',", val.Value.Value.Replace(',', '.'));
                            break;
                        default:
                            values += String.Format(" '{0}',", val.Value.Value);
                            break;
                    }
                }
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);

            return String.Format("insert into {0}({1}) values({2});", tableName, columns, values);
        }

        public string GeneratUpdateStatement(String tableName, Dictionary<string, MyValue> data, String where)
        {
            String vals = "";

            if (data.Count >= 1)
            {
                foreach (KeyValuePair<string, MyValue> val in data)
                {
                    if (val.Value.Value == "null" || val.Value.Value == "")
                    {
                        vals += String.Format(" {0} = {1},", val.Key.ToString(), "null");
                    }
                    else
                    {
                        switch (val.Value.Type)
                        {
                            case DbType.Int64:
                                vals += String.Format(" {0} = {1},", val.Key.ToString(), val.Value.Value);
                                break;
                            case DbType.Int32:
                                vals += String.Format(" {0} = {1},", val.Key.ToString(), val.Value.Value);
                                break;
                            case DbType.Int16:
                                vals += String.Format(" {0} = {1},", val.Key.ToString(), val.Value.Value);
                                break;
                            case DbType.DateTime:
                                vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.Value);
                                break;
                            case DbType.String:
                                vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.Value);
                                break;
                            case DbType.Double:
                                double newDouble;
                                bool good = false;

                                if (!(double.TryParse(val.Value.Value, out newDouble)))
                                {
                                    if (double.TryParse(val.Value.Value.Replace('.', ','), out newDouble) || newDouble > 0)
                                    {
                                        good = true;
                                        vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.Value);
                                    }

                                    if (!good)
                                    {
                                        if (double.TryParse(val.Value.Value.Replace(',', '.'), out newDouble) || newDouble > 0)
                                        {
                                            vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.Value.Replace(',', '.'));
                                        }
                                    }
                                }
                                else
                                {
                                    vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.Value.Replace(',', '.'));
                                }
                                break;
                            default:
                                vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.Value);
                                break;
                        }
                    }
                }
            }
            vals = vals.Substring(0, vals.Length - 1);

            return String.Format("update {0} set {1} where {2};", tableName, vals, where);
        }

        public void createTables()
        {
            string[] sql = {
                           "create table audio_files(" +
                           "audio_id       INTEGER      not null, " +
                           "path           VARCHAR(255) ," +
                           "name           VARCHAR(255) ," +
                           "primary key (audio_id)" +
                           ")",
                           
                           "create table video_files(" +
                           "video_id       INTEGER      not null, " +
                           "path           VARCHAR(255) ," +
                           "name           VARCHAR(255) ," +
                           "primary key (video_id)" +
                           ")",
                           
                           "create table playlist(" +
                           "playlist_id       INTEGER       not null, " +
                           "name              VARCHAR(255)  ," +
                           "primary key (playlist_id)" +
                           ")",

                           "create table playlists(" +
                           "audio_id       INTEGER  not null, " +
                           "playlist_id    INTEGER  not null," +
                           "primary key (audio_id, playlist_id), " +
                           "foreign key (audio_id) references audio_files (audio_id) ," +
                           "foreign key (playlist_id) references playlist (playlist_id)" +
                           ")"

                           };


            for (int i = 0; i < sql.Length; i++)
            {
                addStatement(sql[i]);
            }

            run();
        }




        public void addStatement(string statement)
        {
            lock (statementList)
            {
                this.statementList.Add(statement);
            }
        }

        public void removeStatement(string statement)
        {
            lock (statementList)
            {
                this.statementList.Remove(statement);
            }
        }

        public List<string> getStatementList()
        {
            lock (statementList)
            {
                return statementList;
            }
        }

        public void run()
        {
            lock (statementList)
            {
                using (SQLiteConnection dbConnection = new SQLiteConnection(DatabaseConnection))
                {
                    dbConnection.Open();

                    using (SQLiteTransaction trans = dbConnection.BeginTransaction())
                    {
                        try
                        {
                            foreach (string statement in statementList)
                            {
                                using (SQLiteCommand command = new SQLiteCommand(statement, dbConnection))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                            statementList.Clear();
                        }
                        catch (SQLiteException e)
                        {
                            Debug.WriteLine(e.Message);

                            if (trans != null)
                            {
                                trans.Rollback();
                            }

                        }
                        finally
                        {
                            dbConnection.Close();
                        }
                    }
                }
            }

        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    statementList = null;
                    dbPlacement = null;
                    dbConnection = null;
                    lockObj = null;
                    connected = false;
                }

                connected = false;
                disposed = true;
            }
        }

    }
}
