
var mms = mms || {};
mms.viewModel = mms.viewModel || {};
var change_Sum = 0;
var _editors = {};
mms.viewModel.searchEdit = function (data) {
    var self = this;
    this.urls = data.urls;
    this.addurl = data.addurl;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    this.defaultRow = data.defaultRow;
    this.setting = data.setting;
    delete this.form.__ko_mapping__;

    var classLength = $('.clear').length > 1 ? 25 : 15;
    //
    this.grid = {
        size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + classLength },
        url: self.urls.query,
        queryParams: ko.observable(),
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
    this.deleteClick = function () {
        self.gridEdit.deleterow;
        var post = {
            form: com.formChanges(self.form, {}, self.setting.postFormKeys)
        };
        if (post.form.PlanMonth == "" || post.form.PlanMonth == undefined) {
            com.message('error', "请先选择要删除的版本和计划月份");
            return;
        }
        else if (post.form.Version == undefined) {
            com.message('error', "请先选择要删除的版本和计划月份");
            return;
            if (post.form.Version.substring(0, 1).toUpperCase() != "V") {
                com.message('error', "请先选择要删除的版本和计划月份");
                return;
            }

        }
        if (confirm("确认删除该版本数据")) {
            com.ajax({
                url: self.addurl.PostDeleteEdit,
                data: ko.toJSON(post),
                success: function (d) {
                    var arrayD = d.split('$');
                    switch (arrayD[0]) {
                        case "Exist":
                            com.message('error', arrayD[1]);
                            break;
                        case "No":
                            com.message('error', arrayD[1]);
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
    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        //var dayLoop = self.dataSource.work_calender;
        //for (var i = 1; i <= dayLoop; i++) {
        //    self.grid.datagrid('getColumnOption', 'Day_' + i).editor = {};
        //}
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
                        if (post.list.updated[i][self.setting.dayListFieds[key]]== null || post.list.updated[i][self.setting.dayListFieds[key]]== "") {
                            post.list.updated[i][self.setting.dayListFieds[key]]=0;
                        }
                        plan_sum +=  parseInt(post.list.updated[i][self.setting.dayListFieds[key]]);
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

    this.inWarehouseClick = function () {
        self.gridEdit.ended() 
        var post = {
            form: com.formChanges(self.form, {  }, self.setting.postFormKeys)
        };
        if (!post.form._changed) {
            return com.message('error', "请先选择计划月份和版本");
        }
        if (post.form.PlanMonth == "" || post.form.PlanMonth == undefined || post.form.Version == undefined || post.form.Version == "") {
            return com.message('error', "请先选择计划月份和版本");
        }
        if (confirm("确认生成入库计划？")) {
            com.ajax({
                url: self.addurl.inwarehouse,
                contentType: 'application/json',
                type: 'POST',
                data: JSON.stringify(post),
                success: function (d) {
                    switch (d) {
                        case "OK":
                            com.message('success', "入库计划生成成功");
                            break;
                        case "IsProduct":
                            com.message('error', "该版本数据入库计划已经生成，请重新导入发交计划");
                            break;
                        default:
                            com.message('success', "入库计划生成成功");
                            break;
                    }
                }
            });
        }

    }
    this.productWarehouseClick = function () {
        self.gridEdit.ended()
        var post = {
            form: com.formChanges(self.form, data.form, self.setting.postFormKeys)
        };
        if (!post.form._changed) {
            return com.message('error', "请先选择计划月份和版本");
        }
        if (post.form.PlanMonth == "" || post.form.PlanMonth == undefined || post.form.Version == undefined || post.form.Version == "") {
            return com.message('error', "请先选择计划月份和版本");
        }
        if (confirm("确认生成排产计划？")) {
            com.ajax({
                url: self.addurl.inwarehouse,
                contentType: 'application/json',
                type: 'POST',
                data: JSON.stringify(post),
                success: function (d) {
                    switch (d) {
                        case "OK":
                            com.message('success', "排产计划生成成功");
                            break;
                        case "IsProduct":
                            com.message('error', "该版本数据排产计划已经生成，请重新生成入库计划");
                            break;
                        default:
                            com.message('success', "排产计划生成成功");
                            break;
                    }

                }
            });
        }

    }
    this.confirmplanClick = function () {
        self.gridEdit.ended()
        var post = {
            form: com.formChanges(self.form, data.form, self.setting.postFormKeys)
        };
        if (!post.form._changed) {
            return com.message('error', "请先选择计划月份和版本");
        }
        if (post.form.PlanMonth == "" || post.form.PlanMonth == undefined || post.form.Version == undefined || post.form.Version == "") {
            return com.message('error', "请先选择计划月份和版本");
        }
        if (self.gridEdit.ended()) {
            com.ajax({
                url: self.urls.confirmplan,
                contentType: 'application/json',
                type: 'POST',
                data: JSON.stringify(post),
                success: function (d) {
                    if (parseInt(d) > 0) {
                        com.message('error', "当前版本已审核");
                    }
                    else {
                        com.message('success', self.resx.editSuccess);
                    }

                }
            });
        }
    }

    this.cancelplanClick = function () {
        self.gridEdit.ended()
        var post = {
            form: com.formChanges(self.form, data.form, self.setting.postFormKeys)
        };
        if (!post.form._changed) {
            return com.message('error', "请先选择计划月份和版本");
        }
        if (post.form.PlanMonth == "" || post.form.PlanMonth == undefined || post.form.Version == undefined || post.form.Version == "") {
            return com.message('error', "请先选择计划月份和版本");
        }
        if (self.gridEdit.ended()) {
            com.ajax({
                url: self.urls.cancelplan,
                contentType: 'application/json',
                type: 'POST',
                data: JSON.stringify(post),
                success: function (d) {
                    if (parseInt(d) > 0) {
                        com.message('success', "撤销成功");
                    }
                    else {
                        com.message('error', "请将现有的审核版本撤销！");
                    }

                }
            });
        }
    }
    this.downloadClick = function (vm, event) {
        com.exporter(self.grid).download($(event.currentTarget).attr("suffix"));
    };

    this.downloadTemplateClick = function (vm, event) { 
        var newGrid = $.extend(true, self.grid, { url: self.urls.downTemplate }, { excelTemplate: data.excelTemplate  });
        com.exporterTemplate(newGrid).download($(event.currentTarget).attr("suffix"));
    };
    this.downloadTemplateClick_Car = function (vm, event) {
        var newGrid = $.extend(true, self.grid, { url: self.addurl.downTemplate_car, dataAction: '/api/Mms/Wo_ERPPlan_MonthDetail' }, { excelTemplate: data.excelTemplate1 });

        com.exporterTemplate(newGrid).download($(event.currentTarget).attr("suffix"));
    };
    this.downloadTemplateClick_Board = function (vm, event) {
        var newGrid = $.extend(true, self.grid, { url: self.addurl.downTemplate_board, dataAction: '/api/Mms/Wo_ERPPlanMaterialBoard' }, { excelTemplate: data.excelTemplate2, excelTemplateTitle: data.excelTemplate2Title, templateObject: self.templateObject });

        com.exporterTemplate(newGrid).download($(event.currentTarget).attr("suffix"));
    };
    this.templateObject = {
        field: "",
        title: "",
        hidden: "false"
    }

};

