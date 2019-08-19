
/**
* 模块名：com viewModel
* 程序名: SearchEdit.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
var com = com || {};
com.viewModel = com.viewModel || {};

com.viewModel.searchEdit = function (data) {
	var self = this;
	thatData = data;
	this.urls = data.urls;
	this.resx = data.resx;
	this.dataSource = data.dataSource;
	this.form = ko.mapping.fromJS(data.form);
	this.defaultRow = data.defaultRow;
	this.setting = data.setting;
	delete this.form.__ko_mapping__;

	this.grid = {
		size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + 15 },
		url: self.urls.query,
		queryParams: ko.observable(),
		pagination: true,
		singleSelect: true
	};
	this.gridEdit = new com.editGridViewModel(this.grid);
	this.grid.onDblClickRow = this.editClick;
	this.grid.onClickRow = this.gridEdit.ended;
	this.grid.OnAfterCreateEditor = function (editors) {
		if (editors[self.setting.idField]) com.readOnlyHandler('input')(editors[self.setting.idField].target, true);
	};
	this.searchClick = function () {
		var param = ko.toJS(this.form);
		this.grid.queryParams(param);
	};

	this.clearClick = function () {
		$.each(self.form, function () { this(''); });
		this.searchClick();
	};
	this.refreshClick = function () {
		window.location.reload();
	};

	this.editClick = function () {
		if (thatData.editFlag == "No") {
			return;
		}
		else {

			var row = self.grid.datagrid('getSelected');
			if (!row) return com.message('warning', self.resx.noneSelect);
			if (row.Status == 11 || row.Status == 12) {
				var staturStr = getStatusStr(row.Status);
				com.message("warningAlert", "正在生产或生产完成的计划不允许调序");
				return;
			}
			var _date = new Date();
			var year = _date.getFullYear();
			var month = (_date.getMonth() + 1) > 9 ? (_date.getMonth() + 1).toString() : "0" + (_date.getMonth() + 1).toString();
			var day = _date.getDate() > 9 ? _date.getDate().toString() : "0" + _date.getDate().toString();
			var currentDate = year + "-" + month + "-" + day; 
			if (dateCompare(row["Planned_Produce_Date"].toString().substring(0, 10), currentDate)) {
			    return;
			}
			else {
			     self.gridEdit.begin();
			}
			
		}
	};
	this.grid.onDblClickRow = this.editClick;

	//订单调序
	this.OrderAjust = function () {
		//添加调序条件，循环数据
		var rowMain = self.grid.datagrid('getSelections'); 
		if (!rowMain) { 
			alert("请选择计划进行操作");
			return;
		} 
		else {
			self.gridEdit.ended(); //结束grid编辑状态
			var post = {};
			post.list = self.gridEdit.getChanges(self.setting.postListFields);
			post.form = com.formChanges(self.form, data.form, self.setting.postFormKeys);
			com.ajax({
				url: '/api/mms/Work_SEdit_QAD/PostOrderAjust/"',
				data: ko.toJSON(post),
				success: function (d) {
					switch (d) { 
						case "No":
						    com.message('error', "调序失败，原因:"+d);
							break;
						case "OK":
							com.message('success', "调序成功");
							break;
						default:
							break;
					}
					window.location.reload();
				}
			});
		}

	};

	this.downloadClick = function (vm, event) {
		com.exporter(self.grid).download($(event.currentTarget).attr("suffix"));
	};
	this.refreshClick = function () {
		window.location.reload();
	};

	this.productPlanClick = function () {
	    if (confirm("请确认重新生成本月入库排产计划？")) {
            //com.message("alert","入库排序计划生成")
	        com.ajax({
	            url: '/api/mms/Work_SEdit_QAD/PostProductPlan/"', 
	            success: function (d) {
	                switch (d) {
	                    case "No":
	                        com.message('error', "生成失败，原因:" + d);
	                        break;
	                    case "OK":
	                        com.message('success', "生成成功");
	                        break;
	                    default:
	                        break;
	                }
	                window.location.reload();
	            }
	        });
	    }
	}
};

var dateCompare = function (date1, date2) {
	var oDate1 = new Date(date1);
	var oDate2 = new Date(date2);
	if (oDate1.getTime() < oDate2.getTime()) {
		com.message("warningAlert", "不能修改当天日期以前的计划");
		return true;
	}
	 
}

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
		case 11:
			statusStr = "生产中";
			break;
		case 11:
			statusStr = "生产完成";
			break;
		default:
			statusStr = "为未知状态请联系管理员";
			break;
	}
	return statusStr;
};


var _SignalRFun = function (userName, sendData) {
	connection.send(sendData);
}
