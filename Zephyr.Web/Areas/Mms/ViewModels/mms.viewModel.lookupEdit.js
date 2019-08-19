/**
* 模块名：com viewModel
* 程序名: lookupEdit.js
* Copyright(c) 2013-2015 liwenxiang [ liwenxiang.xm@gmail.com ] 
**/
var com = com || {};
com.viewModel = com.viewModel || {};

com.viewModel.searchEdit = function (data) {
    var self = this;
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    var defaultRow = data.defaultRow;
    this.setting = data.setting;
    delete this.form.__ko_mapping__;

    this.grid = {
        size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + 15 },
        url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true
    };
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onDblClickRow = this.gridEdit.begin;
    this.grid.onClickRow = this.gridEdit.ended;
    this.grid.OnAfterCreateEditor = function (editors) {
        var MasterTypeNo = {};
        if (editors[self.setting.idField]) com.readOnlyHandler('input')(editors[self.setting.idField].target, true);

    };
    this.searchClick = function () {
        var param = ko.toJS(this.form);
        this.grid.queryParams(param);
    };
    this.clearClick = function () {
        $.each(self.form, function () { this(''); });
        this.searchClick();
    };
    this.refreshClick = function () {
        window.location.reload();
    };
    this.addClick = function () {
        com.ajax({
            type: 'GET',
            url: self.urls.newkey,
            success: function (d) {
                var row = $.extend({ ID: d }, self.defaultRow);
                self.gridEdit.addnew(row);
            }
        });
    };
    this.deleteClick = self.gridEdit.deleterow;
    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        self.gridEdit.begin()
    };
    this.grid.onDblClickRow = this.editClick;
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
                    self.gridEdit.accept();
                }
            });
        }
    };
    this.saveClickDup = function () {
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {};
        post.list = self.gridEdit.getChanges(self.setting.postListFields);
        if (self.gridEdit.ended() && post.list._changed) {
            com.ajax({
                url: self.urls.edit,
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', self.resx.editSuccess);
                    self.gridEdit.accept();
                }
            });
        }
    };
    this.downloadClick = function (vm, event) {
        com.exporter(self.grid).download($(event.currentTarget).attr("suffix"));
    };

    this.addClickSpec = function () {
        var param = {};
        var grid = self.grid;
        var orgRows = grid.datagrid('getData').rows;
        var orgLength = orgRows.length;
        var compareArray = ["Material_No", "MaterialCode", "Station_No"];
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

        var rowNo = [];
        var opt;
        var url = self.urls.query.split('/')[3];
        switch (url) {
            case "Sendtype_r_Worker"://工位信息
                param = { _xml: 'mms.Wo_Station' }
                break;
            case "Material_Send_Type"://物料配送类别
                //param = { _xml: 'mms.Material_Send_Type' }
                param = { _xml: 'mms.jit_order' }
                opt = { title: '选择在库材料', width: 800, height: 600, modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
                opt.content = "<iframe id='frm_win_material' src='/mms/home/LookupMaterialSendType' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
                break;
            case "ERP_Equipment"://设备档案
                param = { _xml: 'mms.ERP_Equipment' }
                break;
            case "Jit_Order": //投料
                param = { _xml: 'mms.jit_order' }
                opt = { title: '选择在库材料', width: 800, height: 600, modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
                opt.content = "<iframe id='frm_win_material' src='/mms/home/LookupJitOrder' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
                break;
            case "Equipment_JobSEdit"://设备维修记录
                param = { _xml: 'mms.erp_equipment' }
                opt = { title: '选择设备', width: 800, height: 550, modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
                opt.content = "<iframe id='frm_win_material' src='/mms/home/LookupEquipment' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
                break;

            default:

        }

        opt.paramater = param;      //可传参数
        opt.onSelect = function (data) {
            var rows = data.rows;
            var total = data.total;
            com.ajax({
                type: 'GET',
                url: '/api/mms/home/GetGuidArray',
                data: { total: total },
                success: function (d) {
                    var objectIdList = d;
                    for (var i in rows) {
                        grid.datagrid('appendRow', $.extend({ _isnew: true, Object_ID: objectIdList[i] }, defaultRow, rows[i]));
                        grid.datagrid('beginEdit', orgLength);
                    }
                }
            });


        };
        target.window(opt);
    }
};

function uniqueArray(array) {
    var n = {}, r = [], arr = {}; //n为hash表，r为临时数组
    for (var i = 0; i < array.length; i++) //遍历当前数组
    {
        if (!n[array[i]]) //如果hash表中没有当前项
        {
            n[array[i]] = true; //存入hash表
            //r.push(array[i]); //把当前数组的当前项push到临时数组里面

        }
        else {
            array.splice(i, 1);
        }
    }
    return array;
}
