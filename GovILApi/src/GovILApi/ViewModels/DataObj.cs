using System;
using System.Collections.Generic;

namespace GovILApi.ViewModels
{
    public class DataObj
    {
        public int dataType { get; set; }
        public int titleSentenceCode { get; set; }
        public string titleSentencePlaceHoldersValues { get; set; }
        public DescriptionDataObj[] descriptionDataList { get; set; }
        public int urgent { get; set; }
        public string dataDate { get; set; }
        public int[] tags { get; set; }
    }
}