﻿/**
* 模块名：mms viewModel
* 程序名: SearchEdit.js
* Copyright(c) 2013-2015 liwenxiang [ liwenxiang.xm@gmail.com ] 
**/

com.viewModel = com.viewModel || {};
com.viewModel.searchEdit = function(data) {
    var self = this;
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    this.defaultRow = data.defaultRow;
    this.setting = data.setting;
    delete this.form.__ko_mapping__;

    this.grid = {
        size: { w: 4, h: 94 },
        url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true
    };
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onDblClickRow = this.gridEdit.begin;
    this.grid.onClickRow = this.gridEdit.ended;
    this.grid.OnAfterCreateEditor = function (editors) {
        com.readOnlyHandler('input')(editors.Id.target, true);
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
                var row = $.extend({ Id: d }, self.defaultRow);
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
                data:JSON.stringify(d),
                success: function () {
                    com.message('success', self.resx.auditSuccess);
                }
            });
        });
    };
    this.saveClick = function () {
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
};
 