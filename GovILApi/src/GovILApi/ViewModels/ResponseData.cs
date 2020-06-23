using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovILApi.ViewModels
{
    public class ResponseData
    {
        public UserDataObj[] userDataList { get; set; }
        public ErrorCode errorCode { get; set; }
    }
}
