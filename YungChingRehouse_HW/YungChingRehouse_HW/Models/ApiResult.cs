using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YungChingRehouse_HW.Models
{
    public class ApiResult<T>
    {
        /// <summary>
        /// 執行成果與否
        /// </summary>
        public bool IsSucc { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMsg { get; set; }

        public DateTime DataTime { get; set; }

        /// <summary>
        /// 回傳結果資料
        /// </summary>
        public T Data { get; set; }

        public ApiResult() 
        {
            IsSucc = true;
            ErrorMsg = string.Empty;
            DataTime = DateTime.Now;
        }
    }
}