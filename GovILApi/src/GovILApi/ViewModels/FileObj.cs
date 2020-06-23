using System;
using System.Collections.Generic;

namespace GovILApi.ViewModels
{
    public class FileObj
    {
        public FileIdObj fileId { get; set; }
        public string fileName { get; set; }
        public int fileType { get; set; }
        public long fileSize { get; set; }
        public int fileTitleSentenceCode { get; set; }
        public string fileTitleSentenceCodePlaceHoldersValues { get; set; }
        public DateTime fileDate { get; set; }
        public DateTime fileCreationDate { get; set; }
        public int[] fileTags { get; set; }
    }
}