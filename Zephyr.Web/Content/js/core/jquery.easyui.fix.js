/// <reference path="../jquery-easyui-1.3.1/plugins/jquery.datetimebox.js" />
/// <reference path="../jquery-easyui-1.3.1/plugins/jquery.datetimebox.js" />
/// <reference path="../jquery-easyui-1.3.1/plugins/jquery.datebox.js" />
/// <reference path="../jquery-easyui-1.3.1/plugins/jquery.datetimebox.js" />
/// <reference path="../jquery-easyui-1.3.1/plugins/jquery.datetimebox.js" />
/// <reference path="../jquery-easyui-1.3.1/plugins/jquery.datetimebox.js" />
/// <reference path="../jquery-easyui-1.3.1/plugins/jquery.datetimebox.js" />
/// <reference path="../jquery-easyui-1.3.1/plugins/jquery.datetimebox.js" />
/**
* 模块名：easyui方法修改
* 程序名: easyui.fix.js
* Copyright(c) 2013-2015 liwenxiang [ liwenxiang.xm@gmail.com ] 
**/

var easyuifix = {};

/* for easyloader.js
 * add after row 13 usage: easyuifix.addLoadModules(_1);
 */
easyuifix.easyloader_addLoadModules = function (modules) {
    $.extend(modules, {
        juidatepick: {
            js: "/Content/js/jquery-plugin/jquery-ui/js/jquery-ui-datepick.min.js",
            css: "/Content/js/jquery-plugin/jquery-ui/css/jquery-ui.css"
        },
        daterange: {
            js: "/Content/js/jquery-plugin/daterange/jquery.daterange.js",
            css: "/Content/js/jquery-plugin/daterange/jquery.daterange.css",
            dependencies: ["juidatepick"]
        },
        lookup: {
            js: "/Content/js/jquery-easyui-1.3.1/plugins/jquery.lookup.js",
            dependencies: ["combo"]
        },
        autocomplete: {
            js: "/Content/js/jquery-easyui-1.3.1/plugins/jquery.autocomplete.js",
            dependencies: ["combo"]
        },
        extend: {
            js: "/Content/js/jquery-easyui-1.3.2/jquery.easyui.extend.js"
        }
    });
};

easyuifix.easyloader_setting = function (easyloader, src) {
    easyloader.theme = utils.getRequest("theme", src);
    easyloader.locale = utils.getRequest("locale", src);
};

/* for parser.js
* add after row 89 usage: easyuifix.parser_addplugins($.parser.plugins);
*/
easyuifix.parser_addplugins = function (plugins) {
    var arr = ["daterange", "lookup", "autocomplete", "combobox", "datetimebox", "datebox"];
    for (var i in arr)
        plugins.push(arr[i]);
};

/* for jquery.easyui.min.js 
* add after row 3745 between row [tab.panel("options").tab.remove();] and row [tab.panel("destroy");]
* for clear memory 
* usage: 
*     tab.panel("options").tab.remove();
*     easyuifix.easyui_min_setIframeFree();
*     tab.panel("destroy");
*/
easyuifix.easyui_min_setIframeFree = function (tab) {
    var frame = $('iframe', tab); if (frame.length > 0) { frame[0].contentWindow.document.write(''); frame[0].contentWindow.close(); frame.remove(); if ($.browser.msie) { CollectGarbage(); } }
}

/* for tabs.js
* add after row 392 _5a.onSelect.call(_58,_5f,_31(_58,_5d));
  usage: easyuifix.tabs_showtabonselect(_5d);
*/
easyuifix.tabs_showtabonselect = function (tab) {
    tab.show();    //打开时其它页签先隐藏,,提升用户体验，点击时再显示
}

/* for easyui-lang_zh_CN.js
*/
easyuifix.lang_zh_CN = function () {
    if ($.fn.lookup) {
        $.fn.lookup.defaults.missingMessage = '该输入项为必输项';
    }
};

/* for easyui-datagrid.js
* _175 = easyuifix.datagrid_editors_checkboxVal(_173, 174);
*/
easyuifix.datagrid_editors_checkboxVal = function (checkbox, value) {
    return (typeof value == 'boolean' && $(checkbox).val() == value.toString());
};

