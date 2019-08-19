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


mms.com.bindCalcTotalMoneyPuton = function (self, fieldIn_Num, fieldPlan_Price, fieldPlan_Money, fieldTrue_Price, fieldTrue_Money) {
    return function (editors) {
        var In_Num = editors[fieldIn_Num].target;
        var Plan_Price = editors[fieldPlan_Price].target;
        var True_Price = editors[fieldTrue_Price].target;
        var Plan_Money = editors[fieldPlan_Money].target;
        var True_Money = editors[fieldTrue_Money].target;
        com.readOnlyHandler('input')(editors[fieldPlan_Money].target, true);
        com.readOnlyHandler('input')(editors[fieldTrue_Money].target, true);
        var calc = function () {
            Plan_Money[0].value = In_Num[0].value * Plan_Price[0].value;
            True_Money[0].value = In_Num[0].value * True_Price[0].value;
        };
        In_Num.blur(calc);
        if (True_Price.blur(calc)) {
            True_Price.blur(calc);
            Plan_Price.blur(calc);
        }
        else {
            Plan_Price.blur(calc);
            True_Price.blur(calc);

        }


    };
};



mms.com.bindNewValue = function (self, fieldWH_Code) {
    return function (editors) {
        var cookieWhCode = true;
        if (SendWhCodeValueExist) {
            cookieWhCode = false;
            $(editors[fieldWH_Code].target).combobox('loadData', SendWhCodeValue);
        }
        if (cookieWhCode) {
            var rowData = self.grid.datagrid('getSelected');
            var url = self.addurl.getstroecode;
            var SendPlaceNo = $('#Send_PlaceNo').combo('getValue');
            self.form.Send_PlaceNo._latestValue = SendPlaceNo;
            com.ajax({
                type: 'GET',
                dataType: 'json',
                url: url,
                async: false,
                data: { whLevel: 2, parentWhCode: SendPlaceNo },
                success: function (d) {
                    SendWhCodeValueExist = true;
                    SendWhCodeValue = d;
                    $(editors[fieldWH_Code].target).combobox('loadData', d);
                    //刘进删添加的WH_CODE代码在onAfterChange方法
                }
            });
        }
    }
};

mms.com.bindNewValue1 = function (self, fieldWH_Code) {
    return function (editors) {
        var cookieWhCode = true;
        if (AccptWhCodeValueExist) {
            cookieWhCode = false;
            $(editors[fieldWH_Code].target).combobox('loadData', AccptWhCodeValue);
        }
        if (cookieWhCode) {
            var rowData = self.grid.datagrid('getSelected');
            var url = self.addurl.getstroecode;
            var SendPlaceNo = $('#Accept_PlaceNo').combo('getValue');
            com.ajax({
                type: 'GET',
                dataType: 'json',
                url: url,
                async: false,
                data: { whLevel: 2, parentWhCode: SendPlaceNo },
                success: function (d) {
                    AccptWhCodeValueExist = true;
                    AccptWhCodeValue = d;
                    $(editors[fieldWH_Code].target).combobox('loadData', d);
                }
            });
        }
    }
};

mms.com.loadWhNameValue = function (value, row, index) {
    return row.WH_Code;
}

mms.com.getNewWHCode = function (self, SendPlaceNo, businessType, isSend) {
    return function (editors) { //businessType 1:入库 2出库 3上架 4下架 5 调拨
        var grid = self.grid;
        if (grid != "undefined") {
            var rows = self.grid.datagrid("getRowIndex");
            if (rows >= 0) {
                com.ajax({
                    type: 'GET',
                    dataType: 'json',
                    url: '/api/mms/Put_OnMaster/getWhCodeValue/',
                    async: false,
                    data: { whLevel: 2, parentWhCode: SendPlaceNo },
                    success: function (d) {
                        switch (businessType) {
                            case 1:
                                AccptWhCodeValue = d;
                                editors.WH_Code.target.combobox('loadData', d);
                                break;
                            case 2:
                                SendWhCodeValue = d;
                                editors.WH_Code.target.combobox('loadData', d);//editors[10]为指定的WH_Code
                                break;

                            case 3:
                                if (isSend == 1) {
                                    SendWhCodeValue = d;
                                    editors.WH_Code.target.combobox('loadData', d);//editors[11]为指定的WH_CodeOut
                                }
                                else {
                                    AccptWhCodeValue = d;
                                    editors.RWH_Code.target.combobox('loadData', d);//editors[9]为指定的WH_CodeIN
                                }

                                break;
                            default:

                        }

                    }
                });
            }
        }
    }
}

