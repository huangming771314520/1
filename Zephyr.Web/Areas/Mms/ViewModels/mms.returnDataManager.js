
var mms = mms || {};
mms.viewModel = mms.viewModel || {};
var change_Sum = 0;
var _editors = {};
mms.viewModel.searchEdit = function (data) {
    var self = this;
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    this.defaultRow = data.defaultRow;
    this.setting = data.setting;
    delete this.form.__ko_mapping__;
    var classLength = $('.clear').length > 1 ? 25 : 15;
    
    this.grid = {
        size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + classLength },
        url: self.urls.query,
        queryParams: ko.observable(),
        singleSelect: false,
        pagination: true
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
                var row = $.extend({ ID: d }, self.defaultRow);
                self.gridEdit.addnew(row);
            }
        });
    };
    this.deleteClick = self.gridEdit.deleterow;
    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);  
        self.gridEdit.begin();


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
            var plan_sum = 0;
            if (post.list.updated.length > 0) {
                for (var i = 0; i < post.list.updated.length; i++) {
                    for (var key in self.setting.dayListFieds) {
                        plan_sum += (post.list.updated[i][self.setting.dayListFieds[key]] == null || post.list.updated[i][self.setting.dayListFieds[key]] == "") ? parseInt(0) : parseInt(post.list.updated[i][self.setting.dayListFieds[key]]);
                    }
                    post.list.updated[i].Plan_Sum = plan_sum;
                }
            }
            com.ajax({
                url: self.urls.edit,
                data: ko.toJSON(post),
                success: function (d) {
                    if (d != "") {
                        com.message('error', d + "版本未生成入库计划,请先生成" + d + "入库计划!");
                        self.gridEdit.ended();
                    }
                    else {
                        com.message('success', self.resx.editSuccess);
                        self.gridEdit.accept();
                    }

                }
            });
        }
    };
    this.downloadClick = function (vm, event) {
        com.exporter(self.grid).download($(event.currentTarget).attr("suffix"));
    };
    this.prncClick = function () {
       
        var rows = self.grid.datagrid('getSelections'); 
        if (!rows) return com.message('warning', self.resx.noneSelect);
        var returnNoArray = new Array();
        for (var rowid in rows) {
            returnNoArray.push(rows[rowid]["Return_No"]);
        }
        var postRows = unique2(returnNoArray);

        if (confirm("确认回票操作？")) {
            com.ajax({
                type: 'POST',
                url: urlprnc,
                data: JSON.stringify(postRows),
                success: function () {
                    com.message('success', '回票确认成功！');
                },
                error: function (e) {
                    com.message("error", e.responseText);
                }
            })
        }
    };
    this.downloadTemplateClick = function (vm, event) {
        var newGrid = $.extend(true, self.grid, { url: self.urls.downTemplate }, { excelTemplate: data.excelTemplate });

        com.exporterTemplate(newGrid).download($(event.currentTarget).attr("suffix"));
    };

};
//数组判重
function unique2(array) {
    array.sort(); //先排序
    var res = [array[0]];
    for (var i = 1; i < array.length; i++) {
        if (array[i] !== res[res.length - 1]) {
            res.push(array[i]);
        }
    }
    return res;
}