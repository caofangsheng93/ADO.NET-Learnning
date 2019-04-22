using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//我们会使用SQL Server提供程序，然而使用ADO.NET工厂模式来获得更好的灵活性也是可以的
using System.Data;
using System.Data.SqlClient;
namespace AutoLotConnectedLayer
{
    /*
     * 前言：事务的特性：原子性（全有或全无）、一致性（数据在整个事务中保持稳定）、隔离性（事务之间不会相互影响）以及持久性（事务会被保存和记录日志）。
     1. .NET平台以各种形式支持事务。对于ADO.NET[本章]来说，最重要的就是ADO.NET数据提供程序的事务对象
     （在这里就是System.Data.SqlClient中的SqlTransaction）.此外，.NET基础类库提供了许多API来支持事务。
     **.System.EnterpriseServices:这个命名空间（位于System.EnterpriseServices.dll程序集）提供的类型允许我们和COM+运行库结合使用，包括对分布式事务的支持。
     **.System.Transactions:这个命名空间（位于System.Transactions.dll程序集）包含的类允许我们为各种服务写事务性应用程序和资源管理器（如MSMQ、ADO.NET、COM+等）。
     **.WCF（Windows Communication Foundation）：WCF API提供的服务使服务便于处理各种分布式绑定类。 
     除了.NET基础类库中现成的事务支持外，还可以使用数据管理系统中的SQL语言本身来实现事务。
     例如，我们可以编写存储过程来利用Begin Transaction、Rollback和Commit语句。
     */

    public class InventoryDAL
    {
        //这个成员会被所有方法使用
        private SqlConnection conn = null;

        #region 打开连接
        /// <summary>
        /// 打开连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public void OpenConnection(string connectionString)
        {
            conn = new SqlConnection();
            conn.ConnectionString = connectionString;
            conn.Open();
        }
        #endregion

        #region 关闭连接
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnection()
        {
            conn.Close();
        }
        #endregion

        #region 插入数据到Inventory表中
        /// <summary>
        /// 插入数据到Inventory表中
        /// </summary>
        /// <param name="id">CarID</param>
        /// <param name="make">Make</param>
        /// <param name="color">Color</param>
        /// <param name="petName">PetName</param>
        public bool InsertAuto(int id, string make, string color, string petName)
        {
            bool result = false;
            //格式化并且执行SQL语句
            string sql = string.Format("Insert Into Inventory(CarID,Make,Color,PetName)values('{0}','{1}','{2}','{3}');", id, make, color, petName);
            //创建并 打开连接
            OpenConnection("server=.;database=AutoLot;uid=sa;pwd=Password_1");
            //创建命令
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            //执行命令
            result = cmd.ExecuteNonQuery() > 0 ? true : false;
            //关闭连接
            CloseConnection();
            return result;
        }
        #endregion

        #region 插入数据到Inventory表中
        /// <summary>
        /// 插入数据到Inventory表中
        /// </summary>
        /// <param name="model">Car对象</param>
        /// <returns></returns>
        public bool InsertAuto(Car model)
        {
            bool result = false;
            //格式化sql
            string sql = string.Format("Insert Into Inventory(CarID,Make,Color,PetName)values('{0}','{1}','{2}','{3}')", model.CarID, model.Make, model.Color, model.PetName);
            //创建并 打开连接
            OpenConnection("server=.;database=AutoLot;uid=sa;pwd=Password_1");
            //创建命令
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            //执行命令
            result = cmd.ExecuteNonQuery() > 0 ? true : false;
            //关闭连接
            CloseConnection();
            return result;
        }
        #endregion

        #region 根据ID删除Inventory表记录
        /// <summary>
        /// 根据ID删除Inventory表记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DelteCar(int id)
        {
            bool result = false;
            string sql = string.Format("Delete from Inventory where CarID='{0}'", id);
            //创建并 打开连接
            OpenConnection("server=.;database=AutoLot;uid=sa;pwd=Password_1");
            //创建命令
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            //执行命令
            result = cmd.ExecuteNonQuery() > 0 ? true : false;
            //关闭连接
            CloseConnection();
            return result;
        }
        #endregion

        #region  更新汽车名称
        /// <summary>
        /// 更新汽车名称
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newPetName"></param>
        /// <returns></returns>
        public bool UpdateCarPetName(int id, string newPetName)
        {
            bool result = false;
            string sql = string.Format("Update Inventory Set PetName='{0}' Where CarID='{1}'", newPetName, id);
            //创建并 打开连接
            OpenConnection("server=.;database=AutoLot;uid=sa;pwd=Password_1");
            //创建命令
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            //执行命令
            result = cmd.ExecuteNonQuery() > 0 ? true : false;
            //关闭连接
            CloseConnection();
            return result;
        }
        #endregion

