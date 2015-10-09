using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace JQWidgetsSugar
{
    public class GridConfig
    {
        public string width = "100%";
        public string height = "500px";
        public string source = "dataAdapter";
        public string theme = "arctic";
        public int pageSize = 20;
        public bool sortable = true;
        public bool editable = false;
        public bool filterable = true;
        public bool rowDetails = false;
        public string filterMode = FilterModel.defaultValue;
        public string selectionMode = SelectionMode.multipleRows;
        public bool pageable = true;
        public int pagerButtonsCount = 10;
        public bool showToolbar = true;
        public bool columnsResize = true;
        public string rendered { get; set; }
        public bool altRows = false;
        public int toolbarHeight = 35;
        public string localization = "${localization}";
        public string initRowDetails { get; set; }
        public string renderToolbar { get; set; }
        public bool serverProcessing = true;
        public List<GridColumn> columns { get; set; }
        public List<ColumnGroups> columnGroups { get; set; }
        [ScriptIgnoreAttribute()]
        public List<GridButton> gridbuttons = new List<GridButton>();
    }

    public class GridButton
    {
        public string name { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public string click { get; set; }
    }


}
