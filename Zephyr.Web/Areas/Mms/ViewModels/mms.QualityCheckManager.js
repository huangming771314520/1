
var mms = mms || {};
mms.viewModel = mms.viewModel || {};
var change_Sum = 0;
var _editors = {};
var isInsert = true;
var isInsert1 = true;
mms.viewModel.searchEdit = function (data) {
    var self = this;
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form || data.defaultForm);
    this.defaultRow = data.defaultRow;
    this.setting = data.setting;
    delete this.form.__ko_mapping__;

    var classLength = $('.clear').length > 1 ? 25 : 15;
    var gridtb = $('#gridtb').height() * 2;

    this.grid = {
        size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + classLength + gridtb },
        //url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true,
        url: ko.observable(self.urls.getdetail + self.defaultRow.Object_ID)
    };
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onDblClickRow = this.gridEdit.begin;
    this.grid.onClickRow = this.gridEdit.ended;
    this.grid.OnAfterCreateEditor = function (editors) {

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
                var row = $.extend(self.defaultRow);
                self.gridEdit.addnew(row);
            }
        });
    };
    this.addRowClick = function () {
        var row = self.defaultRow;
        self.gridEdit.addnew(row);
    }
    this.deleteClick = function () {
        var selectRow = self.grid.datagrid('getSelected');
        var selectIndex = self.grid.datagrid('getRowIndex', selectRow);
        self.grid.datagrid('deleteRow', selectIndex);
        self.gridEdit.deleterow;
        isInsert = false;
    }
    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        self.gridEdit.begin();
        isInsert = false;
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

        post = {           //传递到后台的数据 
            form: com.formChanges(self.form, data.form, self.setting.postFormKeys),
            list: self.gridEdit.getChanges(self.setting.postListFields)
        };
        if (isInsert) {
            delete post.form.RowNo;
        }
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

};
mms.viewModel.Operate = function (operateType, Object_ID, RowNo) {
    switch (operateType) {
        case 1: //新增 
            com.openTab('零件抽检记录', 'mms/QTS_BD_MaterialPropertyMaster/edit1/' + RowNo); 
            break;
        case 2: //编辑
            com.openTab('零件抽检记录', 'mms/QTS_BD_MaterialPropertyMaster/edit1/' + RowNo);
            break;
        default:
            break;
    }
 
}

mms.viewModel.edit1 = function (data) {

    var self = this;
    this.addurl = data.addurl;
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form || data.defaultForm);
    this.defaultRow = data.defaultRow;
    this.setting = data.setting;
    delete this.form.__ko_mapping__;
    classLength = $('.clear').length > 1 ? 25 : 15; 
    this.grid = {
        size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + classLength }, 
        queryParams: ko.observable(),
        pagination: true,
        url: ko.observable(self.addurl.getdetail1 + self.defaultRow.Object_ID)
    };

    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onDblClickRow = this.gridEdit.begin;
    this.grid.onClickRow = this.gridEdit.ended;
    this.grid.OnAfterCreateEditor = function (editors) {

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
     
    this.addRowClick = function () {
        var row = self.defaultRow;
        self.gridEdit.addnew(row);
    }
    this.deleteClick = function () {
        var selectRow = self.grid.datagrid('getSelected');
        var selectIndex = self.grid.datagrid('getRowIndex', selectRow); 
        self.grid.datagrid('deleteRow', selectIndex);
        self.gridEdit.deleterow;
        isInsert = false;
    }
    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        self.gridEdit.begin();
        isInsert1 = false;
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

        post = {           //传递到后台的数据 
            form: com.formChanges(self.form, data.form, self.setting.postFormKeys),
            list: self.gridEdit.getChanges(self.setting.postListFields)
        };
        if (isInsert1) {
            delete post.form.RowNo;
        }
        if (self.gridEdit.ended() && post.list._changed) {
            com.ajax({
                url: self.addurl.postEdit,
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

};