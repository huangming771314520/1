/**
* 模块名：com viewModel
* 程序名: SearchEdit.js
* Copyright(c) 2013-2015 liwenxiang [ liwenxiang.xm@gmail.com ] 
**/
var com = com || {};
com.viewModel = com.viewModel || {};

com.viewModel.searchEdit = function (data) {
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
        size: { w: 6, h: $(".z-toolbar").height() + $(".container_12").height() + 50 },
        pagination: false,
        remoteSort: false,
        url: ko.observable(self.urls.getdetail + self.scrollKeys.current())
    };

    this.grid.onClickRow = function () { //this.grid.onDblClickRow
        if (self.readonly()) return;
        self.gridEdit.begin();
    };
    this.grid.toolbar = "#gridtb";

    //新增保存

    this.saveClick_add = function () { 
        var post = {           //传递到后台的数据
            form: com.formChanges($.extend(true, self.form, { RowNo: 0 }), $.extend(true, data.form, { RowNo: 0 }), self.setting.postFormKeys),
            list: {}

        };
        if ((com.formValidate()) && (post.form._changed || post.list._changed)) {
            com.ajax({
                url: self.urls.edit,
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', self.resx.editSuccess);
                    com.readonly("remadeAdd", true);
                }
            });
        }
    };
    this.downloadClick = function (vm, event) {
        com.exporter(self.grid).download($(event.currentTarget).attr("suffix"));
    };
    this.refreshClick = function () {
        window.location.reload();
    };

};
 //出库管理viewMode
var outM = outM || {};
outM.viewModel = outM.viewModel || {};

outM.viewModel.edit = function (data) {
    var self = this;
    var thatData = data;
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
        size: { w: 6, h: $(".z-toolbar").height() + $(".container_12").height() + 50 },
        pagination: false,
        remoteSort: false,
        url: ko.observable(self.urls.getdetail + self.scrollKeys.current())
    };
    this.gridEdit = new com.editGridViewModel(self.grid);
    this.grid.onClickRow = function () { //this.grid.onDblClickRow
        
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
    };
    this.refreshClick = function () {
        window.location.reload();
    };
     

    this.searchDetail = function () {
        window.location.reload();
    }
    //this.editClick = function () {
    //    if (thatData.editFlag == "No") {
    //        return;
    //    }
    //    else {
    //        var row = self.grid.datagrid('getSelected');
    //        if (!row) return com.message('warning', self.resx.noneSelect);
    //        com.openTab(self.resx.detailTitle, self.urls.edit1 + row[self.idField]);
    //    }
    //};

    //this.grid.onDblClickRow = this.editClick;

    this.saveClick_add = function () {
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {           //传递到后台的数据
            form: {},
            list: self.gridEdit.getChanges(self.setting.postListFields)

        };
        if ((com.formValidate()) && (post.form._changed || post.list._changed)) {
            com.ajax({
                url: self.urls.edit,
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', self.resx.editSuccess);
                    com.readonly("remadeAdd", true);
                }
            });
        }
    };
};