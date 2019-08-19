
/**
* 模块名：com viewModel
* 程序名: SearchEdit.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
var com = com || {};
com.viewModel = com.viewModel || {};
var lookup_form = {}

com.viewModel.searchEdit = function (data) {
    var self = this;
    thatData = data;
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    this.defaultRow = data.defaultRow;
    this.setting = data.setting;
    delete this.form.__ko_mapping__;
    if (typeof (data._type) == "undefined") {

        this.grid = {
            size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + 15 },
            url: self.urls.query,
            queryParams: ko.observable(),
            pagination: true,
            singleSelect: true
        };
    }
    else {
        var workOrderType = { workOrderType: "fj" };
        this.grid = {
            size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + 15 },
            url: self.urls.query,
            queryParams: ko.observable(workOrderType),
            pagination: true,
            singleSelect: true
        };
    }
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onDblClickRow = this.editClick;
    this.grid.onClickRow = this.gridEdit.ended;
    this.grid.OnAfterCreateEditor = function (editors) {
        if (editors[self.setting.idField]) com.readOnlyHandler('input')(editors[self.setting.idField].target, true);
    };
    this.searchClick = function () {
        var param = ko.toJS(this.form);
        this.grid.queryParams(param);
    };
    lookup_form = this.form;
    //生产线分解使用
    this.searchClick_fj = function (data) {
        var param = ko.toJS($.extend(true, this.form, { workOrderType: data }));
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
            url: self.urls.newkey,
            success: function (d) {
                var row = $.extend({}, self.defaultRow);
                row[self.setting.idField] = d;
                self.gridEdit.addnew(row);
            }
        });
    };
    this.deleteClick = self.gridEdit.deleterow; 
    this.editClick = function () {
        if (thatData.editFlag == "No") {
            return;
        }
        else {
             
            var row = self.grid.datagrid('getSelected');
            if (!row) return com.message('warning', self.resx.noneSelect);
            if (row.Status == 11 || row.Status == 12) {
                var staturStr = getStatusStr(row.Status);
                com.message("warningAlert", "正在生产或生产完成的计划不允许调序");
                return;
            }
            if (true) {
                dateCompare(row["Planned_Produce_Date"])
            }
            self.gridEdit.begin();

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
                }
            });
        }
    };
    this.EditSaveClick = function () {
        self.gridEdit.ended(); //结束grid编辑状态 
        var post = {
            Main_ID: "111",
            Object_ID: this.form.Object_ID._latestValue,
            Planned_Produce_Date: this.form.Planned_Produce_Date._latestValue,
            Planned_Date_Shift_Seq: this.form.Planned_Date_Shift_Seq._latestValue,
            Planned_Shift: this.form.Planned_Shift._latestValue,
            Work_Order_Series_No: this.form.Work_Order_Series_No._latestValue
        };
        com.ajax({
            url: self.urls.ajust,
            data: ko.toJSON(post),
            success: function (d) {
                if (d > 0) {
                    com.message('success', self.resx.editSuccess);
                } else {
                    alert(d);
                }
            }
        });
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
    //张欢欢添加自定义按钮 2016 01 07
    com.changeSelect = function () {
        self.grid.singleSelect = false;
    };
    com.EditSaveClickSelf = function () {
        var bool = true;
        self.gridEdit.ended(); //结束grid编辑状态
        var post;
        if (self.urls.type == "Repair_SEdit") {
            post = {
                Main_ID: "111",
                Object_ID: this.form.Object_ID._latestValue,
                Plan_Year: this.form.Plan_Year._latestValue,
                Holiday_Type: this.form.Holiday_Type._latestValue,
                Equipment_No: this.form.Equipment_No._latestValue,
                Equipment_Name: this.form.Equipment_Name._latestValue,
                Model: this.form.Model._latestValue,
                Type: this.form.Type._latestValue,
                Department: this.form.Department._latestValue,
                Solve_Problem: this.form.Solve_Problem._latestValue,
                Repair_Content: this.form.Repair_Content._latestValue,
                Begin_Date: this.form.Begin_Date._latestValue,
                End_Date: this.form.End_Date._latestValue
            };
        } else if (self.urls.type == "EquipmentCP_SEdit") {
            post = {
                Main_ID: "111",
                Object_ID: this.form.Object_ID,
                Plan_Year: this.form.Plan_Year,
                Equipment_No: this.form.Equipment_No,
                Equipment_Name: this.form.Equipment_Name,
                Model: this.form.Model,
                Type: this.form.Type,
                Part: this.form.Part
            };
        } else if (self.urls.type == "Equipment_RepairSEdit") {
            post = {
                Main_ID: "111",
                Object_ID: this.form.Object_ID._latestValue,
                Plan_Year: this.form.Plan_Year._latestValue,
                Equipment_No: this.form.Equipment_No._latestValue,
                Equipment_Name: this.form.Equipment_Name._latestValue,
                Model: this.form.Model._latestValue,
                Type: this.form.Type._latestValue
            };
        } else if (self.urls.type == "Equipment_ChangePSEdit") {
            post = {
                Main_ID: "111",
                Object_ID: this.form.Object_ID._latestValue,
                Plan_Year: this.form.Plan_Year._latestValue,
                Equipment_No: this.form.Equipment_No._latestValue,
                Equipment_Name: this.form.Equipment_Name._latestValue,
                Model: this.form.Model._latestValue,
                Type: this.form.Type._latestValue
            };
        }
        if (bool) {
            com.ajax({
                url: self.urls.save,
                data: ko.toJSON(post),
                async: false,
                success: function (d) {
                    if (d > 0) {
                        com.message('success', self.resx.editSuccess);
                        bool = false;
                    } else {
                        alert(d);
                    }
                }
            });
        }

    };
    com.FormatDate = function (datevalue) {
        var tempdate = new Date(datevalue);
        return tempdate.getFullYear() + "-" + (tempdate.getMonth() + 1) + "-" + tempdate.getDate() + " " + tempdate.getHours() + ":" + tempdate.getMinutes();

    };
    com.uploadFun = function () { };
    //点击到一个新的Tab页面
    this.editClickTab = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        com.openTab(self.resx.detailTitle, self.urls.edit + "/" + row["Object_ID"]);
    };
    //点击新建到一个新的Tab页
    this.editNewTab = function () {
        com.openTab(self.resx.detailTitle, self.urls.edit);
    };

    //按工艺路线分解
    this.decomposeAssembly = function () {
        var self = this;
        var rowMain = self.grid.datagrid('getSelected');
        if (rowMain) {
            var statusStr = getStatusStr(rowMain.Status);
            if (!(rowMain.Planned_Amount)) {
                alert("计划数量为空或零，无法分解");
                return;
            }
            else if (rowMain.Status == 2) {
                com.dialog({
                    title: "按工艺路线分解，计划数量为：" + rowMain.Planned_Amount + "，计划状态：" + statusStr,
                    iconCls: 'icon-node_tree',
                    width: 600,
                    height: 410,
                    html: _gongyi(rowMain),
                    viewModel: function (w) {
                        var that = this;
                        var workOrderType = { workOrderType: "fj" };
                        this.setting = w.setting;
                        this.grid = {
                            height: 245,
                            width: 585,
                            url: self.urls.gongyi,
                            queryParams: ko.observable(workOrderType),
                            pagination: true,
                            singleSelect: true,
                            rownumbers: true
                        };
                        this.gridEdit = new com.editGridViewModel(that.grid);
                        this.editClick = function () {
                            var row = that.grid.datagrid('getSelected');
                            if (!row) { return com.message('warning', that.resx.noneSelect); }
                            else {
                                that.gridEdit.begin()
                            }
                        };
                        this.grid.onDblClickRow = this.editClick;
                        this.grid.onClickRow = this.editClick;
                        this.cancelClick = function () {
                            self.searchClick_fj('fj');
                            w.dialog('close');
                        };
                        this.confirmClick = function () {
                            that.gridEdit.ended()
                            var rows = that.grid.datagrid("getData");
                            var jsonStr = "";
                            var allCount = 0;
                            com.ddd = rows;
                            for (var i = 0; i < rows.rows.length; i++) {
                                if (rows.rows[i]["Counts"]) {
                                    allCount += parseFloat(rows.rows[i]["Counts"]);
                                }
                            }
                            if (allCount == rowMain.Planned_Amount) {
                                rows.total = rowMain.Object_ID;
                                com.ajax({
                                    type: 'POST',
                                    url: self.urls.decomposeGongYi,
                                    data: ko.toJSON(rows),
                                    success: function (d) {
                                        if (d == "OK") {
                                            _SignalRFun("OrderChange", rowMain.Work_Order_No + "|以按生产线分解");
                                            w.dialog('close');
                                            self.searchClick_fj('fj');
                                        }
                                        else {
                                            alert("分解失败");
                                        }
                                    }
                                });

                            }
                            else {
                                alert("计划按工艺路线分解计划数量合计要和分解前计划数量一致：原计划数量：" + rowMain.Planned_Amount + ",填写的数量为：" + allCount);
                            }
                            self.searchClick_fj('fj');
                        };
                    }
                });
            }
            else {
                alert("该计划状态为:" + statusStr + ",无法进行分解操作");
                return;
            }


        }
        else {
            alert("请勾选一条数据！");
        }
        this.searchClick_fj('fj');
    };
    //按生产线分解
    //按生产线分解
    this.decomposeLine = function () {
        var self = this;
        var rowMain = self.grid.datagrid('getSelected');
        if (rowMain) {

            var statusStr = getStatusStr(rowMain.Status);
            // alert(rowMain.Status);
            if (!(rowMain.Planned_Amount)) {
                alert("计划数量为空或零，无法分解");
                return;
            }
            else if (rowMain.Status == 2) {
                com.dialog({
                    title: "按生产线分解，计划数量为：" + rowMain.Planned_Amount + "，计划状态：" + statusStr,
                    iconCls: 'icon-node_tree',
                    width: 600,
                    height: 410,
                    html: _decompose(rowMain),
                    viewModel: function (w) {
                        var that = this;
                        var data = { Material_No: rowMain.Sale_Material_No, workOrderType: 'fj' };
                        this.setting = w.setting;
                        this.grid = {
                            height: 315,
                            width: 585,
                            url: self.urls.line,
                            queryParams: ko.observable(data),
                            pagination: true,
                            singleSelect: true,
                            rownumbers: true
                        };
                        this.gridEdit = new com.editGridViewModel(that.grid);
                        this.editClick = function () {
                            var row = that.grid.datagrid('getSelected');
                            if (!row) { return com.message('warning', that.resx.noneSelect); }
                            else {
                                that.gridEdit.begin()
                            }
                        };
                        this.grid.onDblClickRow = this.editClick;
                        this.grid.onClickRow = this.editClick;
                        this.cancelClick = function () {
                            self.searchClick_fj('fj');
                            w.dialog('close');
                        };
                        this.confirmClick = function () {
                            that.gridEdit.ended()
                            var rows = that.grid.datagrid("getData");
                            var jsonStr = "";
                            var allCount = 0;
                            com.ddd = rows;
                            for (var i = 0; i < rows.rows.length; i++) {
                                if (rows.rows[i]["Counts"]) {
                                    allCount += parseFloat(rows.rows[i]["Counts"]);
                                }
                            }
                            if (allCount == rowMain.Planned_Amount) {
                                rows.total = rowMain.Object_ID;
                                com.ajax({
                                    type: 'POST',
                                    url: self.urls.decomposeLine,
                                    data: ko.toJSON(rows),
                                    success: function (d) {
                                        if (d == "OK") {
                                            _SignalRFun("OrderChange", rowMain.Work_Order_No + "|以按生产线分解");
                                            w.dialog('close');
                                            self.searchClick_fj('fj');
                                        }
                                        else {
                                            alert("分解失败");
                                        }
                                    }
                                });

                            }
                            else {
                                alert("计划按生产线分解计划数量合计要和分解前计划数量一致：原计划数量：" + rowMain.Planned_Amount + ",填写的数量为：" + allCount);
                            }
                            //self.searchClick_fj('fj');
                        };
                    }
                });
            }
            else {
                alert("该计划状态为:" + statusStr + ",只有暂停状态才可以分解");
                return;
            }


        }
        else {
            alert("请勾选一条数据！");
        }
        this.searchClick_fj('fj');
    };

    //间接物料供应商分解
    this.decomposeIndirect = function () {
        var self = this;
        var rowMain = self.grid.datagrid('getSelected');
        if (rowMain) {
            var statusStr = getStatusStr(rowMain.Status);//获取计划状态 
            if (!(rowMain.Plan_Num)) {
                alert("计划数量为空或零，无法分解");
                return;
            }
            else if (rowMain.Status == 1 || rowMain.Status == 2) {
                com.dialog({
                    title: "按供应商分解，计划数量为：" + rowMain.Plan_Num + "，计划状态：" + statusStr,
                    iconCls: 'icon-node_tree',
                    width: 600,
                    height: 410,
                    html: _decomposeIndirect(rowMain),
                    viewModel: function (w) {
                        var that = this;
                        var data = { Material_No: rowMain.Material_No, Material_Tu: rowMain.Material_Tu, Material_Name: rowMain.Material_Name };
                        this.setting = w.setting;
                        this.grid = {
                            height: 315,
                            width: 585,
                            url: self.urls.decomposeIndirect,
                            queryParams: ko.observable(data),
                            pagination: true,
                            singleSelect: true,
                            rownumbers: true
                        };
                        this.gridEdit = new com.editGridViewModel(that.grid);
                        this.editClick = function () {
                            var row = that.grid.datagrid('getSelected');
                            if (!row) { return com.message('warning', that.resx.noneSelect); }
                            else {
                                that.gridEdit.begin()
                            }
                        };
                        this.grid.onDblClickRow = this.editClick;
                        this.grid.onClickRow = this.editClick;
                        this.cancelClick = function () {
                            self.searchClick_fj('fj');
                            w.dialog('close');
                        };
                        this.confirmClick = function () {
                            that.gridEdit.ended()
                            var rows = that.grid.datagrid("getData");
                            var jsonStr = "";
                            var allCount = 0;
                            com.ddd = rows;
                            for (var i = 0; i < rows.rows.length; i++) {
                                if (rows.rows[i]["QUATA"]) {
                                    allCount += parseFloat(rows.rows[i]["QUATA"]);
                                }
                            }
                            if (allCount == rowMain.Plan_Num) {
                                rows.total = rowMain.Object_ID;
                                com.ajax({
                                    type: 'POST',
                                    url: self.urls.decomposeIndirect,
                                    data: ko.toJSON(rows),
                                    success: function (d) {
                                        if (d == "OK") {
                                            _SignalRFun("OrderChange", rowMain.Plan_Code + "|以按生产线分解");
                                            w.dialog('close');
                                            self.searchClick_fj('fj');
                                        }
                                        else {
                                            alert("分解失败");
                                        }
                                    }
                                });

                            }
                            else {
                                alert("计划按生产线分解计划数量合计要和分解前计划数量一致：原计划数量：" + rowMain.Planned_Amount + ",填写的数量为：" + allCount);
                            }
                            //self.searchClick_fj('fj');
                        };
                    }
                });
            }
            else {
                alert("该计划状态为:" + statusStr + ",只有启用或者暂停状态才可以分解");
                return;
            }

        }
        else {
            alert("请勾选一条数据！");
        }
        this.searchClick_fj('fj');
    };



    //订单计划指派
    this.OrderAssign = function () {
        var rowMain = self.grid.datagrid('getSelected');
        var rowsCurent;
        if (rowMain) {
            if (rowMain.Status == "0") {
                alert("该计划已经关闭无法操作");
                return;
            }

            com.dialog({
                title: "生产线指派，当前默认生产线编码：" + rowMain.Line_No,
                iconCls: 'icon-node_tree',
                width: 600,
                height: 410,
                html: _OrderAssign(),
                viewModel: function (w) {
                    var that = this;
                    var data = { Material_No: rowMain.Sale_Material_No };
                    this.setting = w.setting;
                    this.grid = {
                        height: 245,
                        width: 540,
                        url: "/api/mms/Work_SEdit/GetLineInfo",
                        queryParams: ko.observable(data),
                        pagination: true,
                        singleSelect: true,
                        rownumbers: true,
                        onLoadSuccess: function (data) {
                            if (data) {
                                for (var i = 0; i < data.total; i++) {
                                    if (rowMain.Line_No == data.rows[i].Line_No) {
                                        rowsCurent = data.rows[i];
                                        break;
                                    }
                                }
                            }

                        }
                    };
                    this.cancelClick = function () {
                        self.searchClick();
                        w.dialog('close');
                    };
                    this.confirmClick = function () {
                        var rows = that.grid.datagrid("getSelected");
                        if (rows) {
                            rows.Main_ID = rowMain.Object_ID;
                        }
                        else {
                            rows = rowsCurent;
                            rows.Main_ID = rowMain.Object_ID;
                        }
                        com.ajax({
                            type: 'POST',
                            url: self.urls.assign,
                            data: ko.toJSON(rows),
                            success: function (d) {
                                self.searchClick();
                                w.dialog('close');
                                com.message('success', "订单指派成功！");
                            }
                        });
                        self.searchClick();
                    };
                }

            });
        }
        else {
            alert("请勾选一条数据！");
        }
        this.searchClick();
    };
    //订单合并
    this.OrderCombine = function () {
        var rowMain = self.grid.datagrid('getSelections');
        if (rowMain.length > 0) {
            var lineStrs = "";
            //*要验证相同的字段_待添加*//
            var tpLineNo = "";
            var orderTime = rowMain[0].Planned_Produce_Date;
            var orderSale_Material_No = rowMain[0].Sale_Material_No;
            for (var y = 0; y < rowMain.length; y++) {
                if (rowMain[y].Status == "1") {

                    lineStrs += "_" + rowMain[y].Line_No;
                    tpLineNo += "'" + rowMain[y].Line_No + "',";
                }
                else if (rowMain[y].Status == "2") {
                    lineStrs += "_" + rowMain[y].Line_No;
                    tpLineNo += "'" + rowMain[y].Line_No + "',";
                }
                else if (rowMain[y].Status == "3") {
                    lineStrs += "_" + rowMain[y].Line_No;
                    tpLineNo += "'" + rowMain[y].Line_No + "',";
                }
                else {

                    alert("只有开启、暂停、下达状态的计划可以合并");
                    return;
                }
                if (orderTime != rowMain[y].Planned_Produce_Date || orderSale_Material_No != rowMain[y].Sale_Material_No) {
                    alert("所选择合并的订单计划必须计划时间和销售订单号一致！请检查所选数据！");
                    return;
                }
            }
            com.dialog({
                title: "选择合并后的生产线，选中合并的计划生产线有：" + lineStrs,
                iconCls: 'icon-node_tree',
                width: 600,
                height: 410,
                html: _OrderAssign(),
                viewModel: function (w) {
                    var that = this;
                    var tpdata = { Material_No: orderSale_Material_No, tpLineNo: tpLineNo };
                    this.setting = w.setting;
                    this.grid = {
                        height: 310,
                        width: 585,
                        url: "/api/mms/Work_SEdit/GetLineInfo",
                        queryParams: ko.observable(tpdata),
                        pagination: true,
                        singleSelect: true,
                        rownumbers: true
                    };
                    this.cancelClick = function () {
                        self.searchClick();
                        w.dialog('close');
                    };
                    this.confirmClick = function () {
                        var rows = that.grid.datagrid("getSelected");
                        for (var ii = 0; ii < rowMain.length; ii++) {
                            rowMain[ii].Line_No = rows.Line_No
                        }
                        com.ajax({
                            type: 'POST',
                            url: self.urls.combine,
                            data: ko.toJSON(rowMain),
                            success: function (d) {
                                if (d == "OK") {
                                    _SignalRFun("OrderChange", rowMain[0].Work_Order_No + "|订单合并");
                                    w.dialog('close');
                                    com.message('success', "订单合并成功！");
                                }
                                else {
                                    alert("订单合并失败");
                                }
                                self.searchClick();
                            }
                        });
                        self.searchClick();
                    };
                }

            });
        }
        else {
            alert("请勾选一条数据！");
        }
        this.searchClick();
    };
    //订单冻结
    this.OrderFreeze = function () {
        var rowMain = self.grid.datagrid('getSelected');
        if (rowMain) {
            if (rowMain.Status == 2 || rowMain.Status == 1 || rowMain.Status == 3) {
                rowMain.Main_ID = "11";
                com.ajax({
                    type: 'POST',
                    url: self.urls.freeze,
                    async: false,
                    data: ko.toJSON(rowMain),
                    success: function (d) {
                        self.searchClick();
                        com.message('success', "订单冻结成功！");
                    }
                });
            }
            else {
                var statusStr = getStatusStr(rowMain.Status);
                alert("无法进行冻结操作，该订单状态为：" + statusStr);
                return;
            }
        } else {
            alert("请勾选订单进行操作");
            return;
        }

    };
    //订单调序
    this.OrderAjust = function () {
        //添加调序条件，循环数据
        var rowMain = self.grid.datagrid('getSelections');
        var rows = self.grid.datagrid('getSelections');
        for (var i = 0; i < rows.length; i++) {
             
        }
        if (rowMain) {
        } else {
            alert("请勾选订单进行操作");
            return;
        }
        if (rowMain.Status == 11 || rowMain.Status == 12) {
            var staturStr = getStatusStr(rowMain.Status);
            com.message("error", "正在生产或生产完成的计划不允许调序");
            return
        }
        
        else {
            self.gridEdit.ended(); //结束grid编辑状态
            var post = {};
            post.list = self.gridEdit.getChanges(self.setting.postListFields);
            post.form = com.formChanges(self.form, data.form, self.setting.postFormKeys);
            com.ajax({
                url: '/api/mms/Work_SEdit_QAD/PostOrderAjust/"',
                data: ko.toJSON(post),
                success: function (d) {
                    switch (d) {
                        case "Exist":
                            com.message('error', "删除失败，请从最新版本开始删除！");
                            break;
                        case "No":
                            com.message('error', "删除失败，入库计划已生成！");
                            break;
                        case "OK":
                            com.message('success', "删除成功");
                            break;
                        default:
                            break;
                    }
                    window.location.reload();
                }
            });
        }

    };
    //订单挂起
    this.OrderPause = function () {
        var rowMain = self.grid.datagrid('getSelected');
        if (rowMain) {

        } else {
            alert("请勾选订单进行操作");
            return;
        }
        rowMain.Main_ID = "1";
        var statusStr = getStatusStr(rowMain.Status);
        /*挂起条件*/
        if (rowMain.Status == 3 || rowMain.Status == 6) {
            com.ajax({
                url: self.urls.pause,
                data: ko.toJSON(rowMain),
                async: false,
                success: function (d) {
                    com.message('success', "订单挂起成功！");
                }
            });

        } else {
            alert("只有下达、正在上线的订单才可以做挂起操作，该订单状态为：" + statusStr);
            return;
        }

        this.searchClick();
    };

    //订单暂停
    this.OrderWait = function () {
        var rowMain = self.grid.datagrid('getSelected');
        if (rowMain) {

        } else {
            alert("请勾选订单进行操作");
            return;
        }
        rowMain.Main_ID = "1";
        var statusStr = getStatusStr(rowMain.Status);
        /*挂起条件*/
        if (rowMain.Status == 1 || rowMain.Status == 3 || rowMain.Status == 4 || rowMain.Status == 6) {
            com.dialog({
                title: "计划暂停原因",
                iconCls: 'icon-node_tree',
                width: 270,
                height: 150,
                html: _OrderWait(),
                viewModel: function (w) {

                    this.cancelClick = function () {

                        w.dialog('close');
                    };
                    this.confirmClick = function () {
                        var stopReason = $("#stopreason", parent.document).val();
                        if (stopReason.length > 0) {
                            rowMain.stopReason = stopReason;
                            if (rowMain.Status == 1 || rowMain.Status == 3 || rowMain.Status == 4 || rowMain.Status == 6) {
                                com.ajax({
                                    url: self.urls.wait,
                                    data: ko.toJSON(rowMain),
                                    async: false,
                                    success: function (d) {
                                        w.dialog('close');
                                        window.location.reload();
                                        com.message('success', "订单暂停成功！");
                                    }
                                });

                            } else {
                                alert("只有启动、下达、挂起、正在上线的订单才可以做暂停操作，该订单状态为：" + statusStr);
                                return;
                            }
                        }
                        else {
                            alert('请填写计划暂停原因')
                        }

                    };
                }

            });
        }
        else {
            alert("只有启动、下达、挂起、正在上线的订单才可以做暂停操作，该订单状态为：" + statusStr);
            return;
        }
        this.searchClick();
    };
    //订单启用
    this.OrderEnabled = function () {
        var rowMain = self.grid.datagrid('getSelected');
        if (rowMain) {
            if (rowMain.Status == 2 || rowMain.Status == 4 || rowMain.Status == 5) {
                rowMain.Main_ID = "1";
                com.ajax({
                    url: self.urls.enabled,
                    data: ko.toJSON(rowMain),
                    async: false,
                    success: function (d) {
                        com.message('success', "订单启用成功！");

                    }
                });
            }
            else {
                var statusStr = getStatusStr(rowMain.Status);
                alert("该订单状态为：" + statusStr + ",无法进行启用操作");
                return;
            }

        } else {
            if (confirm("确认启用所有暂停的订单")) {
                rowMain = new Object();
                rowMain.Main_ID = "0";
                com.ajax({
                    url: self.urls.enabled,
                    data: ko.toJSON(rowMain),
                    async: false,
                    success: function (d) {
                        com.message('success', "订单启用成功！");

                    }
                });
            }
        }

        this.searchClick();
    };
    //订单关闭
    this.OrderDelete = function () {
        var rowMain = self.grid.datagrid('getSelected');
        if (rowMain) {
            if (rowMain.Status == 9 || rowMain.Status == 10) {
                var statusStr = getStatusStr(rowMain.Status);
                alert("无法对废弃、关闭的订单进行关闭操作");
                return;
            }
            else {
                if (confirm("确定关闭此订单？")) {
                    rowMain.Main_ID = "1";
                    com.ajax({
                        url: self.urls.delete,
                        data: ko.toJSON(rowMain),
                        async: false,
                        success: function (d) {
                            com.message('success', "订单关闭成功！");
                        }
                    });
                }
            }

        } else {
            alert("请勾选订单进行操作");
            return;
        }

        this.searchClick();
    };
    //订单委外
    this.OrderOutsource = function () {
        var rowMain = self.grid.datagrid('getSelected');
        if (rowMain) {

        }
        else {
            alert("请勾选订单进行操作");
            return;
        }
    };
    //上传
    this.uploadClick = function () {
        com.uploadFun();
    };
    //节假日检修计划制定修改计划
    this.RepairFormulate = function () {
        //添加修改条件
        var rowMain = self.grid.datagrid('getSelected');
        //if (rowMain.Status != 2) { 
        //    return;
        //}
        this.editClickTab();
    }

    $("#be_return").click(function () {
        var returnValue = $("#be_return").val();
        var jsonData = eval("(" + returnValue + ")");
        if (jsonData) {
            for (var key in jsonData) {

                data.form[key] = jsonData[key];
            }
            ko.applyBindings(data);

        }
    });
};

