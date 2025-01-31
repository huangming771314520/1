﻿/**
* jquery.lookup.js
* create by liwenxiang 2012-11-07
*/
(function ($) {
    $.fn.lookup = function (arg0, arg1) {
        if (typeof arg0 == "string") {
            var methods = $.fn.lookup.methods[arg0];
            if (methods) {
                return methods(this, arg1);
            } else {
                return this.combo(arg0, arg1);
            }
        }
        arg0 = arg0 || {};
        return this.each(function () {
            var lookup = $(this).data("lookup");
            if (lookup) {
                $.extend(true, lookup.options, arg0);
                initlookup(this);
            } else {
                lookup = $(this).data("lookup", { options: $.extend(true, {}, $.fn.lookup.defaults, $.fn.lookup.parseOptions(this), arg0) });
                initlookup(this);
            }
        });
    };

    $.fn.lookup.methods = {
        setValue: function (jq, value, queryText) {
            return jq.each(function () {
                setValuesHandle(this, value, queryText);
            });
        }
    };

    function setValuesHandle(target, value, queryText) {
        queryText = queryText || true;
        var that = $(target), options = that.data('lookup').options, text;
        that.combo('setValue', value);
        if (queryText) {
            if (value && options.url) {
                var queryParams = {};
                queryParams[options.valueField] = value;
                queryParams["_lookupType"] = options.lookupType;
                queryParams["_valueFeild"] = options.valueField;
                $.getJSON(options.url, queryParams, function (d) {
                    text = $.map(d.rows || d, function (v) { return v[options.textField] }).join(',');
                    that.combo('setText', value || text);
                });
            }
            else {
                that.combo('setText', value);
            }
        }
    }

    $.fn.lookup.parseOptions = function (target) {
        var t = $(target);
        return $.extend({},
		$.fn.combo.parseOptions(target), $.parser.parseOptions(target, ["valueField", "textField", "action", "grid", "view", 'window', 'valueTitle', 'textTitle']));
    };


    $.fn.lookup.defaults = $.extend({}, $.fn.combo.defaults, {
        view: ''
        , url: ''
        , customShowPanel: false
        , textField: ''
        , textTitle: ''
        , valueField: ''
        , valueTitle: ''
        , parentField: ''
        , multiple: false
        , lookupType: ''
        , queryParams: {}
        , grid: {}
        , searchValueDefault: ''
        , extendColumns: []
        , extendTitleColumns: []
        , window: {
            title: '弹出选择'
            , width: 600
            , height: 420
            , modal: true
            , collapsible: false
            , minimizable: false
            , maximizable: true
            , closable: true
        }
    });

    function setSelection(jq, selectionStart, selectionEnd) {
        if (jq.lengh == 0) return jq;
        input = jq[0];

        if (input.createTextRange) {
            var range = input.createTextRange();
            range.collapse(true);
            range.moveEnd('character', selectionEnd);
            range.moveStart('character', selectionStart);
            range.select();
        } else if (input.setSelectionRange) {
            input.focus();
            input.setSelectionRange(selectionStart, selectionEnd);
        }
        return jq;
    }

    function clearIframe(context) {
        var frame = $('iframe', context);
        if (frame.length > 0) {
            frame[0].contentWindow.document.write('');
            frame[0].contentWindow.close();
            frame.remove();
            if ($.browser.msie) {
                CollectGarbage();
            }
        }
    }

    function initlookup(target) {
        var that = $(target), options = that.data('lookup').options;
        var _newValue = "";
        var fnShow = function () {
            //$(target).combo({
            //    onChange: function (newValue, oldValue) {
            //        //$("#be_return").click();
            //        var toBeCallback = $('#be_return');
            //        var toFormCallback = $('#form_return');
            //        //判断元素是否存在
            //        if (toBeCallback.length > 0 && newValue.indexOf('}') > 0) {
            //            $('#be_return').val(newValue);
            //            $('#be_return').trigger("click");
            //        }
            //        if (toFormCallback.length > 0 && newValue.indexOf('}') > 0) {
            //            $('#form_return').val(newValue);
            //            $('#form_return').trigger("click");
            //        }
            //        _newValue = newValue;
            //    }
            //});
            that.combo('hidePanel');
            var panel = that.data('combo').panel.remove('style').addClass('lookup-win');
            var pPanel = parent.$(panel);
            options.window.content = "<iframe id='frm_win_" + options.valueField + "' src='" + options.view + (options.view.indexOf('?') > -1 ? '&' : '?') + "r=" + Math.random() + "' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>" //frameborder="0" for ie7
            options.window.onClose = function () {
                var rtnValue = pPanel.data("returnValue");
                if (rtnValue) {
                    if (rtnValue.text == "list") {
                        if ("undefined" != typeof lookup_list) {
                            var selectIndex = $(target).parents('.datagrid-row').attr('datagrid-row-index');
                            var row = lookup_list.datagrid('getData').rows[selectIndex];
                            //var row = lookup_list.datagrid('getSelected');
                            //var selectIndex = lookup_list.datagrid('getRowIndex', row);
                            lookup_list.datagrid("endEdit", selectIndex);
                            if (selectIndex >= 0) {
                                var jsonData = eval("(" + rtnValue.value + ")");
                                if (row) {
                                    for (var key in row) {
                                        if (typeof (jsonData[key]) != "undefined") {
                                            row[key] = jsonData[key];
                                        }
                                    }
                                }
                                lookup_list.datagrid('refreshRow', selectIndex).datagrid('beginEdit', selectIndex);
                            }
                        }
                    }
                    else {
                        if ("undefined" != typeof lookup_form) {
                            jsonData = eval("(" + rtnValue.value + ")");
                            delete jsonData["undefined"];
                            ko.mapping.fromJS(jsonData, {}, lookup_form)
                        }
                    }
                    //$(target).lookup('setText', rtnValue.text);
                    //$(target).lookup('setValue', rtnValue.value);
                }
                clearIframe(pPanel);
                pPanel.window('destroy');
                if (that.data("combo")) {
                    var txt = that.data("combo").combo.find(".combo-text");
                    var len = txt.val().length;
                    setSelection(txt, len, len);
                }
            };
            options.text = $(target).combo('getText'); 
            $(target).combo('setValue', _newValue);
            //options.value = $(target).combo('getValue');
            options.value = _newValue;
            pPanel.window(options.window);
            pPanel.data("lookup", options);
        };
        that.addClass("lookup-f");

        if (!options.customShowPanel && options.lookupType) {    //防止访问url,参数不对，产生错误，自定义的不需要这个url

            options.view = '/plugins/lookup';
            options.url = '/plugins/getlookupdata';
            options.onShowPanel = fnShow;
        }

        that.combo(options);
        that.data('combo').combo.addClass("lookup");
        //that.lookup('setValue', that.val()); //再次增加属性时值被清空，删除此句
    }

    //function initlookup(target) {
    //    var that = $(target), options = that.data('lookup').options;
    //    var fnShow = function () {
    //        that.combo('hidePanel');
    //        var panel = that.data('combo').panel.remove('style').addClass('lookup-win');
    //        var pPanel = parent.$(panel);
    //        options.window.content = "<iframe id='frm_win_" + options.valueField + "' src='" + options.view + (options.view.indexOf('?') > -1 ? '&' : '?') + "r=" + Math.random() + "' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>" //frameborder="0" for ie7
    //        options.window.onClose = function () {
    //            var rtnValue = pPanel.data("returnValue");
    //            if (rtnValue) {
    //                $(target).combo('setText', rtnValue.text);
    //                $(target).combo('setValue', rtnValue.value,false);
    //            }

    //            clearIframe(pPanel);
    //            pPanel.window('destroy');
    //            var txt = that.data("combo").combo.find(".combo-text");
    //            var len = txt.val().length;
    //            setSelection(txt, len, len);
    //        };

    //        options.text = $(target).combo('getText');
    //        options.value = $(target).combo('getValue');
    //        pPanel.window(options.window);
    //        pPanel.data("lookup", options);
    //    };
    //    that.addClass("lookup-f");

    //    if (!options.customShowPanel && options.lookupType) {    //防止访问url,参数不对，产生错误，自定义的不需要这个url
    //        options.view = '/plugins/lookup';
    //        options.url = '/plugins/getlookupdata';
    //        options.onShowPanel = fnShow;
    //    }

    //    that.combo(options);
    //    that.data('combo').combo.addClass("lookup");
    //    //that.lookup('setValue', that.val()); //再次增加属性时值被清空，删除此句
    //}
    function isExitsVariable(variableName) {
        try {
            if (typeof (variableName) == "undefined") {
                return false;
            } else {
                return true;
            }
        } catch (e) { }
        return false;
    }

})(jQuery);