
$(function () {
    $('#cg').combogrid({
        panelWidth: 400,
        idField: 'Type_No',
        textField: 'Type_Name',
        url: '/api/Mms/Material_r_Sendtype/GetValueTextListByType',
        fitColumns: true,
        striped: true,
        editable: true,
        pagination: true,
        rownumbers: true,
        collapsible: false,
        fit: true,
        method: 'post',
        columns: [
            { filed: 'Type_No', title: '类别编号', width: 80, hidden: false },
            { filed: 'Type_Name', title: '类别名称', width: 80, hidden: false },
            { filed: 'Description', title: '描述', width: 80, hidden: false }
        ],
        keyHandler: {
            up: function () {
                var selected = $('#cg').combogrid('grid').datagrid('getSelected');
                if (selected) {
                    var index = $('#cg').combogrid('grid').datagrid('getRowIndex', selected);
                    if (index > 0) {
                        $('#cg').combogrid('grid').datagrid('selectRow', index - 1);
                    }
                    else {
                        var rows = $('#cg').combogrid('grid').datagrid('getRows');
                        $('#cg').combogrid('grid').datagrid('selectRow', rows.length - 1);
                    }
                }
            },
            down: function () {
                var selected = $('#cg').combogrid('grid').datagrid('getSelected');
                if (selected) {
                    var index = $('#cg').combogrid('grid').datagrid('getRowIndex', selected);
                    if (index < $('#cg').combogrid('grid').datagrid('getData').rows.length - 1) {
                        $('#cg').combogrid('grid').datagrid('selectRow', index + 1);
                    }
                    else {
                        $('#cg').combogrid('grid').datagrid('selectRow', 0);
                    }
                }

            },
            enter: function () {
                $('#txtGender').val($('#cg').combogrid('grid').datagrid('getSelected').Gender);

                $('#cg').combogrid('hidePanel');
            },
            query: function (keyword) {
                var queryParams = $('#cg').combogrid('grid').datagrid('options').queryParams;
                queryParams.keyword = keyword;
                $('#cg').combogrid('grid').datagrid('options').queryParams = queryParams;
                $('#cg').combogrid('grid').datagrid('reload');
                $('#cg').combogrid('setValue', keyword);
                $('#hdKeyword').val(keyword);
            }
        },
        onSelect: function () {
            $('#txtGender').val($('#cg').combogrid('grid').datagrid('getSelected').Gender);
        }
    });

    var pager = $('#cg').combogrid('grid').datagrid('getPager');
    if (pager) {
        $(pager).pagination({
            pageSize: 10,
            pageList: [20, 10, 5],
            beforePageText: '第',
            afterPageText: '页    共{pages}页',
            displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            onSelectPage: function (pageNumber,pageSize) {
                getData(pageNumber, pageSize);
                $('#cg').combogrid('grid').datagrid('options').pageSize = pageSize;
                $('#cg').combogrid('setValue', $('#hdkeyword').val());
                $('#txtGender').val('');
            },
            onChangePageSize: function () {
                getData(pageNumber, pageSize);
                $('#cg').combogrid("setValue", $('#hdKeyword').val());
                $('#txtGender').val('');
            },
            onRefresh: function (pageNumber, pageSize) { }
        });
    }

    var getData = function (page, rows) {
        $.ajax({
            type: 'POST',
            url: '/api/Mms/Material_r_Sendtype/GetValueTextListByType',
            data: "page=" + page + "&rows=" + rows + "&keyword=" + $('#hdKeyword').val(),
            error: function (XMLHttpRequest,textStatus,errorThrown) {
                alert(textStatus);
                $.messager.progress('close');
            },
            success: function (data) {
                $('#cg').combogrid('grid').datagrid('loadData', data);  
            }
        });
    }
});