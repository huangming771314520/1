/**
* 模块名：mms viewModel
* 程序名: mms.viewModel.search.js
* Copyright(c) 2013-2015 liwenxiang [ liwenxiang.xm@gmail.com ] 
**/
var mms = mms || {};
mms.viewModel = mms.viewModel || {};

mms.viewModel.search = function (data) {
    var self = this;
    this.idField = data.idField || "BillNo";
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    delete this.form.__ko_mapping__;

    this.grid = {
        size: { w: 4, h: 94 },
        url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true,
        customLoad: false
    };

    this.grid.queryParams(data.form);

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
            url: self.urls.billno,
            success: function (d) {
                com.openTab(self.resx.detailTitle, self.urls.edit1 + d);
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

    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        com.openTab(self.resx.detailTitle, self.urls.edit + row[self.idField]);
    };

    this.grid.onDblClickRow = this.editClick;

    this.auditClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        com.auditDialog(function (d) {
            com.ajax({
                type: 'POST',
                url: self.urls.audit + row[self.idField],
                data: JSON.stringify(d),
                success: function () {
                    com.message('success', self.resx.auditSuccess);
                }
            });
        });
    };

    this.downloadClick = function (vm, event) {
        com.exporter(self.grid).download($(event.currentTarget).attr("suffix"));
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
};
 