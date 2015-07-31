using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;
namespace JQWidgetsSugar
{
    /// <summary>
    /// jqxgrid通用类
    /// </summary>
    public class JQXGrid
    {
        private static System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        public static JsonResultModel<T> GetWidgetsSource<T>(Sqlable sable, GridSearchParams pars) where T : class
        {
            var query = System.Web.HttpContext.Current.Request.QueryString;
            Type type = typeof(T);
            string tableName = type.Name;
            sable = sable.Where(BuildQuery(query));
            string orderBy = null;
            if (!string.IsNullOrEmpty(pars.sortorder))
            {
                orderBy = (pars.sortdatafield + " " + pars.sortorder);
            }
            else
            {
                orderBy = "getdate()";
            }
            var rows = sable.SelectToPageList<T>("*", orderBy, pars.pagenum + 1, pars.pagesize);
            var result = new JsonResultModel<T>()
            {
                TotalRows = sable.Count(),
                Rows = rows
            };
            return result;
        }


        public static string BindGrid(string gridSelector, GridDataAdapterSource gda, GridTable gt)
        {

            if (gda.datafields == null || gda.datafields.Count == 0)
            {
                foreach (var it in gt.columns)
                {
                    gda.datafields.Add(new GridDatafield() { type=it.datatype, name=it.datafield });
                }
            }

            StringBuilder gridHtml = new StringBuilder();
            gridHtml.AppendFormat(@"var source =
{0}", jss.Serialize(gda));
            gridHtml.AppendLine();
            gridHtml.Append(@" var dataAdapter = new $.jqx.dataAdapter(source, {
                downloadComplete: function (data) {
                    source.totalrecords = data.TotalRows;
                }
            });");
            gridHtml.AppendLine();
            gridHtml.AppendFormat(@"$(""{0}"").jqxDataTable(", gridSelector);
            if (string.IsNullOrEmpty(gt.renderToolbar))
            {
                gt.renderToolbar = "${toolbar}";
            }
            gridHtml.Append(jss.Serialize(gt));
            gridHtml.Append(");");
            var reval = gridHtml.ToString();
            reval = reval
                        .Replace("\"${toolbar}\"", GetToolbar(gridSelector))
                        .Replace("\"source\":\"dataAdapter\"", "\"source\":dataAdapter");
            return string.Format("$(function(){{  {0} }})", reval);
        }

