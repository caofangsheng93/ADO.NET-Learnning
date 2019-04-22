using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AutoLotConnectedLayer;
using System.Data;

namespace AutoLotCUIClient
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             I:插入新的记录到Inventory表中
             U:更新Inventory表的记录
             D:从Inventory表中删除记录
             L:使用数据读取器显示当前库存
             S:为用户显示这些选项
             P:根据汽车ID查询昵称
             Q:退出程序
             */

           
            Console.WriteLine("***** The AutoLot Console UI *****");

            //从配置文件中读取连接字符串
            string constr = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;
            bool userDone = false;
            string userCommand = "";


            InventoryDAL dal = new InventoryDAL();
            dal.ProcessCreditRisk(false, 1);
            dal.OpenConnection(constr);

            //不断请求，直到用户按下Q
            try
            {
                ShowInstructions();
                do
                {
                    Console.WriteLine("\n请输入指令：");
                    userCommand = Console.ReadLine();
                    Console.ReadLine();
                    switch (userCommand.ToUpper())
                    {
                        case "I":
                            InsertCar(dal);
                            break;
                        case "U":
                            UpdateCarPetName(dal);
                            break;
                        case "D":
                            DeleteCar(dal);
                            break;
                        case "L":
                            ListInventoryVisList(dal);
                            break;
                        case "S":
                            ShowInstructions();
                            break;
                        case "P":
                            LookUpPetName(dal);
                            break;
                        case "Q":
                            userDone = true;
                            break;
                        default:
                            Console.WriteLine("数据错误，请重试！");
                            break;
                    }

                } while (!userDone);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dal.CloseConnection();
            }

            Console.ReadKey();
        }

        #region  显示指令
        /// <summary>
        /// 显示指令
        /// </summary>
        static void ShowInstructions()
        {
            Console.WriteLine("I:Inser a new car");
            Console.WriteLine("U:Updates an existing car");
            Console.WriteLine("D:Deletes an existing car");
            Console.WriteLine("L:List current inventory");
            Console.WriteLine("S:Show these instructions");
            Console.WriteLine("P:Looks Up pet Name");
            Console.WriteLine("Quit Program");
        }
        #endregion

        #region 获取库存列表Datatable
        /// <summary>
        /// 获取库存列表Datatable
        /// </summary>
        /// <param name="dal"></param>
        static void ListInventory(InventoryDAL dal)
        {
            ///*获取库存列表*/
            DataTable dt = dal.GetALLInventoryTable();
            //将datatable传递给辅助函数用于显示数据
            DisplayTable(dt);
        } 
        #endregion

        #region 显示数据
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="dt"></param>
        static void DisplayTable(DataTable dt)
        {
            //输出列名
            for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
            {
                Console.WriteLine(dt.Columns[curCol].ColumnName + "\t");
            }
            Console.WriteLine("\n---------------------------------------");
            //输出datatable
            for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
            {
                for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
                {
                    Console.WriteLine(dt.Rows[curRow][curCol].ToString());
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region 获取库存列表List
        /// <summary>
        /// 获取库存列表List
        /// </summary>
        /// <param name="dal"></param>
        static void ListInventoryVisList(InventoryDAL dal)
        {
            List<Car> lstCar = dal.GetALLInventoryList();
            foreach (Car item in lstCar)
            {
                Console.WriteLine("CarID:{0},Make:{1},Color:{2},PetName:{3}", item.CarID, item.Make, item.Color, item.PetName);
            }
        }
        #endregion

        #region 删除CAR
        /// <summary>
        /// 删除CAR
        /// </summary>
        /// <param name="dal"></param>
        static void DeleteCar(InventoryDAL dal)
        {
            //获取要删除的carID
            Console.WriteLine("请输入要删除的CarID");
            int id = int.Parse(Console.ReadLine());
            //以防违反，引用完整性
            try
            {
                dal.DelteCar(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 插入数据
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="dal"></param>
        static void InsertCar(InventoryDAL dal)
        {
            //获取用户数据
            int newCarID;
            string newCarColor, newCarMake, newCarPetName;
            Console.WriteLine("Enter car ID:");
            newCarID = int.Parse(Console.ReadLine());

            Console.WriteLine("输入Color:");
            newCarColor = Console.ReadLine();

            Console.WriteLine("输入Car Make:");
            newCarMake = Console.ReadLine();

            Console.WriteLine("输入Pet Name:");
            newCarPetName = Console.ReadLine();

            dal.InsertAuto(newCarID, newCarMake, newCarColor, newCarPetName);
        }
        #endregion

        #region 更新PetName 
        /// <summary>
        /// 更新PetName 
        /// </summary>
        /// <param name="dal"></param>
        static void UpdateCarPetName(InventoryDAL dal)
        {
            //获取用户数据
            int carId;
            string newCarPetName;

            Console.WriteLine("输入Car ID:");
            carId = int.Parse(Console.ReadLine());
            Console.WriteLine("输入Pet Name");
            newCarPetName = Console.ReadLine();
            dal.UpdateCarPetName(carId, newCarPetName);
        }
        #endregion

        #region 查询Pet Name
        /// <summary>
        /// 查询Pet Name
        /// </summary>
        /// <param name="dal"></param>
        static void LookUpPetName(InventoryDAL dal)
        {
            Console.WriteLine("请输入要查找的CarID:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("PetName of {0} is {1}", id, dal.LookUpPetName(id).TrimEnd());
        } 
        #endregion
    }
}
