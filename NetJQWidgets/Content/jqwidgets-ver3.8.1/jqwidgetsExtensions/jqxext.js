function jqxAlert(msg,title) {
    if (title == null) title = "消息提醒";
    var html = "<div id=\"eventWindow\" >" +
    "<div>" +title+
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
    "<div>" +title+
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