var dateCompare = function (date1,date2) {
    var oDate1 = new Date(date1);
    var oDate2 = new Date(date2);
    if (oDate1.getTime() > oDate2.getTime()) {
        com.message("warningAlert", "不能修改当天日期以前的计划");
        return ;
    }
}

var getStatusStr = function (statusInt) {
    switch (statusInt) {

        case 1:
            statusStr = "启用";
            break;
        case 2:
            statusStr = "暂停";
            break;
        case 3:
            statusStr = "下达";
            break;
        case 4:
            statusStr = "挂起";
            break;
        case 5:
            statusStr = "冻结";
            break;
        case 6:
            statusStr = "正在上线";
            break;
        case 7:
            statusStr = "上线完成";
            break;
        case 8:
            statusStr = "下线完成";
            break;
        case 9:
            statusStr = "关闭";
            break;
        case 10:
            statusStr = "废弃";
            break;
        case 11:
            statusStr = "正在生产";
            break;
        default:
            statusStr = "为未知状态请联系管理员";
            break;
    }
    return statusStr;
};

var _decompose = function (tobj) {
    return '   <style type="text/css">                                                                                                                                        '
            + '       .z-toolbar{ border-width:0; margin:0;}                                                                                                                    '
            + '       .lbl { text-align:right; line-height:25px;}                                                                                                               '
            + '       .datagrid-wrap{ border-width:0 !important;}                                                                                                               '
            + '   </style>     <script type="text/javascript">'
             + '   function checkInput(ct) {                   '
             + '       if (ct) {                               '
             + '           var re = /^[1-9]+[0-9]*]*$/;        '
             + '           if (!re.test(ct)) {        '
             + '                value="";                               '
             + '               alert("请输入数字");            '
             + '               return "";                      '
             + '           }                                   '
             + '           else {                              '
             + '               return ct  ;                    '
             + '           }                                   '
             + '       }                                       '
             + '   }                                           '
             + '   </script>  '
            + '<table id="list" data-bind="datagrid:grid">'
            + '  <thead>  '
            + '  <tr>  '
            + '     <th field="Object_ID"   hidden="hidden"    sortable="true" align="left"    width="70"  >记录序号             </th>  '
            + '     <th field="Line_No"        sortable="true" align="left"    width="150"   >生产线编码         </th>'
            + '     <th field="Line_Name"      sortable="true" align="left"    width="100"   >生产线名称         </th>     '
            + '     <th field="Counts"         sortable="true" align="left"   data-options=" formatter:function(value){return checkInput(value);}"    width="100" editor="text">计划数量</th>      '
            + ' </tr>                                                                                                            '
            + '    </thead>                                                                                                         '
            + '   </table>                                                                                                        '
            + '   <div class="z-toolbar" style="text-align: center;padding:4px 0;border-top-width:1px; ">                        '
            + '     <button id="btnConfirm" class="sexybutton"  data-bind="click:confirmClick"><span><span><span class="ok">确定</span></span></span></button> &nbsp; '
            + '     <button id="btnCancel"  class="sexybutton"  data-bind="click:cancelClick"><span><span><span class="cancel">取消</span></span></span></button>    '
            + '   </div>'

};

