using System;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace MysqlUtility
{
    
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
           
            MySqlCommand cmd = new MySqlCommand(sql,_mySqlConnection);
            
            var mySqlDataReader = cmd.ExecuteReader();
            if ( mySqlDataReader.Read())
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
    
    
        public static bool AddPlayer(string account ,string password,string sex,string bornYear)
        {
            //fixed format
            string sql = $"insert into 20203233欧阳文庆_info(account,password,sex,bornyear) " +
                         $"values('{account}','{password}','{sex}',{bornYear})";
            
            OpenDatabase();
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql,_mySqlConnection);
                var executeNonQuery = cmd.ExecuteNonQuery();
                CloseDataBase();
                return executeNonQuery != 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e+"sql:"+sql);
                CloseDataBase();
                throw;
            }
          
           
          
        }


        public static bool AddGameData()
        {
            return false;
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
