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
        if (_xml == 'mms.ytr_mtr_send') {
            cols = [[
                 { title: '单据编号', field: 'JIT_ORDER_NO', sortable: true, align: 'center', width: 100 },
                 { title: '条码编号', field: 'BAR_CODE', sortable: true, align: 'center', width: 80 },
                 { title: '供应商编号', field: 'SUPPLY_NO', sortable: true, align: 'center', width: 60 },
                 { title: '供应商名称', field: 'SUPPLY_NAME', sortable: true, align: 'center', width: 60 },
                 { title: '发货时间', field: 'SEND_TIME', sortable: true, align: 'center', width: 100 },
                 { title: '创建时间', field: 'CreateTime', sortable: true, align: 'center', width: 120 },
                 { title: '物料编码', field: 'Material_No', sortable: true, align: 'center', width: 80 },
                 { title: '物料名称', field: 'Material_Name', sortable: true, align: 'center', width: 60 },

                 { title: '单位', field: 'Unit', sortable: true, align: 'center', width: 200 },
                 { title: '要货数量', field: 'SNP_AMOUNT', sortable: true, align: 'center', width: 80 },
                 { title: 'snp数', field: 'SNP', sortable: true, align: 'center', width: 60 },
                 { title: '实际要货数量', field: 'Account', sortable: true, align: 'center', width: 60 },
                 { title: '保管员编号', field: 'SER_CODE', sortable: true, align: 'center', width: 60 },
                 { title: '保管员名称', field: 'USER_NAME', sortable: true, align: 'center', width: 200 },
                 { title: '仓库代码', field: 'To_WareHouse', sortable: true, align: 'center', width: 60 },
                 { title: '紧急标记', field: 'PRIORITY', sortable: true, align: 'center', width: 60 }

            ]];
            opt = $.extend({}, defaults, {
                height: 400,
                pagination: true,
                url: '/api/mms/home/GetWHCon',
                queryParams: param,
                pageSize: 10,
                columns: cols,
                onClickRow: function (index, row) {
                    //selected.total = selected.rows.push(row);
                    //$('#total').html(selected.total);
                }
            });
        }
        //定义返回值
        var selected = { total: 0, rows: [] };


        grid1.datagrid(opt);

        //设置查询条件
        var search = function (value) {
            if (_xml == 'mms.ytr_mtr_send') {
                var queryParams = $.extend({}, param, {
                    'A.JIT_ORDER_NO': $('#JIT_ORDER_NO').val(),
                    'A.BAR_CODE': $('#BAR_CODE').val(),
                    'A.SUPPLY_NO': $('#SUPPLY_NO').val(),
                    'A.SUPPLY_NAME': $('#SUPPLY_NAME').val(),
                    'A.CreateTime': $('#CreateTime').val()
                });
                grid1.datagrid('reload', queryParams);
            }


        };

        $('#btnSearch').click(search);
        $('#btnClear').click(function () { $('#master').find("input").val(""); search(); });


        $('#btnConfirm').click(function () {
            //var allRows = $("#list").datagrid('getChecked');   
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