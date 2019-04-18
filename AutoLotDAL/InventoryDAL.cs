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
    }
}
