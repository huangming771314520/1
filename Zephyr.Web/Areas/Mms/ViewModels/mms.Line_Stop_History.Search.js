/**
* 模块名：com viewModel
* 程序名: com.viewModel.search.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
var com = com || {};
com.viewModel = com.viewModel || {};

com.viewModel.search = function (data) {

    var thatData = data;
    var self = this;
    this.idField = data.idField || "Objiec_ID";
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    delete this.form.__ko_mapping__;

    this.grid = {
        size: { w: 4, h: $(".z-toolbar").height() + $(".container_12").height() + 15 },
        url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true,
        customLoad: false
    };

    this.grid.queryParams(data.form);

    this.searchClick = function () {

        var CycleValue = "undefined";
        var arr = document.getElementsByName('Cycle')
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].checked) {
                CycleValue = arr[i].value;            
            }
        }
        var param = {};
        if (CycleValue != 'undefined') {
            param = ko.toJS($.extend(true, this.form, { Cycle: CycleValue }));
        }
        else {
            param = ko.toJS(this.form);
        }
        this.grid.queryParams(param);
    };

    this.clearClick = function () {
        $.each(self.form, function () { this(''); });
        this.searchClick();
    };

    this.refreshClick = function () {
        window.location.reload();
    };


    


    
};
