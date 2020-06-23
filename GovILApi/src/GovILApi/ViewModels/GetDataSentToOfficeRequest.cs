using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovILApi.ViewModels
{
    public class GetDataSentToOfficeRequest
    {
        public PersonIdObj[] personId { get; set; }
        public DataWidgetObj[] dataWidgets { get; set; }
    }
}
