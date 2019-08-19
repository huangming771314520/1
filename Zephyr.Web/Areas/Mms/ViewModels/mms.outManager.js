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

//计算总金额 用法： 
mms.com.bindCalcTotalMoneyPutoff = function (self, fieldOut_Num, fieldPlan_Price, fieldPlan_Money, fieldTrue_Price, fieldTrue_Money) {
    return function (editors) {
        var Out_Num = editors[fieldOut_Num].target;
        var Plan_Price = editors[fieldPlan_Price].target;
        var True_Price = editors[fieldTrue_Price].target;
        var Plan_Money = editors[fieldPlan_Money].target;
        var True_Money = editors[fieldTrue_Money].target;
        com.readOnlyHandler('input')(editors[fieldPlan_Money].target, true);
        com.readOnlyHandler('input')(editors[fieldTrue_Money].target, true);
        var calc = function () {
            Plan_Money[0].value = Out_Num[0].value * Plan_Price[0].value;
            True_Money[0].value = Out_Num[0].value * True_Price[0].value;
        };
        Out_Num.blur(calc);
        if (True_Price.blur(calc)) {
            True_Price.blur(calc);
            Plan_Price.blur(calc);
        }
        if (Plan_Price.blur(calc)) {
            Plan_Price.blur(calc);
            True_Price.blur(calc);

        }
    };
};

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

