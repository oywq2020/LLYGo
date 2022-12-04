using System;
using System.Collections.Generic;
using System.Text;
using _core.AcountInfo;
using MySql.Data.MySqlClient;
using UnityEditorInternal;
using UnityEngine;
using CharacterInfo = UnityEngine.CharacterInfo;

namespace MysqlUtility
{
    public enum DatabaseTable
    {
        PlayerInfoTable,
        CharacterInfoTable
    }
        
    //this static class must counteract the this enum above
    public static class TableSet
    {
        private static string[] TableNam =
        {
            "20203233欧阳文庆_info",
            "20203233欧阳文庆_character"
        };

        public static string GetTableName(DatabaseTable table)
        {
            return TableNam[(int) table];
        }
    }

    public class MysqlTool
    {
        private static MySqlConnection _mySqlConnection;

        //database info for connecting
        private static string ConnectStr
            = "server=127.0.0.1;port=3306;database=vrdb_1;user=root;password=123456";


        public static string GetPassword(string account)
        {
            //fixed format
            string sql = $"select password from 20203233欧阳文庆_info where account = '{account}'";

            OpenDatabase();

            MySqlCommand cmd = new MySqlCommand(sql, _mySqlConnection);

            var mySqlDataReader = cmd.ExecuteReader();
            if (mySqlDataReader.Read())
            {
                var s = mySqlDataReader.GetString("password");
                CloseDataBase();
                return s;
            }
            else
            {
                CloseDataBase();
                return "";
            }
        }


        public static bool AddPlayer(string account, string password, string sex, string bornYear)
        {
            //fixed format
            string sql = $"insert into 20203233欧阳文庆_info(account,password,sex,bornyear) " +
                         $"values('{account}','{password}','{sex}',{bornYear})";

            OpenDatabase();
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, _mySqlConnection);
                var executeNonQuery = cmd.ExecuteNonQuery();
                CloseDataBase();
                return executeNonQuery != 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "sql:" + sql);
                CloseDataBase();
                throw;
            }
        }


        public static bool UpdateData(string[] columnNames, string[] newData, string account)
        {
            if (columnNames.Length != newData.Length) return false;
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE 20203233欧阳文庆_info SET ");

            for (int i = 0; i < columnNames.Length; i++)
            {
                sql.Append(columnNames[i] + "=" + newData[i] + " ");
            }

            sql.Append($"WHERE account = {account}");


            OpenDatabase();
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql.ToString(), _mySqlConnection);
                var executeNonQuery = cmd.ExecuteNonQuery();
                CloseDataBase();
                return executeNonQuery != 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "sql:" + sql);
                CloseDataBase();
                return false;
            }
        }


        public static T GetInfoByKey<T>(string key, DatabaseTable databaseTable = DatabaseTable.PlayerInfoTable) where T : new()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");

            var info = new T();

            var propertyInfos = typeof(T).GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                sql.Append(propertyInfo.Name + ",");
            }

            //remove the superfluous comma in the end of string builder
            sql.Remove(sql.Length - 1, 1);

            sql.Append($" FROM {TableSet.GetTableName(databaseTable)} WHERE account = '{key}'");

            OpenDatabase();
            

            MySqlCommand cmd = new MySqlCommand(sql.ToString(), _mySqlConnection);

            var mySqlDataReader = cmd.ExecuteReader();
            if (mySqlDataReader.Read())
            {
                var dataReader = mySqlDataReader;

                foreach (var propertyInfo in propertyInfos)
                {
                    var value = mySqlDataReader.GetString(propertyInfo.Name);
                    propertyInfo.SetValue(info, value);
                }
                CloseDataBase();
                return info;
            }

            CloseDataBase();
            return default;
        }

        public static int UpdateDataByKey<T>(string key,T newInfo,DatabaseTable databaseTable = DatabaseTable.PlayerInfoTable)
            where T : new()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append($"UPDATE {TableSet.GetTableName(databaseTable)} SET ");

            var propertyInfos = typeof(T).GetProperties();
            
            foreach (var propertyInfo in propertyInfos)
            {
                var value = propertyInfo.GetValue(newInfo);

                if (value!=null&& (string)value!="")
                {
                    //this data is modified 
                    sql.Append(propertyInfo.Name + "=" + $"'{(string) value}'"+",");
                }
            }
            
            //remove the superfluous comma in the end of string builder
            sql.Remove(sql.Length - 1, 1);
            sql.Append($" WHERE account = '{key}'");

          
            OpenDatabase();
            MySqlCommand cmd = new MySqlCommand(sql.ToString(), _mySqlConnection);

            var executeNonQuery = cmd.ExecuteNonQuery();

            CloseDataBase();
            
            //return the number of rows effected
            return executeNonQuery;
        }


        public static int UpdatePassword(string key,string newPassword)
        {
            string sql = $"UPDATE 20203233欧阳文庆_info set password = '{newPassword}' where account = '{key}'";
            OpenDatabase();
            MySqlCommand cmd = new MySqlCommand(sql, _mySqlConnection);
            var executeNonQuery = cmd.ExecuteNonQuery();
            CloseDataBase();
            return executeNonQuery;
        }
        
        
        private static void OpenDatabase()
        {
            try
            {
                _mySqlConnection = new MySqlConnection(ConnectStr);
                _mySqlConnection.Open();
            }
            catch (Exception e)
            {
                Debug.Log(ConnectStr);
                Console.WriteLine(e);
                throw;
            }
        }

        private static void CloseDataBase()
        {
            _mySqlConnection.Close();
            _mySqlConnection.Dispose();
        }
    }
}