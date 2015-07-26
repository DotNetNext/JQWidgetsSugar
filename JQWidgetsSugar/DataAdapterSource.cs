using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQWidgetsSugar
{
    public class DataAdapterSource
    {
        public string Url { get; set; }
        public string EditUrl { get; set; }
        public string AddUrl { get; set; }
        public string DeleteUrl { get; set; }
        public List<GridDatafield> datafields { get; set; }
        public string datatype = "json";
    }
}
