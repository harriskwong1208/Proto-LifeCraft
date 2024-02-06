$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/event/getall' },
       "columns": [
           { data: 'name', "width": "15%" },
           { data: 'description', "width": "15%" },
           { data: 'date', "width": "15%" },
           { data: 'category.name', "width": "15%" }
        ]
    });
}