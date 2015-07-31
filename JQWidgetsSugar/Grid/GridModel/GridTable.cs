using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQWidgetsSugar
{
    public class GridTable
    {
        public string width = "100%";
        public string source = "dataAdapter";
        public string theme = "arctic";
        public int pageSize = 20;
        public bool sortable = true;
        public bool editable = true;
        public bool filterable = true;
        public bool pageable = true;
        public int pagerButtonsCount = 10;
        public bool showToolbar = true;
        public int toolbarHeight = 35;
        public string renderToolbar { get; set; }
        public bool serverProcessing = true;
        public List<GridColumn> columns { get; set; }
    }
}
