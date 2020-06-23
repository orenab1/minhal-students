using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovILApi.ViewModels
{
    public class GetFileResponse
    {
        public byte[] binaryFile { get; set; }
        public ErrorCode errorCode { get; set; }
        public string idNum { get; set; }
    }
}
