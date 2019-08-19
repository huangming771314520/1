/// <reference path="common.js" />

/**
* 模块名：共通脚本
* 程序名: 材料模块通用方法mms.com.js
* Copyright(c) 2013-2015 liwenxiang [ liwenxiang.xm@gmail.com ] 
**/

var mms = mms || {};
mms.com = {};
var AccptWhCodeValue = null;
var sendplacebeifen = "";
var acceptplacebeifen = "";
var SendWhCodeValue = null;
var AccptWhCodeValueExist = false;
var SendWhCodeValueExist = false; 

//弹出选择材料窗口
mms.com.selectMaterial = function (vm, param, DocumentTypeNo) {
    var grid = vm.grid;
    var addnew = vm.gridEdit.addnew;
    var defaultRow = vm.defaultRow;
    var url = vm.addurl.outrowid;
    var Object_ID = vm.scrollKeys.current();
    var orgRows = grid.datagrid('getData').rows;
    var compareArray = ["M_Code"];
    //var comapreArray = param._xml == "mms.material_batches" ? ['MaterialCode', 'SrcBillType', 'SrcBillNo'] : ['Material_No'];
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

    var opt = { title: '选择在库材料', width: 800, height: 550, modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
    opt.content = "<iframe id='frm_win_material' src='/mms/home/LookupWHCon' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
    opt.paramater = param;      //可传参数
    opt.DocumentTypeNo = DocumentTypeNo;
    opt.onSelect = function (data) {
        //可接收选择数据
        var total = data.total;
        var rows = data.rows;
        for (var i in rows) {
            if (!fnExist(rows[i]))
                addnew($.extend({  Object_ID: Object_ID }, defaultRow, rows[i]));
        } 
    };
    target.window(opt);
};

//出库管理viewMode
var conM = conM || {};
conM.viewModel = conM.viewModel || {};
var lookup_form = {}
var lookup_list = {}

conM.viewModel.edit = function (data) {
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
    this.addRowClick = function () {
        if (self.readonly()) return;
        mms.com.selectMaterial(self, { _xml: 'mms.BD_INSTRUMENT' });
    };
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
            if (self.form.TotalMoney)
                conM.com.calcTotalMoneyWhileRemoveRow(self, "Money", "TotalMoney");
        }

    };
    this.rejectClick = function () {
        if (self.readonly()) return;
        ko.mapping.fromJS(data.form, {}, self.form);
        self.gridEdit.reject();
        com.message('success', self.resx.rejected);
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
        var sendPlaceNo = "";
        var SendPlaceName = "";

        self.gridEdit.ended(); //结束grid编辑状态
        var post = {           //传递到后台的数据
            form: com.formChanges(self.form, data.form, self.setting.postFormKeys),
            //form: com.formChanges($.extend(true, self.form), data.form, self.setting.postFormKeys),
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
                }
            });
        }

    };

    //出入库上下架使用
    this.saveClickInOut = function () {
        if (self.readonly()) return;

        var sendPlaceNo = self.form.Send_PlaceNo();
        var SendPlaceName = "";
        var acceptPlaceNo = self.form.Accept_PlaceNo();//获取收货单位
        var acceptPlaceName = "";
        if (sendPlaceNo == acceptPlaceNo && self.urls.edit.indexOf("Out_OutChange") > 0) {
            com.message('warning', '保存失败，收货方与发货方不能相同');
            return;
        }
        if (sendPlaceNo == '' || acceptPlaceNo == '') {
            com.message('warning', '保存失败，发货仓库与收货仓库不能为空');
            return;
        }
        var rows = self.grid.datagrid("getRows");
        if (rows.length > 0) {
            self.gridEdit.ended(); //结束grid编辑状态
            var post = {           //传递到后台的数据
                //form: com.formChanges(self.form, data.form, self.setting.postFormKeys),
                form: com.formChanges($.extend(true, self.form, { Send_PlaceNo: sendPlaceNo, Send_PlaceName: SendPlaceName, Accept_PlaceNo: acceptPlaceNo, Accept_PlaceName: acceptPlaceName }),
                data.form, self.setting.postFormKeys),
                list: self.gridEdit.getChanges(self.setting.postListFields),
                tpData: { Send_PlaceNo: sendPlaceNo, Send_PlaceName: SendPlaceName, Accept_PlaceNo: acceptPlaceNo, Accept_PlaceName: acceptPlaceName }
            };
            if ((self.gridEdit.ended() && com.formValidate()) && (post.form._changed || post.list._changed)) {
                com.ajax({
                    url: self.urls.edit,
                    data: ko.toJSON(post),
                    success: function (d) {
                        com.message('success', self.resx.editSuccess);
                        ko.mapping.fromJS(post.form, {}, data.form); //更新旧值
                        self.gridEdit.accept(); 
                    }
                });
            }
        }
        else {
            com.message('warning', '保存失败，明细列表不能为空');
            return;
        }
    };




    this.auditClick = function () {
        self.gridEdit.ended();
        //var args = arguments;
        var updateArray = ['ApproveState', 'ApproveRemark'];
        var d = {
            status: this.form.ApproveState() == "passed" ? "reject" : "passed",
            comment: this.form.ApproveRemark()
        };
         
        com.ajax({
            type: 'POST',
            url: self.urls.audit + self.scrollKeys.current(),
            data: JSON.stringify(d),
            success: function () {
                com.message('success', d.status == "passed" ? self.resx.auditPassed : self.resx.auditReject);
                if (data.form)
                    for (var i in updateArray) data.form[updateArray[i]] = self.form[updateArray[i]]();
            },
            error: function (e) {
                if (data.form)
                    for (var i in updateArray) self.form[updateArray[i]](data.form[updateArray[i]]);
                com.message('error', e.responseText);
            }
        });
        //  });
    };
    this.approveButton = {
        iconCls: ko.computed(function () { return self.form.ApproveState() == "passed" ? "icon-user-reject" : "icon-user-accept"; }),
        text: ko.computed(function () { return self.form.ApproveState() == "passed" ? "反审" : "审核"; })
    };
    this.printClick = function () {
        com.openTab('打印报表', '/report?area=conM&rpt=' + self.urls.report + '&BillNo=' + self.form.BillNo(), 'icon-printer_color');
    };
    this.onAfterChange = function () {

    };

    this.searchDetail = function () {
        window.location.reload();
    }

    this.grid.onDblClickRow = this.editClick;

    this.grid.OnAfterCreateEditor = function (editors) {
        var cookie = document.cookie;
        var businessType = self.dataSource.businessType;
         
    }

    $("#form_return").click(function () {
        var returnValue = $("#form_return").val();
        var jsonData = eval("(" + returnValue + ")");  
        ko.mapping.fromJS(jsonData, {}, self.form) 
    });

    lookup_form = this.form;
    lookup_list = this.grid;
}
