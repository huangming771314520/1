/**
* 模块名：mms viewModel
* 程序名: organize.js
* Copyright(c) 2013-2015 liwenxiang [ liwenxiang.xm@gmail.com ] 
**/

function viewModelOranize() {
    var self = this;
    this.refreshClick = function () {
        window.location.reload();
    };
    this.save = function (vm,win) {
        var post = { form: ko.toJS(vm.form) };
        com.ajax({
            type: 'POST',
            url: '/api/mms/EQP_BD_ProductionLine/edit',
            data: JSON.stringify(post),
            success: function (d) {
                data = d;
                //console.log(d);
                com.message('success', '保存成功！');
                win.dialog('close');
                self.initGraph(d);
            }
        });
    }
    this.addClick = function () {
        var defaults = { ParentCode: (self.selectNode || {}).LineCode || 0 };
        self.openDiloag("添加新生产线", defaults, function (vm, win) {
            if (com.formValidate(win)) {
                vm.form._LineCode = vm.form.LineCode();
                self.save(vm,win);
            }
        });
    };
    this.editClick = function () {
        if (!self.selectNode) return com.message('warning', '请先选择一个生产线！');
        //console.log(self.selectNode);
        self.openDiloag("编辑生产线-"+self.selectNode.LineName,self.selectNode, function (vm, win) {
            if (com.formValidate(win)) {
                self.save(vm,win);
            }
        });
    };
    this.deleteClick = function () {
        if (!self.selectNode) return com.message('warning', '请先选择一个生产线！');
        com.message('confirm', '确认要删除选中的生产线吗？', function (b) {
            if (b) {
                com.ajax({
                    type: 'DELETE',
                    url: '/api/mms/EQP_BD_ProductionLine/' + self.selectNode.LineCode,
                    success: function (d) {
                        com.message('success', '删除成功！');
                        self.initGraph(d);
                    }
                });
            }
        });
    };


    this.openDiloag = function (title,node,fnConfirm) {
        com.dialog({
            title: title,
            height: 270,
            width: 400,
            html: "#edit-template",
            viewModel: function (w) {
                var that = this;
                this.combotree = {
                    width:228,
                    data: self.combotreeData,
                    onBeforeSelect: function (node) {
                        var isChild = utils.isInChild(self.combotreeData, that.form._LineCode, node.id);
                        com.messageif(isChild, 'warning', '不能将自己或下级设为上级生产线');
                        return !isChild;
                    }
                },
                this.form = {
                    _LineCode:node.LineCode,
                    ParentCode: ko.observable(node.ParentCode),
                    LineCode: ko.observable(node.LineCode),
                    LineName: ko.observable(node.LineName),
                    IsReturn: ko.observable(node.IsReturn),
                    Barcode: ko.observable(node.Barcode)
                };
                this.calcCode = function (v) { //新增时 自动计算LineCode
                    if (!that.form._LineCode) {
                        v = v == 0 ? "" : v;
                        var list = [], suffix;
                        for (var i in self.data) {
                            list.push(self.data[i].LineCode);
                        }
                        for (var j = 1; j < 100; j++) {
                            suffix = j < 10 ? ("0" + j.toString()) : j.toString();
                            if ($.inArray(v + suffix,list) == -1)  
                                break;
                        }
                        that.form.LineCode(v + suffix);
                    }
                };

                this.form.ParentCode.subscribe(this.calcCode);
                this.calcCode(node.ParentCode);

                this.confirmClick = function () {
                    fnConfirm(this,w);
                };
                this.cancelClick = function () {
                    w.dialog('close');
                };
            }
        });
    };
    this.initTreeData = function () {
        var list = utils.filterProperties(data, ['LineCode as id', 'ParentCode as pid', 'LineName as text']);
        var treeData = utils.toTreeData(list, "id", "pid", "children");
        treeData.unshift({ "id": "F", "text": "==请选择==" });
        self.combotreeData = treeData;
    };
    this.initGraph = function (data) {
        self.data = data;
        var wrapper = $("div.wrapper").empty();
        var treeData = utils.toTreeData(data, "LineCode", "ParentCode", "children");

        var tb = renderTreeGraph(treeData);
        tb.appendTo(wrapper);
 
        //绑定事件
        $(wrapper).find(".td-node").click(function () {
            $(".td-node").css({ "background-color": "#f6f6ff", "color": "" });
            $(this).css({ "background-color": "#faffbe", "color": "#FF0000" });
            self.selectNode = $(this).data("node");
        }).dblclick(self.editClick);
        if (self.selectNode) {
            $("#td" + self.selectNode.LineCode).css({ "background-color": "#faffbe", "color": "#FF0000" });
        }
    };
    this.initGraph(data);
    this.initTreeData();
}

