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
    public class CustomerController : ApiController
    {
        /// <summary>
        /// 取得客戶資訊
        /// 查詢全部客戶資訊，router最後帶all
        /// 查詢特定客戶資訊，router最後帶CustomerID
        /// router : GET api/Customer/cusid=XXX
        /// </summary>
        /// <returns> 如果查無資料會回傳NULL </returns>
        [HttpGet]
        public ApiResult< List<Customers> > Get([FromUri]string cusid)
        {
            SqlConnection sqlconn = new SqlConnection( Properties.Settings.Default.DBConnStr );

            //查詢全部資料
            string QueryAll_sqlcmd = "SELECT * FROM [Northwind].[dbo].[Customers];";
            //查詢特定資料
            string QuerySpec_sqlcmd = "SELECT * FROM [Northwind].[dbo].[Customers] WHERE CustomerID = @CustomerID;";
            SqlCommand sqlcom;

            ApiResult< List<Customers> > result = new ApiResult< List<Customers> >();

            try
            {
                sqlconn.Open();

                //查詢全部客戶資訊，不加here條件
                //反之加上Where條件
                if (cusid.ToLower().Trim() != "all")
                {
                    sqlcom = new SqlCommand(QuerySpec_sqlcmd, sqlconn);
                    sqlcom.Parameters.AddWithValue("@CustomerID", cusid);
                }
                else
                {
                    sqlcom = new SqlCommand(QueryAll_sqlcmd, sqlconn);
                }

                sqlcom.CommandTimeout = 300;
                SqlDataReader sqlreader = sqlcom.ExecuteReader();

                result.IsSucc = true;

                while ( sqlreader.Read() )
                {
                    if (result.Data == null)
                        result.Data = new List<Customers>();

                    Customers item = new Customers();

                    item.CustomerID = sqlreader["CustomerID"].ToString();
                    item.CompanyName = sqlreader["CompanyName"].ToString();
                    item.ContactName = sqlreader["ContactName"].ToString();
                    item.ContactTitle = sqlreader["ContactTitle"].ToString();
                    item.Address = sqlreader["Address"].ToString();
                    item.City = sqlreader["City"].ToString();
                    item.Region = sqlreader["Region"].ToString();
                    item.PostalCode = sqlreader["PostalCode"].ToString();
                    item.Country = sqlreader["Country"].ToString();
                    item.Phone = sqlreader["Phone"].ToString();
                    item.Fax = sqlreader["Fax"].ToString();

                    result.Data.Add( item );
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
        /// 建立新的客戶資訊
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public ApiResult<List<Customers>> Post([FromBody] Customers _newcus)
        {
            SqlConnection sqlconn = new SqlConnection(Properties.Settings.Default.DBConnStr);

            ApiResult<List<Customers>> result = new ApiResult<List<Customers>>();

            string sqlcmd = @"
                INSERT INTO [dbo].[Customers]
                (
		            [CustomerID]
		            ,[CompanyName]
		            ,[ContactName]
		            ,[ContactTitle]
		            ,[Address]
		            ,[City]
		            ,[Region]
		            ,[PostalCode]
		            ,[Country]
		            ,[Phone]
		            ,[Fax]
	            )
                VALUES
                (
		            @CustomerID
		            ,@CompanyName
		            ,@ContactName
		            ,@ContactTitle
		            ,@Address
		            ,@City
		            ,@Region
		            ,@PostalCode
		            ,@Country
		            ,@Phone
		            ,@Fax
	            )";
            SqlCommand sqlcom;

            try
            {
                //先確認這個CustomerID是否存在
                //存在就是新增失敗，回傳CustomerID已經存在訊息
                //不存在就新增
                ApiResult<List<Customers>> checkcusid = Get(_newcus.CustomerID);

                if ( checkcusid.Data == null )
                {
                    sqlconn.Open();
                    sqlcom = new SqlCommand(sqlcmd, sqlconn);
                    sqlcom.CommandTimeout = 1000;

                    sqlcom.Parameters.AddWithValue("@CustomerID", _newcus.CustomerID);
                    sqlcom.Parameters.AddWithValue("@CompanyName", _newcus.CompanyName);
                    sqlcom.Parameters.AddWithValue("@ContactName", _newcus.ContactName);
                    sqlcom.Parameters.AddWithValue("@ContactTitle", _newcus.ContactTitle);
                    sqlcom.Parameters.AddWithValue("@Address", _newcus.Address);
                    sqlcom.Parameters.AddWithValue("@City", _newcus.City);
                    sqlcom.Parameters.AddWithValue("@Region", _newcus.Region);
                    sqlcom.Parameters.AddWithValue("@PostalCode", _newcus.PostalCode);
                    sqlcom.Parameters.AddWithValue("@Country", _newcus.Country);
                    sqlcom.Parameters.AddWithValue("@Phone", _newcus.Phone);
                    sqlcom.Parameters.AddWithValue("@Fax", _newcus.Fax);

                    sqlcom.ExecuteNonQuery();
                }
                else
                {
                    result.IsSucc = false;
                    result.ErrorMsg = "This CustomerID already exists.";
                }
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
        /// 修改客戶資訊
        /// </summary>
        /// <param name="_oldcus"></param>
        /// <returns></returns>
        [HttpPatch]
        public ApiResult<List<Customers>> Patch([FromBody] Customers _oldcus)
        {
            SqlConnection sqlconn = new SqlConnection(Properties.Settings.Default.DBConnStr);

            ApiResult<List<Customers>> result = new ApiResult<List<Customers>>();

            string sqlcmd = @"
                UPDATE [dbo].[Customers]
                SET 
	                [CustomerID]    = @CustomerID
                    ,[CompanyName]  = @CompanyName
                    ,[ContactName]  = @ContactName
                    ,[ContactTitle] = @ContactTitle
                    ,[Address]      = @Address
                    ,[City]         = @City
                    ,[Region]       = @Region
                    ,[PostalCode]   = @PostalCode
                    ,[Country]	  = @Country
                    ,[Phone]        = @Phone
                    ,[Fax]          = @Fax
                WHERE [CustomerID] = @CustomerID";
            SqlCommand sqlcom;

            try
            {
                //先確認這個CustomerID是否存在
                //存在就是能更新，回傳更新是否成功
                //不存在就不能更新，回傳不存在訊息
                ApiResult<List<Customers>> checkcusid = Get( _oldcus.CustomerID );

                if (checkcusid.Data == null )
                {
                    result.IsSucc = false;
                    result.ErrorMsg = "This CustomerID doesn't exists.You can't update.";
                }
                else
                {
                    sqlconn.Open();
                    sqlcom = new SqlCommand(sqlcmd, sqlconn);
                    sqlcom.CommandTimeout = 1000;

                    sqlcom.Parameters.AddWithValue("@CustomerID", _oldcus.CustomerID);
                    sqlcom.Parameters.AddWithValue("@CompanyName", _oldcus.CompanyName);
                    sqlcom.Parameters.AddWithValue("@ContactName", _oldcus.ContactName);
                    sqlcom.Parameters.AddWithValue("@ContactTitle", _oldcus.ContactTitle);
                    sqlcom.Parameters.AddWithValue("@Address", _oldcus.Address);
                    sqlcom.Parameters.AddWithValue("@City", _oldcus.City);
                    sqlcom.Parameters.AddWithValue("@Region", _oldcus.Region);
                    sqlcom.Parameters.AddWithValue("@PostalCode", _oldcus.PostalCode);
                    sqlcom.Parameters.AddWithValue("@Country", _oldcus.Country);
                    sqlcom.Parameters.AddWithValue("@Phone", _oldcus.Phone);
                    sqlcom.Parameters.AddWithValue("@Fax", _oldcus.Fax);

                    sqlcom.ExecuteNonQuery();
                }               
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
        /// 刪除客戶資訊
        /// </summary>
        /// <param name="_oldcus"></param>
        /// <returns></returns>
        [HttpDelete]
        public ApiResult<List<Customers>> Delete([FromBody] Customers _oldcus)
        {
            SqlConnection sqlconn = new SqlConnection(Properties.Settings.Default.DBConnStr);

            ApiResult<List<Customers>> result = new ApiResult<List<Customers>>();

            string sqlcmd = @"
                DELETE FROM [dbo].[Customers]
                WHERE [CustomerID] = @CustomerID";
            SqlCommand sqlcom;

            try
            {
                //先確認這個CustomerID是否存在
                //存在就是能刪除失敗
                //不存在就是不能刪除
                ApiResult<List<Customers>> checkcusid = Get(_oldcus.CustomerID);

                if (checkcusid.Data == null)
                {
                    result.IsSucc = false;
                    result.ErrorMsg = "This CustomerID doesn't exists.You can't delete.";
                }
                else
                {
                    sqlconn.Open();
                    sqlcom = new SqlCommand(sqlcmd, sqlconn);
                    sqlcom.CommandTimeout = 1000;

                    sqlcom.Parameters.AddWithValue("@CustomerID", _oldcus.CustomerID);

                    sqlcom.ExecuteNonQuery();
                }       
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
