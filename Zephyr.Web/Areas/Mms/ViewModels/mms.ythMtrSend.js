/// <reference path="common.js" />

/**
* 模块名：共通脚本
* 程序名: 材料模块通用方法mms.com.js
* Copyright(c) 2013-2015 liwenxiang [ liwenxiang.xm@gmail.com ] 
**/
var send = send || {};
send.viewModel = send.viewModel || {};
send.com = {};
send.com.auditDialog = function () {
    var query = parent.$;
    var winAudit = query('#w_audit_div'), args = arguments;
    if (winAudit.length == 0) {
        var html = utils.functionComment(function () { });
        html = "";
        var wrapper = query(html).appendTo("body");
        wrapper.find(".easyui-linkbutton").linkbutton();
        winAudit = wrapper.find(".easyui-dialog").dialog();
    }
};


send.com.Operate = function (operateType, OBJECT_ID,Row_No) {
    if (operateType) {
        var url = ""; 
        if (operateType) {
            com.ajax({
                type: 'get',
                url: self.addUrl.operate,
                data: { operateType: operateType, OBJECT_ID: OBJECT_ID, Row_No: Row_No },
                success: function (d) {
                    if (parseInt(d) > 0) {
                        $('#dg').datagrid('reload');
                        alert('执行成功!')
                    }
                    else {
                        alert('执行失败!')
                    }
                    // com.openTab(self.resx.detailTitle, self.urls.edit1 + d);
                }
            });
        }
    }
}


send.viewModel.edit = function (data) {
    var self = this;

    this.dataSource = data.dataSource;                          //下拉框的数据源
    this.urls = data.urls;
    this.resx = data.resx;                                      //中文资源
    this.addUrl = data.addUrl;
    this.scrollKeys = ko.mapping.fromJS(data.scrollKeys);       //数据滚动按钮（上一条下一条）
    this.form = ko.mapping.fromJS(data.form || data.defaultForm); //表单数据
    this.setting = data.setting;
    //主表的默认值
    this.readonly = ko.computed(function () { return self.form.ApproveState() == "passed"; });

    this.grid = {
        size: { w: 6, h: $(".z-toolbar").height() + $(".container_12").height() +35 },
        pagination: false,
        remoteSort: false,
        singleSelect: false,
        url: ko.observable(self.urls.getdetail + self.scrollKeys.current())
    };

    this.refreshClick = function () {
        window.location.reload();
    };

    this.grid.toolbar = "#gridtb";

};
