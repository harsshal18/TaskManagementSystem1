$(document).ready(function () {

    var taskModal = new bootstrap.Modal(document.getElementById('taskModal'));

    // 🔹 Format Microsoft JSON Date
    function formatDate(jsonDate) {
        if (!jsonDate) return '';
        var match = /\/Date\((\d+)\)\//.exec(jsonDate);
        if (!match) return jsonDate;
        var date = new Date(parseInt(match[1]));
        return date.toISOString().split('T')[0];
    }

    // 🔹 Load all tasks using AJAX + DataTables
    function loadTasks() {
        var status = $('#filterStatus').val();
        var assignedTo = $('#filterAssignedTo').val();

        $.ajax({
            url: '/Task/GetTasks',
            type: 'GET',
            data: { status: status, assignedTo: assignedTo },
            success: function (res) {
                if (res.success) {
                    // Destroy old DataTable if already initialized
                    if ($.fn.DataTable.isDataTable('#tasksTable')) {
                        $('#tasksTable').DataTable().destroy();
                    }

                    // Populate table rows
                    var rows = '';
                    $.each(res.data, function (i, t) {
                        rows += `
                            <tr>
                                <td>${t.TaskId}</td>
                                <td>${t.Title}</td>
                                <td>${t.AssignedTo || ''}</td>
                                <td>${formatDate(t.DueDate)}</td>
                                <td>${t.Status || ''}</td>
                                <td>
                                    <button class="editBtn btn btn-primary btn-sm" data-id="${t.TaskId}">Edit</button>
                                    <button class="delBtn btn btn-danger btn-sm" data-id="${t.TaskId}">Delete</button>
                                </td>
                            </tr>`;
                    });
                    $('#tasksTable tbody').html(rows);

                    // ✅ Initialize DataTable
                    $('#tasksTable').DataTable({
                        pageLength: 5,
                        lengthChange: false,
                        searching: true,
                        ordering: true,
                        language: { search: "Search:" }
                    });
                } else {
                    showError('Failed to load tasks');
                }
            },
            error: function () {
                showError('Error while loading tasks');
            }
        });
    }

    // 🔹 Show error message
    function showError(message) {
        var alertDiv = `
            <div class="alert alert-danger alert-dismissible fade show mt-3" role="alert">
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>`;
        $('.container').prepend(alertDiv);
    }

    // 🔹 Initial load
    loadTasks();

    // 🔹 Filter button
    $('#btnFilter').click(loadTasks);

    // 🔹 New Task button
    $('#btnNew').click(function () {
        $('#taskForm')[0].reset();
        $('#TaskId').val('');
        taskModal.show();
    });

    // 🔹 Edit button
    $(document).on('click', '.editBtn', function () {
        var id = $(this).data('id');
        $.get('/Task/GetTask', { id: id }, function (res) {
            if (res.success) {
                var t = res.data;
                $('#TaskId').val(t.TaskId);
                $('#Title').val(t.Title);
                $('#Description').val(t.Description);
                $('#AssignedTo').val(t.AssignedTo);
                $('#DueDate').val(formatDate(t.DueDate));
                $('#Status').val(t.Status);
                taskModal.show();
            } else {
                showError('Task not found');
            }
        });
    });

    // 🔹 Delete button
    $(document).on('click', '.delBtn', function () {
        if (!confirm('Delete this task?')) return;
        var id = $(this).data('id');
        $.post('/Task/DeleteTask', { id: id }, function (res) {
            if (res.success) {
                loadTasks();
            } else {
                showError('Delete failed');
            }
        });
    });

    // 🔹 Save form
    $('#taskForm').submit(function (e) {
        e.preventDefault();

        var formData = {
            TaskId: $('#TaskId').val() ? parseInt($('#TaskId').val()) : 0,
            Title: $('#Title').val(),
            Description: $('#Description').val(),
            AssignedTo: $('#AssignedTo').val(),
            DueDate: $('#DueDate').val(),
            Status: $('#Status').val()
        };

        var url = formData.TaskId > 0 ? '/Task/UpdateTask' : '/Task/CreateTask';

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            success: function (res) {
                if (res.success) {
                    alert('Task saved successfully!');
                    taskModal.hide();
                    loadTasks();
                } else {
                    showError('Save failed: ' + (res.message || JSON.stringify(res.errors)));
                }
            },
            error: function () {
                showError('Error saving task');
            }
        });
    });
});