/* for easyui-datagrid.js
*/
easyuifix.datagrid_beforeDestroyEditor = function (jq, rowIndex, row) {
    var opts = $.data(jq, "datagrid").options;
    if (opts.OnBeforeDestroyEditor) {
        var editors = {}, list = $(jq).datagrid('getEditors', rowIndex) || [];
        $.each(list, function () { editors[this.field] = this; });
        opts.OnBeforeDestroyEditor(editors, row, rowIndex, jq);
    }
};
/* for easyui-datagrid.js and easyui.min.js
*/
easyuifix.datagrid_afterCreateEditor = function (jq, rowIndex, row) {
    //var opts = $.data(jq, "datagrid").options;
    //if (opts.OnAfterCreateEditor) {
    //    var editors = {}, list = $(jq).datagrid('getEditors', rowIndex) || [];
    //    $.each(list, function () { editors[this.field] = this; });
    //    opts.OnAfterCreateEditor(editors, row, rowIndex, jq);
    //}
    //debugger;
    var opts = $.data(jq, "datagrid").options;
    if (opts.OnAfterCreateEditor) {
        var editors = {}, list = $(jq).datagrid('getEditors', rowIndex) || [];
        $.each(list, function () {
            editors[this.field] = this;

            var fields = [
                "ProcessDesc",//工艺管理-生产整改方案管理-单据明细  描述
                "RectificationContent",//工艺管理-生产整改方案管理-单据明细  整改内容
                "Reason",//工艺管理-设计更改申请  原因
                "ChangeContent",//工艺管理-设计更改申请  更改内容
                "WorkStepsContent",//工艺管理-产品工时定额编制  工步描述
                "Remark",//工艺管理-板材报料  备注
                "ApproveRemark"
            ]

            if ($.inArray(this.field, fields) != -1) {
                this.target.bind('focus', function () {
                    var txtself = this;
                    var elmOffset = $(txtself).offset()
                    var divObj = $("<div style='display:block;position:absolute;z-index:100;'></div>");
                    var areaObj = $("<textArea rows=10 style='width:100%;'/>");
                    areaObj.val($(txtself).val());
                    divObj.append(areaObj);
                    $(document.body).prepend(divObj);
                    divObj.animate({ left: elmOffset.left, top: elmOffset.top, width: "450px", height: "100px" });
                    areaObj.focus();
                    areaObj.dblclick(function () {
                        $(txtself).val(areaObj.val());
                        divObj.remove();
                    });
                    areaObj.blur(function () {
                        $(txtself).val(areaObj.val());
                        divObj.remove();
                    });
                });
            }
        });
        //创建编辑器时 绑定事件
        opts.OnAfterCreateEditor(editors, row, rowIndex, jq);
    }
};

/* for easyui-combo.js to convert disable to readonly
*/
easyuifix.combo_disableToReadonly = function (jq, b) {
    var options = $.data(jq, "combo").options;
    var combo = $.data(jq, "combo").combo;
    if (b) {
        options.disabled = true;
        $(jq).attr("readonly", true);
        combo.find(".combo-value").attr("readonly", true).addClass("readonly");
        combo.find(".combo-text").attr("readonly", true).addClass("readonly");
    } else {
        options.disabled = false;
        $(jq).removeAttr("readonly");
        combo.find(".combo-value").removeAttr("readonly").removeClass("readonly");
        combo.find(".combo-text").removeAttr("readonly").removeClass("readonly");
    }
};

/* for easyui-spinner.js to convert disable to readonly
*/
easyuifix.spinner_disableToReadonly = function (jq, b) {
    var options = $.data(jq, "spinner").options;
    if (b) {
        options.disabled = true;
        $(jq).attr("readonly", true).addClass("readonly");
    } else {
        options.disabled = false;
        $(jq).removeAttr("readonly").removeClass("readonly");
    }
};



/* for datagrid.js
 * add at last if (easyuifix) easyuifix.datagrid_editor_extend();
 */