        private static string GetToolbar(string gridSelector)
        {
            return @"
         function (toolBar) {

                    var theme = ""Arctic"";
                    var toTheme = function (className) {
                        if (theme == """") return className;
                        return className + "" "" + className + ""-"" + theme;
                    }
                    // appends buttons to the status bar.
                    var container = $(""<div style='overflow: hidden; position: relative; height: 100%; width: 100%;'></div>"");
                    var buttonTemplate = ""<div style='float: left; padding: 3px; margin: 2px;'><div style='margin: 4px; width: 16px; height: 16px;'></div></div>"";
                    var addButton = $(buttonTemplate);
                    var editButton = $(buttonTemplate);
                    var deleteButton = $(buttonTemplate);
                    var cancelButton = $(buttonTemplate);
                    var updateButton = $(buttonTemplate);
                    container.append(addButton);
                    container.append(editButton);
                    container.append(deleteButton);
                    container.append(cancelButton);
                    container.append(updateButton);
                    toolBar.append(container);
                    addButton.jqxButton({ cursor: ""pointer"", enableDefault: false, height: 25, width: 25 });
                    addButton.find('div:first').addClass(toTheme('jqx-icon-plus'));
                    addButton.jqxTooltip({ position: 'bottom', content: ""Add"" });
                    editButton.jqxButton({ cursor: ""pointer"", disabled: true, enableDefault: false, height: 25, width: 25 });
                    editButton.find('div:first').addClass(toTheme('jqx-icon-edit'));
                    editButton.jqxTooltip({ position: 'bottom', content: ""Edit"" });
                    deleteButton.jqxButton({ cursor: ""pointer"", disabled: true, enableDefault: false, height: 25, width: 25 });
                    deleteButton.find('div:first').addClass(toTheme('jqx-icon-delete'));
                    deleteButton.jqxTooltip({ position: 'bottom', content: ""Delete"" });
                    updateButton.jqxButton({ cursor: ""pointer"", disabled: true, enableDefault: false, height: 25, width: 25 });
                    updateButton.find('div:first').addClass(toTheme('jqx-icon-save'));
                    updateButton.jqxTooltip({ position: 'bottom', content: ""Save Changes"" });
                    cancelButton.jqxButton({ cursor: ""pointer"", disabled: true, enableDefault: false, height: 25, width: 25 });
                    cancelButton.find('div:first').addClass(toTheme('jqx-icon-cancel'));
                    cancelButton.jqxTooltip({ position: 'bottom', content: ""Cancel"" });
                    var updateButtons = function (action) {
                        switch (action) {
                            case ""Select"":
                                addButton.jqxButton({ disabled: false });
                                deleteButton.jqxButton({ disabled: false });
                                editButton.jqxButton({ disabled: false });
                                cancelButton.jqxButton({ disabled: true });
                                updateButton.jqxButton({ disabled: true });
                                break;
                            case ""Unselect"":
                                addButton.jqxButton({ disabled: false });
                                deleteButton.jqxButton({ disabled: true });
                                editButton.jqxButton({ disabled: true });
                                cancelButton.jqxButton({ disabled: true });
                                updateButton.jqxButton({ disabled: true });
                                break;
                            case ""Edit"":
                                addButton.jqxButton({ disabled: true });
                                deleteButton.jqxButton({ disabled: true });
                                editButton.jqxButton({ disabled: true });
                                cancelButton.jqxButton({ disabled: false });
                                updateButton.jqxButton({ disabled: false });
                                break;
                            case ""End Edit"":
                                addButton.jqxButton({ disabled: false });
                                deleteButton.jqxButton({ disabled: false });
                                editButton.jqxButton({ disabled: false });
                                cancelButton.jqxButton({ disabled: true });
                                updateButton.jqxButton({ disabled: true });
                                break;
                        }
                    }
                    var rowIndex = null;
                    $(""" + gridSelector + @""").on('rowSelect', function (event) {
                        var args = event.args;
                        rowIndex = args.index;
                        updateButtons('Select');
                    });
                    $(""" + gridSelector + @""").on('rowUnselect', function (event) {
                        updateButtons('Unselect');
                    });
                    $(""" + gridSelector + @""").on('rowEndEdit', function (event) {
                        updateButtons('End Edit');
                    });
                    $(""" + gridSelector + @""").on('rowBeginEdit', function (event) {
                        updateButtons('Edit');
                    });
                    addButton.click(function (event) {
                        if (!addButton.jqxButton('disabled')) {
                            // add new empty row.
                            $(""" + gridSelector + @""").jqxDataTable('addRow', null, {}, 'first');
                            // select the first row and clear the selection.
                            $(""" + gridSelector + @""").jqxDataTable('clearSelection');
                            $(""" + gridSelector + @""").jqxDataTable('selectRow', 0);
                            // edit the new row.
                            $(""" + gridSelector + @""").jqxDataTable('beginRowEdit', 0);
                            updateButtons('add');
                      
                        }
                    });
                    cancelButton.click(function (event) {
                        if (!cancelButton.jqxButton('disabled')) {
                            // cancel changes.
                            $(""" + gridSelector + @""").jqxDataTable('endRowEdit', rowIndex, true);
                        }
                    });
                    updateButton.click(function (event) {
                        if (!updateButton.jqxButton('disabled')) {
                            // save changes.
                            $(""" + gridSelector + @""").jqxDataTable('endRowEdit', rowIndex, false);
                        }
                    });
                    editButton.click(function () {
                        if (!editButton.jqxButton('disabled')) {
                            $(""" + gridSelector + @""").jqxDataTable('beginRowEdit', rowIndex);
                            updateButtons('edit');
                        }
                    });
                    deleteButton.click(function () {
                        if (!deleteButton.jqxButton('disabled')) {
                            $(""" + gridSelector + @""").jqxDataTable('deleteRow', rowIndex);
                            updateButtons('delete');
                        }
                    });
}";
        }

        private static string BuildQuery(System.Collections.Specialized.NameValueCollection query)
        {
            var result = query.GetValues("filterslength");
            if (result == null)
                return string.Empty;
            var filtersCount = int.Parse(query.GetValues("filterslength")[0]);
            var tmpDataField = "";
            var tmpFilterOperator = "";
            var where = "";
            if (filtersCount > 0)
            {
                where = "  (";
            }
            for (var i = 0; i < filtersCount; i += 1)
            {
                var filterValue = query.GetValues("filtervalue" + i)[0];
                var filterCondition = query.GetValues("filtercondition" + i)[0];
                var filterDataField = query.GetValues("filterdatafield" + i)[0];
                var filterOperator = query.GetValues("filteroperator" + i)[0];
                if (tmpDataField == "")
                {
                    tmpDataField = filterDataField;
                }
                else if (tmpDataField != filterDataField)
                {
                    where += ") AND (";
                }
                else if (tmpDataField == filterDataField)
                {
                    if (tmpFilterOperator == "")
                    {
                        where += " AND ";
                    }
                    else
                    {
                        where += " OR ";
                    }
                }
                // build the "WHERE" clause depending on the filter's condition, value and datafield.
                where += GetFilterCondition(filterCondition, filterDataField, filterValue);
                if (i == filtersCount - 1)
                {
                    where += ")";
                }
                tmpFilterOperator = filterOperator;
                tmpDataField = filterDataField;
            }
            return where;
        }

        private static string GetFilterCondition(string filterCondition, string filterDataField, string filterValue)
        {
            switch (filterCondition)
            {
                case "NOT_EMPTY":
                case "NOT_NULL":
                    return " " + filterDataField + " NOT LIKE '" + "" + "'";
                case "EMPTY":
                case "NULL":
                    return " " + filterDataField + " LIKE '" + "" + "'";
                case "CONTAINS_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE '%" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS";
                case "CONTAINS":
                    return " " + filterDataField + " LIKE '%" + filterValue + "%'";
                case "DOES_NOT_CONTAIN_CASE_SENSITIVE":
                    return " " + filterDataField + " NOT LIKE '%" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "DOES_NOT_CONTAIN":
                    return " " + filterDataField + " NOT LIKE '%" + filterValue + "%'";
                case "EQUAL_CASE_SENSITIVE":
                    return " " + filterDataField + " = '" + filterValue + "'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "EQUAL":
                    return " " + filterDataField + " = '" + filterValue + "'";
                case "NOT_EQUAL_CASE_SENSITIVE":
                    return " BINARY " + filterDataField + " <> '" + filterValue + "'";
                case "NOT_EQUAL":
                    return " " + filterDataField + " <> '" + filterValue + "'";
                case "GREATER_THAN":
                    return " " + filterDataField + " > '" + filterValue + "'";
                case "LESS_THAN":
                    return " " + filterDataField + " < '" + filterValue + "'";
                case "GREATER_THAN_OR_EQUAL":
                    return " " + filterDataField + " >= '" + filterValue + "'";
                case "LESS_THAN_OR_EQUAL":
                    return " " + filterDataField + " <= '" + filterValue + "'";
                case "STARTS_WITH_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE '" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "STARTS_WITH":
                    return " " + filterDataField + " LIKE '" + filterValue + "%'";
                case "ENDS_WITH_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE '%" + filterValue + "'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "ENDS_WITH":
                    return " " + filterDataField + " LIKE '%" + filterValue + "'";
            }
            return "";
        }
    }
}
