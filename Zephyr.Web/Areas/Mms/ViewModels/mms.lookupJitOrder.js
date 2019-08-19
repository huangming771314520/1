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
                $('#list').datagrid('resize', { width: w, height: h - 38 });
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
        if (_xml == 'mms.jit_order') {
            cols = [[
                 { title: '订单号', field: 'Work_Order_No', sortable: true, align: 'center', width: 100 },
                 { title: '计划序', field: 'Work_Order_Series_No', sortable: true, align: 'center', width: 80 },
                 { title: '计划日期', field: 'Work_Order_Plan_Date', sortable: true, align: 'center', width: 120 },
                 { title: '物料编码', field: 'Material_No', sortable: true, align: 'center', width: 60 },
                 { title: '物料名称', field: 'Material_Name', sortable: true, align: 'center', width: 60 },
                 { title: '数量', field: 'Amount_Work_Order', sortable: true, align: 'center', width: 100 },
                 { title: '生产线编号', field: 'Line_No', sortable: true, align: 'center', width: 120 },
                 { title: '生产线名称', field: 'Line_Name', sortable: true, align: 'center', width: 80 },
                 { title: '收货仓库编码', field: 'Receive_Warehouse_Code', sortable: true, align: 'center', width: 60 },
                 { title: '发货仓库编码', field: 'Send_Warehouse_Code', sortable: true, align: 'center', width: 60 },
                 { title: '创建人', field: 'Create_Person', sortable: true, align: 'center', width: 60 },
                 { title: '工位器具编码', field: 'GWQJ_Code', sortable: true, align: 'center', width: 200 }

            ]];
            opt = $.extend({}, defaults, {
                height: 458,
                pagination: true,
                url: '/api/mms/home/GetMaterialDictionary',
                queryParams: param,
                pageSize: 10,
                columns: cols
                 
            });
        }
        else if (_xml == 'mms.erp_equipment') {
            cols = [[
                 { title: '车间', field: 'Workshop', sortable: true, align: 'center', width: 100 },
                 { title: '生产班组', field: 'Produce_Team', sortable: true, align: 'center', width: 80 },
                 { title: '设备类别', field: 'Equipment_Type', sortable: true, align: 'center', width: 120 },
                 { title: '类别名称', field: 'Equipment_TypeName', sortable: true, align: 'center', width: 120 },
                 { title: '设备编号', field: 'Equipment_No', sortable: true, align: 'center', width: 120 },
                 { title: '设备名称', field: 'Equipment_Name', sortable: true, align: 'center', width: 100 },
                 { title: '设备型号', field: 'Model', sortable: true, align: 'center', width: 120 },
                 { title: '设备规格', field: 'Type', sortable: true, align: 'center', width: 120 },
                 { title: '设备区分', field: 'Equipment_Different', sortable: true, align: 'center', width: 120 },
                 { title: '生产线区分', field: 'LineNo_Different', sortable: true, align: 'center', width: 120 },
                 { title: '生产线区分', field: 'LineName_Different', sortable: true, align: 'center', width: 120 }
            ]];
            opt = $.extend({}, defaults, {
                height: 440,
                pagination: true,
                url: '/api/mms/home/GetEquipment',
                queryParams: param,
                pageSize: 10,
                columns: cols
            });
        }

        //定义返回值
        var selected = { total: 0, rows: [] };


        grid1.datagrid(opt);

        //设置查询条件
        var search = function (value) {
            if (_xml == 'mms.jit_order') {
                var queryParams = $.extend({}, param, {
                    GWQJ_Code: $('#id').val(),
                    Work_Order_No: $('#text').val()
                });
                grid1.datagrid('reload', queryParams);
            }
            else if (_xml == 'mms.erp_equipment') {
                var queryParams = $.extend({}, param, {
                    Equipment_No: $('#id').val(),
                    Workshop: $('#text').val()
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