var _decomposeIndirect = function (tobj) {
    return '   <style type="text/css">                                                                                                                                        '
            + '       .z-toolbar{ border-width:0; margin:0;}                                                                                                                    '
            + '       .lbl { text-align:right; line-height:25px;}                                                                                                               '
            + '       .datagrid-wrap{ border-width:0 !important;}                                                                                                               '
            + '   </style>     <script type="text/javascript">'
             + '   function checkInput(ct) {                   '
             + '       if (ct) {                               '
             + '           var re = /^[1-9]+[0-9]*]*$/;        '
             + '           if (!re.test(ct)) {        '
             + '                value="";                               '
             + '               alert("请输入数字");            '
             + '               return "";                      '
             + '           }                                   '
             + '           else {                              '
             + '               return ct  ;                    '
             + '           }                                   '
             + '       }                                       '
             + '   }                                           '
             + '   </script>  '
            + '<table id="list" data-bind="datagrid:grid">'
            + '  <thead>  '
            + '  <tr>  '
            + '     <th field="Object_ID"   hidden="hidden"    sortable="true" align="left"    width="70"  >记录序号             </th>  '
            + '     <th field="SUPPLY_NO"        sortable="true" align="left"    width="150"   >供应商编码         </th>'
            + '     <th field="SUPPLY_NAME"      sortable="true" align="left"    width="100"   >供应商名称         </th>     '
            + '     <th field="QUATA"         sortable="true" align="left"   data-options=" formatter:function(value){return checkInput(value);}"    width="100" editor="text">计划数量</th>      '
            + ' </tr>                                                                                                            '
            + '    </thead>                                                                                                         '
            + '   </table>                                                                                                        '
            + '   <div class="z-toolbar" style="text-align: center;padding:4px 0;border-top-width:1px; ">                        '
            + '     <button id="btnConfirm" class="sexybutton"  data-bind="click:confirmClick"><span><span><span class="ok">确定</span></span></span></button> &nbsp; '
            + '     <button id="btnCancel"  class="sexybutton"  data-bind="click:cancelClick"><span><span><span class="cancel">取消</span></span></span></button>    '
            + '   </div>'

};

