function jqxAlert(msg, title) {
    if (title == null) title = "消息提醒";
    var html = "<div id=\"eventWindow\" >" +
    "<div>" + title +
        "</div>" +
    "<div>" +
    "<div class=\"body\">" +
         msg +
        "</div>" +
       " <div style=\"float: right; margin-top: 15px; height: 30px\">" +
            "<input type=\"button\" id=\"ok\" value=\"确定\" style=\"margin-right: 10px\" />" +
       " </div>" +
    "</div>" +
"</div>";
    var winObj = $(html);
    winObj.find('#ok').jqxButton({ width: '65px' });
    winObj.find('#ok').focus();
    winObj.jqxWindow({
        maxWidth: 380, minWidth: 250, width: 270,
        resizable: false, isModal: true, modalOpacity: 0.3,
        okButton: winObj.find('#ok'),
        initContent: function () {

        }
    });
}
function jqxConfirm(okFun, msg, title) {
    if (title == null) title = "消息提醒";
    var html = "<div id=\"eventWindow\" >" +
    "<div>" + title +
        " </div>" +
    "<div>" +
    "<div class=\"body\">" +
         msg +
        "</div>" +
       " <div style=\"float: right; margin-top: 15px; height: 30px\">" +
            "<input type=\"button\" id=\"ok\" value=\"确定\" style=\"margin-right: 10px\" />" +
            "<input type=\"button\" id=\"cancel\" value=\"取消\" />" +
       " </div>" +
    "</div>" +
"</div>";
    var winObj = $(html);
    winObj.find('#ok').jqxButton({ width: '65px' });
    winObj.find('#ok').click(function () {
        if (okFun != null) {
            okFun();
        }
    })
    winObj.find('#ok').focus();
    winObj.find('#cancel').jqxButton({ width: '65px' });
    winObj.jqxWindow({
        maxWidth: 380, minWidth: 250, width: 270,
        resizable: false, isModal: true, modalOpacity: 0.3,
        okButton: winObj.find('#ok'), cancelButton: winObj.find('#cancel'),
        initContent: function () {

        }
    });

}

function jqxWindow(selector, title, width, height) {
    $(selector).show();
    if ($(selector).find(".jqx-window-content").size() > 0) {
        $(selector).jqxWindow("open");
    } else {
        $(selector).jqxWindow({ width: width, height: height, isModal: true, modalOpacity: 0.3 });
    }
    $(selector).jqxWindow('setTitle', title);
}





function jqxDelete(options) {
    var gridSelector = options.gridSelector;
    var url = options.url;
    var data = options.data;
    jqxConfirm(function () {
        $.ajax({
            type: "post",
            url: url,
            data: data,
            traditional: true,
            dataType: "json",
            success: function (msg) {
                if (msg.isSuccess == false) {
                    jqxAlert(msg.responseInfo);

                }
                if ($(gridSelector).find(".jqx_datatable_checkbox_all").size() > 0)
                    $(gridSelector).find(".jqx_datatable_checkbox_all").jqxCheckBox('checked', false);
                $(gridSelector).jqxDataTable('updateBoundData');
            }, error: function (msg) {
                console.log(msg);
            }
        })
    }, "您确定要删除吗？")
}




var jqxLocalization =

{
    // separator of parts of a date (e.g. '/' in 11/05/1955)
    '/': "/",
    // separator of parts of a time (e.g. ':' in 05:44 PM)
    ':': ":",
    // the first day of the week (0 = Sunday, 1 = Monday, etc)
    firstDay: 0,
    days: {
        // full day names
        names: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
        // abbreviated day names
        namesAbbr: ["日", "一", "二", "三", "四", "五", "六"],
        // shortest day names
        namesShort: ["日", "一", "二", "三", "四", "五", "六"]
    },
    months: {
        // full month names (13 months for lunar calendards -- 13th month should be "" if not lunar)
        names: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月", ""],
        // abbreviated month names
        namesAbbr: ["一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", ""]
    },
    // AM and PM designators in one of these forms:
    // The usual view, and the upper and lower case versions
    //      [standard,lowercase,uppercase]
    // The culture does not use AM or PM (likely all standard date formats use 24 hour time)
    //      null
    AM: ["AM", "am", "AM"],
    PM: ["PM", "pm", "PM"],
    eras: [
    // eras in reverse chronological order.
    // name: the name of the era in this culture (e.g. A.D., C.E.)
    // start: when the era starts in ticks (gregorian, gmt), null if it is the earliest supported era.
    // offset: offset in years from gregorian calendar
{"name": "A.D.", "start": null, "offset": 0 }
],
    twoDigitYearMax: 2029,
    patterns: {
        // short date pattern
        d: "yyyy-MM-dd",
        // long date pattern
        D: "dddd, MMMM dd, yyyy",
        // short time pattern
        t: "h:mm tt",
        // long time pattern
        T: "h:mm:ss tt",
        // long date, short time pattern
        f: "dddd, MMMM dd, yyyy h:mm tt",
        // long date, long time pattern
        F: "dddd, MMMM dd, yyyy h:mm:ss tt",
        // month/day pattern
        M: "MMMM dd",
        // month/year pattern
        Y: "yyyy MMMM",
        // S is a sortable format that does not vary by culture
        S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss",
        // formatting of dates in MySQL DataBases
        ISO: "yyyy-MM-dd hh:mm:ss",
        ISO2: "yyyy-MM-dd HH:mm:ss",
        d1: "dd.MM.yyyy",
        d2: "dd-MM-yyyy",
        d3: "dd-MMMM-yyyy",
        d4: "dd-MM-yy",
        d5: "H:mm",
        d6: "HH:mm",
        d7: "HH:mm tt",
        d8: "dd/MMMM/yyyy",
        d9: "MMMM-dd",
        d10: "MM-dd",
        d11: "MM-dd-yyyy"
    },
    percentsymbol: "%",
    currencysymbol: "$",
    currencysymbolposition: "before",
    decimalseparator: '.',
    thousandsseparator: ',',
    pagergotopagestring: "跳转到:",
    pagershowrowsstring: "显示条数:",
    pagerrangestring: " of ",
    pagerpreviousbuttonstring: "上一页",
    pagernextbuttonstring: "下一页",
    pagerfirstbuttonstring: "首页",
    pagerlastbuttonstring: "最后一页",
    filterapplystring: "过滤",
    filtercancelstring: "隐藏",
    filterclearstring: "清除",
    filterstring: "高级筛选",
    filtersearchstring: "查询:",
    filterstringcomparisonoperators: ['空', '非空', '包含', '包含（正则）',
   '不包含', '不包含（正则）', '起始', '起始（正则）',
   '结尾', '结尾正则', '等于', '等于(正则)', 'null', '非null'],
    filternumericcomparisonoperators: ['等于', '不等于', '小于', '小于或等于', '大于', '大于或等于', '空', '非空'],
    filterdatecomparisonoperators: ['等于', '不等于', '小于', '小于或等于', '大于', '大于或等于', '空', '非空'],
    filterbooleancomparisonoperators: ['等于', '不等于'],
    validationstring: "请输入有效值！",
    emptydatastring: "没有数据显示",
    filterselectstring: "选择过滤器",
    loadtext: "Loading...",
    clearstring: "清除",
    todaystring: "今天",
    loadingerrormessage: "The data is still loading and you cannot set a property or call a method. You can do that once the data binding is completed. jqxDataTable raises the 'bindingComplete' event when the binding is completed."
};



 