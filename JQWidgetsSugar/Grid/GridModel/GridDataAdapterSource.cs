using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQWidgetsSugar
{
    public class GridDataAdapterSource
    {
        public string url { get; set; }
        public List<GridDatafield> datafields = new List<GridDatafield>();
        public string datatype = "json";
    }
}
