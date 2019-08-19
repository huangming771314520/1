/// <reference path="../jquery-easyui-1.3.1/demo/combobox.html" />
/// <reference path="../jquery-easyui-1.3.1/demo/combobox.html" />
/**
* 模块名：com viewModel
* 程序名: SearchEdit.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
var com = com || {};
com.viewModel = com.viewModel || {};
var lookup_list = {}

com.viewModel.searchEdit = function (data) {
    var stCode = "";
    var strLine = "";
    var strPerson = "";
    var self = this;
    self.addurl = data.addurl;
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    this.defaultRow = data.defaultRow;
    this.setting = data.setting;
    delete this.form.__ko_mapping__;
    classLength = $('.clear').length > 1 ? 25 : 15;
    this.grid = {
        size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + classLength },
        url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true
    };

    

    this.gridEdit = new com.editGridViewModel(this.grid);

    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        self.gridEdit.begin()
    };
    this.grid.onDblClickRow = this.editClick;
    this.grid.onClickRow = this.gridEdit.ended;

    this.searchClick = function () {
        var param = {}
        param = ko.toJS(this.form);

        this.grid.queryParams(param);
    };
    this.clearClick = function () {
        $.each(self.form, function () {

            this('');

        });
        this.searchClick();
    };

    this.refreshClick = function () {
        window.location.reload();
    };

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

    lookup_list = this.grid;

    //公共保存方法
    this.saveClick = function () {
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {};
        post.list = self.gridEdit.getChanges(self.setting.postListFields);
        if (self.gridEdit.ended()) {
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

    //间接物料供应商分解
    this.decomposeIndirectClick = function () {

        var rowMain = self.grid.datagrid('getSelected');
        if (rowMain) {
            var statusStr = getStatusStr(rowMain.Plan_Statue);//获取计划状态 
            if (!(rowMain.Plan_Num)) {
                alert("计划数量为空或零，无法分解");
                return;
            }
            else if (rowMain.Plan_Statue == 1 || rowMain.Plan_Statue == 2) {
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
                            type: 'POST',
                            height: 315,
                            width: 585,
                            url: self.addurl.decomposeSupper,
                            queryParams: ko.observable(),
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
                                    url: self.addurl.decomposeIndirect,
                                    data: ko.toJSON(rows),
                                    success: function (d) {
                                        if (d == "OK") {
                                           // _SignalRFun("OrderChange", rowMain.Plan_Code + "|以按生产线分解");
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
        //this.searchClick_fj('fj');
    };

    //生产线分解使用
    this.searchClick_fj = function (data) {
        var param = ko.toJS($.extend(true, this.form, { workOrderType: data }));
        this.grid.queryParams(param);
    };
    com.FormatDate = function (datevalue) {
        var tempdate = new Date(datevalue);
        return tempdate.getFullYear() + "-" + (tempdate.getMonth() + 1) + "-" + tempdate.getDate() + " " + tempdate.getHours() + ":" + tempdate.getMinutes();

    };

    com.FormatTime = function (value) {
        if (value) {
            var index = value.indexOf('T');
            if (index > 0) {
                var time = value.split('T')[0] + ' ' + value.split('T')[1];
                return time;
            }
            else {
                return value;
            }
        }

    }
    com.FormatterDate = function (value) {
        if (value) {
            var index = value.indexOf('T');
            if (index > 0) {
                var time = value.split('T')[0];
                return time;
            }
            else {
                return value.substring(0, 10);
            }
        }
    }

    this.downloadClick = function (vm, event) {
        com.exporter(self.grid).download($(event.currentTarget).attr("suffix"));
    };

    $("#be_return").click(function () {
        var returnValue = $("#be_return").val();
        var jsonData = eval("(" + returnValue + ")");
        self.gridEdit.ended(); //结束grid编辑状态

        var row = self.grid.datagrid('getSelected');
        var selectIndex = self.grid.datagrid('getRowIndex', row);


        if (row) {
            for (var key in row) {
                if (typeof (jsonData[key]) != "undefined") {
                    row[key] = jsonData[key];
                }
            }
        }

        if (!row) return com.message('warning', self.resx.noneSelect);
        self.grid.datagrid('refreshRow', selectIndex);
        self.gridEdit.begin();
    });

};
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
        default:
            statusStr = "为未知状态请联系管理员";
            break;
    }
    return statusStr;
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