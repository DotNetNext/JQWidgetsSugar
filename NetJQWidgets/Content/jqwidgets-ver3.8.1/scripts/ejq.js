/*!
* jQuery Library v0.1
* 说明：http://www.52mvc.com/showtopic-5606.aspx
* 时间: sunkaixuan 2014-8-22  
*/
(function (window, jQuery, undefined) {

    jQuery.extend({
        /*linq*/
        linq: {
            contains: function (thisVal, cobj) {
                if (jQuery.valiData.isEmpty(thisVal)) {
                    return false;
                }
                return thisVal.toString().lastIndexOf(cobj.toString()) != -1;
            },
            /*where*/
            where: function (obj, action) {
                if (action == null) return;
                var reval = new Array();
                $(obj).each(function (i, v) {
                    if (action(v)) {
                        reval.push(v);
                    }
                })
                return reval;
            },
            /*any*/
            any: function (obj, action) {
                if (action == null) return;
                var reval = false;
                $(obj).each(function (i, v) {
                    if (action(v)) {
                        reval = true;
                        return false;
                    }
                })
                return reval;
            },
            /*select*/
            select: function (obj, action) {
                if (action == null) return;
                var reval = new Array();
                $(obj).each(function (i, v) {
                    reval.push(action(v));
                });
                return reval;
            },
            /*each*/
            each: function (obj, action) {
                if (action == null) return;
                jQuery(obj).each(function (i, v) {
                    action(i, v);
                });
            },
            /*first*/
            first: function (obj, action) {
                if (action == null) return;
                var reval = new Array();
                $(obj).each(function (i, v) {
                    if (action(v)) {
                        reval.push(v);
                        return false;
                    }
                })
                return reval[0];
            }

        },

        /*操作*/
        action: {
            //移除最后一个字符
            trimEnd: function (str, c) {
                var reg = new RegExp(c + "([^" + c + "]*?)$");
                return str.replace(reg, function (w) { if (w.length > 1) { return w.substring(1); } else { return ""; } });
            },
            htmlEncode: function (str) {
                return str.replace(/&/g, '&amp').replace(/\"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
            },
            htmlDecode: function (str) {
                return str.replace(/&amp;/g, '&').replace(/&quot;/g, '\"').replace(/&lt;/g, '<').replace(/&gt;/g, '>');
            },
            textEncode: function (str) {
                str = str.replace(/&amp;/gi, '&');
                str = str.replace(/</g, '&lt;');
                str = str.replace(/>/g, '&gt;');
                return str;
            },
            textDecode: function (str) {
                str = str.replace(/&amp;/gi, '&');
                str = str.replace(/&lt;/gi, '<');
                str = str.replace(/&gt;/gi, '>');
                return str;
            },
            //获取json的key和value
            jsonDictionary: function (json, key) {
                var reval = new Array();
                for (key in json) {
                    reval.push({ key: key, value: json[key] });
                }
                return reval;
            },
            insertStr: function (str1, n, str2) {
                if (str1.length < n) {
                    return str1 + str2;
                } else {
                    s1 = str1.substring(0, n);
                    s2 = str1.substring(n, str1.length);
                    return s1 + str2 + s2;
                }
            }

        },
        /*转换*/
        convert: {
            //还原json格式的时间
            jsonReductionDate: function (cellval, format) {
                try {
                    if (cellval == "" || cellval == null) return "";
                    var date = new Date(parseInt(cellval.substr(6)));
                    if (format == null) {
                        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                        return date.getFullYear() + "-" + month + "-" + currentDate;
                    } else {
                        return $.convert.ToDate(date, format);
                    }
                } catch (e) {
                    return "";
                }
            },
            jsonToStr: function (object) {
                var type = typeof object;
                if ('object' == type) {
                    if (Array == object.constructor) type = 'array';
                    else if (RegExp == object.constructor) type = 'regexp';
                    else type = 'object';
                }
                switch (type) {
                    case 'undefined':
                    case 'function':
                    case 'unknown':
                        return;
                        break;
                    case 'function':
                    case 'boolean':
                    case 'regexp':
                        return object.toString();
                        break;
                    case 'number':
                        return isFinite(object) ? object.toString() : 'null';
                        break;
                    case 'string':
                        return '"' + object.replace(/(\\|\")/g, "\\$1").replace(/\n|\r|\t/g, function () {
                            var a = arguments[0];
                            return (a == '\n') ? '\\n' : (a == '\r') ? '\\r' : (a == '\t') ? '\\t' : ""
                        }) + '"';
                        break;
                    case 'object':
                        if (object === null) return 'null';
                        var results = [];
                        for (var property in object) {
                            var value = jQuery.convert.jsonToStr(object[property]);
                            if (value !== undefined) results.push(jQuery.convert.jsonToStr(property) + ':' + value);
                        }
                        return '{' + results.join(',') + '}';
                        break;
                    case 'array':
                        var results = [];
                        for (var i = 0; i < object.length; i++) {
                            var value = jQuery.convert.jsonToStr(object[i]);
                            if (value !== undefined) results.push(value);
                        }
                        return '[' + results.join(',') + ']';
                        break;
                }
            },
            strToJson: function (str) {
                return jQuery.parseJSON(str);
            },
            toDate: function (date, format) {
                var data = new Date(date);
                var o = {
                    "M+": data.getMonth() + 1, //month
                    "d+": data.getDate(), //day
                    "h+": data.getHours(), //hour
                    // "H+": date.getHours(), //hour
                    "m+": data.getMinutes(), //minute
                    "s+": data.getSeconds(), //second
                    "q+": Math.floor((data.getMonth() + 3) / 3), //quarter
                    "S": data.getMilliseconds() //millisecond
                }
                if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
                (data.getFullYear() + "").substr(4 - RegExp.$1.length));
                for (var k in o) if (new RegExp("(" + k + ")").test(format))
                    format = format.replace(RegExp.$1,
                RegExp.$1.length == 1 ? o[k] :
                ("00" + o[k]).substr(("" + o[k]).length));
                return format;
            },
            toInt: function (par) {
                if (par == null || par == NaN || par == "") return 0;
                return parseInt(par);
            },
            toFloat: function (par) {
                if (par == null || par == NaN || par == "") return 0;
                return parseFloat(par);
            },
            xmlToJQuery: function (data) {
                var xml;
                if ($.browser.msie) {// & parseInt($.browser.version) < 9
                    xml = new ActiveXObject("Microsoft.XMLDOM");
                    xml.async = false;
                    xml.loadXML(data);
                    // xml = $(xml).children('nodes'); //这里的nodes为最顶级的节点
                } else {
                    xml = data;
                }
                return $(xml);
            }
        },
        /*数据验证*/
        valiData: {
            isEmpty: function (val) { return val == undefined || val == null || val == "" || val.toString() == ""; },
            isZero: function (val) { return val == null || val == "" || val == 0 || val == "0"; },
            //判断是否为数字
            isNumber: function (val) { return (/^\d+$/.test(val)); },
            //是否是邮箱
            isMail: function (val) { return (/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(val)); },
            //是否是手机
            isMobilePhone: function (val) { return (/\d{11}$/.test(val)); },
            //判断是否为负数和整数
            isNumberOrNegative: function (val) { return (/^\d+|\-\d+$/.test(val)); },
            //金额验证
            isMoney: function (val) { return (/^[1-9]d*.d*|0.d*[1-9]d*|\d+$/.test(val)); },
            isDecimal: function (val) { return (/^(-?\d+)(\.\d+)?$/.test(val)); }

        },
        /*类型验证*/
        valiType: {
            isArray: function (obj) { return (typeof obj == 'object') && obj.constructor == Array; },
            isString: function (str) { return (typeof str == 'string') && str.constructor == String; },
            isDate: function (obj) { return (typeof obj == 'object') && obj.constructor == Date; },
            isFunction: function (obj) { return (typeof obj == 'function') && obj.constructor == Function; },
            isObject: function (obj) { return (typeof obj == 'object') && obj.constructor == Object; }
        }

    });

    /*********************************form操作*********************************/
    jQuery.fn.extend({
        //获取元素属性以","隔开
        attrToStr: function (attr) {
            var reval = "";
            this.each(function () {
                reval += jQuery(this).attr(attr) + ","
            })
            reval = jQuery.jQueryAction.trimEnd(reval, ",");
            return reval;
        },
        formClear: function () {
            this.find("input:text,select,input:hidden").each(function () {
                $(this).val("");
            });
            this.find("input:checkbox,input:radio").each(function () {
                $(this).removeAttr("checked");
            });
        },
        //将json对象自动填充到表单 
        //例如 $('form').formFill({data:{id:1},prefix:"user."}) 填充后  <input name='user.id' value='1' >
        formFill: function (option) {
            var prefix = option.prefix;
            if (prefix == undefined) prefix = "";
            var frmData = option.data;
            for (i in frmData) {
                var dataKey = i;
                var thisData = this.find("[name='" + prefix + i + "']");
                var text = "text";
                var hidden = "hidden";
                if (thisData != null) {
                    var thisDataType = thisData.attr("type");
                    var val = frmData[i];
                    var isdata = (val != null && val.toString().lastIndexOf("/Date(") != -1);
                    if (thisDataType == "radio") {
                        thisData.filter("[value=" + val + "]").attr("checked", "checked")
                        if (val == true || val == "0") val = "True";
                        else if (val == false || val != "0") val = "False";
                        thisData.filter("[value=" + val + "]").not("donbool").attr("checked", "checked")
                    } else if (thisDataType == "checkbox") {
                        if (thisData.size() == 1) {
                            if (val == "true" || val == 1 || val == "True" || val == "1") {
                                thisData.attr("checked", "checked");
                            } else {
                                thisData.removeAttr("checked");
                            }
                        } else {

                            thisData.removeAttr("checked");
                            var cbIndex = i;
                            if (val.lastIndexOf(",") == -1) {
                                this.find("[name='" + prefix + dataKey + "'][value='" + prefix + val + "']").attr("checked", "checked");
                            } else {
                                jQuery(val.split(',')).each(function (i, v) {
                                    this.find("[name='" + prefix + dataKey + "'][value='" + prefix + v + "']").attr("checked", "checked"); ;
                                })
                            }
                        }

                    } else {
                        if (isdata) {
                            val = jQuery.jQueryConvert.jsonReductionDate(val);
                        }
                        if (val == "null" || val == null)
                            val = "";
                        if (val == "" && thisData.attr("watertitle") == thisData.val()) {
                        } else {
                            thisData.val(val + "");
                            thisData.removeClass("watertitle")
                        }
                    }
                }

            }

        }


    });

    /*********************************ajax操作*********************************/
    jQuery.extend({
        ajaxhelper: {
            error: function (msg, action) {
                if (action != null) {
                    action(msg);
                }
                try {
                    console.log(msg);
                } catch (e) {

                }
            }
        }
    });
    /*********************************浏览器操作*********************************/
    jQuery.extend({
        /*requst对象*/
        request: {
            queryString: function () {
                var s1;
                var q = {}
                var s = document.location.search.substring(1);
                s = s.split("&");
                for (var i = 0, l = s.length; i < l; i++) {
                    s1 = s[i].split("=");
                    if (s1.length > 1) {
                        var t = s1[1].replace(/\+/g, " ")
                        try {
                            q[s1[0]] = decodeURIComponent(t)
                        } catch (e) {
                            q[s1[0]] = unescape(t)
                        }
                    }
                }
                return q;
            },
            url: function () {
                return window.location.href;
            },
            domain: function () {
                return window.location.host;
            },
            pageName: function () {
                var a = location.href;
                var b = a.split("/");
                var c = b.slice(b.length - 1, b.length).toString(String).split(".");
                return c.slice(0, 1);
            },
            pageFullName: function () {
                var strUrl = location.href;
                var arrUrl = strUrl.split("/");
                var strPage = arrUrl[arrUrl.length - 1];
                return strPage;
            },
            back: function () {
                history.go(-1);
            }
        },
        response: {
            //页面跳转
            redirect: function (url, params) {
                if (params == null || params == "") {
                    window.location.href = url;
                } else {
                    if (jQuery.linq.contains(url.toString(), "?")) {
                        var rurl = url + "&" + jQuery.param(params);
                        window.location.href = rurl;
                    } else {
                        var rurl = url + "?" + jQuery.param(params);
                        window.location.href = rurl;
                    }
                }
            }

        },
        /*浏览器判段*/
        broVali: {
            isIE6: function () {
                var flag = false;
                if ($.browser.msie && $.browser.version == "6.0")
                    flag = true;
                return flag;
            },
            isIE7: function () {
                var flag = false;
                if ($.browser.msie && $.browser.version == "7.0")
                    flag = true;
                return flag;
            },
            isIE8: function () {
                var flag = false;
                if ($.browser.msie && $.browser.version == "8.0")
                    flag = true;
                return flag;
            },
            isIE9: function () {
                var flag = false;
                if ($.browser.msie && $.browser.version == "9.0")
                    flag = true;
                return flag;
            },
            isIE10: function () {
                var flag = false;
                if ($.browser.msie && $.browser.version == "10.0")
                    flag = true;
                return flag;
            },
            isIE11: function () {
                var flag = false;
                if ($.browser.msie && $.browser.version == "11.0")
                    flag = true;
                return flag;
            },
            isMozilla: function () {
                var flag = false;
                if ($.browser.mozilla)
                    flag = true;
                return flag;
            },
            isOpera: function () {
                var flag = false;
                if ($.browser.opera)
                    flag = true;
                return flag;
            },
            isSafri: function () {
                var flag = false;
                if ($.browser.safari)
                    flag = true;
                return flag;
            },
            isMobile: function () {
                var userAgentInfo = navigator.userAgent;
                var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
                var flag = false;
                for (var v = 0; v < Agents.length; v++) {
                    if (userAgentInfo.indexOf(Agents[v]) > 0) { flag = true; break; }
                }

                return flag;
            },
            isIPhone: function () {
                var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
                return jQuery.jQueryAny(Agents, function (v) {
                    return v == "iPhone";
                });
            },
            isAndroid: function () {
                var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
                return jQuery.jQueryAny(Agents, function (v) {
                    return v == "Android";
                });
            }
        },
        //打印
        print: function (id/*需要打印的最外层元素ID*/) {
            var el = document.getElementById(id);
            var iframe = document.createElement('IFRAME');
            var doc = null;
            iframe.setAttribute('style', 'position:absolute;width:0px;height:0px;left:-500px;top:-500px;');
            document.body.appendChild(iframe);
            doc = iframe.contentWindow.document;
            doc.write('<div>' + el.innerHTML + '</div>');
            doc.close();
            iframe.contentWindow.focus();
            iframe.contentWindow.print();
            if (navigator.userAgent.indexOf("MSIE") > 0) {
                document.body.removeChild(iframe);
            }
        },
        //加入收藏夹
        addFavorite: function (surl, stitle) {
            try {
                window.external.addFavorite(surl, stitle);
            } catch (e) {
                try {
                    window.sidebar.addpanel(stitle, surl, "");
                } catch (e) {
                    alert("加入收藏失败,请使用ctrl+d进行添加");
                }
            }
        },
        //设为首页
        setHome: function (obj, vrl) {
            try {
                obj.style.behavior = 'url(#default#homepage)';
                obj.sethomepage(vrl);
            } catch (e) {
                if (window.netscape) {
                    try {
                        netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
                    } catch (e) {
                        alert("此操作被浏览器拒绝!\n请在浏览器地址栏输入'about:config'并回车\n然后将[signed.applets.codebase_principal_support]的值设置为'true',双击即可。");
                    }
                } else {
                    alert("抱歉，您所使用的浏览器无法完成此操作。\n\n您需要手动设置为首页。");
                }
            }
        }
    });

    /*********************************初始化*********************************/
    jQuery.init = function () {
        String.prototype.ejq_format = function (args) {
            var _dic = typeof args === "object" ? args : arguments;
            return this.replace(/\{([^{}]+)\}/g, function (str, key) {
                return _dic[key] || str;
            });
        }
        String.prototype.ejq_append = function (args) {
            return this + args;
        }
        String.prototype.ejq_appendFormat = function (appendValue, appendArgs) {
            return this + appendValue.ejq_format(appendArgs);
        }
    }
    jQuery.init();

})(window, jQuery)


 