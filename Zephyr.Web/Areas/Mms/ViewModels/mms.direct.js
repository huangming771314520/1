/// <reference path="common.js" />

/**
* 模块名：共通脚本
* 程序名: 材料模块通用方法mms.com.js
* Copyright(c) 2013-2015 liwenxiang [ liwenxiang.xm@gmail.com ] 
**/
var direct = direct || {};
direct.viewModel = direct.viewModel || {};
direct.com = {};
direct.com.auditDialog = function () {
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
direct.com.selectMaterial = function (vm, param, DocumentTypeNo) {
    //if (param.To_WareHouse == "" && 3 == DocumentTypeNo) {
    //    return false; //如果仓库没选择，则返回，给个提示
    //}
    var grid = vm.grid;
    var addnew = vm.gridEdit.addnew;
    var defaultRow = vm.defaultRow;
    var url = vm.addurl.outrowid;

    var Object_ID = vm.scrollKeys.current();
    var orgRows = grid.datagrid('getData').rows;
    var compareArray = ["Material_No"];
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

    var opt = { title: '选择发货单明细', width: 800, height: 550, modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
    //通过url来判断使用html
    
    opt.content = "<iframe id='frm_win_material' src='/mms/home/LookupDirectIn' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
    
    opt.paramater = param;      //可传参数
    opt.DocumentTypeNo = DocumentTypeNo;
    opt.onSelect = function (data) {
        //可接收选择数据
        var total = data.total;
        var rows = data.rows;
        com.ajax({
            type: 'GET',
            url: url,
            data: { IDguid: total, Object_ID: Object_ID },
            success: function (d) {
                var ids = d.split(',');
                for (var i in rows) {
                    if (!fnExist(rows[i]))
                        addnew($.extend({ IDguid: ids[i], Object_ID: Object_ID }, defaultRow, rows[i]));
                }
            }
        });
    };
    target.window(opt);
};

//出库管理viewMode


direct.viewModel.edit = function (data) {
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
                direct.com.calcTotalMoneyWhileRemoveRow(self, "Money", "TotalMoney");
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

    //出入库上下架使用
    this.saveClickInOut = function () {
        if (self.readonly()) return;

        var sendPlaceNo = self.form.Send_PlaceNo(); //第二次保存后报错
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
                        //if (d == 'NooneExist') {
                        //    com.message('success', self.resx.editSuccess);
                        //    ko.mapping.fromJS(post.form, {}, data.form); //更新旧值
                        //    self.gridEdit.accept();
                        //}
                        //else {
                        //    com.message('error', d + '数据已存在,请删除,');
                        //}
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
        com.openTab('打印报表', '/report?area=direct&rpt=' + self.urls.report + '&BillNo=' + self.form.BillNo(), 'icon-printer_color');
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
        var SendPlaceNo = self.form.Send_PlaceNo();//获取发货单位
        var AcceptPlaceNo = self.form.Accept_PlaceNo();//获取收货单位
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
