/**
* 模块名：com viewModel
* 程序名: com.viewModel.search.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
var com = com || {};
com.viewModel = com.viewModel || {};
var lookup_form = {} //全局form参数
var lookup_grid = {} //全局gird参数

com.viewModel.search = function (data) {

    var thatData = data;
    var self = this;
    this.idField = data.idField || "Object_ID";
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);

    

    delete this.form.__ko_mapping__;

    this.grid = {
        size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + 15 },
        url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true,
        customLoad: false
    };

    this.grid.queryParams(data.form);

    lookup_form = this.form;//全局form参数赋值
    lookup_grid = this.grid;//全局grid参数赋值

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
        com.ajax({
            type: 'GET',
            url: self.urls.billno,
            success: function (d) {
                if (self.urls["edit2"] && self.urls.edit2.indexOf("customerEdit")) {
                    com.openTab(self.resx.detailTitle, self.urls.edit2.replace("newid",d));
                }else
                {
                    com.openTab(self.resx.detailTitle, self.urls.edit1 + d);
                }
                
            }
        });
    };

    this.deleteClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        com.message('confirm', self.resx.deleteConfirm, function (b) {
            if (b) {
                com.ajax({
                    type: 'DELETE', url: self.urls.remove + row[self.idField], success: function () {
                        com.message('success', self.resx.deleteSuccess);
                        self.searchClick();
                    }
                });
            }
        });
    };
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.deleteClickSpec = function () {
        var selectRow = self.grid.datagrid('getSelected');
        if (selectRow) {
            if (!confirm("确认要删除？")) {
                window.event.returnValue = false;
            } else {

                if (selectRow) {
                    var selectIndex = self.grid.datagrid('getRowIndex', selectRow);
                    if (selectIndex == self.editIndex) {
                        self.grid.datagrid('cancelEdit', self.editIndex);
                        self.editIndex = undefined;
                    }
                    self.grid.datagrid('deleteRow', selectIndex);
                }
                var tableNameTP = self.urls.deletespec;
                var selectRowData = $.extend(true, selectRow, { tableNameTP: tableNameTP })
                if (selectRow) {
                    com.ajax({
                        url: self.urls.deletespec,
                        data: ko.toJSON(selectRow),
                        success: function (d) {
                            com.message('success', '删除成功');
                            self.gridEdit.accept();
                        }
                    });
                }
            }
        }
        else {
            com.message('error', '请先选择要删除的数据');
        }
    };


    this.editClick = function () {
        if (thatData.editFlag == "No") {
            return;
        }
        else {
            var row = self.grid.datagrid('getSelected');
            if (!row) return com.message('warning', self.resx.noneSelect);
            if (self.urls["edit2"] && self.urls.edit2.indexOf("customerEdit")) {
                com.openTab(self.resx.detailTitle, self.urls.edit2.replace("newid", row[self.idField]));
            } else {
                com.openTab(self.resx.detailTitle, self.urls.edit1 + row[self.idField]);
            }
            //com.openTab(self.resx.detailTitle, self.urls.edit1 + row[self.idField]);
        }
    };

    this.grid.onDblClickRow = this.editClick;

    this.auditClick = function () {
        // self.grid.datagrid({ singleSelect: false });
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        if (row.ApproveState == 'passed') return com.message('warning', '该单据不能重复审核！');
        com.auditDialog(function (d) {

            var AuditStatus = this.form.status();
            var AuditCmment = this.form.comment();
            if (AuditStatus == "passed") {
                self.gridEdit.ended();
                var post = {
                    form: $.extend(true, self.form, {
                        status: AuditStatus,
                        comment: this.form.comment(),
                        Object_ID: self.grid.datagrid('getSelected').Object_ID
                    }),
                    list: {}
                };
                com.ajax({
                    type: 'POST',
                    url: self.urls.audit,
                    data: ko.toJSON(post),
                    success: function () {
                        com.message('success', "单据审核成功！");
                        self.grid.datagrid('reload');
                    },
                    error: function (e) {
                        com.message('error', e.responseText);
                    }
                });
            }
        });
    };

    this.downloadClick = function (vm, event) {
        com.exporter(self.grid).download($(event.currentTarget).attr("suffix"));
    };

    //点击后 显示checkbox选中列 隐藏checkbox未选中列
    this.ChangeColumn = function () {
        if (!document.getElementById('dg') || !document.getElementById('CheckColumns')) {
            return;
        }
        var checkValues = $("#CheckColumns").combotree("getValues");
        if ($.inArray('111', checkValues) >= 0) {
            checkValues.splice($.inArray('111', checkValues), 1);
        }
        for (var i in AllArray) {
            $("#dg").datagrid('hideColumn', AllArray[i]);
        }
        for (var i in checkValues) {
            $("#dg").datagrid('showColumn', checkValues[i]);
        }
    }
    var AllArray = new Array();//列表所有列数组
    //列表加载完后动态填充combotree控件，列表显示列checkbox选中，列表隐藏列checkbox不选中
    this.grid.onLoadSuccess = function () {
       
        if (!document.getElementById('CheckColumns')||!document.getElementById('dg')) {
            return;
        }
        
        var opts = $("#dg").data('datagrid').options.columns[0];
        var CheckArray = new Array();
        var checkObject = { 'id': '', 'pid': '', 'text': '' };
        for (var item in opts) {
            checkObject = {};
            checkObject.id = opts[item].field;
            checkObject.text = opts[item].title;
            checkObject.pid = '111';
            CheckArray.push(checkObject);
            AllArray.push(opts[item].field);
        }
        var hightColumn = opts.length * 15 > 400 ? 400 : opts.length * 20;
        CheckArray.push({ 'id': '111', 'pid': '000', 'text': '全部列名' });
        CheckArray = utils.transData(CheckArray, 'id', 'pid', 'children');
        $('#CheckColumns').combotree
            ({
                data: CheckArray,
                valueField: 'id',
                textField: 'text',
                required: true,
                editable: false,
                panelHeight: hightColumn,
                onClick: function (node) {

                },
                onLoadSuccess: function (node, data) {
                    //全部折叠
                    //$('#CheckColumns').combotree('tree').tree("collapseAll");
                    var opts = $("#dg").data('datagrid').options.columns[0];
                    var ShowArray = new Array();
                    for (var item in opts) {
                        if (opts[item].hidden != true) {
                            ShowArray.push(opts[item].field);
                        }
                    }
                    $('#CheckColumns').combotree('setValues', ShowArray);
                }
            });
    }

};
