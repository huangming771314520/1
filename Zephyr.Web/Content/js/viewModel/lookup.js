/**
* 模块名：共通弹出窗口viewModel
* 程序名: lookup.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

function viewModel(data) {
    this.form = {};
    this.form[data.textField] = data.text;
    this.form[data.valueField] = data.value;
    this.textTitle = data.textTitle;
    this.valueTitle = data.valueTitle;
    this.extendColumns = data.extendColumns;

    this.gridSetting = data.grid;
    this.gridSetting.queryParams = "";
    this.searchClick = function () {
        this.form[data.textField] = $('#textField').val();
        if (data.searchValueDefault != '') {
            this.form[data.searchValueDefault] = $('#valueField').val();
        }
        else {
            this.form[data.valueField] = $('#valueField').val();
        }

        delete this.form.value;
        delete this.form.text;
        var queryParams = $.extend({ _lookupType: data.lookupType, _r: Math.random() }, data.queryParams, ko.toJS(this.form));
        this.gridSetting.queryParams(queryParams);
    };
    this.clearClick = function () {
        //$.each(this.form, function () { this(''); });
        //this.searchClick();
        $('#valueField').val('');
        $('#textField').val('');
        this.searchClick();
    };
    this.confirmClick = function () {
        var rows = grid.datagrid('getSelections') || [];

        var opts = grid.datagrid('getColumnFields'); //这是获取到所有的FIELD
        var colField = [];
        for (i = 0; i < opts.length; i++) {
            var col = grid.datagrid("getColumnOption", opts[i]);
            colField.push(col.field);//把Filed PUSH到数组里去
        }

        var txt = '', val = '';
        if (data.extendColumns) {
            var _length = 0;
            var _length1=0;
            $.each(data.extendColumns, function () {
                _length = _length + 1;
               
            });
            if (data.formColumns) {
                $.each(data.formColumns, function () {
                    _length1 = _length1 + 1;

                });
            }
           
           
            if (_length > 0) {
                val += '{';
                $.each(rows, function (_index) {
                    $.each(data.extendColumns, function (index) {
                        val += "'" + data.extendColumns[index] + "':'" + rows[_index][data.extendColumns[index]] + "',";
                    })

                });
                if (val.indexOf(',') > 0) {
                    val = val.substr(0, val.length - 1) + '},';
                }
                txt = "list ";
            }
            else if (_length1>0) {
                var selectVal = "";
                var selectTxt = "";
                $.each(rows, function () {
                    selectVal = this[data.valueField];
                    selectTxt = this[data.textField];
                });
                val += "{'" + data.formColumns[0] + "':'" + selectVal + "','" + data.formColumns[1] + "':'" + selectTxt + "'},";
            }
            else  {
                $.each(rows, function () {
                    txt += this[data.textField] + ',';
                    val += this[data.valueField] + ',';
                });
                //$.each(rows, function (i, el) {
                //    var ell = el["WH_Code"];
                //    val += "{";
                //    for (var item in colField) {
                //        val += "'" + colField[item] + "':'" + el["" + colField[item] + ""] + "',";
                //    }                    
                //    val = val.substr(0, val.length - 1);
                //    val += "},";
                //});
            }

        }
        if (txt) txt = txt.substr(0, txt.length - 1);
        if (val) val = val.substr(0, val.length - 1);
        window.returnValue = { value: val, text: txt };
        this.cancelCick();
        return false;
    };
    this.cancelCick = function () {
        data.panel.data("returnValue", window.returnValue);
        data.panel.window('close');
    };
    this.keyDown = function (vm, e) {
        var enterKey = "13", key = e.keyCode || e.which || e.charCode; //兼容IE(e.keyCode)和Firefox(e.which)
        if (key == enterKey) {
            this.searchClick();
            e.preventDefault();
        }
        return true;
    }

    var vm = ko.mapping.fromJS(this);

    vm.gridSetting.queryParams($.extend(data.queryParams, { _lookupType: data.lookupType }));
    return vm;
}

//获取属性
function getOptions() {
    var iframe = getThisIframe();
    var panel = parent.$(iframe).parent();
    var options = $.extend({}, $.fn.lookup.defaults, ko.toJS(panel.data("lookup") || {}), true);
    options.panel = panel;

    //从columns中取得title
    var txtTitle, valTitle;
    options.grid.columns = options.grid.columns || [[]];
    $.each(options.grid.columns[0], function () {
        if (this.field == options.textField)
            txtTitle = this.title;
        if (this.field == options.valueField)
            valTitle = this.title;
    });

    var w = $(window).width() - 48;
    var valueWidth = options.grid.columns[0].length ? 150 : Math.floor(w * 0.3);
    var textWidth = options.grid.columns[0].length ? 150 : Math.floor(w * 0.7);

    //从textField valueField生成columns
    if (valTitle)
        options.valueTitle = valTitle;
    else if (options.valueTitle != "" && options.searchValueDefault == '') {
        //options.valueField = 'value';
        // options.valueTitle = '编码';
        options.grid.columns[0].push({ title: options.valueTitle, field: options.valueField, align: 'left', sortable: true, width: valueWidth });
    }

    if (txtTitle)
        options.textTitle = txtTitle;
    else if (options.textTitle != "" && options.searchValueDefault == '') {
        //options.textField = 'text';
        //options.textTitle = '名称';
        options.grid.columns[0].push({ title: options.textTitle, field: options.textField, align: 'left', sortable: true, width: textWidth });
    }
    if (options.extendColumns) {
        var columnCount = 0;
        $.each(options.extendColumns, function () {
            columnCount++;
        })
        var _width = 0;
        switch (columnCount) {
            case 2:
                _width = 200;
                break;
            case 3:
                _width = 180;
                break;
            default:
                _width = 120;
                break;
        }

        $.each(options.extendColumns, function (index) {

            options.grid.columns[0].push({ title: options.extendTitleColumns[index], field: options.extendColumns[index], align: 'left', sortable: true, width: _width });
        })
    }
    //合并参数
    options.grid = $.extend(options.grid, {
        size: { w: 0, h: 68 },
        url: '/plugins/getlookupdata?_r=' + Math.random(),
        pagination: true,
        idField:  options.valueField,
        onDblClickRow: function (index, row) {
            rows = row || index; //for treegrid dbClickRow(row)
            var txt = '', val = '';
            if (options.extendColumns) {
                var _length = 0;
                var _length1 = 0;
                $.each(options.extendColumns, function () {
                    _length = _length + 1;
                });
                if (options.formColumns) {
                    $.each(options.formColumns, function () {
                        _length1 = _length1 + 1;
                    });
                }

                if (_length > 0) {
                    val += '{';
                    $.each(options.extendColumns, function (index) {
                        val += "'" + options.extendColumns[index] + "':'" + rows[options.extendColumns[index]] + "',";
                    })
                    if (val.indexOf(',') > 0) {
                        val = val.substr(0, val.length - 1) + '},';
                    }
                    txt = "list ";
                }
                else if (_length1 > 0) {
                    var selectVal = "";
                    var selectTxt = "";
                    $.each(rows, function () {
                        selectVal = rows[options.valueField];
                        selectTxt = rows[options.textField];
                    });
                    val += "{'" + options.formColumns[0] + "':'" + selectVal + "','" + options.formColumns[1] + "':'" + selectTxt + "'},";
                }
                else {
                    $.each(rows, function () {
                        txt += rows[options.textField] + ',';
                        val += rows[options.valueField] + ',';
                    });
                }
            }
            if (txt) txt = txt.substr(0, txt.length - 1);
            if (val) val = val.substr(0, val.length - 1);
            window.returnValue = { value: val, text: txt };
            vmodel.cancelCick();
        },
        singleSelect: !options.multiple
    });

    //扩展树的参数
    if (options.parentField)
        $.extend(options.grid, { pagination: false, treeField: options.valueField, parentField: options.parentField });

    //多选
    if (options.multiple) {
        options.grid.rownumbers = false;
        options.grid.frozenColumns = [[{ field: 'ck', checkbox: true }]];
    }

    return options;
}

//获取本页面所在的iframe
function getThisIframe() {
    if (!parent) return null;
    var iframes = parent.document.getElementsByTagName('iframe');
    if (iframes.length == 0) return null;
    for (var i = 0; i < iframes.length; ++i) {
        var iframe = iframes[i];
        if (iframe.contentWindow === self) { //self 指的是当前窗口
            return iframe;
        }
    }
}

//实现ctrl + shift多选
//function clickrow(rowIndex, rowData) {
//    var row = grid.datagrid('getSelected');
//    if (row) {
//        var ctrlKey = event ? event.ctrlKey : false;
//        var shiftKey = event ? event.shiftKey : false;
//        var index = grid.datagrid('getRowIndex', row);
// 
//        if (ctrlKey) {

//        }
//        else if (shiftKey) {
//            var step = rowIndex > index ? 1 : -1;
//            while(index!=rowIndex)
//            {
//                index = index + step;
//                grid.datagrid('selectRow', index);
//            }
//        }
//        else { 
//            grid.datagrid('clearSelections'); //unselectAll
//            grid.datagrid('selectRow', rowIndex);
//        }

//    }
//}

//实现方向键功能 上下（行移动）左右（翻页）
//grid.data('datagrid').panel.keydown(undownkey);
//function undownkey(e) {
//    var row, index, data, key = (e.keyCode) || (e.which) || (e.charCode);
//    if (key == "37") { //left
//        var options = grid.datagrid('getPager').data('pagination').options;
//        if (options.pageNumber > 1) {
//            grid.datagrid('getPager').pagination('select', options.pageNumber - 1);
//        }
//    }
//    if (key == "39") { //right
//        var options = grid.datagrid('getPager').data('pagination').options;
//        if (options.pageNumber < Math.ceil(options.total / options.pageSize)) {
//            grid.datagrid('getPager').pagination('select', options.pageNumber + 1);
//        }
//    }
//    if (key == "38") //up
//    {
//        row = grid.datagrid('getSelected');
//        var index = row?(grid.datagrid('getRowIndex', row) -1):0;
//        if (index < 0) index = 0;
//        grid.datagrid('selectRow', index);
//        e.preventDefault();
//    }
//    if (key == "40") //down
//    {
//        data = grid.datagrid('getData').rows;
//        row = grid.datagrid('getSelected');
//        var index =row? (grid.datagrid('getRowIndex', row) +1):0;
//        if (index<data.length) grid.datagrid('selectRow', index);
//        e.preventDefault();
//    }
//}


