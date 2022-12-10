var table;

$(document).ready(function () {
     table = $('#Table').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title"},
            { "data": "isbn"},
            { "data": "price"},
            { "data": "category.name"},
            { "data": "coverType.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Admin/Product/Upsert/${data}" class="btn btn-primary" > <i class="bi bi-pencil-square"></i> &nbsp;Edit</a >
                        <a onclick=Delete('/Admin/Product/Delete/${data}') class="btn btn-primary"><i class="bi bi-trash"></i> &nbsp;Delete</a>
                    `
                }
            }
        ]
     });

});

function Delete (url) {
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
                url:url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        table.ajax.reload();
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