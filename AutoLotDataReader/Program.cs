using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AutoLotDataReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Fun with Data Readers");

            //string constr = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;

            //通过SqlConnectionStringBuilder对象来构建连接字符串
            SqlConnectionStringBuilder constrBuilder = new SqlConnectionStringBuilder();
            constrBuilder.DataSource = ".";
            constrBuilder.InitialCatalog = "AutoLot";
            constrBuilder.UserID = "sa";
            constrBuilder.Password = "Password_1";

            /*ADO.NET连接数据库的方式*/
            //1创建连接对象[配置连接字符串]
            using (SqlConnection conn = new SqlConnection(constrBuilder.ConnectionString))
            {
                //2.打开连接
                conn.Open();

                ShowConnectionStatus(conn);

                //3.创建命令对象
                SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = "Select * From Inventory";
                cmd.CommandText = "Select * From Inventory;select * from Customers";
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                //4.发送命令
                #region SqlDataReader 读取一个结果集
                //using (SqlDataReader reader = cmd.ExecuteReader())
                //{
                //    //5.处理数据
                //    while (reader.Read())
                //    {
                //        //Console.WriteLine("-->Make:{0},PetName:{1},Color:{2}",
                //        //reader["Make"],reader["PetName"],reader["Color"]);
                //        //防止通过硬编码列名，可以这样：
                //        Console.WriteLine("**** Record ***");
                //        for (int i = 0; i < reader.FieldCount; i++)
                //        {
                //            Console.WriteLine("{0}={1}", reader.GetName(i), reader.GetValue(i));
                //        }
                //    }
                //} 
                #endregion

                List<Inventory> lstInventory = new List<Inventory>();
                List<Customers> lstCustomers = new List<Customers>();
                int index = 0;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("*** Record ***");
                    do
                    {
                        //如果有数据
                        while (reader.Read())
                        {
                            switch (index)
                            {
                                case 0:
                                    Inventory InvenModel = new Inventory();
                                    InvenModel.CarID = Convert.ToInt32(reader["CarID"]);
                                    InvenModel.Make = reader["Make"].ToString();
                                    InvenModel.Color = reader["Color"].ToString();
                                    InvenModel.PetName = reader["PetName"].ToString();
                                    lstInventory.Add(InvenModel);
                                    break;
                                case 1:
                                    Customers CustModel = new Customers();
                                    CustModel.CustID= Convert.ToInt32(reader["CustID"]);
                                    CustModel.FirstName= reader["FirstName"].ToString();
                                    CustModel.LastName = reader["LastName"].ToString();
                                    lstCustomers.Add(CustModel);
                                    break;
                            }
                        }
                        index++;
                    }
                    while (reader.NextResult());
                }

            }
            Console.ReadKey();
        }

        #region 显示连接对象信息
        /// <summary>
        /// 显示连接对象信息
        /// </summary>
        /// <param name="conn"></param>
        static void ShowConnectionStatus(SqlConnection conn)
        {
            //显示当前连接对象的各种状态
            Console.WriteLine("**** Info about your connection ****");
            Console.WriteLine("Database location:{0}", conn.DataSource);
            Console.WriteLine("Database Name:{0}", conn.Database);
            Console.WriteLine("Conncetion TimeOut:{0}", conn.ConnectionTimeout);
            Console.WriteLine("Connection State:{0}", conn.State);
            /*
             * 1.说明ConnectionState枚举尽管有多个值，但只有Open和Closed是有效的额，其他的枚举成员为将来的使用做保留；
             * 2.如果连接状态是Closed,再去关闭一次，连接对象不会出现什么问题；
             */
        }
        #endregion
    }
}