        #region 获取汽车数据
        /// <summary>
        /// 获取汽车数据
        /// </summary>
        /// <returns></returns>
        public List<Car> GetALLInventoryList()
        {
            List<Car> lstModel = new List<Car>();
            string sql = "select * from Inventory";
            //创建并 打开连接
            OpenConnection("server=.;database=AutoLot;uid=sa;pwd=Password_1");
            //创建命令
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstModel.Add(new Car()
                {
                    CarID = Convert.ToInt32(reader["CarID"]),
                    Color = reader["Color"].ToString(),
                    Make = reader["Make"].ToString(),
                    PetName = reader["PetName"].ToString(),
                });
            }
            CloseConnection();
            reader.Close();

            return lstModel;
        }
        #endregion

        #region 获取汽车数据
        /// <summary>
        /// 获取汽车数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetALLInventoryTable()
        {
            DataTable dt = new DataTable();
            string sql = "select * from Inventory";
            //创建并 打开连接
            OpenConnection("server=.;database=AutoLot;uid=sa;pwd=Password_1");
            //创建命令
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            CloseConnection();
            reader.Close();
            return dt;
        }
        #endregion

        #region 根据CarID获取PetName
        /// <summary>
        /// 根据CarID获取PetName
        /// </summary>
        /// <param name="carID"></param>
        /// <returns></returns>
        public string LookUpPetName(int carID)
        {
            string carPetName = string.Empty;
            //创建并 打开连接
            OpenConnection("server=.;database=AutoLot;uid=sa;pwd=Password_1");
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "GetPetName";
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                //输入参数[默认为input参数]
                SqlParameter parCarID = new SqlParameter("@CarID", carID);
                cmd.Parameters.Add(parCarID);

                //输出参数
                SqlParameter parName = new SqlParameter();
                parName.ParameterName = "@PetName";
                parName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parName);

                cmd.ExecuteNonQuery();

                //返回输出参数
                carPetName = (string)cmd.Parameters["@PetName"].Value;
            }
            return carPetName;
        }
        #endregion

        /// <summary>
        /// 用这个方法演示事务
        /// </summary>
        /// <param name="throwEx"></param>
        /// <param name="custID"></param>
        public void ProcessCreditRisk(bool throwEx, int custID)
        {
            //首先，根据客户ID查询用户信息
            string firstName = string.Empty;
            string lastName = string.Empty;
            //创建并 打开连接
            OpenConnection("server=.;database=AutoLot;uid=sa;pwd=Password_1");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from Customers where CustID=@CustID";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@CustID", custID));
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();//读取一行
                    firstName = reader["FirstName"].ToString();
                    lastName = reader["LastName"].ToString();
                }
                else
                {
                    return;
                }
            }
            //模拟事务，删除Customer表中的一行，同时在CreditRisks表中添加一行
            SqlCommand cmdRemove = new SqlCommand();
            cmdRemove.CommandText = "Delete from Customers where CustID=@CustID";
            cmdRemove.Connection = conn;
            cmdRemove.CommandType = CommandType.Text;
            cmdRemove.Parameters.Add(new SqlParameter("@CustID", custID));

            SqlCommand cmdInsert = new SqlCommand();
            cmdInsert.CommandText = "Insert Into CreditRisks(CustID,FirstName,LastName)values(@CustID,@FirstName,@LastName)";
            cmdInsert.Connection = conn;
            cmdInsert.CommandType = CommandType.Text;

            List<SqlParameter> lstPar = new List<SqlParameter>();
            lstPar.Add(new SqlParameter("@CustID",custID));
            lstPar.Add(new SqlParameter("@FirstName", firstName));
            lstPar.Add(new SqlParameter("@LastName", lastName));
            cmdInsert.Parameters.AddRange(lstPar.ToArray());

            //定义一个事务变量
            SqlTransaction tx = null;
            try
            {
                //开始一个事务
               tx= conn.BeginTransaction();
               //把命令加入到事务中
               cmdRemove.Transaction = tx;
               cmdInsert.Transaction = tx;

               int a= cmdRemove.ExecuteNonQuery();
               int b = cmdInsert.ExecuteNonQuery();
               //模拟错误
               if (throwEx)
               {
                    throw new Exception("sorry,数据库错误，事务失败。。。");
               }
                //提交事务
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                tx.Rollback();
            }

        }
    }
}
