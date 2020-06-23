using System.Collections.Generic;

namespace GovILApi.ViewModels
{
    public class DataWidgetInfoObj
    {
        public bool dataWidgetCheckBox { get; set; }
        public int[] dataWidgetMultipleSelection { get; set; }
        public string dataWidgetInputNumber { get; set; }
        public string dataWidgetInputString { get; set; }
        public string dataWidgetInputDate { get; set; }
    }
}