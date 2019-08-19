

jQuery(document).ready(function () {
    /* initialize the external events */
    jQuery('#external-events div.external-event').each(function () {

        // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
        // it doesn't need to have a start or end
        var eventObject = {
            title: jQuery.trim(jQuery(this).text()) // use the element's text as the event title
        };

        // store the Event Object in the DOM element so we can get to it later
        jQuery(this).data('eventObject', eventObject);

        // make the event draggable using jQuery UI
        jQuery(this).draggable({
            zIndex: 999,
            revert: true,      // will cause the event to go back to its
            revertDuration: 0  //  original position after the drag
        });

    });

    //#region Ajax请求
    Ajaxrequest = function (rUrl, rType, rData, dType, fSucceed, fError, Async) {
        if (Async == undefined)
            Async = false;
        $.ajax({
            type: rType,
            url: rUrl,
            data: rData,
            dataType: dType,
            cache: false,
            async: true,
            success: fSucceed,
            error: fError,
            complete: function (XMLHttpRequest, textStatus) {
                if (XMLHttpRequest.responseText != undefined) {
                    try {
                        if (XMLHttpRequest.responseText.indexOf("window != top") > 0 || XMLHttpRequest.responseText.indexOf("userName") > 0) {
                            var url = getRootPath() + "/Admin/Login";
                            if (window != window.parent) {
                                window.parent.location.replace(url);
                            }
                            else {
                                window.location.replace(url);
                            }
                            $.unblockUI();
                        }
                        else {
                            $.unblockUI();
                        }
                    }
                    catch (ex) { }
                }
            }
        });
    }
    //#endregion 

    //#region 执行ajax请求返回的脚本
    executeScript = function (html) {
        var reg = /<script[^>]*>([^\x00]+)$/i;
        //对整段HTML片段按<\/script>拆分
        var htmlBlock = html.split("<\/script>");
        for (var i in htmlBlock) {
            //匹配正则表达式的内容数组，
            var blocks;
            if (blocks = htmlBlock[i].match(reg)) {
                //清除可能存在的注释标记，对于注释结尾-->可以忽略处理，eval一样能正常工作
                var code = blocks[1].replace(/<!--/, '');
                try {
                    //执行脚本
                    eval(code)
                }
                catch (e) {
                }
            }
        }
    };
    //#endregion 

    //#region 验证Ajax请求返回数据是否有效
    /*验证Ajax请求返回数据是否有效
    *data-->请求返回数据
    */
    isValid = function (data) {
        return data.indexOf("window != top") > 0 || data.indexOf("logintop") > 0;
    };
    //#endregion 

    function ImportAjax(url) {
        var dialog = ImportDialog();
        var okFunction = function (data) {
            if (isValid(data))
                return;
            dialog.content(data);
            if (data.indexOf("script") >= 0)
                executeScript(data);

        };
        var fError = function () {
        };
        //请求
        Ajaxrequest(url, "GET", null, "html", okFunction, fError);
    }



    //#region 编辑，删除
    function EditImportAjax(url, event) {
        var dialog = EditImportDialog(event);
        var okFunction = function (data) {
            if (isValid(data))
                return;
            dialog.content(data);
            if (data.indexOf("script") >= 0)
                executeScript(data);
        };
        var fError = function () {
        };
        //请求
        Ajaxrequest(url, "GET", null, "html", okFunction, fError);
    }
    function LoadCalendarData(viewStart, selCalTypeID) {
        var taskdata = { modeltype: "GetWorkCalender", selDate: viewStart, selCalTypeID: selCalTypeID };
        $.ajax({
            type: "POST", //使用post方法访问后台
            url: "../../DataHandle/OperaData.ashx", //要访问的后台地址
            data: taskdata, //要发送的数据
            success: function (data) {
                //清除其他月份的样式
                $(".fc-border-separate").find('td.fc-other-month').removeClass('fc-state-highlight-weekend');
                if (new Date(viewStart).getMonth() == 11) {
                    $(".fc-header .fc-button-prev").show();
                    $(".fc-header .fc-button-next").hide();
                } else if (new Date(viewStart).getMonth() == 0) {
                    $(".fc-header .fc-button-prev").hide();
                    $(".fc-header .fc-button-next").show();
                } else {
                    $(".fc-header .fc-button-prev").show();
                    $(".fc-header .fc-button-next").show();
                }
                if (data != null && data.length > 0) {
                    data = eval(data);
                    for (var i = 0; i < data.length; i++) {
                        var wday = new Date(data[i].Wo_Date).getDate();
                        var iswd = data[i].Is_WorkDate;
                        for (var c = 0; c < 7; c++) {// col
                            if ($(".fc-week0").find('td:eq(' + c + ')').attr("class").indexOf("fc-other-month") < 0) {
                                if ($(".fc-week0").find('td div.fc-day-number')[c].innerHTML == wday) {
                                    if (iswd == 1)//工作日
                                    {
                                        $(".fc-week0").find('td:eq(' + c + ')').removeClass('fc-state-highlight-weekend');
                                    } else {
                                        $(".fc-week0").find('td:eq(' + c + ')').addClass('fc-state-highlight-weekend');
                                    }
                                }
                            }
                        }
                    }
                    for (var i = 0; i < data.length; i++) {
                        var wday = new Date(data[i].Wo_Date).getDate();
                        var iswd = data[i].Is_WorkDate;
                        for (var c = 0; c < 7; c++) {// col
                            if ($(".fc-week1").find('td:eq(' + c + ')').attr("class").indexOf("fc-other-month") < 0) {
                                if ($(".fc-week1").find('td div.fc-day-number')[c].innerHTML == wday) {
                                    if (iswd == 1)//工作日
                                        $(".fc-week1").find('td:eq(' + c + ')').removeClass('fc-state-highlight-weekend');
                                    else $(".fc-week1").find('td:eq(' + c + ')').addClass('fc-state-highlight-weekend');
                                }
                            }
                        }
                    }
                    for (var i = 0; i < data.length; i++) {
                        var wday = new Date(data[i].Wo_Date).getDate();
                        var iswd = data[i].Is_WorkDate;
                        for (var c = 0; c < 7; c++) {// col
                            if ($(".fc-week2").find('td:eq(' + c + ')').attr("class").indexOf("fc-other-month") < 0) {
                                if ($(".fc-week2").find('td div.fc-day-number')[c].innerHTML == wday) {
                                    if (iswd == 1)//工作日
                                        $(".fc-week2").find('td:eq(' + c + ')').removeClass('fc-state-highlight-weekend');
                                    else $(".fc-week2").find('td:eq(' + c + ')').addClass('fc-state-highlight-weekend');
                                }
                            }
                        }
                    }
                    for (var i = 0; i < data.length; i++) {
                        var wday = new Date(data[i].Wo_Date).getDate();
                        var iswd = data[i].Is_WorkDate;
                        for (var c = 0; c < 7; c++) {// col
                            if ($(".fc-week3").find('td:eq(' + c + ')').attr("class").indexOf("fc-other-month") < 0) {
                                if ($(".fc-week3").find('td div.fc-day-number')[c].innerHTML == wday) {
                                    if (iswd == 1)//工作日
                                        $(".fc-week3").find('td:eq(' + c + ')').removeClass('fc-state-highlight-weekend');
                                    else $(".fc-week3").find('td:eq(' + c + ')').addClass('fc-state-highlight-weekend');
                                }
                            }
                        }
                    }
                    for (var i = 0; i < data.length; i++) {
                        var wday = new Date(data[i].Wo_Date).getDate();
                        var iswd = data[i].Is_WorkDate;
                        for (var c = 0; c < 7; c++) {// col
                            if ($(".fc-week4").find('td:eq(' + c + ')').attr("class").indexOf("fc-other-month") < 0) {
                                if ($(".fc-week4").find('td div.fc-day-number')[c].innerHTML == wday) {
                                    if (iswd == 1)//工作日
                                        $(".fc-week4").find('td:eq(' + c + ')').removeClass('fc-state-highlight-weekend');
                                    else $(".fc-week4").find('td:eq(' + c + ')').addClass('fc-state-highlight-weekend');
                                }
                            }
                        }
                    }
                    for (var i = 0; i < data.length; i++) {
                        var wday = new Date(data[i].Wo_Date).getDate();
                        var iswd = data[i].Is_WorkDate;
                        for (var c = 0; c < 7; c++) {// col
                            if ($(".fc-week5").find('td:eq(' + c + ')').attr("class").indexOf("fc-other-month") < 0) {
                                if ($(".fc-week5").find('td div.fc-day-number')[c].innerHTML == wday) {
                                    if (iswd == 1)//工作日
                                        $(".fc-week5").find('td:eq(' + c + ')').removeClass('fc-state-highlight-weekend');
                                    else $(".fc-week5").find('td:eq(' + c + ')').addClass('fc-state-highlight-weekend');
                                }
                            }
                        }
                    }
                }
            }
        });

        $("#calendar").fullCalendar('removeEvents');
    }
    
    //#endregion

    /* 初始化calendar */
    jQuery('#calendar').fullCalendar({
        header: {
            left: 'month,agendaWeek,agendaDay',
            center: 'title',
            right: 'today, prev, next'
        },
        monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
        monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
        dayNames: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
        dayNamesShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
        today: ["今天"],
        firstDay: 1,
        buttonText: {
            prev: '&laquo;',
            next: '&raquo;',
            prevYear: '&nbsp;&lt;&lt;&nbsp;',
            nextYear: '&nbsp;&gt;&gt;&nbsp;',
            today: '今天',
            month: '月',
            week: '周',
            day: '日'
        },
        viewDisplay: function (view) {
            //动态把数据查出，按照月份动态查询
            var viewStart = $.fullCalendar.formatDate(view.start, "yyyy-MM-dd HH:mm:ss");
            $("#hidCurYYMM").val(viewStart);
            var selCalTypeID = $("#selCalendarType").val();
            LoadCalendarData(viewStart, selCalTypeID);
        },


        dayClick: function (date, allDay, jsEvent, view) {
            if (isexist <= 0) { return; }//isexist获取当前用户是否有工作日历管理的权限，如没有则不往下执行
            var confirmStr = "";
            var caltag = 1;//1工作日,0休息日
            //选择当前日期的时间转换      
            var selectdate = $.fullCalendar.formatDate(date, "yyyy-MM-dd");
            var selCalTypeID = $("#selCalendarType").val();
            if (jsEvent.currentTarget.className.indexOf("fc-state-highlight-weekend") > 0) {
                caltag = 1;
                confirmStr = "确定要将" + selectdate + "修改为工作日吗?";
            } else { caltag = 0; confirmStr = "确定要将" + selectdate + "修改为休息日吗?" };
            if (confirm(confirmStr)) {
                var taskdata = { modeltype: "SaveWorkCalender", selDate: selectdate, ISWork: caltag, selCalTypeID: selCalTypeID };
                $.ajax({
                    type: "POST", //使用post方法访问后台
                    url: "../../DataHandle/OperaData.ashx", //要访问的后台地址
                    data: taskdata, //要发送的数据
                    success: function (data) {
                        if (data != null && data > 0) {
                            com.message('success', "修改成功");
                            if (caltag==0) {
                                $(jsEvent.currentTarget).addClass('fc-state-highlight-weekend');
                            } else {
                                $(jsEvent.currentTarget).removeClass('fc-state-highlight-weekend');
                            }
                        }

                    }
                });
            }
        },
        loading: function (bool) {
            if (bool) $('#loading').show();
            else $('#loading').hide();
        },


        //#region 数据绑定上去后添加相应信息在页面上(一开始加载数据时运行)
        eventAfterRender: function (event, element, view) {

            var fstart = $.fullCalendar.formatDate(event.start, "HH:mm");
            var fend = $.fullCalendar.formatDate(event.end, "HH:mm");
            var confbg = '<span class="fc-event-bg"></span>';
            if (view.name == "month") {//按月份                
                var evtcontent = '<div class="fc-event-vert"><a>';
                evtcontent = evtcontent + confbg;
                //evtcontent = evtcontent + '<span class="fc-event-titlebg">' + fstart + " - " + fend  + event.fullname + '</span>';   
                evtcontent = evtcontent + '<span class="fc-event-titlebg">' + event.fullname + '</span>';
                element.html(evtcontent);
            } else if (view.name == "agendaWeek") {//按周

                var evtcontent = '<a>';
                evtcontent = evtcontent + confbg;
                evtcontent = evtcontent + '<span class="fc-event-time">' + fstart + "-" + fend + '</span>';
                evtcontent = evtcontent + '<span>' + event.fullname + '</span>';
                element.html(evtcontent);
            } else if (view.name == "agendaDay") {//按日

                var evtcontent = '<a>';
                evtcontent = evtcontent + confbg;
                evtcontent = evtcontent + '<span class="fc-event-time">' + fstart + " - " + fend + '</span>';
                element.html(evtcontent);
            }
        },
        //#endregion

        //#region 鼠标放上去显示信息
        eventMouseover: function (calEvent, jsEvent, view) {
            //var fstart = $.fullCalendar.formatDate(calEvent.start, "yyyy/MM/dd HH:mm");
            //var fend = $.fullCalendar.formatDate(calEvent.end, "yyyy/MM/dd HH:mm");
            //$(this).attr('title', fstart + " - " + fend + " " + calEvent.fullname);
            $(this).attr('title', calEvent.fullname);
            $(this).css('font-weight', 'normal');
            //            $(this).tooltip({
            //                effect: 'toggle',
            //                cancelDefault: true
            //            });
        },
        eventClick: function (event) {
            var url = "/HbmsFullCalendar/Edit/" + event.id;
            EditImportAjax(url, event);
        },
        events: [],
        //#endregion

        editable: true,
        droppable: true, // this allows things to be dropped onto the calendar !!!
        drop: function (date, allDay) { // this function is called when something is dropped

            // retrieve the dropped element's stored Event Object
            var originalEventObject = jQuery(this).data('eventObject');

            // we need to copy it, so that multiple events don't have a reference to the same object
            var copiedEventObject = jQuery.extend({}, originalEventObject);

            // assign it the date that was reported
            copiedEventObject.start = date;
            copiedEventObject.allDay = allDay;

            // render the event on the calendar
            // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
            jQuery('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

            // is the "remove after drop" checkbox checked?

            jQuery(this).remove();

        }
    });



    ///// SWITCHING LIST FROM 3 COLUMNS TO 2 COLUMN LIST /////
    function reposTitle() {
        if (jQuery(window).width() < 1000) {
            if (!jQuery('.fc-header-title').is(':visible')) {
                if (jQuery('h3.calTitle').length == 0) {
                    var m = jQuery('.fc-header-title h2').text();
                    jQuery('<h3 class="calTitle">' + m + '</h3>').insertBefore('#calendar table.fc-header');
                }
            }
        } else {
            jQuery('h3.calTitle').remove();
        }
    }
    reposTitle();

    ///// ON RESIZE WINDOW /////
    jQuery(window).resize(function () {
        reposTitle();
    });

    $(function () {
        $(".fc-header-left").hide();
        $("#selCalendarType").change(function () {
            var viewStart = $("#hidCurYYMM").val();
            var selCalTypeID = $(this).val();
            LoadCalendarData(viewStart, selCalTypeID);
        });

    });

});

