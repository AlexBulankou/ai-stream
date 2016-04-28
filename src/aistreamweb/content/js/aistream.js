
$(function () {
    var avg = function (dataSet) {
        var sum = 0;
        for (var i = 0; i < dataSet.length; i++) {
            sum += dataSet[i];
        }
        return sum / dataSet.length;
    };

    var getChartDate = function (d) {
        if (!d)
        {
            d = new Date();
        }
        return d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
    }

    var charts = [
        {
            chart: null,
            data: null,
            lastDataSet: [0, 0],
            dataInit: function () { return google.visualization.arrayToDataTable([["Time", "Events generated", "Events sent"], [getChartDate(), 0, 0]]) },
            ctor: function () { return new google.visualization.LineChart($(".chart-eps")[0]) },
            options: {
                title: "Generated vs. Sent",
                curveType:"function"
            },
            processItem: function (item, dataSet) {
                dataSet[0]++;
                if (item.sent) {
                    dataSet[1]++;
                }
            },
            aggregate: function (firstVal, dataSet) {
                return [firstVal, dataSet[0], dataSet[1]];
            }
        },
         {
             chart: null,
             data: null,
             lastDataSet: [0, 0, 0, 0, 0, 0, 0, 0],
             dataInit: function () { return google.visualization.arrayToDataTable([["Time", "Event count", "Request count", "RDD count", "Exception count", "Trace count", "Performance counter count", "Metric count", "Page View count"], [getChartDate(), 0, 0, 0, 0, 0, 0, 0, 0]]) },
             ctor: function () { return new google.visualization.ColumnChart($(".chart-event-types")[0]) },
             options: {
                 title: "Event types",
                 bars: 'vertical',
                 isStacked: true
             },
             processItem: function (item, dataSet) {
                 switch (item.aidata.data.baseType) {
                     case "EventData":
                         dataSet[0]++;
                         break;
                     case "RequestData":
                         dataSet[1]++;
                         break;
                     case "RemoteDependencyData":
                         dataSet[2]++;
                         break;
                     case "ExceptionData":
                         dataSet[3]++;
                         break;
                     case "TraceData":
                         dataSet[4]++;
                         break;
                     case "PerformanceCounterData":
                         dataSet[5]++;
                         break;
                     case "MetricData":
                         dataSet[6]++;
                         break;
                     case "PageViewData":
                         dataSet[7]++;
                         break;

                 }
             },
             aggregate: function (firstVal, dataSet) {
                 return [firstVal, dataSet[0], dataSet[1], dataSet[2], dataSet[3], dataSet[4], dataSet[5], dataSet[6], dataSet[7]];
             }
         },
         {
             chart: null,
             data: null,
             lastDataSet: [0],
             dataInit: function () { return google.visualization.arrayToDataTable([["Time", "Requests received"], [getChartDate(), 0]]) },
             ctor: function () { return new google.visualization.LineChart($(".chart-rps")[0]) },
             options: { title: "Requests volume", curveType: "function" },
             processItem: function (item, dataSet) {
                 if (item.aidata.data.baseType === "RequestData") {
                     dataSet[0]++;
                 }
             },
             aggregate: function (firstVal, dataSet) {
                 return [firstVal, dataSet[0]];
             }
         },
         {
             chart: null,
             data: null,
             lastDataSet: [],
             dataInit: function () { return google.visualization.arrayToDataTable([["Time", "Request duration"], [getChartDate(), 0]]) },
             ctor: function () { return new google.visualization.LineChart($(".chart-request-duration")[0]) },
             options: { title: "Request duration", curveType: "function" },
             processItem: function (item, dataSet) {
                 if (item.aidata.data.baseType === "RequestData") {
                     dataSet.push(item.metric)
                 }
             },
             aggregate: function (firstVal, dataSet) {
                 if (dataSet.length > 0) {
                     return [firstVal, avg(dataSet)];
                 }
                 else {
                     return [firstVal, null];
                 }
             }
         },
         {
             chart: null,
             data: null,
             lastDataSet: [],
             dataInit: function () { return google.visualization.arrayToDataTable([["Time", "Dependency duration"], [getChartDate(), 0]]) },
             ctor: function () { return new google.visualization.LineChart($(".chart-rdd-duration")[0]) },
             options: { title: "Dependency duration", curveType: "function" },
             processItem: function (item, dataSet) {
                 if (item.aidata.data.baseType === "RemoteDependencyData") {
                     dataSet.push(item.metric)
                 }
             },
             aggregate: function (firstVal, dataSet) {
                 if (dataSet.length > 0) {
                     return [firstVal, avg(dataSet)];
                 }
                 else {
                     return [firstVal, null];
                 }
             }
         }
    ];
    var items = [];
    var chat = $.connection.aIHub || {};
    chat.client = chat.client || {};
    var maxItemsInArray = 200;

    $("#root").html("<div id=\"controlPanel\">" +
            "<p id=\"connectionInitiator\">" +
                "<input id=\"keyInput\" type=\"text\" placeholder=\"Please enter aistream:key value from your web.config and click Start\" />" +
                "<input id=\"startButton\" type=\"button\" value=\"Start\" />" +
            "</p>" +
            "<div id=\"chart-container\">" +
              "<div class=\"chart chart-eps\"></div>" +
              "<div class=\"chart chart-event-types\"></div>" +
              "<div class=\"chart chart-rps\"></div>" +
              "<div class=\"chart chart-request-duration\"></div>" +
              "<div class=\"chart chart-rdd-duration\"></div>" +
            "</div>" +
            "<div id=\"streamControls\">" +
                "<input id=\"saveButton\" type=\"button\" value=\"Save\" title=\"Save current stream to new window\" /> " +
                "<input id=\"clearButton\" type=\"button\" value=\"Clear\" title=\"Clear stream\" />" +
                "<input type=\"text\" id=\"searchTextBox\" placeholder=\"Type to filter and highlight\" />" +
            "</div>" +
        "</div>" +
        "<div id=\"container\"></div>");

    $.connection.hub.url = "aistream";

    $("#streamControls").hide();
    $("#chart-container").hide();


    var regexEscape = function (text) {
        return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
    };

    var initCharts = function () {
        for (var i = 0; i < charts.length; i++) {
            charts[i].data = charts[i].dataInit();
            charts[i].chart = charts[i].ctor();
            charts[i].chart.draw(charts[i].data, charts[i].options);
            charts[i].defaultDataSet = charts[i].lastDataSet.slice(0);
        }
    }

    var syntaxHighlight = function (json, searchFilterR, searchFilterLength) {
        json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
        return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
            var cls = 'number';
            if (/^"/.test(match)) {
                if (/:$/.test(match)) {
                    cls = 'key';
                } else {
                    if ((/"(RequestData|MessageData|RemoteDependencyData|EventData|PerformanceCounterData|ExceptionData|MetricData|ViewData)"/.test(match))) {
                        cls = 'tp';
                    } else {
                        cls = 'string';
                    }
                }
            } else if (/true|false/.test(match)) {
                cls = 'boolean';
            } else if (/null/.test(match)) {
                cls = 'null';
            }

            var retHtmlString = "";
            if (searchFilterLength > 0) {
                var matchStartIndex = match.search(searchFilterR);
                if (matchStartIndex > -1) {
                    if (matchStartIndex > 0) {
                        retHtmlString += '<span class="' + cls + '">' + match.substr(0, matchStartIndex) + '</span>';
                    }

                    retHtmlString += '<span class="filter-match">' + match.substr(matchStartIndex, searchFilterLength) + '</span>';

                    if (matchStartIndex + searchFilterLength < match.length) {
                        retHtmlString += '<span class="' + cls + '">' + match.substr(matchStartIndex + searchFilterLength) + '</span>';
                    }
                }
                else {
                    retHtmlString = '<span class="' + cls + '">' + match + '</span>';
                }
            }
            else {
                retHtmlString = '<span class="' + cls + '">' + match + '</span>';
            }
            return retHtmlString;
        });
    }

    /* utils */
    //

    var getAgoString = function (timeStamp) {
        var now = new Date();
        secondsPast = (now.getTime() - timeStamp.getTime()) / 1000;
        return parseInt(secondsPast) + "s ago";
    };

    /* end utils */


    chat.client.addItems = function (str) {
        var aiStreamTelemetryItems = JSON.parse(str);

        for (var i = 0; i < aiStreamTelemetryItems.length; i++) {
            /* add item to the beginning of array */
            var item =
                 {
                     sent: aiStreamTelemetryItems[i].s,
                     data: aiStreamTelemetryItems[i].d,
                     tracked: new Date(aiStreamTelemetryItems[i].ta),
                     aidata: JSON.parse(aiStreamTelemetryItems[i].d),
                     metric:aiStreamTelemetryItems[i].m

                 };
            items.unshift(item);
            for (var j=0;j<charts.length; j++)
            {
                charts[j].processItem(item, charts[j].lastDataSet);
            }
        }

        if (items.length > maxItemsInArray) {
            for (var i = maxItemsInArray - 1; i < items.length; i++) {
                items[i].toBeDeleted = true;
            }
        }
    };

    chat.client.confirmConnected = function (str) {
        $("#connectionInitiator").hide();
        $("#streamControls").show();
        $("#chart-container").show();
    };

    $('#saveButton').click(function () {
        var win = window.open("", "", "width=800,height=600,scrollbars=yes,resizable=yes");
        win.document.write(JSON.stringify(items));
        win.document.title = "AI Raw Data"
    });
    $('#clearButton').click(function () {
        // Clean the UI and the array
        $("#container > div").remove();
        items.length = 0;
    });


    // Start the connection.
    try {
        $.connection.hub.start().done(function () {
            $('#startButton').click(function () {
                chat.server.start($("#keyInput").val());
            });
        });
    }
    catch (err) {
        // test mode
    }

    var getTelemetryItemMetaContent = function (item) {
        if (item.sent) {
            return "<div class='telemetryItemMeta'><div class='telemetryItemTime'></div><div class='telemetryItemStatus'></div></div>";
        } else {
            return "<div class='telemetryItemMeta'><div class='telemetryItemTime'></div><div class='telemetryItemFiltered'></div></div>";
        }
    }

    var processItems = function () {

        var container = document.getElementById("container");
        var searchFilter = regexEscape(document.getElementById("searchTextBox").value);
        var searchFilterR = new RegExp(searchFilter, "i");

        var visibleItemCount = 0;
        var maxVisibleItemCount = 20;
        var visibleElements = [];
        var refNode = null;
        for (var i = 0; i < charts.length; i++)
        {
            var dataRow = charts[i].aggregate(getChartDate(), charts[i].lastDataSet);
            if (dataRow) {
                if (charts[i].data.getNumberOfRows() > 10) {
                    charts[i].data.removeRow(0);
                }

                charts[i].data.addRow(dataRow);
                charts[i].chart.draw(charts[i].data, charts[i].options);
            }
            charts[i].lastDataSet = charts[i].defaultDataSet.slice(0);
        }

        for (var i = 0; i < items.length; i++) {
            if (items[i].toBeDeleted) {
                if (items[i].element) {
                    container.removeChild(items[i].element);
                }
                items.splice(i, 1);
                i--;
                continue;
            }

            if (!items[i].element) {
                var jElement = $("<div class='telemetryItemRoot'>" + getTelemetryItemMetaContent(items[i]) + "<div class='telemetryItem'></div></div>").hide();
                items[i].element = jElement[0];

                if (refNode) {
                    if (refNode.nextSibling) {
                        container.insertBefore(items[i].element, refNode.nextSibling);
                    } else {
                        container.appendChild(items[i].element);
                    }
                } else if (container.firstChild) {
                    container.insertBefore(items[i].element, container.firstChild);
                } else {
                    container.appendChild(items[i].element);
                }
            }

            refNode = items[i].element;

            if (visibleItemCount >= maxVisibleItemCount) {
                $(items[i].element).hide();
                items[i].visible = false;
            } else {
                if (items[i].lastSearchFilter !== searchFilter) {
                    if (items[i].data.search(searchFilterR) > -1) {

                        //set innerHtml
                        $(items[i].element).find(".telemetryItem").html(syntaxHighlight(items[i].data, searchFilterR, searchFilter.length));



                        $(items[i].element).show();
                        items[i].visible = true;
                    }
                    else {
                        $(items[i].element).hide();
                        items[i].visible = false;
                    }

                    items[i].lastSearchFilter = searchFilter;
                }

                if (items[i].visible) {
                    $(items[i].element).find(".telemetryItemTime").html(getAgoString(items[i].tracked));
                    $(items[i].element).show();
                    visibleItemCount++;
                }
            }
        }


        window.setTimeout(processItems, 1000);
    }

    initCharts();
    processItems();
   


    window.aistream = {
        confirmConnected: chat.client.confirmConnected,
        addItems: chat.client.addItems
    };
});
