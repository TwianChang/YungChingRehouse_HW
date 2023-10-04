using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YungChingRehouse_HW.Models;

namespace YungChingRehouse_HW.Controllers
{
    public class OrderController : ApiController
    {
        /// <summary>
        /// 查詢客戶訂單
        /// 可以查全部或特定
        /// 輸入ALL為全部
        /// </summary>
        [HttpGet]
        public ApiResult<List<Order>> Get([FromUri]string cusid)
        {
            SqlConnection sqlconn = new SqlConnection(Properties.Settings.Default.DBConnStr);
            
            SqlCommand sqlcom;

            //預存名稱
            string spnm = "QueryOrder";

            ApiResult<List<Order>> result = new ApiResult<List<Order>>();

            try
            {
                sqlconn.Open();
                sqlcom = new SqlCommand("",sqlconn);
                sqlcom.CommandTimeout = 1000;
                sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcom.CommandText = spnm;
                sqlcom.Parameters.AddWithValue("@CustomerID",cusid);

                SqlDataReader reader = sqlcom.ExecuteReader();

                while (reader.Read())
                {
                    if(result.Data == null)
                        result.Data = new List<Order>();

                    Order order = new Order();
                    order.OrderID = Convert.ToInt32( reader["OrderID"] );
                    order.ContactName = reader["ContactName"].ToString();
                    order.CustomerID = reader["CustomerID"].ToString();
                    order.ProductName = reader["ProductName"].ToString();
                    order.ProductID = reader["ProductID"].ToString();
                    order.Quantity = Convert.ToInt32( reader["Quantity"] );
                    order.UnitPrice = Convert.ToDecimal( reader["UnitPrice"] );
                    order.FirstName = reader["FirstName"].ToString();
                    order.LastName = reader["LastName"].ToString();
                    order.EmployeeID = Convert.ToInt32(reader["EmployeeID"]);
                    order.OrderDate = Convert.ToDateTime( reader["OrderDate"] );

                    result.Data.Add( order );
                }
            }
            catch ( Exception ex )
            {
                result.IsSucc = false;
                result.ErrorMsg = ex.StackTrace + ex.Message;
            }
            finally
            {
                sqlconn.Close();
            }

            return result;
        }

        /// <summary>
        /// 新增一筆訂單
        /// </summary>
        /// <param name="_order"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<List<Order> > Post([FromBody] Order _order)
        {
            SqlConnection sqlconn = new SqlConnection(Properties.Settings.Default.DBConnStr);

            SqlCommand sqlcom;

            //預存名稱
            string spnm = "CreateOrder";

            ApiResult<List<Order>> result = new ApiResult<List<Order>>();

            try
            {
                sqlconn.Open();
                sqlcom = new SqlCommand( "" , sqlconn );
                sqlcom.CommandTimeout = 1000;
                sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcom.CommandText = spnm;
                sqlcom.Parameters.AddWithValue("@CustomerID", _order.CustomerID);
                sqlcom.Parameters.AddWithValue("@EmployeeID", _order.EmployeeID);
                sqlcom.Parameters.AddWithValue("@ProductID", _order.ProductID);
                sqlcom.Parameters.AddWithValue("@Quantity", _order.Quantity);
                sqlcom.Parameters.AddWithValue("@UnitPrice", _order.UnitPrice);
                sqlcom.Parameters.AddWithValue("@OrderDate", _order.OrderDate);

                sqlcom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result.IsSucc = false;
                result.ErrorMsg = ex.StackTrace + ex.Message;
            }
            finally
            {
                sqlconn.Close();
            }

            return result;
        }

        /// <summary>
        /// 修改一筆訂單
        /// </summary>
        /// <param name="_order"></param>
        /// <returns></returns>
        [HttpPatch]
        public ApiResult<List<Order>> Patch([FromBody] Order _order)
        {
            SqlConnection sqlconn = new SqlConnection(Properties.Settings.Default.DBConnStr);

            SqlCommand sqlcom;

            //預存名稱
            string spnm = "UpdateOrder";

            ApiResult<List<Order>> result = new ApiResult<List<Order>>();

            try
            {
                sqlconn.Open();
                sqlcom = new SqlCommand("", sqlconn);
                sqlcom.CommandTimeout = 1000;
                sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcom.CommandText = spnm;
                sqlcom.Parameters.AddWithValue("@OrderId", _order.OrderID);
                sqlcom.Parameters.AddWithValue("@ProductID", _order.ProductID);
                sqlcom.Parameters.AddWithValue("@Quantity", _order.Quantity);

                sqlcom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result.IsSucc = false;
                result.ErrorMsg = ex.StackTrace + ex.Message;
            }
            finally
            {
                sqlconn.Close();
            }

            return result;
        }

        /// <summary>
        /// 修改一筆訂單
        /// </summary>
        /// <param name="_order"></param>
        /// <returns></returns>
        [HttpDelete]
        public ApiResult<List<Order>> Delete([FromBody] Order _order)
        {
            SqlConnection sqlconn = new SqlConnection(Properties.Settings.Default.DBConnStr);

            SqlCommand sqlcom;

            //預存名稱
            string spnm = "DeleteOrder";

            ApiResult<List<Order>> result = new ApiResult<List<Order>>();

            try
            {
                sqlconn.Open();
                sqlcom = new SqlCommand("", sqlconn);
                sqlcom.CommandTimeout = 1000;
                sqlcom.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcom.CommandText = spnm;
                sqlcom.Parameters.AddWithValue("@OrderId", _order.OrderID);

                sqlcom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result.IsSucc = false;
                result.ErrorMsg = ex.StackTrace + ex.Message;
            }
            finally
            {
                sqlconn.Close();
            }

            return result;
        }
    }
}
