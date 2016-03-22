using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQWidgetsSugar
{
    public class PublicMehtod
    {
        /// <summary>
        /// 给grid columns 索引0处添加checkbox columns
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public  void ColumnsPrependCheckbox(List<GridColumn> columns) {
            columns.Insert(0, new GridColumn() { width="25px", text = "", datafield = "checkbox", renderer = "rendererFunc", rendered = "renderedFunc", cellsRenderer = "cellsRendererFunc", sortable = false, filterable = false });
        }
    }
}
