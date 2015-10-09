using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQWidgetsSugar
{
    /// <summary>
    /// jqxGrid 列属性设置
    /// </summary>
    public class GridColumn
    {
        /// <summary>
        /// 显示列名
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string datafield { get; set; }
        /// <summary>
        /// 过滤类型
        /// </summary>
        public string filtertype { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public dynamic width = "auto";
        /// <summary>
        /// 对齐方式
        /// </summary>
        public string cellsalign { get; set; }
        /// <summary>
        /// 格式化
        /// </summary>
        public string cellsformat { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string datatype { get; set; }
        /// <summary>
        /// 表头分组
        /// </summary>
        public string columngroup { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
        public bool hidden = false;

        /// <summary>
        /// 是否锁列
        /// </summary>
        public bool pinned = false;
        /// <summary>
        /// 自定义单元格
        /// </summary>
        public string cellsRenderer { get; set; }
        /// <summary>
        /// 自定义表头
        /// </summary>
        public string renderer { get; set; }
        /// <summary>
        /// 自定义表头后
        /// </summary>
        public string rendered { get; set; }
        /// <summary>
        /// 启动排序
        /// </summary>
        public bool sortable = true;
        /// <summary>
        /// 启动过滤
        /// </summary>
        public bool filterable = true;
        /// <summary>
        /// 单元格自动设置高度
        /// </summary>
        public bool autoRowHeight { get; set; }
        /// <summary>
        /// 创建行编辑
        /// </summary>
        public string createEditor { get; set; }
        /// <summary>
        /// 初始化行编辑
        /// </summary>
        public string initEditor { get; set; }
        /// <summary>
        /// 获取行编辑
        /// </summary>
        public string getEditorValue { get; set; }

        public string columntype = ColumnType.Default;
    }

    /// <summary>
    /// 列分组
    /// </summary>
    public class ColumnGroups
    {
        public string text { get; set; }
        public string align { get; set; }
        public string name { get; set; }
        public string parentGroup { get; set; }
    }

    /// <summary>
    /// 对齐方式
    /// </summary>
    public class AlignType
    {
        public const string left = "left";
        public const string right = "right";
        public const string center = "center";
    }

    public class ColumnType {
        public const string Default = null;
        public const string template = "template";
        public const string custom="custom";
    
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public class Datatype
    {
        public const string datastring = "string";
        public const string datanumber = "number";
        public const string datadate = "date";
        public const string dataint = "int";
        public const string datafloat = "float";
        public const string databool = "bool";
    }
    /// <summary>
    /// 过滤类型
    /// </summary>
    public class FilterModel
    {
        public const string defaultValue = "default";
        public const string simple = "simple";
        public const string advanced = "advanced";
    }
    /// <summary>
    /// 选择类型
    /// </summary>
    public class SelectionMode {
        public const string singleRow = "singleRow";
        public const string multipleRows = "multipleRows";
        public const string custom = "custom";
    }
}
