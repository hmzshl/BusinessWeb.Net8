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
    RenderViewerEmail: function (elementID, reportViewerOptions, emailConfig) {
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
                        content: 'Envoyer le rapport en pièce jointe'
                    }
                }]
            },
            toolBarItemClick: function (args) {
                // Call ontoolBarItemClick with emailConfig object
                ontoolBarItemClick(args, emailConfig);
            }
        });
    }
}
function ontoolBarItemClick(args, emailConfig) {
    if (args.value == "E-Mail") {
        var proxy = $('#report-viewer').data('boldReportViewer');
        var Report = proxy.model.reportPath;
        var lastIndex = Report.lastIndexOf("/");
        var reportName = Report.substring(lastIndex + 1);
        var requrl = proxy.model.reportServiceUrl + '/SendEmail';
        var _json = {
            exportType: "PDF",
            reportViewerToken: proxy._reportViewerToken,
            ReportName: reportName,
            smtpServer: emailConfig.smtpServer,
            port: emailConfig.port,
            senderEmail: emailConfig.senderEmail,
            password: emailConfig.password,
            recipientEmail: emailConfig.recipientEmail,
            objet: emailConfig.objet,
            fileName: emailConfig.fileName     
        };

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: requrl,
            data: JSON.stringify(_json),
            dataType: "json",
            crossDomain: true,
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("E-mail non envoyé !!! Details: " + error);
            }
        });
    }
}
function downloadFile(reportName, data) {
    var _json = {
        ReportName: reportName,
        data: data
    };

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: 'api/BoldReportsJSON/Export',
        data: JSON.stringify(_json),
        dataType: "json",
        crossDomain: true,
        success: function (response) {
            if (response.success) {
                alert(response.message);
            } else {
                alert(response.message);
            }
        },
        error: function (xhr, status, error) {
            alert("E-mail non envoyé !!! Details: " + error);
        }
    });
}


function downloadReport(reportViewerOptions) {

    var requrl = 'api/BoldReportsJSON/Export';
    var _json = {
        exportType: "PDF",
        ReportName: reportViewerOptions.ReportName

    };

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: requrl,
        data: JSON.stringify(_json),
        dataType: "json",
        crossDomain: true,
        success: function (response) {
            if (response.success) {
                alert(response.message);
            } else {
                alert(response.message);
            }
        },
        error: function (xhr, status, error) {
            alert("Mail Not sent, check your email configuration details: " + error);
        }
    });
}
function getWriterFormat() {
    var formatType = "";
    if ($('#rbtnPDf').is(':checked')) {
        formatType = $('#rbtnPDf').val();
    } else if ($('#rbtnWord').is(':checked')) {
        formatType = $('#rbtnWord').val();
    } else if ($('#rbtnxls').is(':checked')) {
        formatType = $('#rbtnxls').val();
    } else if ($('#rbtnCSV').is(':checked')) {
        formatType = $('#rbtnCSV').val();
    }
    return formatType;
}


