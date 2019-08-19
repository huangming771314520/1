$(function () {
    using(['layout', 'datagrid', 'tree'], function () {
        //获取window信息
        var iframe = getThisIframe();
        var thiswin = parent.$(iframe).parent();
        var options = thiswin.window("options");
        var param = options.paramater;

        //初始化layout
        var box = $("#layoutbox"), right = $('#right').layout();
        box.width($(window).width()).height($(window).height()).layout();
        $(window).resize(function () { box.width($(window).width()).height($(window).height()).layout('resize'); });

        //调整layout时自动调整grid
        var panels = $('#right').data('layout').panels;
        panels.north.panel({
            onResize: function (w, h) {
                $('#list').datagrid('resize', { width: w, height: h - 78 });
            }
        });
        //设置grid列
        var cols;
        //设置明细表格的属性
        var opt;
        var grid1 = $('#list');
        var defaults = {
            iconCls: 'icon icon-list',
            nowrap: true,           //折行
            rownumbers: true,       //行号
            striped: true,          //隔行变色
            singleSelect: false,     //单选
            remoteSort: true,       //后台排序
            pagination: false,      //翻页
            pageSize: com.getSetting("gridrows", 20),
            contentType: "application/json",
            method: "GET"
        };
        var _xml = param._xml;

        cols = [[
             { title: '设备设备号', field: 'Material_No', sortable: true, align: 'center', width: 100 },
             { title: '设备名称', field: 'Material_Name', sortable: true, align: 'center', width: 80 },
             { title: '型号', field: 'Model', sortable: true, align: 'center', width: 60 },
             { title: '规格', field: 'Type', sortable: true, align: 'center', width: 60 },
             { title: '库存', field: 'R_Num', sortable: true, align: 'center', width: 80 },
             { title: '设备分类编码', field: 'Equipment_Type', sortable: true, align: 'center', width: 100 },
             { title: '设备分类名称', field: 'Equipment_TypeName', sortable: true, align: 'center', width: 120 }         

        ]];
        opt = $.extend({}, defaults, {
            height: 400,
            pagination: true,
            url: '/api/mms/home/GetWHCon',
            queryParams: param,
            pageSize: 10,
            columns: cols,
            onClickRow: function (index, row) { 
            }
        });

        //定义返回值
        var selected = { total: 0, rows: [] };
        grid1.datagrid(opt);

        //设置查询条件
        var search = function (value) {
            if (_xml == 'mms.erp_recipients') {
                var queryParams = $.extend({}, param, {
                    'Equipment_Id': $('#Equipment_Id').val(),
                    'Equipment_Name': $('#Equipment_Name').val()
                });
                grid1.datagrid('reload', queryParams);
            }


        };

        $('#btnSearch').click(search);
        $('#btnClear').click(function () { $('#master').find("input").val(""); search(); });


        $('#btnConfirm').click(function () {  
            var allRows = $("#list").datagrid('getSelections');
            selected.rows = allRows;
            selected.total = allRows.length;
            options.onSelect(selected);
            destroyIframe(iframe);
            thiswin.window('destroy');
        });

        $('#btnCancel').click(function () {
            destroyIframe(iframe);
            thiswin.window('destroy');
        });
    });
});

function getThisIframe() {
    if (!parent) return null;
    var iframes = parent.document.getElementsByTagName('iframe');
    if (iframes.length == 0) return null;
    for (var i = 0; i < iframes.length; ++i) {
        var iframe = iframes[i];
        if (iframe.contentWindow === self) {
            return iframe;
        }
    }
    return null;
}

function destroyIframe(iframeEl) {
    if (!iframeEl) return;
    iframeEl.parentNode.removeChild(iframeEl);
    iframeEl = null;
};