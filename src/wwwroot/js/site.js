function deleteTodo(i)
{
    $.ajax({
        url: 'Home/Delete',
        type: 'DELETE',
        data: {
            id: i
        },
        success: function() {
            window.location.reload();
        }
    })
}

function gettingByIdForm(i)
{
    $.ajax({
        url: 'Home/GettingByIdForm',
        type: 'POST', //there was GET
        data: {
            id: i
        },
        dataType: 'json',
        success: function (response) {
            $("#Todo_Name").val(response.name);
            $("#Todo_Id").val(response.id);
            $("#form-button").val("Update Todo");
            $("#form-action").attr("action", "/Home/Update");
        }
    });
}