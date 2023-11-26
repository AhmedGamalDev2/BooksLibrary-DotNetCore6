function ShowSweetAlertSuccessMessage(successMessage) {
    Swal.fire({
        icon: "success",
        title: successMessage,
    });
}
function ShowToastrSuccessMessage(successMessage) {
    toastr["success"](successMessage)

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "2000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

$(document).ready(function () {
    var successMessage = $('#SuccessMessage').text();
    //attention to space in successMessage because is not == ""
    if (successMessage !== "") {
        ShowSweetAlertSuccessMessage(successMessage);
    }
})