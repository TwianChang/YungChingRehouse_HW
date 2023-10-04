using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YungChingRehouse_HW.Models
{
    public class Customers
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerID { get; set; }

        /// <summary>
        /// 客戶的公司名稱
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 聯絡人姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 聯絡人職稱
        /// </summary>
        public string ContactTitle { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 縣市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 區
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// 國家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 電話號碼
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 傳真號碼
        /// </summary>
        public string Fax { get; set; }
    }
}