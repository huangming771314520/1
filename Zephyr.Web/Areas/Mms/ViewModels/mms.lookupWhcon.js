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
        var cols = [[
                 { title: '工位器具编码', field: 'M_Code', sortable: true, align: 'left', width: 150 },
                 { title: '工位器具名称', field: 'M_Name', sortable: true, align: 'left', width: 150 },
                 { title: '工位器具型号', field: 'M_Type', sortable: true, align: 'left', width: 80 },
                 { title: '条形码', field: 'Bar_Code', sortable: true, align: 'right', width: 120 },
                 { title: 'SNP', field: 'SNP', sortable: true, align: 'right', width: 60 }
        ]];


        //定义返回值
        var selected = { total: 0, rows: [] };
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

        //设置明细表格的属性
        var opt = $.extend({}, defaults, {
            height: 310,
            pagination: true,
            url: '/api/mms/home/GetWHCon',
            queryParams: param,
            pageSize: 10,
            columns: cols,
            onClickRow: function (index, row) {
               
            }
        });
        grid1.datagrid(opt);

        var search = function (value) {
            var queryParams = $.extend({}, param, {

                GWQJ_Code: $('#id').val(),
                GWQJ_Name: $('#text').val()
            });
            grid1.datagrid('reload', queryParams);
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