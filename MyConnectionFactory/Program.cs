using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace MyConnectionFactory
{
    class Program
    {

        #region Main
        static void Main(string[] args)
        {
            Console.WriteLine("***** 非常简单的连接工厂 *****");
              
            //获取某个连接
            //IDbConnection conn= GetConnection(DataProvider.SqlServer);

            //读取提供程序键
            string dataProvString = ConfigurationManager.AppSettings["provider"];

            //把字符串转换为枚举
            DataProvider dp = DataProvider.None;
            if (Enum.IsDefined(typeof(DataProvider), dataProvString))
            {
                dp = (DataProvider)Enum.Parse(typeof(DataProvider), dataProvString);
            }
            else
            {
                Console.WriteLine("Sorry, no provider exists!");
            }
            //获取某个连接
            IDbConnection conn= GetConnection(dp);
            if (conn != null)
            {
                Console.WriteLine("你的连接对象是:{0}", conn.GetType().Name);
            }
            //打开、使用和关闭连接


            Console.ReadKey();
        } 
        #endregion

        #region  根据DataProvider的枚举值获取连接对象
        /// <summary>
        /// 根据DataProvider的枚举值获取连接对象
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        static IDbConnection GetConnection(DataProvider dp)
        {
            IDbConnection conn = null;
            switch (dp)
            {
                case DataProvider.SqlServer:
                    conn = new SqlConnection();
                    break;
                case DataProvider.OleDb:
                    conn = new OleDbConnection();
                    break;
                case DataProvider.Odbc:
                    conn = new OdbcConnection();
                    break;
            }
            return conn;
        } 
        #endregion
    }
    enum DataProvider
    {
        SqlServer,
        OleDb,
        Odbc,
        None
    }
}
