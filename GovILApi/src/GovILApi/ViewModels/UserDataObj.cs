using System.Collections.Generic;

namespace GovILApi.ViewModels
{
    public class UserDataObj
    {
        public string idNum { get; set; }
        public DataObj[] dataList { get; set; }
        public ErrorCode errorCode { get; set; }
    }
}