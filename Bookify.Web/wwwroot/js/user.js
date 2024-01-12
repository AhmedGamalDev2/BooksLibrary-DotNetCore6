$(document).ready(function () {

    $(".js-checkEmail").on("keyup", function () {
        // يتم تنفيذ هذه الدالة عند الضغط على أي مفتاح في العنصر
        console.log("upuuuuu")
    });

    $(".js-checkEmail").on("keydown", function () {
        var btn = $(this);
        console.log(btn)
        console.log("hello")

    })

    $('.js-select2').on('select2:select', function (e) { // here, we revalidate on select2 when select event
        console.log("selecccccccct")
    });
    $(".js-checkEmail").on("click", function () {
         
       
        console.log("helloooooooooooo")

    })

    /*
    $(".js-toggle-status").on("click", function () {
        var btn = $(this);
        //here call ToggleStatus fucntion
        var authorId = btn.data('id');
        bootbox.confirm({
            message: 'Do you want to toggle status?',
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-primary'
                }
            },
            callback: function (result) {

                if (result) {
                    $.post({
                        url: "/Authors/ToggleStatus/" + authorId,
                        data: { // request body
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val(),
                        },
                        success: function (response) {
                            //to convert date to 	11/26/2023, 11:54:30
                            const normalDate = new Date(response.lastUpdatedOn);
                            const formattedDate = normalDate.toLocaleString('en-US', {
                                year: 'numeric',
                                month: '2-digit',
                                day: '2-digit',
                                hour: 'numeric',
                                minute: 'numeric',
                                second: 'numeric',
                                hour12: true,
                            });

                            var statusRow = btn.parents('tr').find('.jsStatus');
                            var lastUpdateRow = btn.parents('tr').find('.js-updated-on');
                            lastUpdateRow.html(formattedDate)

                            if (!response.isDeleted) {
                                statusRow.text("Available").toggleClass('badge-light-danger badge-light-success');
                            } else {
                                statusRow.text("Deleted").toggleClass('badge-light-success badge-light-danger');
                            }
                            statusRow.toggleClass("animate__animated animate__flash ") //animation
                            lastUpdateRow.toggleClass("animate__animated animate__flash") //animation

                            ////alert function
                            ShowToastrSuccessMessage("Saved Successfully");
                        },
                        error: function (error) {
                            //alert 
                            Swal.fire({
                                icon: "error",
                                title: "Error",
                                text: "There is Error",

                            });
                        }
                    })

                }
            }
        });
 
    })
    */
})