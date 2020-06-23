using System.Collections.Generic;

namespace GovILApi.ViewModels
{
    public class CTAValue
    {
        public int ctaTitleSentenceCode { get; set; }
        public string ctaTitleSentenceCodePlaceHoldersValues { get; set; }
        public int linkId { get; set; }
        public int presentationType { get; set; }
        public KeyValuesCTAObj[] ctaKeyValues { get; set; }
    }
}