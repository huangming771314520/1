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
                form: $.extend({}, data.dataSource.pageData.form, { status: this.form.status() == "passed" ? 1 : 0, comment: this.form.comment() })
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
            if (self.readonly()) return;
            edit.begin();
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
            edit.deleterow();
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
