var CompanyTable;
$(document).ready(function () {
    CompanyTable = $('#CompanyTable').DataTable({
        ajax: {
            url: "/Admin/Company/GetAll"
        },
        columns: [
            { data: "name" },
            { data: "streetAddress" },
            { data: "city" },
            { data: "state" },
            { data: "postalCode" },
            { data: "phoneNumber" },
            {
                data: "Id",
                render: function (data) {
                    return `
                    <a href="/Admin/Company/Upsert/${data}" class="btn btn-primary">Edit</>
                    <a onclick=Delete("/Admin/Company/Delete/${data}") class="btn btn-primary">Delete</>
                    `
                }

            }

        ]
    });
})

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        table.ajax.reload();
                        CompanyTable.ajax.reload();
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    }
                }
            })
        }
    })
}