using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace JQWidgetsSugar
{
    public class GridDataAdapterSource
    {
        public string url { get; set; }
        public List<GridDatafield> datafields = new List<GridDatafield>();
        public string datatype = "json";
        public string updateRow = "${updateRow}";
        [ScriptIgnoreAttribute()]
        public string extendData { get; set; }
    }
}
