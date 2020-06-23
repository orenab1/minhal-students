using System.Collections.Generic;

namespace GovILApi.ViewModels
{
    public class DescriptionDataObj
    {
        public int descriptionSentenceCode { get; set; }
        public string descriptionSentencePlaceHoldersValues { get; set; }
        public DataPresentationTypeObj dataPresentationType { get; set; }
        public CTAValue[] ctaList { get; set; }
        public int urgent { get; set; }
        public FileIdObj[] files { get; set; }
    }
}