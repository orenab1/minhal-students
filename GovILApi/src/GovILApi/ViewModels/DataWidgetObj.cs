using System.Collections.Generic;

namespace GovILApi.ViewModels
{
    public class DataWidgetObj
    {
        public int dataWidgetId { get; set; }
        public DataWidgetInfoObj[] dataWidgetInfo { get; set; }
        public DataWidgetExtraInfoObj[] dataWidgetExtraInfo { get; set; }
    }
}