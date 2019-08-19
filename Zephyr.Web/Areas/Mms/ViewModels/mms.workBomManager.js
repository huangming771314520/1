 
var com = com || {};
com.viewModel = com.viewModel || {};
var tabLength = 0;
var classLength = 0;
com.viewModel.searchEdit = function (data) {
    var thatData = data;
    var strLine = "";
    var strPerson = "";
    var self = this;
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    this.defaultRow = data.defaultRow;
    this.setting = data.setting;
    delete this.form.__ko_mapping__;
    classLength = $('.clear').length > 1 ? 25 : 15;
    tabLength = $('.easyui-tabs').length;
    if (tabLength >= 1) {
        tabLength = 20;
    }
    this.grid = {
        size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + classLength + tabLength },
        url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true
        //singleSelect: false
    };
    this.grid1 = {
        size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + classLength },
        url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true,
        //singleSelect: false
    };
    this.grid.queryParams(data.form);
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onDblClickRow = this.gridEdit.begin;
    this.grid.onClickRow = this.gridEdit.ended;
    this.grid.OnAfterCreateEditor = function (editors) {
        if (editors[self.setting.idField]) com.readOnlyHandler('input')(editors[self.setting.idField].target, true);

    };
      
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
        var row = $.extend({}, self.defaultRow);
        self.gridEdit.addnew(row);
    };
     

    //新的delete方法
    this.deleteClick = function () {
        var selectRow = self.grid.datagrid('getSelected');
        if (selectRow) {
            if (!confirm("确认要删除？")) {
                window.event.returnValue = false;
            } else {
                //var selectRow = self.grid.datagrid('getSelected');
                if (selectRow) {
                    var selectIndex = self.grid.datagrid('getRowIndex', selectRow);
                    if (selectIndex == self.editIndex) {
                        self.grid.datagrid('cancelEdit', self.editIndex);
                        self.editIndex = undefined;
                    }
                    self.grid.datagrid('deleteRow', selectIndex);
                }
                self.gridEdit.ended(); //结束grid编辑状态
                var post = {};
                post.list = self.gridEdit.getChanges(self.setting.postListFields);
                if (self.gridEdit.ended() && post.list._changed) {
                    com.ajax({
                        url: self.urls.edit,
                        data: ko.toJSON(post),
                        success: function (d) {
                            com.message('success', '删除成功');
                            self.gridEdit.accept();
                        }
                    });
                }
            }
        }
        else {
            alert("请先选择要删除的数据！")
        }

    }
    this.editClick = function () {
        if (thatData.editFlag == "No") {
            return;
        }
        else {
            var row = self.grid.datagrid('getSelected');
            if (!row) return com.message('warning', self.resx.noneSelect);
            self.gridEdit.begin()
        }
    };
    this.grid.onDblClickRow = this.editClick;
    
    //公共保存方法
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
                    window.location.reload();
                }
            });
        }
    };


    this.downloadClick = function (vm, event) {
        com.exporter(self.grid).download($(event.currentTarget).attr("suffix"));
    };
    

    //公共保存方法--判重
    this.saveClickDup = function () {
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {};
        post.list = self.gridEdit.getChanges(self.setting.postListFields);
        if (self.gridEdit.ended() && post.list._changed) {
            com.ajax({
                url: self.urls.edit,
                data: ko.toJSON(post),
                success: function (d) {
                    if (d == 'NooneExist') {
                        com.message('success', self.resx.editSuccess);
                        self.gridEdit.accept();
                    }
                    else {
                        com.message('error', d + '数据已存在,请删除');
                    }
                }
            });
        }
    };
 
    
     

 
    this.downloadTemplateClick = function (vm, event) {
        var newGrid = $.extend(true, self.grid, { url: self.urls.downTemplate }, { excelTemplate: data.excelTemplate });

        com.exporterTemplate(newGrid).download($(event.currentTarget).attr("suffix"));
    };



};

