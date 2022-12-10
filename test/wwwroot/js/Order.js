$(document).ready(function () {

    var status = "";
    var url = window.location.search;
    if (url.includes("pending"))
        status = "pending";
    else if (url.includes("approved"))
        status = "approved";
    else if (url.includes("inprocess"))
        status = "Processing";
    else if (url.includes("completed"))
        status = "completed";

    $('#myTable').DataTable({
        ajax: {
            url: "/Customer/Order/GetAll?status=" + status
        },
        columns: [
            { data: "id" },
            { data: "name" },
            { data: "phoneNumber" },
            { data: "applicationUser.email" },
            { data: "orderStatus" },
            { data: "orderTotal" },
            {
                data: "id",
                render: function (data) {
                    return `
                    <a href="/Customer/Order/Details/${data}" class="btn btn-primary">Details</a>
                    `
                }
            }
        ]
    })
})