var oldNodeId = "";
//物料信息菜单
this.tree = {
    method: 'GET',
    url: '/api/mms/MaterialDictionary_SEdit/GetCodeType_MaterialDir?nodeId=1&topType=',
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
            $.get('/api/mms/MaterialDictionary_SEdit/GetCodeType_MaterialDir?topType=' + node.id, function (result) {
                //获取选中节点所有子节点，并全部删除
                var allChildren = $('#mm-tree').tree('getChildren', node.target);

                for (var i = 0; i < allChildren.length; i++) {
                    $('#mm-tree').tree('remove', allChildren[i].target);
                }
                if (result != '') {
                    //在当前节点下添加新子节点
                    $('#mm-tree').tree('append', {
                        parent: node.target,
                        data: result
                    });
                }

                self.CodeType(node.id);
                stCode = node.id;
            });
        } 
       
    },
    onBeforeLoad: function (node, param) {
        if (node == null) {
            $('#mm-tree').tree('options').url = "/api/mms/MaterialDictionary_SEdit/GetCodeType_MaterialDir?nodeId=1";//加载顶层节点
        } else {
            $('#mm-tree').tree('options').url = "/api/mms/MaterialDictionary_SEdit/GetCodeType_MaterialDir?topType=" + node.id;//加载下层节点
        }
    }
};