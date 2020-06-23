using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovILApi.ViewModels
{
    public class GetFileRequest
    {
        public PersonIdObj[] personId { get; set; }
        public FileIdObj fileId { get; set; }
    }
}
