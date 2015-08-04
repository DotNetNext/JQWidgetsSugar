//基于validate验证
//作者:sunkaixuan
//时间:2015-6-5
var validateFactory = function (form,errorImg) {
    this.init = function () {
        addMethod();
        bind();
    }
    this.ajaxSubmit = function (rollback) {
        if (form.data("validateFactory")) {
            if (form.valid()) {
                if (rollback != null) {
                    rollback();
                }
            }
        }
    }
    function addMethod() {
        form.find("[validatesize]").each(function () {
            var th = $(this);
            var count = GetValidatesize(th);
            for (var i = 0; i < count; i++) {
                var methodName = GetMdthodName(th, i);
                $.validator.addMethod(methodName, function (value, element, params) {

                    var pattern = GetPattern(th, params);
                    if (/myfun\:.*/.test(pattern)) {
                        var isvalid = eval(pattern.split(':')[1] + "()");
                        return this.optional(element) || isvalid;
                    } else {
                        return this.optional(element) || new RegExp(pattern).test(value);
                    }
                }, errorImg + GetTip(th, i));
            }
        });
    }
    function bind() {
        var rules = {};
        var messages = {};
        form.find("[validatesize]").each(function () {
            var th = $(this);
            var count = GetValidatesize(th);
            var name = GetName(th);
            rules[name] = {};
            for (var i = 0; i < count; i++) {
                var methodName = GetMdthodName(th, i);
                rules[name][methodName] = i;
            }
            if (GetIsRequired(th)) {
                rules[name].required = true;

                messages[name] = {};
                //                for (var i = 0; i < count; i++) {
                //                    var methodName = GetMdthodName(th, i);
                //                    messages[name][methodName] = GetTip(th,i);
                //                }
                messages[name].required =  errorImg+"不能为空！";

            }


        });
        var v = form.validate({
            onfocusout: function (element) {
                $(element).valid();
            },
            onkeyup: function (element) {
                $(element).valid();
            },
            rules: rules,
            messages: messages,
            debug: false,
            errorPlacement: function (error, element) {
                if (element.is(":radio,:checkbox")) {
                    element.parent().append(error);
                } else {
                    element.after(error);
                }
            }
        });
        form.data("validateFactory", v);
    }


    function GetMdthodName(ele, i) {
        return ele.attr("name") + "ValidateMethod" + i;
    }
    function GetName(ele) {
        return ele.attr("name");
    }
    function GetPattern(ele, i) {
        return ele.attr("pattern" + i);
    }
    function GetTip(ele, i) {
        return ele.attr("tip" + i);
    }
    function GetIsRequired(ele) {
        if (ele.attr("required")) {
            return true;
        } else {
            return false;
        }
    }
    function GetValidatesize(ele) {
        return parseInt(ele.attr("validatesize"));
    }
};