var _gongyi = function (tobj) {
    return '   <style type="text/css">                                                                                                                                        '
            + '       .z-toolbar{ border-width:0; margin:0;}                                                                                                                    '
            + '       .lbl { text-align:right; line-height:25px;}                                                                                                               '
            + '       .datagrid-wrap{ border-width:0 !important;}                                                                                                               '
            + '   </style>     <script type="text/javascript">'
             + '   function checkInput(ct) {                   '
             + '       if (ct) {                               '
             + '           var re = /^[1-9]+[0-9]*]*$/;        '
             + '           if (!re.test(ct)) {        '
             + '                value="";                               '
             + '               alert("请输入数字");            '
             + '               return "";                      '
             + '           }                                   '
             + '           else {                              '
             + '               return ct  ;                    '
             + '           }                                   '
             + '       }                                       '
             + '   }                                           '
             + '   </script>  '
            + '<table id="list" data-bind="datagrid:grid">'
            + '  <thead>  '
            + '  <tr>  '
            + '     <th field="Object_ID"   hidden="hidden"   sortable="true" align="left"    width="100"  >记录序号             </th>  '
            + '     <th field="Process_Route_No"        sortable="true" align="left"    width="150"   >工艺路线编号         </th>'
            + '     <th field="Process_Route_Name"      sortable="true" align="left"    width="100"   >工艺路线名称         </th>     '
            + '     <th field="Counts"         sortable="true" align="left"   data-options=" formatter:function(value){return checkInput(value);}"    width="100" editor="text">计划数量</th>      '
            + ' </tr>                                                                                                            '
            + '    </thead>                                                                                                         '
            + '   </table>                                                                                                        '
            + '   <div class="z-toolbar" style="text-align: center;padding:4px 0;border-top-width:1px; ">                        '
            + '     <button id="btnConfirm" class="sexybutton"  data-bind="click:confirmClick"><span><span><span class="ok">确定</span></span></span></button> &nbsp; '
            + '     <button id="btnCancel"  class="sexybutton"  data-bind="click:cancelClick"><span><span><span class="cancel">取消</span></span></span></button>    '
            + '   </div>'
};
var _OrderAssign = function () {
    return '   <style type="text/css">                                                                                                                                        '
            + '       .z-toolbar{ border-width:0; margin:0;}                                                                                                                    '
            + '       .lbl { text-align:right; line-height:25px;}                                                                                                               '
            + '       .datagrid-wrap{ border-width:0 !important;}                                                                                                               '
            + '   </style>     '
            + '<table id="list12" data-bind="datagrid:grid">'
            + '  <thead>  '
            + '  <tr>   <th field=" ck" checkbox="true"></th>'
            + '     <th field="Object_ID"     hidden="hidden"  sortable="true" align="left"    width="70"  >记录序号             </th>  '
            + '     <th field="Line_No"        sortable="true" align="left"    width="150"   >生产线编码         </th>'
            + '     <th field="Line_Name"      sortable="true" align="left"    width="100"   >生产线名称         </th>     '
            + ' </tr>                                                                                                            '
            + '    </thead>                                                                                                         '
            + '   </table>                                                                                                        '
            + '   <div class="z-toolbar" style="text-align: center;padding:4px 0;border-top-width:1px; ">                        '
            + '     <button id="btnConfirm" class="sexybutton"  data-bind="click:confirmClick"><span><span><span class="ok">确定</span></span></span></button> &nbsp; '
            + '     <button id="btnCancel"  class="sexybutton"  data-bind="click:cancelClick"><span><span><span class="cancel">取消</span></span></span></button>    '
            + '   </div>'

};

//计划暂停弹窗
var _OrderWait = function () {
    return '   <style type="text/css">                                                                                                                                        '
            + '       .sexybutton{ border-width:0;margin:20px 0px 0px 5px}                                                                                                                    '
            + '                                                                                                                     '
            + '   </style>     '
            + '<table> <tr> '
            + '<td><div style="margin:10px 0px 0px 10px"  >暂停原因:</div></td> <td><input type="text" id="stopreason" style="margin-left:1px" required="required"/></div> '
            + ' </tr> </td>'
            + '                            '
            + '  <tr> <td>  <button id="btnConfirm" class="sexybutton"  data-bind="click:confirmClick"><span> <span><span class="ok">确定</span></span></span></button> &nbsp;</td> '
            + '     <td><button id="btnCancel"  class="sexybutton"  data-bind="click:cancelClick"><span><span><span class="cancel">取消</span></span></span></button>  </td> </tr> </table> '


};
var _SignalRFun = function (userName, sendData) {
    connection.send(sendData);
}
