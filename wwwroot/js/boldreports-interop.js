// Interop file to render the Bold Reports component with properties.
window.BoldReports = {
    RenderViewer: function (elementID, reportViewerOptions) {
        $("#" + elementID).boldReportViewer({
            reportPath: reportViewerOptions.reportName,
            reportServiceUrl: reportViewerOptions.serviceURL,
            parameters: reportViewerOptions.parameters,
            printMode: true,
            ajaxBeforeLoad: function (args) {

                args.data = reportViewerOptions.data;

            },
            toolbarSettings: {
                showToolbar: true,
                customItems: [{
                    groupIndex: 0,
                    index: 0,
                    type: 'Default',
                    cssClass: "e-icon e-mail e-reportviewer-icon",
                    id: 'E-Mail',
                    tooltip: {
                        header: 'E-Mail',
                        content: 'Send rendered report as mail attachment'
                    }
                    
                }]
            },
            toolBarItemClick: 'ontoolBarItemClick'
        });
    }
}

function ontoolBarItemClick(args) {
    if (args.value == "E-Mail") {
        var proxy = $('#report-viewer').data('boldReportViewer');
        var Report = proxy.model.reportPath;
        var lastsIndex = Report.lastIndexOf("/");
        var reportName = Report.substring(lastsIndex + 1);
        var requrl = proxy.model.reportServiceUrl + '/SendEmail';
        var _json = {
            exportType: "PDF", reportViewerToken: proxy._reportViewerToken, ReportName: reportName
        };
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: requrl,
            data: JSON.stringify(_json),
            dataType: "json",
            crossDomain: true
        })
    }
}