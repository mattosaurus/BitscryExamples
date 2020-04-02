/*
--------------------------------------------------------------------------
DATATABLES ODATA V4 ADDON
--------------------------------------------------------------------------
Enables jQuery DataTables to read data from an OData service
Version: 1.0.4
Author: Michele Bersini
Copyright 2016 Michele Bersini, all rights reserved.
This source file is free software, licensed under 
MIT license (http://datatables.net/license/mit)
Permission is hereby granted, free of charge, to any person obtaining a 
copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the 
Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in 
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
*/

function ajaxOData(data, callback, settings) {

    var getColumnFieldName = function (column) {
        return column.name || column.data;
    }

    var request = {};

    // Use JSONP if OData service is on another domain
    var oDataViaJsonp = settings.oInit.oDataViaJSONP;
    if (oDataViaJsonp) {
        request.$callback = "odatatable_" + (settings.oFeatures.bServerSide ? data.draw : ("load_" + Math.floor((Math.random() * 1000) + 1)));
    }

    // Get column names for select
    $.each(settings.aoColumns, function (i, value) {
        var fieldName = getColumnFieldName(value);
        if (!fieldName) {
            return;
        }
        if (!request.$select) {
            request.$select = fieldName;
        } else {
            request.$select += "," + fieldName;
        }
    });

    // Filters and ordering on server side if requested
    if (settings.oFeatures.bServerSide) {

        request.$skip = settings._iDisplayStart;
        if (settings._iDisplayLength > -1) {
            request.$top = settings._iDisplayLength;
        }
        request.$count = true;

        var filters = [];
        var columnFilters = [];
        var globalFilter = data.search.value;
        $.each(settings.aoColumns, function (i, value) {
            var fieldName = getColumnFieldName(value);
            var columnFilter = data.columns[i].search.value;
            var columnType = value.type || "string";

            if ((!globalFilter && !columnFilter) || !value.bSearchable || !fieldName)
                return;

            switch (columnType) {
                case "string":
                case "html":
                    if (globalFilter && globalFilter.trim()) {
                        filters.push("indexof(tolower(" + fieldName + "), '" + globalFilter.toLowerCase() + "') gt -1");
                    }

                    if (columnFilter && columnFilter.trim()) {
                        columnFilters.push("indexof(tolower(" + fieldName + "), '" + columnFilter.toLowerCase() + "') gt -1");
                    }
                    break;

                case "date":
                case "num":
                case "numeric":
                case "number":
                    var parseValue = function (val) {
                        var f = window.Globalize ? window.Globalize.parseFloat(val) : Number.parseFloat(val);
                        if (isNaN(f)) return null;
                        return f;
                    }
                    if (columnType === "date") {
                        parseValue = function (val) {
                            var d = window.Globalize ? window.Globalize.parseDate(val) : Date.parse(val);
                            if (!d) return null;
                            return d.toISOString();
                        }
                    }

                    var processRange = function (val) {
                        var result = "";
                        var separator = "";
                        var range = val.split("~");
                        var formattedValue;

                        if (range.length > 1) {
                            formattedValue = parseValue(range[0]);
                            if (formattedValue) {
                                result = fieldName + " ge " + formattedValue;
                                separator = " and ";
                            }
                            formattedValue = parseValue(range[1]);
                            if (formattedValue) {
                                result += separator + fieldName + " le " + formattedValue;
                            }
                        } else {
                            formattedValue = parseValue(val);
                            if (formattedValue) {
                                result = fieldName + " eq " + formattedValue;
                            }
                        }

                        if (result) {
                            result = "(" + result + ")";
                        }

                        return result;
                    }

                    // Numeric and date filters are supported also in form lower~upper
                    if (columnFilter && columnFilter !== "~") {
                        var colFilter = processRange(columnFilter);
                        if (colFilter) { columnFilters.push(colFilter); }
                    }

                    if (globalFilter && globalFilter !== "~") {
                        var globFilter = processRange(globalFilter);
                        if (globFilter) { filters.push(globFilter); }
                    }

                    break;
                default:
            }
        });

        if (filters.length > 0) {
            request.$filter = filters.join(" or ");
        }

        if (columnFilters.length > 0) {
            if (request.$filter) {
                request.$filter = " ( " + request.$filter + " ) and ( " + columnFilters.join(" and ") + " ) ";
            } else {
                request.$filter = columnFilters.join(" and ");
            }
        }

        var orderBy = [];
        $.each(data.order, function (i, value) {
            orderBy.push(getColumnFieldName(settings.aoColumns[value.column]) + " " + value.dir);
        });

        if (orderBy.length > 0) {
            request.$orderby = orderBy.join();
        }
    }

    if (settings.oInit.oDataAbort) {
        if (settings.jqXHR && settings.jqXHR.readystate != 4) {
            settings.jqXHR.abort();
        }
    }

    var jqXHR = $.ajax(jQuery.extend({}, settings.oInit.ajax, {
        "url": settings.oInit.oDataUrl,
        "data": request,
        "jsonp": oDataViaJsonp,
        "dataType": oDataViaJsonp ? "jsonp" : "json",
        "jsonpCallback": data["$callback"],
        "cache": false,
        "success": function (ajaxData) {
            var dataSource = {
                draw: parseInt(data.draw) // Cast for security reason
            };

            dataSource.data = ajaxData.value;
            var recordCount = ajaxData["@odata.count"];

            if (recordCount != null) {
                dataSource.recordsFiltered = recordCount;
            } else {
                if (dataSource.data.length === settings._iDisplayLength) {
                    dataSource.recordsFiltered = settings._iDisplayStart + settings._iDisplayLength + 1;
                } else {
                    dataSource.recordsFiltered = settings._iDisplayStart + dataSource.data.length;
                }
            }
            dataSource.recordsTotal = dataSource.recordsFiltered;

            callback(dataSource);
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            settings.oApi._fnCallbackFire(settings, null, 'xhr', [settings, null, settings.jqXHR]);
            settings.oApi._fnProcessingDisplay(settings, false);
            settings.oApi._fnLog(settings, 0, "Error while loading data: " + textStatus + " - " + errorThrown);
        }
    }));

    return jqXHR;
};