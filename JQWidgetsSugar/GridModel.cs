using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQWidgetsSugar
{
    public class GridModel<T>
    {
        public int TotalRows { get; set; }
        public List<T> Rows { get; set; }
    }
}
