$(function () {
    using(['layout', 'datagrid', 'tree'], function () {
        //获取window信息
        var iframe = getThisIframe();
        var thiswin = parent.$(iframe).parent();
        var options = thiswin.window("options");
        var param = options.paramater;
        var oldNodeId = "";
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
                 { title: '物料编码', field: 'Material_No', sortable: true, align: 'left', width: 100 },
                 { title: '物料名称', field: 'Material_Name', sortable: true, align: 'left', width: 120 },
                 { title: '规格型号', field: 'Model', sortable: true, align: 'left', width: 80 },
                 { title: '计划价格', field: 'Plan_Price', sortable: true, align: 'right', width: 60 }
        ]];

        //if (param._xml == "mms.material_send" || param._xml == "mms.material_dict")
        //    cols[0].pop();

        if (param._xml == "mms.material_check") {
            cols = [[
                 { title: '物料编码', field: 'MaterialCode', sortable: true, align: 'left', width: 100 },
                 { title: '物料名称', field: 'MaterialName', sortable: true, align: 'left', width: 120 },
                 { title: '规格型号', field: 'Model', sortable: true, align: 'left', width: 80 },
                 { title: '账面数量', field: 'BookNum', sortable: true, align: 'right', width: 60 }
            ]];
        }
        else if (param._xml == "mms.material_puton") {
            cols = [[
                { title: '物料编码', field: 'Material_No', sortable: true, align: 'left', width: 100 },
                { title: '物料名称', field: 'Material_Name', sortable: true, align: 'left', width: 120 },
                { title: '收数量', field: 'In_Num,', sortable: true, align: 'right', width: 60 },
                { title: '单位', field: 'Unit', sortable: true, align: 'left', width: 80 },
                { title: '自地点', field: 'Oneself_Place', sortable: true, align: 'left', width: 80 },
                { title: '自仓库', field: 'Oneself_WareHouse', sortable: true, align: 'left', width: 80 },
                { title: '自批次', field: 'Oneself_Batch', sortable: true, align: 'left', width: 80 },
                { title: '至地点', field: 'To_Place', sortable: true, align: 'left', width: 80 },
                { title: '至仓库', field: 'To_WareHouse', sortable: true, align: 'left', width: 80 },
                { title: '至批次', field: 'To_Batch', sortable: true, align: 'left', width: 80 }
            ]];
        }
        //if (param._xml == "mms.material_batches" || param._xml == "mms.material_send" || param._xml == "mms.material_rentin") {
        //    cols[0].push({ title: '来源类型', field: 'SrcBillType', sortable: true, align: 'left', width: 60 ,formatter:mms.com.formatSrcBillType});
        //    cols[0].push({ title: '来源单号', field: 'SrcBillNo', sortable: true, align: 'left', width: 85 });
        //}

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
            url: '/api/mms/In_InMaster/GetMaterial',
            queryParams: param,
            pageSize: 10,
            columns: cols,
            onClickRow: function (index, row) {
                
            }
        });
        grid1.datagrid(opt);
        var typeid = '';
        var clickType = function (node) {
            var Material_No = $("#id").val();
            var Material_Name = $('#text').val();
            var queryParams = $.extend({}, param, {
                Material_Type: node.id,
                Material_No: Material_No,
                Material_Name: Material_Name
            });
            grid1.datagrid('reload', queryParams);
        };

        var search = function (value) {
            var queryParams = $.extend({}, param, {
                Material_Type: typeid,
                Material_No: $('#id').val(),
                Material_Name: $('#text').val()
            });
            grid1.datagrid('reload', queryParams);
        };

        var paramStr = "mms.material_stock";
        for (var key in param)
            paramStr += (paramStr ? "&" : "?") + key + "=" + param[key];

        $('#btnSearch').click(search);
        $('#btnClear').click(function () { $('#master').find("input").val(""); search(); });
        $('#typetree').tree({
            
            method: 'GET',
            url: '/api/mms/MaterialDictionary_SEdit/GetCodeType_MaterialDir_New?nodeId=1&topType=',
            lines: true,
            animate: true,
            loadFilter: function (d) {
                if (d.length > 0) {
                    var jsonData = eval(d);
                    return utils.toTreeData(jsonData, 'id', 'pid', "children");
                }

            },
            onSelect: function (node) {
                if (oldNodeId != node.id && (oldNodeId.indexOf(node.id) < 0 || oldNodeId == '')) {
                    oldNodeId = node.id
                    $.get('/api/mms/MaterialDictionary_SEdit/GetCodeType_MaterialDir_New?topType=' + node.id, function (result) {
                        //获取选中节点所有子节点，并全部删除
                        var allChildren = $('#typetree').tree('getChildren', node.target);

                        for (var i = 0; i < allChildren.length; i++) {
                            $('#typetree').tree('remove', allChildren[i].target);
                        }
                        if (result != '') {
                            //在当前节点下添加新子节点
                            $('#typetree').tree('append', {
                                parent: node.target,
                                data: result
                            });
                        }
                         
                        clickType(node)
                    });
                }

            }
        });

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