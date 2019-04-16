using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;
using System.Data;

namespace DataProviderFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun With Data Provider Factories*******");

            //从配置文件中获取连接字符串和提供程序
            string dp = ConfigurationManager.AppSettings["provider"];
            //string constr = ConfigurationManager.AppSettings["Constr"];
            string constr = ConfigurationManager.ConnectionStrings["AutoLotOleDbProvider"].ConnectionString;
            //得到工厂提供程序
            DbProviderFactory df = DbProviderFactories.GetFactory(dp);

            //1.获取连接对象[设置连接字符串]
            using (DbConnection conn= df.CreateConnection())
            {
                Console.WriteLine("你的连接对象是:{0}",conn.GetType().Name);
                conn.ConnectionString = constr;

                //2.打开连接
                conn.Open();

                //3.创建命令对象[设置命名对象的属性]
                DbCommand cmd= df.CreateCommand();
                Console.WriteLine("你的命令对象是:{0}",cmd.GetType().Name);
                cmd.Connection = conn;
                cmd.CommandText = "Select * from Inventory";
                cmd.CommandType = CommandType.Text;
                //4.发送命令
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    //5.处理数据
                    Console.WriteLine("你的数据读取器对象是:{0}",reader.GetType().Name);

                    Console.WriteLine("\n***** Current Inventory ****");
                    while (reader.Read())
                    {
                        Console.WriteLine("-->Car #{0} is a {1}",reader["CarID"],reader["Make"]);
                    }

                }

            }

            Console.ReadKey();
        }
    }
}
