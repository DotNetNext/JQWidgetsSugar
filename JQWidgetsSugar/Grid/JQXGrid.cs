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
        public static JsonResultModel<T> GetWidgetsSource<T>(Sqlable sable, GridSearchParams pars,string selectFields="*") where T : class
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
            var rows = sable.SelectToPageList<T>(selectFields, orderBy, pars.pagenum + 1, pars.pagesize);
            var result = new JsonResultModel<T>()
            {
                TotalRows = sable.Count(),
                Rows = rows
            };
            return result;
        }


        public static object BindGrid(string gridSelector, GridDataAdapterSource gda, GridConfig gc)
        {

            if (gda.datafields == null || gda.datafields.Count == 0)
            {
                foreach (var it in gc.columns)
                {
                    gda.datafields.Add(new GridDatafield() { type = it.datatype, name = it.datafield });
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
            if (string.IsNullOrEmpty(gc.renderToolbar))
            {
                gc.renderToolbar = "${toolbar}";
            }
            gridHtml.Append(jss.Serialize(gc));
            gridHtml.Append(");");
            var reval = gridHtml.ToString();
            reval = reval
                        .Replace("\"${toolbar}\"", GetToolbar(gridSelector, gc))
                        .Replace("\"source\":\"dataAdapter\"", "\"source\":dataAdapter")
                        .Replace("\"${localization}\"", "jqxLocalization");
            reval= string.Format("$(function(){{  {0} }})", reval);
            reval = ("<script>\r\n")+reval+("\n\r</script>");
            return reval;
        }

        private static string GetToolbar(string gridSelector, GridConfig gc)
        {
            StringBuilder sb = new StringBuilder(@"
         function (toolBar) {

                    var theme = ""Arctic"";
                    var toTheme = function (className) {
                        if (theme == """") return className;
                        return className + "" "" + className + ""-"" + theme;
                    }
                    // appends buttons to the status bar.
                    var container = $(""<div style='overflow: hidden; position: relative; height: 100%; width: 100%;'></div>"");
                    var buttonTemplate = ""<div style='float: left; padding: 3px; margin: 2px;'><div style='margin: 4px; width: 16px; height: 16px;'></div></div>"";");
            sb.AppendLine();
            gc.gridbuttons.ForEach(it =>
            {

                sb.AppendFormat("var {0} = $(buttonTemplate);", it.name);
                sb.AppendLine();
            });

            gc.gridbuttons.ForEach(it =>
            {

                sb.AppendFormat(" container.append({0});", it.name);
                sb.AppendLine();
            });
            sb.AppendLine();
            sb.Append("toolBar.append(container)");
            sb.AppendLine();
            gc.gridbuttons.ForEach(it =>
            {
                sb.AppendFormat(@"
                    {0}.jqxButton({{ cursor: ""pointer"", enableDefault: false, height: 25, width: 25 }});
                    {0}.find('div:first').addClass(toTheme('{2}'));
                    {0}.jqxTooltip({{ position: 'bottom', content: ""{1}"" }});", it.name, it.title, it.icon);
                sb.AppendLine();
            });
         
            sb.AppendLine();
            sb.Append(@"
                    var row = null;
                    $(""" + gridSelector + @""").on('rowSelect', function (event) {
                        row= event.args.row;
                    });");
            sb.AppendLine();
            sb.Append(@"
                    $(""" + gridSelector + @""").on('rowUnselect', function (event) {
                        row = null;
                    });");
            sb.AppendLine();
            gc.gridbuttons.ForEach(it =>
            {
                if (!string.IsNullOrWhiteSpace(it.click))
                {
                    sb.AppendLine();
                    string clickHtml = it.click;
                    sb.AppendFormat(@"  {1}.click(function (event) {{
                       {0}(row);
                    }});", clickHtml,it.name);

                }
            });
            sb.AppendLine();
            sb.Append("}");
            return sb.ToString();
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
