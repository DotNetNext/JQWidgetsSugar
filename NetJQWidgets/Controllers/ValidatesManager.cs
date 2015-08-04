using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SyntacticSugar;
namespace NetJQWidgets
{
    public class ValidatesManager
    {
        public static void Init()
        {
            ValidationSugar.Init("validate_key_grid_index",
                               ValidationSugar.CreateOptionItem().Set("name", true/*是否必填*/, "名称").AddRegex(".{5,15}", "长度为5-15字符").ToOptionItem(),
                               ValidationSugar.CreateOptionItem().Set("quantity",true,"数量").Add(ValidationSugar.OptionItemType.Int,"请输入整数").ToOptionItem(),
                               ValidationSugar.CreateOptionItem().Set("date", true/*是否必填*/, "日期").Add(ValidationSugar.OptionItemType.Date,"格式为yyyy-MM-dd").ToOptionItem()
                );
        }
    }
}