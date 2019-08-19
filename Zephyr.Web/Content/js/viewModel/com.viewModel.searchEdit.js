/// <reference path="../jquery-easyui-1.3.1/demo/combobox.html" />
/// <reference path="../jquery-easyui-1.3.1/demo/combobox.html" />
/**
* 模块名：com viewModel
* 程序名: SearchEdit.js
* Copyright(c) 2013-2017
**/
var com = com || {};
com.viewModel = com.viewModel || {};
var lookup_form = {}
var lookup_list = {}
com.viewModel.searchEdit = function (data) {
    var self = this;
    this.singleSelect = data.singleSelect == undefined ? true : data.singleSelect;
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    this.defaultRow = data.defaultRow;
    this.setting = data.setting;
    delete this.form.__ko_mapping__;
    this.grid = {
        size: { w: '100%', h: $(".z-toolbar").height() + $(".container_12").height() + $('.easyui-tabs').height() + 15 },
        url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true,
        //singleSelect: self.singleSelect,
        checkOnSelect: true,
        selectOnCheck: false,
        singleSelect: true
    };

    this.grid.queryParams(data.form);
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onDblClickRow = this.gridEdit.begin;
    this.grid.onClickRow = this.gridEdit.ended;
    this.grid.OnAfterCreateEditor = function (editors) {
        if (editors[self.setting.idField]) com.readOnlyHandler('input')(editors[self.setting.idField].target, true);
    };

    lookup_form = this.form;
    lookup_list = this.grid;
    this.searchClick = function () {
        param = ko.toJS(this.form);
        this.grid.queryParams(param);
    };
    this.clearClick = function () {
        var jsonData = ko.toJS(this.form);
        for (var key in jsonData) {
            jsonData[key] = '';
        }
        ko.mapping.fromJS(jsonData, {}, self.form)
        this.searchClick();
    };

    this.refreshClick = function () {
        window.location.reload();
    };

    this.addClick = function () {
        var row = $.extend({}, self.defaultRow);
        self.gridEdit.addnew(row);
    };

    this.addByNumberClick = function () {
        com.ajax({
            type: 'GET',
            url: self.urls.newkey,
            success: function (d) {
                var row = $.extend({}, self.defaultRow);
                row[self.setting.idField] = d;
                self.gridEdit.addnew(row);
            }
        });
    };

    this.deleteAllDataClick = function () {
        var selectRow = self.grid.datagrid('getSelected');
        if (selectRow) {
            self.gridEdit.ended(); //结束grid编辑状态
            var post = { row: selectRow };
            if (self.gridEdit.ended()) {
                com.ajax({
                    url: self.urls.delete,
                    data: ko.toJSON(post),
                    success: function (d) {
                        com.message('success', '删除成功');
                        self.gridEdit.accept();
                        window.location.reload();
                    }
                });
            }

        }
        else {
            alert("请先选择要删除的数据！")
        }
    }

    //新的delete方法
    this.deleteClick = function () {
        var selectRow = self.grid.datagrid('getSelected');
        if (selectRow) {
            com.message('confirm', "确定要删除选中的单据吗？", function (b) {
                //var selectRow = self.grid.datagrid('getSelected');
                if (selectRow) {
                    var selectIndex = self.grid.datagrid('getRowIndex', selectRow);
                    if (selectIndex == self.editIndex) {
                        self.grid.datagrid('cancelEdit', self.editIndex);
                        self.editIndex = undefined;
                    }
                    self.grid.datagrid('deleteRow', selectIndex);
                }
                self.gridEdit.ended(); //结束grid编辑状态
                var post = {};
                post.list = self.gridEdit.getChanges(self.setting.postListFields);
                if (self.gridEdit.ended() && post.list._changed) {
                    com.ajax({
                        url: self.urls.edit,
                        data: ko.toJSON(post),
                        success: function (d) {
                            com.message('success', '删除成功');
                            self.gridEdit.accept();
                        }
                    });
                }
            });
        }
        else {
            alert("请先选择要删除的数据！")
        }
    }

    this.auditClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        com.auditDialog(function (d) {
            com.ajax({
                type: 'POST',
                url: self.urls.audit + row.BillNo,
                data: JSON.stringify(d),
                success: function () {
                    com.message('success', self.resx.auditSuccess);
                }
            });
        });
    };

    //公共保存方法
    this.saveClick = function () {
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {};
        post.list = self.gridEdit.getChanges(self.setting.postListFields);
        if (self.gridEdit.ended() && post.list._changed) {
            com.ajax({
                url: self.urls.edit,
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', self.resx.editSuccess);
                    self.searchClick();
                    self.gridEdit.accept();
                }
            });
        }
    };

    this.downloadClick = function (vm, event) {
        com.exporter(self.grid).download($(event.currentTarget).attr("suffix"));
    };

    this.copyClick = function () {
        var source = self.grid.datagrid('getSelected');
        if (!source) return com.message('warning', self.resx.noneSelect);
        com.ajax({
            type: 'GET',
            url: self.urls.newkey,
            success: function (d) {
                var target = $.extend({}, source);
                target[self.setting.idField] = d;
                self.gridEdit.addnew(target);
            }
        });
    };

    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        self.gridEdit.begin()
    };

    this.grid.onDblClickRow = this.editClick;

    //公共保存方法--判重
    this.saveClickDup = function () {
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {};
        post.list = self.gridEdit.getChanges(self.setting.postListFields);
        if (self.gridEdit.ended() && post.list._changed) {
            com.ajax({
                url: self.urls.edit,
                data: ko.toJSON(post),
                success: function (d) {
                    if (d == 'NooneExist') {
                        com.message('success', self.resx.editSuccess);
                        self.gridEdit.accept();
                    }
                    else {
                        com.message('error', d + '数据已存在,请删除');
                    }
                }
            });
        }
    };

    this.downloadTemplateClick = function (vm, event) {
        var newGrid = $.extend(true, self.grid, { url: self.urls.downTemplate }, { excelTemplate: data.excelTemplate, excelTemplateTitle: data.excelTemplateTitle || {} });
        com.exporterTemplate(newGrid).download($(event.currentTarget).attr("suffix"));
    };

    this.ImportExcelClick = function (xmlName, className) {
        var target = parent.$('#CommonImportExcelPage').length ? parent.$('#CommonImportExcelPage') : parent.$('<div id="CommonImportExcelPage"></div>').appendTo('body');
        utils.clearIframe(target);
        var opt;
        opt = { title: '选择Excel文件', width: 400, height: 200, modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
        opt.content = "<iframe src='/mms/home/CommonImportExcel' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
        param = { xmlName: xmlName, className: className };
        opt.paramater = param;
        target.window(opt);
    }

    this.SearchDialog = function () {
        var setting = {
            type: this.type,
            targetField: this.targetField,
            sourceField: this.sourceField,
            settingArray: this.settingArray
        }
        //console.log(setting);
        //settingArray 0:width 1:height 2:xmlName
        var target = parent.$('#CommonSearchDialog').length ? parent.$('#CommonSearchDialog') : parent.$('<div id="CommonSearchDialog"></div>').appendTo('body');
        utils.clearIframe(target);
        var opt;
        opt = { title: '列表', width: setting.settingArray[0], height: setting.settingArray[1], modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
        opt.content = "<iframe src='/mms/home/CommonDialog?xmlName=" + setting.settingArray[2] + "' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
        params = { xmlName: setting.settingArray[2] };
        opt.paramater = params;
        opt.onSelect = function (data) {
            //可接收选择数据
            var total = data.total;
            var rows = data.rows;
            if (setting.type == 'form') {
                var jsonData = "{ " + setting.targetField + ": rows[0]['" + setting.sourceField + "'] }";
                jsonData = eval("(" + jsonData + ")");
                ko.mapping.fromJS(jsonData, {}, self.form);
            }
        };
        target.window(opt);
    }
};

//文件上传按钮格式化
function columnFormat(value, rowData, rowIndex, tblName) {
    return '<a href="javascript:void(0)" onclick="ShowUploadFileDialog(\'' + rowData.Object_ID + '\',\'' + tblName + '\')">上传附件</a>';
}

//文件上传弹窗
function ShowUploadFileDialog(Object_ID, tblName) {
    var target = parent.$('#CommonUploadFilePage').length ? parent.$('#CommonUploadFilePage') : parent.$('<div id="CommonUploadFilePage"></div>').appendTo('body');
    utils.clearIframe(target);
    var opt;
    opt = { title: '选择文件', width: 500, height: 300, modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
    opt.content = "<iframe src='/mms/home/CommonUploadFile' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
    param = { Object_ID: Object_ID, TableName: tblName };
    opt.paramater = param;
    target.window(opt);
}

//var SearchDialog = function () {
//    var setting = {
//        type: this.type,
//        targetField: this.targetField,
//        sourceField: this.sourceField,
//        settingArray: this.settingArray
//    }
//    //console.log(setting);
//    //settingArray 0:width 1:height 2:xmlName
//    var target = parent.$('#CommonSearchDialog').length ? parent.$('#CommonSearchDialog') : parent.$('<div id="CommonSearchDialog"></div>').appendTo('body');
//    utils.clearIframe(target);
//    var opt;
//    opt = { title: '列表', width: setting.settingArray[0], height: setting.settingArray[1], modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
//    opt.content = "<iframe src='/mms/home/CommonDialog?xmlName=" + setting.settingArray[2] + "' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
//    params = { xmlName: setting.settingArray[2] };
//    opt.paramater = params;
//    opt.onSelect = function (data) {
//        //可接收选择数据
//        var total = data.total;
//        var rows = data.rows;
//        if (setting.type == 'form') {
//            var jsonData = "{ " + setting.targetField + ": rows[0]['" + setting.sourceField + "'] }";
//            jsonData = eval("(" + jsonData + ")");
//            ko.mapping.fromJS(jsonData, {}, self.form);
//        }
//    };
//    target.window(opt);
//}