mms.com.calcTotalMoneyWhileRemoveRow = function (self, fieldRowTotal, fieldAllTotal) {
    var allMoney = 0, fieldRowTotal = fieldRowTotal || "Money", fieldAllTotal = fieldAllTotal || "TotalMoney";
    $.each(self.grid.datagrid('getData').rows, function () {
        var addMoney = (Number(this[fieldRowTotal] * 100) / 100) || 0;
        allMoney += addMoney
    });
    self.form[fieldAllTotal](allMoney);
};

mms.com.auditDialog = function () {
    var query = parent.$;
    var winAudit = query('#w_audit_div'), args = arguments;
    if (winAudit.length == 0) {
        var html = utils.functionComment(function () { });
        html = "";
        var wrapper = query(html).appendTo("body");
        wrapper.find(".easyui-linkbutton").linkbutton();
        winAudit = wrapper.find(".easyui-dialog").dialog();
    }

};

//弹出选择材料窗口
mms.com.selectMaterial = function (vm, param, DocumentTypeNo) {
    //if (param.To_WareHouse == "" && 3 == DocumentTypeNo) {
    //    return false; //如果仓库没选择，则返回，给个提示
    //}
    var grid = vm.grid;
    var addnew = vm.gridEdit.addnew;
    var defaultRow = vm.defaultRow;
    var url = vm.addurl.outrowid;

    var Object_ID = vm.scrollKeys.current();
    var orgRows = grid.datagrid('getData').rows;
    var compareArray = ["Material_No", "MaterialCode"];
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
    //通过url来判断使用html
    var urlsEdit = vm.urls.edit;
    if (urlsEdit.indexOf('Put_OffMaster') > 0) {
        opt.content = "<iframe id='frm_win_material' src='/mms/home/LookupMaterialPutOff' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
    }
    else {
        opt.content = "<iframe id='frm_win_material' src='/mms/home/LookupMaterialInlist' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
    }

    opt.paramater = param;      //可传参数
    opt.DocumentTypeNo = DocumentTypeNo;
    opt.onSelect = function (data) {
        //可接收选择数据
        var total = data.total;
        var rows = data.rows;
        com.ajax({
            type: 'GET',
            url: url,
            data: { OutListId: total, Object_ID: Object_ID },
            success: function (d) {
                var ids = d.split(',');
                for (var i in rows) {
                    if (!fnExist(rows[i]))
                        addnew($.extend({ OutListId: ids[i], InlistId: ids[i], ChangeListId: ids[i], RowId: ids[i], Object_ID: Object_ID }, defaultRow, rows[i]));
                }
            }
        });
    };
    target.window(opt);
};

//出库管理viewMode
var outM = outM || {};
outM.viewModel = outM.viewModel || {};

outM.viewModel.edit = function (data) {
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

    this.grid = {
        size: { w: 6, h: $(".z-toolbar").height() + $(".container_12").height() + 15 },
        pagination: true,
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
        mms.com.selectMaterial(self, { _xml: 'mms.material_stock' });
    };


  
};

var mmsEdit = mmsEdit || {};
mmsEdit.viewModel = mmsEdit.viewModel || {};

mmsEdit.viewModel.edit = function (data) {
    var self = this;
    this.dataSource = data.dataSource;                          //下拉框的数据源
    this.urls = data.urls;                                      //api服务地址
    this.resx = data.resx;                                      //中文资源
    this.scrollKeys = ko.mapping.fromJS(data.scrollKeys);       //数据滚动按钮（上一条下一条）
    this.form = ko.mapping.fromJS(data.form || data.defaultForm); //表单数据
    this.setting = data.setting;
    this.defaultRow = data.defaultRow;                          //默认grid行的值
    this.defaultForm = data.defaultForm;                        //主表的默认值
    this.grid = {
        size: { w: 6, h: 177 },
        pagination: false,
        remoteSort: false,
        url: ko.observable(self.urls.getdetail + self.scrollKeys.current())
    };
    this.gridEdit = new com.editGridViewModel(self.grid);
    this.grid.onClickRow = function () { //this.grid.onDblClickRow
        if (self.readonly()) return;
        self.gridEdit.begin();
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
   
};
