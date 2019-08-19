/**
* 模块名：mms viewModel
* 程序名: mms.viewModel.edit.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
var com = com || {};
com.viewModel = com.viewModel || {};

com.viewModel.edit = function (data) {
    var self = this;

    //常量
    this.tabConst = { grid: 'grid', form: 'form', tab: 'tab', edit: 'gridEdit' };

    //设置
    this.urls = data.urls;                                      //api服务地址
    this.resx = data.resx;                                      //中文资源
    this.form = data.form;
    this.tabs = data.tabs;

    //数据
    this.dataSource = data.dataSource;                          //下拉框的数据源
    this.pageData = ko.mapping.fromJS(data.dataSource.pageData);//页面数据

    //撤销
    this.rejectClick = function () {
        ko.mapping.fromJS(data.dataSource.pageData, {}, self.pageData);
        com.message('success', self.resx.rejected);
    };

    //刷新
    this.refreshClick = function () {
        window.location.reload();
    };

    //新增
    this.addClick = function () {
        com.ajax({
            type: 'GET',
            url: self.urls.addbillno,
            success: function (d) {
                com.openTab(self.resx.addnewTitle, self.urls.addnew + d);
            }
        });
    }

    //数据滚动
    this.firstClick = function () {
        self.scrollTo(self.pageData.scrollKeys.first());
    };
    this.previousClick = function () {
        self.scrollTo(self.pageData.scrollKeys.previous());
    };
    this.nextClick = function () {
        self.scrollTo(self.pageData.scrollKeys.next());
    };
    this.lastClick = function () {
        self.scrollTo(self.pageData.scrollKeys.last());
    };
    this.scrollTo = function (id) {
        //if (id == self.pageData.scrollKeys.current()) return;
        com.setLocationHashId(id);
        com.ajax({
            type: 'GET',
            url: self.urls.getdata + id,
            success: function (d) {
                data.dataSource.pageData = d;
                ko.mapping.fromJS(d, self.pageData);
            }
        });
    };

    //保存
    this.saveClick = function () {
        //数据验证
        var validMessage = self.fnIsPageValid();
        if (validMessage) {
            com.message('warning', validMessage);
            return;
        }

        //取得数据
        var post = self.fnIsPageChanged();
        if (!post._changed) {
            com.message('success', '页面没有数据被修改！');
            return;
        }

        //数据提交
        com.ajax({
            url: self.urls.edit,
            data: ko.toJSON(post),
            success: function (d) {
                com.message('success', self.resx.editSuccess);
                ko.mapping.fromJS(self.pageData.form, {}, data.dataSource.pageData.form);//更新旧值
                for (var i in self.tabs)
                    if (self.tabs[i].type == self.tabConst.grid)
                        self[self.tabConst.edit + i].accept();
                    else if (self.tabs[i].type == self.tabConst.form)
                        ko.mapping.fromJS(self.pageData[[self.tabConst.tab + i]], {}, data.dataSource.pageData[self.tabConst.tab + i]);
            }
        });
    };

    this.fnIsPageChanged = function () {
        var result = { form: {}, tabs: [], _changed: false };

        result.form = com.formChanges(self.pageData.form, data.dataSource.pageData.form, self.form.primaryKeys);
        result._changed = result._changed || result.form._changed;

        for (var i in self.tabs) {
            var tab = self.tabs[i], tabData;
            if (tab.type == self.tabConst.grid) {
                var edit = self[self.tabConst.edit + i];
                edit.ended();
                tabData = edit.getChanges(tab.postFields);
            }
            else if (tab.type == self.tabConst.form) {
                var name = self.tabConst.tab + i;
                tabData = com.formChanges(self.pageData[name], data.dataSource.pageData[name], tab.primaryKeys);
            }
            result.tabs.push(tabData);
            result._changed = result._changed || tabData._changed;
        }

        return result;
    };

    this.fnIsPageValid = function () {
        var formValid = com.formValidate();
        if (!formValid)
            return '验证不通过，数据未保存！';
        for (var i in self.tabs) {
            var tab = self.tabs[i], tabData;
            if (tab.type == self.tabConst.grid) {
                var edit = self[self.tabConst.edit + i];
                if (!edit.ended())
                    return '第' + i + '个页签中验证不通过';
            }
            else if (tab.type == self.tabConst.form) {
                //formValid = com.formValidate(document.getElementById("xxx"));
            }
        }

        return '';
    };

    //审核
    this.auditClick = function () {
        var changes = self.fnIsPageChanged();
        if (changes._changed) {
            com.message('warning', '数据有修改，请保存后再审核！');
            return;
        }
        com.auditDialog(function () {
            var post = {
                form: $.extend({}, data.dataSource.pageData.form, { status: this.form.status(), comment: this.form.comment() })
            }
            com.ajax({
                type: 'POST',
                url: self.urls.audit,
                data: ko.toJSON(post),
                success: function () {
                    com.message('success', "单据审核成功！");
                    //self.searchDetail();
                },
                error: function (e) {
                    com.message('error', e.responseText);
                }
            });
        })
    };

    //审核
    //this.auditClick = function () {
    //    var changes = self.fnIsPageChanged();
    //    if (changes._changed) {
    //        com.message('warning', '数据有修改，请保存后再审核！');
    //        return;
    //    }

    //    com.auditDialogForEditVM(self.pageData.form, function (d) {
    //        com.ajax({
    //            type: 'POST',
    //            url: self.urls.audit + self.pageData.scrollKeys.current(),
    //            data: JSON.stringify(d),
    //            success: function () {
    //                com.message('success', d.status == "passed" ? self.resx.auditPassed : self.resx.auditReject);
    //                ko.mapping.fromJS(self.pageData.form, {}, data.dataSource.pageData.form);
    //            },
    //            error: function (e) {
    //                com.message('error', e.responseText);
    //                ko.mapping.fromJS(data.dataSource.pageData.form, {}, self.pageData.form);
    //            }
    //        });
    //    });
    //};

    //打印
    this.printClick = function () {
        com.openTab('打印报表', '/report?area=mms&rpt=' + self.urls.report + '&BillNo=' + self.form.data.BillNo(), 'icon-printer_color');
    };

    //初始化tabs
    this.init = function () {
        //tabs
        for (var i in self.tabs) {
            var tab = self.tabs[i];
            if (tab.type == self.tabConst.grid) {
                var grid = this[self.tabConst.grid + i] = {};
                var edit = this[self.tabConst.edit + i] = new com.editGridViewModel(grid);
                $.extend(true, grid, new self.fnEditGrid(tab, grid, edit, i));
            }
            else if (tab.type == self.tabConst.form) {
                if (data.dataSource.pageData[self.tabConst.tab + i] == null)
                    //if (self.fnIsNew())
                    self.pageData[self.tabConst.tab + i] = ko.mapping.fromJS(tab.defaults);
            }

        }

        if (self.fnIsNew()) {
            self.pageData.form = ko.mapping.fromJS(self.form.defaults); //原始代码
        }
    };

    //取得grid参数对象
    this.fnEditGrid = function (tab, grid, edit, i) {
        this.size = { w: 6, h: 77 };
        this.pagination = false;
        this.remoteSort = false;
        this.data = self.pageData["tab" + i];//绑定每个tab下grid数据
        this.queryParams = ko.observable();
        this.onClickRow = function () {
            edit.ended();
        }
        this.onEditRow = function () {
            if (self.readonly()) return;
            var row = self.grid0.datagrid('getSelected');
            if (!row) {
                return com.message('warning', "请先选择要编辑的行！");
            } else {
                edit.begin();
            }
        };
        this.toolbar = "#gridtb" + i;
        this.addRowClick = function () {
            com.ajax({
                type: 'GET',
                url: self.urls.newkey,
                data: { type: 'grid' + i, key: self.pageData.scrollKeys.current() },
                success: function (d) {
                    var ids = d.split(',');
                    for (var j in ids) {
                        var row = $.extend(true, {}, tab.defaults);
                        row[tab.primaryKey] = ids[j];//self.pageData.scrollKeys.current();//子表的外键ID赋值
                        edit.addnew(row);
                    }
                }
            });
        };
        this.removeRowClick = function () {
            var row = self.grid0.datagrid('getSelected');
            if (!row) {
                return com.message('warning', "请先选择要删除的行！");
            }
            else {
                com.ajax({
                    type: 'GET',
                    url: "/api/Mms/ESB_BD_ServiceMain_Edit/GetMethodsParamsByMethodId/" + row.ID,
                    success: function (d) {
                        if (d == true) {
                            com.message('warning', '请先删除方法中设置的参数！');
                        } else {
                            edit.deleterow();
                        }
                    }
                });
            }
        };
    };

    this.fnIsNew = function () { return data.dataSource.pageData.form == null; };
    this.init();
    this.fnIsAudit = function () {
        return self.pageData.form["ApproveState"] == "passed";
    };
    this.readonly = ko.computed(function () { return self.fnIsAudit(); });
    this.approveButton = {
        iconCls: ko.computed(function () { return self.fnIsAudit() ? "icon-user-reject" : "icon-user-accept"; }),
        text: ko.computed(function () { return self.fnIsAudit() ? "反审" : "审核"; })
    };

    //this.SearchDialog = function () {
    //    var setting = {
    //        type: this.type,
    //        targetField: this.targetField,
    //        sourceField: this.sourceField,
    //        settingArray: this.settingArray
    //    }
    //    //settingArray 0:width 1:height 2:xmlName
    //    var target = parent.$('#CommonSearchDialog').length ? parent.$('#CommonSearchDialog') : parent.$('<div id="CommonSearchDialog"></div>').appendTo('body');
    //    utils.clearIframe(target);
    //    var opt;
    //    opt = { title: '列表', width: setting.settingArray[0], height: setting.settingArray[1], modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
    //    opt.content = "<iframe src='/mms/home/CommonDialog?xmlName=" + setting.settingArray[2] + "' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
    //    params = { xmlName: setting.settingArray[2] };
    //    opt.paramater = params;
    //    opt.onSelect = function (data) {
    //        //可接收选择数据
    //        var total = data.total;
    //        var rows = data.rows;
    //        if (setting.type == 'form') {
    //            var jsonData = "{ " + setting.targetField + ": rows[0]['" + setting.sourceField + "'] }";
    //            jsonData = eval("(" + jsonData + ")");
    //            ko.mapping.fromJS(jsonData, {}, self.form);
    //        }
    //    };
    //    target.window(opt);
    //}

};

var memberDialog = function (row) {
    //获取新增参数的主键
    var pkId = "";
    com.ajax({
        type: 'GET',
        url: '/api/Mms/ESB_BD_ServiceMain_Edit/GetNewMethodsParamsId/',
        success: function (d) {
            pkId = d;
        }
    });
    //点击参数设置前确认行数据是否已经保存
    com.ajax({
        type: 'GET',
        url: '/api/Mms/ESB_BD_ServiceMain_Edit/GetMethodsById/' + row.ID,
        success: function (d) {
            if (d == true) {
                com.dialog({
                    title: "&nbsp;服务方法参数列表",
                    iconCls: 'icon-cog',
                    width: 800,
                    height: 500,
                    html: "#type-template",
                    viewModel: function (w) {
                        var that = this;
                        this.grid = {
                            method: 'GET',
                            width: 800,
                            height: 340,
                            pagination: true,
                            pageSize: 10,
                            url: "/api/Mms/ESB_BD_ServiceMain_Edit/GetMethodsParams/" + row.ID,
                            queryParams: ko.observable()
                        };
                        this.gridEdit = new com.editGridViewModel(this.grid);
                        this.grid.OnAfterCreateEditor = function (editors, row) {
                            //if (!row._isnew)com.readOnlyHandler('input')(editors["ParameterCode"].target, true);
                            //if(self.editIndex==-1)    return com.message('warning', self.resx.noneSelect);
                        };
                        this.grid.onClickRow = that.gridEdit.ended;
                        this.grid.onDblClickRow = that.gridEdit.begin;
                        this.grid.toolbar = [
                            {
                                text: '新增', iconCls: 'icon-add1', handler: function () {
                                    pkId = parseInt(pkId) + 1;
                                    that.gridEdit.addnew({ ID: pkId, MethodId: row.ID });
                                }
                            }, '-',
                            {
                                text: '编辑', iconCls: 'icon-edit', handler: function () {
                                    var rowData = that.grid.datagrid('getSelected');
                                    if (!rowData) return com.message('warning', "请先选择要编辑的行！");
                                    that.gridEdit.begin();
                                }
                            }, '-',
                            {
                                text: '删除', iconCls: 'icon-cross', handler: function () {
                                    var rowData = that.grid.datagrid('getSelected');
                                    if (!rowData) return com.message('warning', "请先选择要删除的行！");
                                    com.message('confirm', "您确定要删除选中的行吗？", function (b) {
                                        if (b) {
                                            that.gridEdit.deleterow();
                                            com.ajax({
                                                type: 'DELETE', url: '/api/Mms/ESB_BD_ServiceMain_Edit/Delete/' + rowData.ID, success: function () {
                                                    com.message('success', '删除成功！');
                                                }
                                            });
                                        }
                                    });
                                }
                            }
                        ];
                        this.confirmClick = function () {
                            if (that.gridEdit.isChangedAndValid()) {
                                var list = that.gridEdit.getChanges(['ID', 'MethodId', 'ParameterCode', 'ParameterName', 'ParameterType', 'InOutType', 'DefaultValue', 'Remark']);
                                com.ajax({
                                    url: '/api/Mms/ESB_BD_ServiceMain_Edit/EditMethodsParams/',
                                    data: ko.toJSON({ list: list }),
                                    success: function (d) {
                                        that.cancelClick();
                                        com.message('success', '保存成功！');
                                    }
                                });
                            }
                        };
                        this.cancelClick = function () {
                            w.dialog('close');
                        };
                        //对象字段设置
                        this.setClick = function () {
                            var selectRow = that.grid.datagrid('getSelected')
                            if(!selectRow) return com.message('warning', "请先选择要设置的行！");
                            com.ajax({
                                type: 'GET',
                                url: '/api/Mms/ESB_BD_ServiceMain_Edit/GetParamsById/' + selectRow.ID,
                                success: function (d) {
                                    if (d) {
                                        var type = selectRow.ParameterType//参数类型
                                        //参数类型为对象时进行字段设置
                                        if (type == 4) {
                                            com.dialog({
                                                title: "&nbsp;对象字段设置",
                                                iconCls: 'icon-cog',
                                                width: 600,
                                                height: 410,
                                                html: "#field-template",
                                                viewModel: function (z) {
                                                    var that = this;
                                                    this.grid = {
                                                        width: 586,
                                                        height: 340,
                                                        pagination: true,
                                                        pageSize: 10,
                                                        url: "/api/Mms/ESB_BD_ServiceMain_Edit/GetParamsFieldById/" + selectRow.ID,
                                                        queryParams: ko.observable()
                                                    };
                                                    this.gridEdit = new com.editGridViewModel(this.grid);
                                                    this.grid.OnAfterCreateEditor = function (editors, row) {
                                                        //if (!row._isnew) com.readOnlyHandler('input')(editors["FiledCode"].target, true);
                                                    };
                                                    this.grid.onClickRow = that.gridEdit.ended;
                                                    this.grid.onDblClickRow = that.gridEdit.begin;
                                                    this.grid.toolbar = [
                                                        {
                                                            text: '新增', iconCls: 'icon-add1', handler: function () {
                                                                that.gridEdit.addnew({ ParameterId: selectRow.ID });
                                                            }
                                                        }, '-',
                                                        {
                                                            text: '编辑', iconCls: 'icon-edit', handler: function () {
                                                                var rowData = that.grid.datagrid('getSelected');
                                                                if (!rowData) return com.message('warning', "请先选择要编辑的行！");
                                                                that.gridEdit.begin();
                                                            }
                                                        }, '-',
                                                        {
                                                            text: '删除', iconCls: 'icon-cross', handler: function () {
                                                                var rowData = that.grid.datagrid('getSelected');
                                                                if (!rowData) return com.message('warning', "请先选择要删除的行！");
                                                                that.gridEdit.deleterow();
                                                            }
                                                        }
                                                    ];
                                                    this.confirmClick = function () {
                                                        if (that.gridEdit.isChangedAndValid()) {
                                                            var list = that.gridEdit.getChanges(['ID', 'ParameterId', 'FiledCode', 'FiledName', 'FiledType', 'DefaultValue']);
                                                            com.ajax({
                                                                url: '/api/Mms/ESB_BD_ServiceMain_Edit/EditParamsField/',
                                                                data: ko.toJSON({ list: list }),
                                                                success: function (d) {
                                                                    that.cancelClick();
                                                                    com.message('success', '保存成功！');
                                                                }
                                                            });
                                                        }
                                                    };
                                                    this.cancelClick = function () {
                                                        z.dialog('close');
                                                    };
                                                }
                                            });
                                        }
                                        else {
                                            com.message('warning', '请选择参数类型为对象的行！');
                                        }
                                    }
                                    else {
                                        com.message('warning', '请先保存数据！');
                                    }
                                }
                            });
                        }
                    }
                });
            } else {
                com.message('warning', '请先保存数据！');
            }
        }
    });
}