function renderTreeGraph(treeData) {
    //生成图形
    var tb = $('<table class="tb-node" cellspacing="0" cellpadding="0" align="center" border="0" style="border-width:0px;border-collapse:collapse;margin:0 auto;vertical-align:top"></table>');
    var tr = $('<tr></tr>');
    for (var i in treeData) {
        if (i > 0) $('<td>&nbsp;</td>').appendTo(tr);
        $('<td style="vertical-align:top;text-align:center;"></td>').append(createChild(treeData[i])).appendTo(tr);
    }
    tr.appendTo(tb);
    return tb;
}
 
//递归生成生产线树图形
function createChild(node, ischild) {
    var length = (node.children || []).length;
    var colspan = length * 2 - 1;
    if (length == 0)
        colspan = 1;

    var fnTrVert = function () {
        var tr1 = $('<tr class="tr-vline"><td align="center" valign="top" colspan="' + colspan + '"><img class="img-v" src="/Content/images/tree/Tree_Vert.gif" border="0"></td></tr>');
        return tr1;
    };
    //1.创建容器
    var tb = $('<table class="tb-node" cellspacing="0" cellpadding="0" align="center" border="0" style="border-width:0px;border-collapse:collapse;margin:0 auto;vertical-align:top"></table>');
    //var tb = $('<table style="BORDER-BOTTOM: 1px solid; BORDER-LEFT: 1px solid; BORDER-TOP: 1px solid; BORDER-RIGHT: 1px solid" border="1" cellspacing="0" cellpadding="2" align="center"></table>');

    //2.如果本节点是子节点，添加竖线在节点上面
    if (ischild) {
        fnTrVert().appendTo(tb);
    }

    // 3.添加本节点到图表
    var tr3 = utils.functionComment(function () {/*
<tr class="tr-node"><td align="center" valign="top" colspan="{0}">
	<table align="center" style="border:solid 2px" border="1" cellpadding="2" cellspacing="0">
	    <tr>
	        <td class="td-node" id='td{3}' data-node='{2}' align="center" valign="top" style="background-color:#f6f6ff;cursor:pointer;padding:2px;">{1}</td>
	    </tr>
	</table>
</td></tr> */
    });
    tr3 = utils.formatString(tr3, colspan, node.LineName, JSON.stringify(node),node.LineCode);
    $(tr3).appendTo(tb);

    // 4.增加上下级的连接线
    if (length > 1) {
        //增加本级连接下级的首节点竖线，在节点下方
        fnTrVert().appendTo(tb);

        //增加本级连接下级的中间横线
        var tr4 = utils.functionComment(function () {/*
<tr class="tr-hline">
    <td colspan="1">
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
            <tbody>
                <tr>
                    <td width="50%" style="background:url(/Content/images/tree/Tree_Empty.gif)"></td>
                    <td width="3px" height="3px" ><img src="/Content/images/tree/Tree_Dot.gif" border="0"></td>
                    <td width="50%" style="background:url(/Content/images/tree/Tree_Dot.gif)"></td>
                </tr>
            </tbody>
        </table>
    </td>
    <td style="background:url(/Content/images/tree/Tree_Dot.gif)" colspan="{0}"></td>
    <td colspan="1">
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
            <tbody>
                <tr>
                    <td width="50%" style="background:url(/Content/images/tree/Tree_Dot.gif)"></td>
                    <td width="3px" height="3px" ><img src="/Content/images/tree/Tree_Dot.gif" border="0"></td>
                    <td width="50%" style="background:url(/Content/images/tree/Tree_Empty.gif)"></td>
                </tr>
            </tbody>
        </table>
    </td>
</tr>*/});
        tr4 = utils.formatString(tr4, colspan - 2);
        $(tr4).appendTo(tb);
    }

    //5.递归增加下级所有子节点到图表
    if (length > 0) {
        var tr5 = $('<tr></tr>');

        for (var i in node.children) {
            if (i > 0) {
                $('<td>&nbsp;</td>').appendTo(tr5);
            }
            $('<td style="vertical-align:top;text-align:center;"></td>').append(createChild(node.children[i], true)).appendTo(tr5);
        }

        tr5.appendTo(tb);
    }

    return tb;
}
 