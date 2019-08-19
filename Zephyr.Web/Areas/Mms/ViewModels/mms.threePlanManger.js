/// <reference path="../jquery-easyui-1.3.1/demo/combobox.html" />
/// <reference path="../jquery-easyui-1.3.1/demo/combobox.html" />
/**
* 模块名：com viewModel
* 程序名: SearchEdit.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
var com = com || {};
com.viewModel = com.viewModel || {};
var MasterTypeNo = {};
var classLength = 0;
var oldNodeId = "";
var stCode = "";
var stText = "";
var lookup_form = {}
var lookup_list = {}
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
    //
    this.grid = {
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

    var AllArray = new Array();

    this.grid.onLoadSuccess = function () {
        //$('td[field="Equipment_Type"]').combotree("tree").tree("collapseAll");
        //$('#dg').combotree("tree").tree("collapseAll");
        if (!document.getElementById('CheckColumns') || !document.getElementById('dg')) {
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
        var hightColumn = opts.length * 15 > 400 ? 400 : opts.length * 18;
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

    function unique1(array) {
        var n = []; //一个新的临时数组 
        //遍历当前数组 
        for (var i = 0; i < array.length; i++) {
            //如果当前数组的第i已经保存进了临时数组，那么跳过， 
            //否则把当前项push到临时数组里面 
            if (n.indexOf(array[i]) == -1) n.push(array[i]);
        }
        return n;
    }

    this.WriteOffOut = function () {
        var checkedItems = this.grid.datagrid('getChecked');
        if (checkedItems.length == 0) {
            com.message('warning', "请选择需要冲销的订单！");
            return;
        }
        var names = []; var Documents_Nos = [];
        $.each(checkedItems, function (index, item) {
            names.push(item.OutListId);
            Documents_Nos.push(item.Documents_No);
        });
        var Documents_NoArray = unique1(Documents_Nos);
        if (Documents_NoArray.length == 1) {
            com.ajax({
                type: 'GET',
                url: '/api/mms/ERP_Out_OutListCom/GetWriteOffList',
                data: { list: names.join(","), Object_ID: checkedItems[0].Object_ID, Documents_TypeNo: checkedItems[0].Documents_TypeNo },
                traditional: true,
                success: function (d) {
                    com.message('success', '该订单出库冲销成功！');
                    window.location.reload();
                }
            });
        }
        else {
            com.message('warning', "每次只能冲销同一笔订单，请核实！");
        }
    };

    this.WriteOffIn = function () {
        var checkedItems = this.grid.datagrid('getChecked');
        if (checkedItems.length == 0) {
            com.message('warning', "请选择需要冲销的订单！");
            return;
        }
        var names = []; var Documents_Nos = [];
        $.each(checkedItems, function (index, item) {
            names.push(item.InlistId);
            Documents_Nos.push(item.Documents_No);
        });
        var Documents_NoArray = unique1(Documents_Nos);
        if (Documents_NoArray.length == 1) {
            com.ajax({
                type: 'GET',
                url: '/api/mms/ERP_In_InListCom/GetWriteOffList',
                data: { list: names.join(","), Object_ID: checkedItems[0].Object_ID, Documents_TypeNo: checkedItems[0].Documents_TypeNo },
                traditional: true,
                success: function (d) {
                    com.message('success', '该订单入库冲销成功！');
                    window.location.reload();
                }
            });
        }
        else {
            com.message('warning', "每次只能冲销同一笔订单，请核实！");
        }
    };

    lookup_form = this.form;
    lookup_list = this.grid;
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
    //生产线生产设备参数查询
    this.clearClickSpec = function () {
        Workshop_No = document.getElementById('Workshop_No');
        Workshop_Name = document.getElementById('Workshop_Name');
        if (Workshop_No) {
            $('#Workshop_No').combobox('setText', '');
            $('#Workshop_No').combobox('setValue', '');
        }
        if (Workshop_Name) {
            $('#Workshop_Name').combobox('setText', '');
            $('#Workshop_Name').combobox('setValue', '');
        }
        $.each(self.form, function () { this(''); });
        this.searchClick();
    }
    this.refreshClick = function () {
        window.location.reload();
    };

    this.addClick = function () {
        var row = $.extend({}, self.defaultRow);
        self.gridEdit.addnew(row);
    };

    this.addClickByNumber = function () {
        com.ajax({
            type: 'GET',
            url: self.urls.newkey,
            success: function (d) {
                var row = $.extend({}, self.defaultRow);
                row[self.setting.idField] = d;
                self.gridEdit.addnew(row);
            }
        });
    };

    //物料基础信息维护
    this.addClickMT = function () {
        com.ajax({
            type: 'GET',
            url: self.urls.newkey,
            async: false,
            success: function (d) {
                var row = $.extend({}, self.defaultRow);
                if (stCode == "") {
                    row[self.setting.postListFields[4]] = 'B';
                } else {
                    row[self.setting.postListFields[4]] = stCode;
                }
                if (self.setting.postListFields[5] == "Code_Num") {

                }
                com.ajax({
                    type: 'GET',
                    url: '/api/mms/ERP_Material_Type/GetMaterialNo',
                    data: { stCode: stCode },
                    success: function (d) {
                        if (d != "") {
                            row[self.setting.postListFields[1]] = d;
                        }
                        else if (d == "" && stCode.indexOf('B') == 0) {
                            var num = 17 - stCode.length;
                            row[self.setting.postListFields[1]] = stCode + pad('1', num);
                        }
                        self.gridEdit.addnew(row);
                    }
                });
            }
        });
    };

    //物料分类信息维护
    this.addClickFL = function () {
        com.ajax({
            type: 'GET',
            url: self.urls.newkey,
            async: false,
            success: function (d) {
                var row = $.extend({}, self.defaultRow);
                if (stCode == "") {
                    row[self.setting.postListFields[4]] = 'B';
                } else {
                    row[self.setting.postListFields[4]] = stCode;
                }
                row[self.setting.postListFields[5]] = parseInt(stText.split('$')[1]) + 1;
                self.gridEdit.addnew(row);
            }
        });
    };

    function pad(num, n) {
        var len = num.toString().length;
        while (len < n) {
            num = "0" + num;
            len++;
        }
        return num;
    }

    function deleteself() {
        self.gridEdit.deleterow;
    }

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
            var curMM = new Date().getMonth() + 1;
            for (var i = 1; i <= curMM; i++) {
                self.grid.datagrid('getColumnOption', 'M' + i).editor = {};
            }
            for (var i = curMM+4; i <= 12; i++) {
                self.grid.datagrid('getColumnOption', 'M' + i).editor = {};
            }
            self.gridEdit.begin()
        }
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
    //仓库信息维护的保存方法

    this.saveClickHouseRstorage = function () {
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {};
        post.list = self.gridEdit.getChanges(self.setting.postListFields);
        if (typeof (post.list.inserted[0]) === "object") {
            post.list.inserted[0].WH_Code = stCode.split('$')[0];
            post.list.inserted[0].WH_Name = stCode.split('$')[1];
        }
        else if (typeof (post.list.deleted[0]) === "object") {
            post.list.deleted[0].WH_Code = stCode.split('$')[0];
            post.list.deleted[0].WH_Name = stCode.split('$')[1];
        }
        else {
            post.list.updated[0].WH_Code = stCode.split('$')[0];
            post.list.updated[0].WH_Name = stCode.split('$')[1];
        }
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
                        com.message('error', d + '数据已存在,请删除,');
                    }
                }
            });
        }
    };

    //公共保存方法
    this.saveClick = function () {
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {};
        post.list = self.gridEdit.getChanges(self.setting.postListFields);
        if (self.gridEdit.ended() && post.list._changed) {
            var plan_sum = 0;
            if (post.list.updated.length > 0) {
                for (var i = 0; i < post.list.updated.length; i++) {
                    for (var key in self.setting.MMListFieds) {
                        plan_sum += (post.list.updated[i][self.setting.MMListFieds[key]] == null || post.list.updated[i][self.setting.MMListFieds[key]] == "") ? parseInt(0) : parseInt(post.list.updated[i][self.setting.MMListFieds[key]]);
                    }
                    post.list.updated[i].Plan_Sum = plan_sum;
                }
            }
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
    this.copyClick = function () {
        var source = self.grid.datagrid('getSelected');
        if (!source) return com.message('warning', self.resx.noneSelect);
        com.ajax({
            type: 'GET',
            url: self.urls.newkey,
            success: function (d) {
                var target = $.extend({}, source);
                target[self.setting.idField] = d;
                self.gridEdit.addnew(target);
            }
        });
    };


    //物料分类树形菜单
    this.treeType = {
        method: 'GET',
        url: '/api/mms/ERP_Material_Type/GetMaterialType',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['Material_Type as id', 'Material_TypeName as text', 'Top_Type as pid']);
            var data = utils.toTreeData(filter, 'id', 'pid', "children");
            return [{ id: '', text: '所有类别', children: data }];
        },
        onSelect: function (node) {
            self.MaterialCodeType(node.id);
            stCode = node.id;
        }
    };

    this.MaterialCodeType = ko.observable();
    this.MaterialCodeType.subscribe(function (value) {
        self.grid.queryParams({ Top_Type: value, opeartor: 'selectclick' });
    });


    this.typeClick = function () {
        com.dialog({
            title: "&nbsp;材料类别",
            iconCls: 'icon-node_tree',
            width: 800,
            height: 600,
            html: "#type-template",
            viewModel: function (w) {
                var that = this;
                this.grid = {
                    width: 785,
                    height: 525,
                    idField: 'Material_Type',
                    treeField: 'Material_Type',
                    url: "/api/mms/MaterialDictionary_SEdit/GetCodeTypeDetail",
                    queryParams: ko.observable(),
                    loadFilter: function (d) {
                        return utils.toTreeData(d.rows || d, 'Material_Type', 'Top_Type', "children");
                    }
                };
                this.gridEdit = new com.editTreeGridViewModel(this.grid);
                this.grid.OnAfterCreateEditor = function (editors, row) {
                    if (!row._isnew) com.readOnlyHandler('input')(editors["Material_Type"].target, true);
                };
                this.grid.onClickRow = that.gridEdit.begin;
                this.grid.toolbar = [
                    {
                        text: '新增', iconCls: 'icon-add1', handler: function () {
                            var row = $.extend({}, that.defaultRow);
                            that.gridEdit.addnew(row);
                        }
                    }, '-',
                    { text: '删除', iconCls: 'icon-cross', handler: that.gridEdit.deleterow }
                ];
                this.confirmClick = function () {
                    that.gridEdit.ended();
                    if (that.gridEdit.isChangedAndValid()) {
                        var list = that.gridEdit.getChanges(['Material_Type', 'Material_TypeName', 'Top_Type', 'Code_Num', 'Is_Last', 'Last_Flag', 'Worker', 'Status', 'Create_Time', 'Create_Person']);
                        com.ajax({
                            url: '/api/mms/MaterialDictionary_SEdit/Edit/',
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

    this.whtree = {
        method: 'GET',
        url: "/api/Mms/BD_Depot/GetWhDetail",
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['WH_Code as id', 'WH_Name as text', 'ParentWHCode as pid']);
            var data = utils.toTreeData(filter, 'id', 'pid', "children");
            return [{ id: '', text: '所有类别', children: data }];
        },
        onSelect: function (node) {
            self.WhType(node.id);
            stCode = node.id;
        }
    };

    this.WhType = ko.observable();
    this.WhType.subscribe(function (value) {
        self.grid.queryParams({ ParentWHCode: value });
    });

    this.st_tree = {
        method: 'GET',
        url: "/api/Mms/BD_Depot/GetWhDetail",
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['WH_Code as id', 'WH_Name as text', 'ParentWHCode as pid']);
            var data = utils.toTreeData(filter, 'id', 'pid', "children");
            return [{ id: '', text: '所有类别', children: data }];
        },
        onSelect: function (node) {
            self.StType(node.id);
            stCode = node.text;
        }
    };

    this.StType = ko.observable();
    this.StType.subscribe(function (value) {
        self.grid.queryParams({ WH_Code: value });
    });

    function uniqueAll(arr) {
        var tmpArr = [], hash = {};//hash为hash表
        for (var i = 0; i < arr.length; i++) {
            if (!hash[arr[i]]) {//如果hash表中没有当前项
                hash[arr[i]] = true;//存入hash表
                tmpArr.push(arr[i]);//存入临时数组
            }
        }
        return tmpArr;
    }

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

    this.addClickHL = function () {
        com.ajax({
            type: 'GET',
            url: self.urls.newkey,
            success: function (d) {
                var row = $.extend({}, self.defaultRow);
                row[self.setting.idField] = d;
                row[self.setting.postListFields[1]] = strLine;
                //row[self.setting.postListFields[2]] = stCode;
                self.gridEdit.addnew(row);
            }
        });
    };
    this.goods_tree = {
        method: 'GET',
        url: "/api/Mms/Line_SEdit/GetWhDetail",
        queryParams: ko.observable(),
        loadFilter: function (d) {

            var filter = utils.filterProperties(d.rows || d, ['WH_Code as id', 'WH_Name as text', 'ParentWHCode as pid']);
            var data = utils.toTreeData(filter, 'id', 'pid', "children");
            return [{ id: '', text: '所有类别', children: data }];
        },
        onSelect: function (node) {
            self.Linetype(node.id);
            stCode = node.text;//stCode数据是这里来的
            strLine = node.id;
        }
    };

    this.Linetype = ko.observable();
    this.Linetype.subscribe(function (value) {
        self.grid.queryParams({ WareHouse_Object_ID: value });
    });
    this.addClickWS = function () {
        com.ajax({
            type: 'GET',
            url: self.urls.newkey,
            success: function (d) {
                var row = $.extend({}, self.defaultRow);
                row[self.setting.idField] = d;
                row[self.setting.postListFields[3]] = stCode;
                //row[self.setting.postListFields[2]] = stCode;
                self.gridEdit.addnew(row);
            }
        });
    };

    this.Person_tree = {
        method: 'GET',
        url: "/api/Mms/Worker_SEdit/GetWhDetail",
        queryParams: ko.observable(),
        loadFilter: function (d) {

            var filter = utils.filterProperties(d.rows || d, ['Material_Type as id', 'Material_TypeName as text', 'Top_Type as pid']);
            var data = utils.toTreeData(filter, 'id', 'pid', "children");
            return [{ id: '', text: '所有类别', children: data }];
        },
        onSelect: function (node) {

            self.PersonType(node.id);
            stCode = node.text;//stCode数据是这里来的
            strPerson = node.id;
        }
    };
    this.PersonType = ko.observable();
    this.PersonType.subscribe(function (value) {
        self.grid.queryParams({ MaterialType_Id: value });

    });


    //物料信息菜单
    this.tree = {
        method: 'GET',
        url: '/api/mms/MaterialDictionary_SEdit/GetCodeType_MaterialDir_New?nodeId=1&topType=',
        lines: true,
        animate: true,
        loadFilter: function (d) {
            if (d.length > 0) {
                var jsonData = eval(d);
                return utils.toTreeData(jsonData, 'id', 'pid', "children");

            }

        },
        onSelect: function (node) {
            //if (oldNodeId != node.id && (oldNodeId.indexOf(node.id) < 0 || oldNodeId == '')) {
            oldNodeId = node.id
            $.get('/api/mms/MaterialDictionary_SEdit/GetCodeType_MaterialDir_New?topType=' + node.id, function (result) {
                //获取选中节点所有子节点，并全部删除
                var allChildren = $('#mm-tree').tree('getChildren', node.target);

                for (var i = 0; i < allChildren.length; i++) {
                    $('#mm-tree').tree('remove', allChildren[i].target);
                }
                if (result != '') {
                    //在当前节点下添加新子节点
                    $('#mm-tree').tree('append', {
                        parent: node.target,
                        data: result
                    });
                }
                self.CodeType(node.id);
                stCode = node.id;
                stText = node.text;
            });
            //}

        }
    };
    this.CodeType = ko.observable();
    this.CodeType.subscribe(function (value) {
        self.grid.queryParams({ Material_Type: value });
    });



    //物料拉动类型
    this.pushtype_tree = {
        method: 'GET',
        url: '/api/mms/JIT_Type_M_Relation/GetMaterialType',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['Push_Type as id', 'Push_Name as text', 'Factory_No as pid']);
            var data = utils.toTreeData(filter, 'id', 'pid', "children");
            return [{ id: '', text: '所有类别', children: data }];
        },
        onSelect: function (node) {
            self.PushType(node.id);
            strLine = node.id;
        }
    };

    this.PushType = ko.observable();
    this.PushType.subscribe(function (value) {
        self.grid.queryParams({ Push_Type: value });
    });


    this.Material_tree = {
        method: 'GET',
        url: '/api/mms/Material_Family_Relation/GetMaterialType',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['Material_FamilyNo as id', 'Material_FamilyName as text', 'Match_Pattern as pid']);
            var data = utils.toTreeData(filter, 'id', 'pid', "children");
            return [{ id: '', text: '所有类别', children: data }];
        },
        onSelect: function (node) {
            self.MaterialCodeType1(node.id);
            strLine = node.id;
        }
    };

    this.MaterialCodeType1 = ko.observable();
    this.MaterialCodeType1.subscribe(function (value) {
        self.grid.queryParams({ Material_FamilyNo: value });
    });


    this.Mater_tree = {
        method: 'GET',
        url: '/api/mms/Sendtype_r_Worker/GetMaterialType',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['Type_No as id', 'Type_Name as text', 'RowNo as pid']);
            var data = utils.toTreeData(filter, 'id', 'pid', "children");
            return [{ id: '', text: '所有类别', children: data }];
        },
        onSelect: function (node) {
            self.MaterCodeType(node.id);
            stCode = node.id;
        }
    };

    this.MaterCodeType = ko.observable();
    this.MaterCodeType.subscribe(function (value) {
        self.grid.queryParams({ Type_No: value });
    });

    //物料分类树形菜单
    this.MStree = {
        method: 'GET',
        url: '/api/mms/Material_r_Sendtype/GetMaterialType',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['Material_No as id', 'Material_Name as text', 'Serial_No as pid']);
            var data = utils.toTreeData(filter, 'id', 'pid', "children");
            return [{ id: '', text: '所有类别', children: data }];
        },
        onSelect: function (node) {
            self.MSCodeType(node.id);
            stCode = node.id;
        }
    };

    this.MSCodeType = ko.observable();
    this.MSCodeType.subscribe(function (value) {
        self.grid.queryParams({ Type_No: value });
    });

    //物料分类树形菜单
    this.wltree = {
        method: 'GET',
        url: '/api/mms/Material_r_Sendtype/GetMaterialType',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['Type_No as id', 'Type_Name as text', '"" as pid']);
            var data = utils.toTreeData(filter, 'id', 'pid', "children");
            return [{ id: '', text: '所有类别', children: data }];
        },
        onSelect: function (node) {
            self.MaterialCodeType(node.id);
            stCode = node.id;
        }
    };



    this.kwtree = {
        method: 'GET',
        url: '/api/mms/HouseRstorage_SEdit/GetType_MaterialDir_New?nodeId=1&topType=',
        lines: true,
        animate: true,
        loadFilter: function (d) {
            if (d.length > 0) {
                var jsonData = eval(d);
                return utils.toTreeData(jsonData, 'id', 'pid', "children");
            }

        },
        onSelect: function (node) {
            //if (oldNodeId != node.id && (oldNodeId.indexOf(node.id) < 0 || oldNodeId == '')) {
            //    oldNodeId = node.id
            $.get('/api/mms/HouseRstorage_SEdit/GetType_MaterialDir_New?topType=' + node.id, function (result) {
                //获取选中节点所有子节点，并全部删除
                var allChildren = $('#kw-tree').tree('getChildren', node.target);

                for (var i = 0; i < allChildren.length; i++) {
                    $('#kw-tree').tree('remove', allChildren[i].target);
                }
                if (result != '') {
                    //在当前节点下添加新子节点
                    $('#kw-tree').tree('append', {
                        parent: node.target,
                        data: result
                    });
                }

                self.kwType(node.id);
                stCode = node.text;
                //console.log(node.text);
            });
            //}
        }
    };
    this.kwType = ko.observable();
    this.kwType.subscribe(function (value) {
        self.grid.queryParams({ WH_Code: value });
    });
    this.downloadTemplateClick = function (vm, event) {
        var newGrid = $.extend(true, self.grid, { url: self.urls.downTemplate }, { excelTemplate: data.excelTemplate });

        com.exporterTemplate(newGrid).download($(event.currentTarget).attr("suffix"));
    };




};

//弹出选择材料窗口
var mms = mms || {};
mms.com = {};
mms.com.selectMaterial = function (vm, param, DocumentTypeNo) {
    var grid = vm.grid;
    var addnew = vm.gridEdit.addnew;
    var defaultRow = vm.defaultRow;
    //var Object_ID = vm.scrollKeys.current();
    var orgRows = grid.datagrid('getData').rows;
    var compareArray = ["Material_No", "Material_Name"];
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

    var opt = { title: '选择在库材料', width: 1000, height: 550, modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
    //通过url来判断使用html
    var urlsEdit = vm.urls.edit;
    opt.content = "<iframe id='frm_win_material' src='/mms/home/LookupMaterialInlist' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
    opt.paramater = param;      //可传参数
    opt.onSelect = function (data) {
        //可接收选择数据
        var total = data.total;
        var rows = data.rows;
        for (var i in rows) {
            if (!fnExist(rows[i]))
                var rowData = $.extend({ _isnew: true }, defaultRow, rows[i], { "WH_Code": stCode.split('$')[0], "WH_Name": stCode.split('$')[1] })
            grid.datagrid('appendRow', rowData);
        }
    };
    target.window(opt);
};

function columnFormat(value, rowData, rowIndex, tblName) {
    return '<a href="javascript:void(0)" onclick="showDialog(\'' + rowData.Object_ID + '\',\'' + tblName + '\')">上传附件</a>';
}

function showDialog(Object_ID, tblName) {
    var target = parent.$('#selectMaterial').length ? parent.$('#selectMaterial') : parent.$('<div id="selectMaterial"></div>').appendTo('body');
    utils.clearIframe(target);
    var opt;
    opt = { title: '选择文件', width: 500, height: 300, modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
    opt.content = "<iframe id='frm_win_material' name='frm_win_material' src='/mms/home/LookupMaterialSendType' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
    param = { Object_ID: Object_ID, TableName: tblName };
    opt.paramater = param;      //可传参数
    target.window(opt);
}