mms.com.bindNewValue2 = function (self, field, fieldDOM) {
    return function (editors) {
        //var cookieWhCode = true;
        //if (AccptWhCodeValueExist) {
        //    cookieWhCode = false;
        //    $(editors[fieldWH_Code].target).combobox('loadData', AccptWhCodeValue);
        //}
        //if (cookieWhCode) {
        var rowData = self.grid.datagrid('getSelected');
        var url = self.addurl.getstroecode;
        var SendPlaceNo = $('#' + fieldDOM + '').combo('getValue');
        com.ajax({
            type: 'GET',
            dataType: 'json',
            url: url,
            async: false,
            data: { whLevel: 2, parentWhCode: SendPlaceNo },
            success: function (d) {
                AccptWhCodeValueExist = true;
                AccptWhCodeValue = d;
                $(editors[field].target).combobox('loadData', d);
            }
        });
        //}
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
    else if (urlsEdit.indexOf('IndirectMaterialReturn') > 0) {
        opt.content = "<iframe id='frm_win_material' src='/mms/home/LookupMaterialOrder' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
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
    this.readonly = ko.computed(function () { return self.form.ApproveState() == "passed"; });

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
                outM.com.calcTotalMoneyWhileRemoveRow(self, "Money", "TotalMoney");
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
        var sendPlaceNo = self.form.Send_PlaceNo._latestValue;
        var SendPlaceName = $('#Send_PlaceName').combobox('getText');
        var acceptPlaceNo = self.form.Accept_PlaceNo._latestValue
        if (sendPlaceNo == acceptPlaceNo && self.urls.edit.indexOf("Out_OutChange") > 0) {
            com.message('warning', '保存失败，收货方与发货方不能相同');
            return;
        }
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {           //传递到后台的数据
            //form: com.formChanges(self.form, data.form, self.setting.postFormKeys),
            form: com.formChanges($.extend(true, self.form, { Send_PlaceNo: sendPlaceNo, Send_PlaceName: SendPlaceName }), data.form, self.setting.postFormKeys),
            list: self.gridEdit.getChanges(self.setting.postListFields),
            tpData: { Send_PlaceNo: sendPlaceNo, Send_PlaceName: SendPlaceName }
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

    //出库下架使用
    this.saveClickInOut = function () {
        if (self.readonly()) return;
        var sendPlaceNo = "";
        var acceptPlaceNo = "";
        var SendPlaceName = "";

        //if (typeof (self.form.Send_PlaceNo) == "function") {
        //    sendPlaceNo = $('#Send_PlaceNo').combo('getValue');
        //}
        //else {
        //    sendPlaceNo = self.form.Send_PlaceNo;
        //}

        //if (typeof (self.form.Accept_PlaceNo) == "function") {
        //    acceptPlaceNo = $('#Accept_PlaceNo').combo('getValue');
        //}
        //else {
        //    acceptPlaceNo = self.form.Accept_PlaceNo;
        //}
        var acceptPlaceName = "";

        sendPlaceNo = $('#Send_PlaceNo').lookup('getValue');
        acceptPlaceNo = $('#Accept_PlaceNo').combo('getValue')
        if (sendPlaceNo == acceptPlaceNo && self.urls.edit.indexOf("Out_OutChange") > 0) {
            com.message('warning', '保存失败，收货方与发货方不能相同');
            return;
        }
        if (sendPlaceNo == '' || acceptPlaceNo == '') {
            com.message('warning', '保存失败，发货仓库与收货仓库不能为空');
            return;
        }
        var useEuipment = "";

        var Use_Euipment = document.getElementById("Use_Euipment");
        if (Use_Euipment) {
            useEuipment = $('#Use_Euipment').combo('getValue');
        }

        var rows = self.grid.datagrid("getRows");
        if (rows.length > 0) {

            self.gridEdit.ended(); //结束grid编辑状态
            var listDetail = self.gridEdit.getChanges(self.setting.postListFields);
            if (listDetail.inserted.length > 0) {

                for (var i = 0; i < listDetail.inserted.length; i++) {
                    var inNum = parseFloat(listDetail.inserted[i].In_Num);
                    var outNum = parseFloat(listDetail.inserted[i].Out_Num);
                    if (outNum < 0 || outNum.toString() == "NaN") {

                        alert("出库数量不能为空或小于0！")
                        return;
                    }
                }
            }
            else if (listDetail.updated.length > 0) {
                for (var i = 0; i < listDetail.updated.length; i++) {
                    var outNum = parseFloat(listDetail.updated[i].Out_Num);
                    if (outNum < 0 || outNum.toString() == "NaN") {
                        alert("出库数量不能为空或小于0！")
                        return;
                    }
                }
            }
            var post = {           //传递到后台的数据
                //form: com.formChanges(self.form, data.form, self.setting.postFormKeys),
                form: com.formChanges($.extend(true, self.form, { Send_PlaceNo: sendPlaceNo, Send_PlaceName: SendPlaceName, Accept_PlaceNo: acceptPlaceNo, Accept_PlaceName: acceptPlaceName, Use_Euipment: useEuipment }),
                data.form, self.setting.postFormKeys),
                list: listDetail,
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

    //入库上架
    this.saveClickIn = function () {
        if (self.readonly()) return;
        var sendPlaceNo = "";
        var acceptPlaceNo = "";
        var SendPlaceName = "";
        if (typeof (self.form.Send_PlaceNo) == "function") {
            sendPlaceNo = $('#Send_PlaceNo').combo('getValue');
        }
        else {
            sendPlaceNo = self.form.Send_PlaceNo;
        }

        if (typeof (self.form.Accept_PlaceNo) == "function") {
            acceptPlaceNo = $('#Accept_PlaceNo').combo('getValue');
        }
        else {
            acceptPlaceNo = self.form.Accept_PlaceNo;
        }
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
            var listDetail = self.gridEdit.getChanges(self.setting.postListFields);
            if (listDetail.inserted.length > 0) {

                for (var i = 0; i < listDetail.inserted.length; i++) {
                    var inNum = parseFloat(listDetail.inserted[i].In_Num);

                    if (inNum < 0 || inNum.toString() == "NaN") {

                        alert("入库数量不能为空或小于0！")
                        return;
                    }
                }
            }
            else if (listDetail.updated.length > 0) {
                for (var i = 0; i < listDetail.updated.length; i++) {
                    var inNum = parseFloat(listDetail.inserted[i].In_Num);

                    if (inNum < 0 || inNum.toString() == "NaN") {

                        alert("入库数量不能为空或小于0！")
                        return;
                    }
                }
            }
            var post = {           //传递到后台的数据
                //form: com.formChanges(self.form, data.form, self.setting.postFormKeys),
                form: com.formChanges($.extend(true, self.form, { Send_PlaceNo: sendPlaceNo, Send_PlaceName: SendPlaceName, Accept_PlaceNo: acceptPlaceNo, Accept_PlaceName: acceptPlaceName }),
                data.form, self.setting.postFormKeys),
                list: listDetail,
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

        // mms.com.auditDialog(this.form, function (d) {
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
        com.openTab('打印报表', '/report?area=outM&rpt=' + self.urls.report + '&BillNo=' + self.form.BillNo(), 'icon-printer_color');
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
        var SendPlaceNo = $('#Send_PlaceNo').combo('getValue');//获取发货单位
        var AcceptPlaceNo = $('#Accept_PlaceNo').combo('getValue');//获取收货单位
         
        if (SendPlaceNo != sendplacebeifen && AcceptPlaceNo != acceptplacebeifen && businessType == 2) {
            sendplacebeifen = SendPlaceNo;
            SendWhCodeValueExist = false;
            acceptplacebeifen = AcceptPlaceNo;
            AccptWhCodeValueExist = false;
            com.ajax({
                type: 'GET',
                dataType: 'json',
                url: '/api/mms/NormalOut_OutMaster/GetWhCodeValue/',
                async: false,
                data: { parentWhCodeSend: SendPlaceNo, parentWhCodeAccept: AcceptPlaceNo },
                success: function (d) {
                    if (d) {
                        //var codeValue = d.split('&');

                        SendWhCodeValue = d[0];
                        AccptWhCodeValue = d[1];
                        editors.WH_Code.target.combobox('loadData', SendWhCodeValue);//editors[10]为指定的WH_Code
                        editors.RWH_Code.target.combobox('loadData', AccptWhCodeValue);//editors[10]为指定的WH_Code
                    }

                }
            });
            return;
        }

        if (SendPlaceNo != sendplacebeifen) {
            sendplacebeifen = SendPlaceNo;
            SendWhCodeValueExist = false;
            if (!SendWhCodeValueExist && businessType == 2) {
                com.ajax({
                    type: 'GET',
                    dataType: 'json',
                    url: '/api/mms/Put_OnMaster/getWhCodeValue/',
                    async: false,
                    data: { whLevel: 2, parentWhCode: SendPlaceNo },
                    success: function (d) {
                        if (d) {
                            SendWhCodeValue = d;
                            editors.WH_Code.target.combobox('loadData', d);//editors[10]为指定的WH_Code
                        }

                    }
                });
            }

            else if (!SendWhCodeValueExist && businessType == 3) {
                com.ajax({
                    type: 'GET',
                    dataType: 'json',
                    url: '/api/mms/Put_OnMaster/getWhCodeValue/',
                    async: false,
                    data: { whLevel: 2, parentWhCode: SendPlaceNo },
                    success: function (d) {
                        if (d) {
                            SendWhCodeValue = d;
                            editors.WH_Code.target.combobox('loadData', d);//editors[10]为指定的WH_Code
                        }

                    }
                });
            }
        }
        else if (SendWhCodeValue) {
            editors.WH_Code.target.combobox('loadData', SendWhCodeValue);
        }
        if (AcceptPlaceNo != acceptplacebeifen) {
            acceptplacebeifen = AcceptPlaceNo;
            AccptWhCodeValueExist = false;
            if (!AccptWhCodeValueExist && businessType == 1) {
                com.ajax({
                    type: 'GET',
                    dataType: 'json',
                    url: '/api/mms/Put_OnMaster/getWhCodeValue/',
                    async: false,
                    data: { whLevel: 2, parentWhCode: AcceptPlaceNo },
                    success: function (d) {
                        if (d) {
                            AccptWhCodeValue = d;
                            editors.WH_Code.target.combobox('loadData', d);
                            AccptWhCodeValueExist = true;
                        }
                    }

                });
            }
            if (!AccptWhCodeValueExist && businessType == 3) {
                com.ajax({
                    type: 'GET',
                    dataType: 'json',
                    url: '/api/mms/Put_OnMaster/getWhCodeValue/',
                    async: false,
                    data: { whLevel: 2, parentWhCode: AcceptPlaceNo },
                    success: function (d) {
                        if (d) {
                            AccptWhCodeValue = d;
                            editors.RWH_Code.target.combobox('loadData', d);
                            AccptWhCodeValueExist = true;
                        }
                    }

                });
            }
        }
        else if (AccptWhCodeValue) {
            editors.WH_Code.target.combobox('loadData', AccptWhCodeValue);
        }
    }
};
