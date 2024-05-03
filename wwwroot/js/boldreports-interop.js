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

            }
        });
    },
    RenderDesigner: function (elementID, reportDesignerOptions) {
        $("#" + elementID).boldReportDesigner({
            serviceUrl: reportDesignerOptions.serviceURL
        });
    }
}