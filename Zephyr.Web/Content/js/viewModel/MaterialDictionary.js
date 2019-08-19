/**
* 模块名：com viewModel
* 程序名: com.viewModel.MaterialDictionary.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
var com = com || {};
com.viewModel = com.viewModel || {};

com.viewModel.search = function (data) {
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
    //this.CodeType = ko.observable();
    //this.CodeType.subscribe(function (value) {
    //    self.grid.queryParams({ Material_No: value });
    //});

    this.addClick = function () {
        // if (!self.CodeType()) return com.message('warning', '请先在左边选择项目！');
        //com.ajax({
        //    type: 'GET',
        //    url: '/api/Mms/MaterialDictionary_SEdit/getnewkey/' + self.CodeType(),
        //    success: function (d) {
        //        var row = { Material_No: self.CodeType(), Material_No: d };
        //        self.gridEdit.addnew(row);
        //    }
        //});
        self.gridEdit = new com.editGridViewModel(self.grid);
        self.gridEdit.addnew();
        //com.ajax({
        //    type: 'GET',
        //    url: self.urls.newkey,
        //    success: function (d) {
        //        var row = $.extend({ ID: d }, self.defaultRow);
        //        self.gridEdit.addnew(row);
        //    }
        //});

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

    this.typeClick = function () {
        com.dialog({
            title: "&nbsp;字典类别",
            iconCls: 'icon-node_tree',
            width: 600,
            height: 410,
            html: "#type-template",
            viewModel: function (w) {
                var that = this;
                this.grid = {
                    width: 586,
                    height: 340,
                    pagination: true,
                    pageSize: 10,
                    url: "/api/sys/code/getcodetype",
                    queryParams: ko.observable()
                };
                this.gridEdit = new com.editGridViewModel(this.grid);
                this.grid.OnAfterCreateEditor = function (editors, row) {
                    if (!row._isnew) com.readOnlyHandler('input')(editors["CodeType"].target, true);
                };
                this.grid.onClickRow = that.gridEdit.ended;
                this.grid.onDblClickRow = that.gridEdit.begin;
                this.grid.toolbar = [
                    { text: '新增', iconCls: 'icon-add1', handler: function () { that.gridEdit.addnew(); } }, '-',
                    { text: '编辑', iconCls: 'icon-edit', handler: that.gridEdit.begin }, '-',
                    { text: '删除', iconCls: 'icon-cross', handler: that.gridEdit.deleterow }
                ];
                this.confirmClick = function () {
                    if (that.gridEdit.isChangedAndValid()) {
                        var list = that.gridEdit.getChanges(['_id', 'CodeType', 'CodeTypeName', 'Description', 'Seq']);
                        com.ajax({
                            url: '/api/sys/code/editcodetype/',
                            data: ko.toJSON({ list: list }),
                            success: function (d) {
                                that.cancelClick();
                                self.tree.$element().tree('reload');
                                com.message('success', '保存成功！');
                            }
                        });
                    }
                };
                this.cancelClick = function () {
                    w.dialog('close');
                };
            }
        });
    };
    this.saveClick = function () {
        self.gridEdit.ended();
        var post = {};
        post.list = self.gridEdit.getChanges(['CodeType', 'Code', 'Value', 'Text', 'ParentCode', 'Description', 'IsEnable', 'IsDefault', 'Seq']);
        if (self.gridEdit.isChangedAndValid) {
            com.ajax({
                url: '/api/sys/code/edit',
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', '保存成功！');
                    //self.grid.queryParams({ CodeType: self.CodeType() });
                    self.gridEdit.accept();
                }
            });
        }
    };

    this.Material_Type = ko.observable();
    this.Material_Type.subscribe(function (value) {
        self.grid.queryParams({ Material_Type: value });

    });
    //this.ChildrenFn = ko.observable();
    this.ChildrenFn = function (value) {
        self.tree1 = {
            method: 'GET',
            url: '/api/Mms/MaterialDictionary_SEdit/GetCodeType_MaterialDir?op=' + value.id,
            queryParams: ko.observable(),
            loadFilter: function (d) {
                var filter = utils.filterProperties(d.rows || d, ['OBJECT_ID as id', 'Material_Type as code', 'Material_TypeName as text']);
                //return [{ id: '', text: '所有类别', code: '', children: filter }];
                return utils.toTreeData(filter, 'id', 'pid', 'children');
            },
            onSelect: function (node) {
                alert(node.id);
                alert(123223);
                //alert(node.code);
                alert(node.text);
                //self.ChildrenFn(node);
            }
        };
    };
    this.tree1 = {
            method: 'GET',
            url: '/api/Mms/MaterialDictionary_SEdit/GetCodeType_MaterialDir?op=1',
            queryParams: ko.observable(),
            loadFilter: function (d) {
                var filter = utils.filterProperties(d.rows || d, ['OBJECT_ID as id', 'Material_Type as code', 'Material_TypeName as text']);
                //return [{ id: '', text: '所有类别', code: '', children: filter }];
                return utils.toTreeData(filter, 'id', 'pid', 'children');
            },
            onSelect: function (node) {
                alert(node.id);
                alert(123223);
                //alert(node.code);
                alert(node.text);
                //self.ChildrenFn(node);
            }
    };
    this.tree = {
        method: 'GET',
        url: '/api/Mms/MaterialDictionary_SEdit/GetCodeType_MaterialDir?op=0',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['OBJECT_ID as id', 'Material_Type as code', 'Material_TypeName as text']);
            return utils.toTreeData(filter, 'id', 'pid', 'children');
        },
        onSelect: function (node) {
            alert(node.id);
            //alert(node.code);
            alert(node.text);
            self.Material_Type(node.id);
            //self.ChildrenFn(node);
        }
    };
};
