var wnd, detailsTemplate;

$(document).ready(function () {

    $("#divLoadingTabStrip").hide();

    $("#foiaRequestsGrid").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        type: "POST",
                        url: "Requests/GetFOIARequests",
                        dataType: 'json',
                        success: function (data) {
                            options.success(data);
                        }
                    });
                }
            },
            pageSize: 20
        },
        height: 800,
        groupable: false,
        sortable: true,
        resizable: true,
        filterable: {
            extra: false,
            operators: {
                string: {
                    startswith: "Starts with",
                    eq: "Is equal to",
                    neq: "Is not equal to",
                    contains: "Contains"
                }
            }
        },
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        columns: [
            {
                field: "CaseNumber",
                title: "Case Number",
                width: "140px"                
            },
            {
                field: "CaseType",
                title: "Case Type",
                width: "140px"
            },
            {
                field: "RequestorFullName",
                title: "Requestor",
                width: "200px"
            },
            {
                field: "RequestorOrganization",
                title: "Requestor Organization",
                width: "200px"

            },
            {
                field: "AssignedOfficerName",
                title: "Assigned Officer",
                width: "200px"

            },
            {
                field: "RequestDate",
                title: "Request Date",
                width: "200px",
                filterable: false,
                hidden: false
            }
        ]
    });

});





