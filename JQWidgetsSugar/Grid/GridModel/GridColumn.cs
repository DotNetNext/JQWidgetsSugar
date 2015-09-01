using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQWidgetsSugar
{
    public class GridColumn
    {
        public string text { get; set; }
        public string datafield { get; set; }
        public string filtertype { get; set; }
        public dynamic width = "auto";
        public string cellsalign { get; set; }
        public string cellsformat { get; set; }
        public string datatype { get; set; }
        public string columngroup { get; set; }
    }
    public class ColumnGroups
    {
        public string text { get; set; }
        public string align { get; set; }
        public string name { get; set; }
        public string parentGroup { get; set; }
    }

    public class AlignType
    {
        public const string left = "left";
        public const string right = "right";
        public const string center = "center";
    }

    public class Datatype
    {
        public const string datastring = "string";
        public const string datanumber = "number";
        public const string datadate = "date";
        public const string dataint = "int";
        public const string datafloat = "float";
        public const string databool = "bool";
    }

    public class FileModel
    {
        public const string defaultValue = "default";
        public const string simple = "simple";
        public const string advanced = "advanced";
    }
}
