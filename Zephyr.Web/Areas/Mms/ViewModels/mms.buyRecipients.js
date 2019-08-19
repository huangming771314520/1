/// <reference path="common.js" />

/**
* 模块名：共通脚本
* 程序名: 材料模块通用方法buyR.com.js
* Copyright(c) 2013-2015 liwenxiang [ liwenxiang.xm@gmail.com ] 
**/
var buyR = buyR || {};
buyR.viewModel = buyR.viewModel || {};

var mms = mms || {};
mms.com = {};
 
//弹出选择材料窗口
mms.com.selectMaterial = function (vm, param, DocumentTypeNo) {
    var grid = vm.grid;
    var addnew = vm.gridEdit.addnew;
    var defaultRow = vm.defaultRow;
    var url = vm.addurl.outrowid; 
    var Object_ID = vm.scrollKeys.current();
    var orgRows = grid.datagrid('getData').rows;
    var compareArray = ["Material_No"];
    var orgLength = orgRows.length;
    var fnEqual = function (row1, row2) {
        for (var key in compareArray)
            if (row1[compareArray[key]] != row2[compareArray[key]])
                return false;
        return true;
    }
    var fnExist = function (row) {
        for (var i in orgRows)
            if (fnEqual(orgRows[i], row))
                return true;
        return false;
    };

    var target = parent.$('#selectMaterial').length ? parent.$('#selectMaterial') : parent.$('<div id="selectMaterial"></div>').appendTo('body');
    utils.clearIframe(target);

    var opt = { title: '选择在库设备', width: 800, height: 550, modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
    //通过url来判断使用html
    var urlsEdit = vm.urls.edit;
    if (urlsEdit.indexOf('RecipientsMain') > 0) {
        opt.content = "<iframe id='frm_win_material' src='/mms/home/LookupRecipient' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
    }
    else {
        opt.content = "<iframe id='frm_win_material' src='/mms/home/LookupRecipient' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
    }

    opt.paramater = param;      //可传参数
    opt.DocumentTypeNo = DocumentTypeNo;
    opt.onSelect = function (data) {
        //可接收选择数据
        var total = data.total;
        var rows = data.rows;
        com.ajax({
            type: 'GET',
            url: '/api/mms/home/GetGuidArray',
            data: { total: total },
            success: function (d) {
                var objectIdList = d;
                var _Object_ID = objectIdList[0];
                for (var i in rows) {
                    //grid.datagrid('appendRow', $.extend({ _isnew: true, Object_ID: objectIdList[i] }, defaultRow, rows[i]));
                    //grid.datagrid('beginEdit', orgLength);
                    addnew($.extend({ Object_ID: _Object_ID }, defaultRow, rows[i]));
                }
            }
        });
    };
    target.window(opt);
};

//出库管理viewMode


buyR.viewModel.edit = function (data) {
    var self = this;

    this.dataSource = data.dataSource;                          //下拉框的数据源
    this.urls = data.urls;                                       //api服务地址
    this.addurl = data.addurl;
    this.resx = data.resx;                                      //中文资源
    this.scrollKeys = ko.mapping.fromJS(data.scrollKeys);       //数据滚动按钮（上一条下一条）
    this.form = ko.mapping.fromJS(data.form || data.defaultForm); //表单数据
    this.setting = data.setting;
    this.defaultRow = data.defaultRow;                          //默认grid行的值
    this.defaultForm = data.defaultForm;                        //主表的默认值
    this.readonly = ko.computed(function () { return self.form.ApproveState() == "passed"; });

    this.grid = {
        size: { w: 6, h: $(".z-toolbar").height() + $(".container_12").height() + 50 },
        pagination: false,
        remoteSort: false,
        url: ko.observable(self.urls.getdetail + self.scrollKeys.current())
    };
    this.gridEdit = new com.editGridViewModel(self.grid);
    this.grid.onClickRow = function () { //this.grid.onDblClickRow
        if (self.readonly()) return;
        self.gridEdit.begin();
    };
    this.grid.toolbar = "#gridtb";
   
    this.removeRowClick = function () {
        if (self.readonly()) return;
        var selected = self.grid.datagrid("getSelected");
        var rowIndex = self.grid.datagrid('getRowIndex', selected)
        if (rowIndex < 0) {
            com.message('error', '请先选择要删除的行');
            return
        }
        else {
            self.gridEdit.deleterow(); 
        }

    };
    this.rejectClick = function () {
        if (self.readonly()) return;
        ko.mapping.fromJS(data.form, {}, self.form);
        self.gridEdit.reject();
        com.message('success', self.resx.rejected);
    };
    this.refreshClick = function () {
        window.location.reload();
    };

    this.firstClick = function () {
        self.scrollTo(self.scrollKeys.first());
    };
    this.previousClick = function () {
        self.scrollTo(self.scrollKeys.previous());
    };
    this.nextClick = function () {
        self.scrollTo(self.scrollKeys.next());
    };
    this.lastClick = function () {
        self.scrollTo(self.scrollKeys.last());
    };
    this.scrollTo = function (id) {
        if (id == self.scrollKeys.current()) return;
        com.setLocationHashId(id);
        com.ajax({
            type: 'GET',
            url: self.urls.getmaster + id,
            success: function (d) {
                ko.mapping.fromJS(d, {}, self);
                ko.mapping.fromJS(d, {}, data);
            }
        });
        self.grid.url(self.urls.getdetail + id);
        self.grid.datagrid('loaded');
    };
    this.saveClick = function () {
        if (self.readonly()) return;
         
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {           //传递到后台的数据
            form: com.formChanges(self.form, data.form, self.setting.postFormKeys), 
            list: self.gridEdit.getChanges(self.setting.postListFields)
             
        };
        if ((self.gridEdit.ended() && com.formValidate()) && (post.form._changed || post.list._changed)) {
            com.ajax({
                url: self.urls.edit,
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', self.resx.editSuccess);
                    ko.mapping.fromJS(post.form, {}, data.form); //更新旧值
                    self.gridEdit.accept();
                    com.readonly("master", true);
                }
            });
        }
    };
       
    this.searchDetail = function () {
        window.location.reload();
    }

    this.grid.onDblClickRow = this.editClick;

    this.grid.OnAfterCreateEditor = function (editors) {
      
    }
};