easyuifix.datagrid_editor_extend = function () {

    //拓展表头菜单，显示隐藏列
    var createGridHeaderContextMenu = function (e, field) {
        e.preventDefault();
        var grid = $(this);/* grid本身 */
        var headerContextMenu = this.headerContextMenu;/* grid上的列头菜单对象 */
        if (!headerContextMenu) {
            var fields = grid.datagrid('getColumnFields');
            var MenuHeight = (fields.length + 1) * 25 > 300 ? 300 : (fields.length + 1) * 25;
            var tmenu = $('<div class="easyui-menu" style="width:100px;height:' + MenuHeight + 'px;overflow:auto;"></div>').appendTo('body');
            $('<div iconCls="icon-empty" field="all" style="color:red;font-weight:bold;"></div>').html('全选').appendTo(tmenu);
            for (var i = 0; i < fields.length; i++) {
                var fildOption = grid.datagrid('getColumnOption', fields[i]);
                if (!fildOption.hidden) {
                    $('<div iconCls="icon-ok" field="' + fields[i] + '"></div>').html(fildOption.title).appendTo(tmenu);
                } else {
                    $('<div iconCls="icon-empty" field="' + fields[i] + '"></div>').html(fildOption.title).appendTo(tmenu);
                }
            }
            headerContextMenu = this.headerContextMenu = tmenu.menu({
                onClick: function (item) {
                    $(item.target).parent().show();
                    var field = $(item.target).attr('field');
                    if (field == 'all') {
                        if (item.iconCls == 'icon-empty') {
                            $(item.target).nextAll().each(function (index, element) {
                                var currentFiled = $(element).attr('field');
                                grid.datagrid('showColumn', currentFiled);
                            });
                            $(item.target).parent().children('.menu-item').each(function (index, element) {
                                $(this).menu('setIcon', {
                                    target: element,
                                    iconCls: 'icon-ok'
                                });
                            });
                        }
                        else {
                            $(item.target).nextAll().each(function (index, element) {
                                var currentFiled = $(element).attr('field');
                                grid.datagrid('hideColumn', currentFiled);
                            });
                            $(item.target).parent().children('.menu-item').each(function (index, element) {
                                $(this).menu('setIcon', {
                                    target: element,
                                    iconCls: 'icon-empty'
                                });
                            });
                        }
                    }
                    else if (item.iconCls == 'icon-ok') {
                        grid.datagrid('hideColumn', field);
                        $(this).menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-empty'
                        });
                    }
                    else if (item.iconCls == 'icon-empty') {
                        grid.datagrid('showColumn', field);
                        $(this).menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-ok'
                        });
                    }
                }
            });
        }
        headerContextMenu.menu('show', {
            left: e.pageX,
            top: e.pageY
            //top: e.pageY
        });
    };

    if ($.fn.datagrid) {
        //拓展表头菜单显示隐藏列
        $.fn.datagrid.defaults.onHeaderContextMenu = createGridHeaderContextMenu;

        //拓展datagrid datetimebox编辑时控件
        $.extend($.fn.datagrid.defaults.editors, {
            datetimebox: {
                init: function (container, options) {
                    var box = $('<input type="text" class="easyui-datetimebox"/>').appendTo(container);
                    box.datetimebox(options);
                    return box;
                },
                getValue: function (target) {
                    return $(target).datetimebox('getValue');
                },
                setValue: function (target, value) {
                    if (value != null && value.indexOf('T') > 0)
                        value = value.replace(/T/, " ")
                    $(target).datetimebox('setValue', value);
                },
                resize: function (target, width) {
                    var box = $(target);
                    box.datetimebox('resize', width);
                },
                destroy: function (target) {
                    $(target).datetimebox('destroy');
                }
            }
        });
        $.extend($.fn.datagrid.defaults.editors, {
            timespinner: {
                init: function (container, options) {
                    var input = $('<input class="easyui-timespinner" style="width:80px;" required="required" data-options="showSeconds:false">').appendTo(
                        container);
                    input.timespinner(options);
                    return input;
                },
                destroy: function (target) {
                    $(target).remove();
                },
                getValue: function (target) {
                    var timVal = $(target).timespinner('getValue');//此处是对获取的时间的转化，个人需要的时间形式不同，转化方法也不一样
                    var arr = timVal.split(":");
                    return timVal;
                },
                setValue: function (target, value) {
                    $(target).timespinner('setValue', value);
                },
                resize: function (target, width) {
                    $(target)._outerWidth(width);
                }
            }
        });

        //拓展datagrid datebox编辑时控件
        $.extend($.fn.datagrid.defaults.editors, {
            datebox: {
                init: function (container, options) {
                    var box = $('<input type="text" class="easyui-datebox"/>').appendTo(container);
                    box.datebox(options);
                    return box;
                },
                getValue: function (target) {
                    return $(target).datebox('getValue');
                },
                setValue: function (target, value) {
                    if (value != null && value.indexOf('T') > 0)
                        value = value.split('T')[0];
                    $(target).datebox('setValue', value);
                },
                resize: function (target, width) {
                    var box = $(target);
                    box.datebox('resize', width);
                },
                destroy: function (target) {
                    $(target).datebox('destroy');
                }
            }
        });

        //拓展datagrid numberspinner编辑时控件
        if ($.fn.numberspinner) {
            $.extend($.fn.datagrid.defaults.editors, {
                numberspinner: {
                    init: function (container, options) {
                        var input = $('<input type="text">').appendTo(container);
                        return input.numberspinner(options);
                    },
                    destroy: function (target) {
                        $(target).numberspinner('destroy');
                    },
                    getValue: function (target) {
                        return $(target).numberspinner('getValue');
                    },
                    setValue: function (target, value) {
                        $(target).numberspinner('setValue', value);
                    },
                    resize: function (target, width) {
                        $(target).numberspinner('resize', width);
                    }
                }
            });
        }

        //拓展datagrid autocomplete编辑时控件
        if ($.fn.autocomplete) {
            $.extend($.fn.datagrid.defaults.editors, {
                autocomplete: {
                    init: function (container, options) {
                        var input = $('<input type="text" class="z-text datagrid-editable-input">').appendTo(container);
                        return input.autocomplete(options);
                    },
                    destroy: function (target) {
                        $(target).autocomplete('destroy');
                    },
                    getValue: function (target) {
                        return $(target).val();
                    },
                    setValue: function (target, value) {
                        $(target).val(value);
                    },
                    resize: function (target, width) {
                        $(target).width(width);
                    }
                }
            });
        }

        //拓展datagrid combogrid编辑时控件
        if ($.fn.combogrid) {
            $.extend($.fn.datagrid.defaults.editors, {
                combogrid: {
                    init: function (container, options) {
                        var input = $('<input type="text" class="datagrid-editable-input">').appendTo(container);
                        input.combogrid(options);
                        return input;
                    },
                    destroy: function (target) {
                        $(target).combogrid('destroy');
                    },
                    getValue: function (target) {
                        return $(target).combogrid('getValue');
                    },
                    setValue: function (target, value) {
                        $(target).combogrid('setValue', value);
                    },
                    resize: function (target, width) {
                        $(target).combogrid('resize', width);
                    }
                }
            });
        }

        //拓展datagrid lookup编辑时控件
        if ($.fn.lookup) {
            $.extend($.fn.datagrid.defaults.editors, {
                lookup: {
                    init: function (container, options) {
                        var input = $('<input type="text" class="z-text datagrid-editable-input">').appendTo(container);
                        return input.lookup(options);
                    },
                    destroy: function (target) {
                        $(target).lookup('destroy');
                    },
                    getValue: function (target) {
                        return $(target).lookup('getValue');
                    },
                    setValue: function (target, value) {
                        $(target).lookup('setValue', value);
                    },
                    resize: function (target, width) {
                        $(target).lookup('resize', width);
                    }
                }
            });
        }

        //拓展datagrid label编辑时控件
        $.extend($.fn.datagrid.defaults.editors, {
            label: {
                init: function (container, options) {
                    var input = $('<div></div>').appendTo(container);
                    return input;
                },
                destroy: function (target) {

                },
                getValue: function (target) {
                    return $(target).html();
                },
                setValue: function (target, value) {
                    $(target).html(value);
                },
                resize: function (target, width) {

                }
            }
        });

        //拓展datagrid searchdialog编辑时控件
        $.extend($.fn.datagrid.defaults.editors, {
            searchdialog: {
                init: function (container, options) {
                    var width = $(container).css("width");
                    width = width.substr(0, width.length - 2) - 30;
                    var setting = JSON.stringify(options[1]).replace(/\"/g, "'");
                    var input = $('<input type="text" style="width:' + width + 'px" class="z-text datagrid-editable-input"><span class="searchbox-button" onclick="SearchDialog.call(' + setting + ')"></span>').appendTo(container);
                    $(input.get(0)).validatebox(options[0]);
                    return input;
                },
                destroy: function (target) {

                },
                getValue: function (target) {
                    return $(target).val();
                },
                setValue: function (target, value) {
                    $(target).val(value);
                },
                resize: function (target, width) {

                }
            }
        });

        $.extend($.fn.datagrid.defaults.editors, {
            textAreanew: {
                init: function (container, options) {
                    var input = $("<span style=\"position: absolute;\"><textarea rows=\"10\" style=\"z-index:100px;position:absolute;top:-11px;\" class=\"datagrid-editable-input\"></textarea></span>").appendTo(container);
                    return input;
                },
                destroy: function (target) {
                },
                getValue: function (target) {
                    return $(target).val();
                },
                setValue: function (target, value) {
                    $(target).val(value);
                },
                resize: function (target, width) {
                    $(target)._outerWidth(width);
                }
            }
        });
    }
}