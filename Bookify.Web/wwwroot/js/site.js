﻿// Shared variables
var table;
var datatable;
var updatedRow;

var exportedCols = [];


function showSuccessMessage(message = 'Saved successfully!') {
    Swal.fire({
        icon: 'success',
        title: 'Good Job',
        text: message,
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}


//sweetalert
function ShowSweetAlertSuccessMessage(successMessage) {
    Swal.fire({
        icon: "success",
        title: successMessage,
    });
}
//toaster alert
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

function showErrorMessage(message = 'Something went wrong!') {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: message,
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}

function disableSubmitButton() {
    $('body :submit').attr('disabled', 'disabled').attr('data-kt-indicator', 'on');
}
function onModalBegin() {
    disableSubmitButton();
}
function onModalComplete() {
    $('body :submit').removeAttr('disabled').removeAttr('data-kt-indicator');
}
function onModalSuccess(row) {
    showSuccessMessage();
    $('#Modal').modal('hide');

    if (updatedRow !== undefined) {
        datatable.row(updatedRow).remove().draw();
        updatedRow = undefined;
    }

    var newRow = $(row);
    datatable.row.add(newRow).draw();
}
//datatables
var headers = $('th');
$.each(headers, function (i) { // select columns that i need to not export
    if (!$(this).hasClass('js-no-export'))
        exportedCols.push(i);
});
// Class definition
var KTDatatables = function () {

    // Private functions
    var initDatatable = function () {

        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            //default => sort by Name or first column
            //'order': [], // prevent default sorting by at least first column (Name)
            'pageLength': 10,
            'drawCallback': function () {
                KTMenu.createInstances();
            },
            'columnDefs': [
                { 'orderable': false, 'targets': -1 },  // آخر عمود (أزرار العمليات) ... action column(edit,status)
            ]
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $('.js-datatables').data('datatable-title');
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [0,1,2,3]  // تحديد الأعمدة التي يمكن تضمينها في التصدير
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: { // دي دريقة 
                        columns: [0, 1, 2, 3] //  //  تحديد الأعمدة التي يمكن تضمينها في التصدير
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: { // ودي طريقة تانية 
                        columns: exportedCols
                    },
                 
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-datatables');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();

 
function disableSubmitButton() { //f16
    $('body :submit').attr('disabled', 'disabled').attr('data-kt-indicator', 'on');
}

$(document).ready(function () {
    //Disable submit button //f16
    $('form').on('submit', function () {
        if ($('.js-tinymce').length > 0) {
            $('.js-tinymce').each(function () {
                var input = $(this);

                var content = tinyMCE.get(input.attr('id')).getContent();
                input.val(content);
            });
        }

        var isValid = $(this).valid();
        if (isValid) disableSubmitButton();
    });
    //tinymce textarea
    if ($('.js-tinymce').length> 0) { // (if)(this means if there page (index or form ) has element that has class with .js-tinymce) => because tinymce('.js-tinymce') not existed(give error) in index page , but only existed in form pages
        var options = { selector: ".js-tinymce", height: "440" };

        if (KTThemeMode.getMode() === "dark") {
            options["skin"] = "oxide-dark";
            options["content_css"] = "dark";
        }

        tinymce.init(options);
    }
    //datepicker
    $('.js-datepicker').daterangepicker({
        singleDatePicker: true,
        autoApply: true,
        drops:'up',
        maxDate: new Date()
    })


    //Select2
    $('.js-select2').select2();
    $('.js-select2').on('select2:select', function (e) { //f16
        var select = $(this);
        $('form').validate().element('#' + select.attr('id'));
        //console.log("hello select");
        //console.log(select)
    });

    /**start datatable*/
    //$('table').DataTable();
    KTUtil.onDOMContentLoaded(function () {
        KTDatatables.init();
    });
    /**end datatable*/
    var successMessage = $('#SuccessMessage').text();
    //attention to space in successMessage because is not == ""
    if (successMessage !== "") {
        ShowSweetAlertSuccessMessage(successMessage);
    }

    //Handle bootstrap modal => Add form with ajax 
    $('body').delegate('.js-render-modal', 'click', function () {
        var btn = $(this);
        var modal = $('#Modal');

        modal.find('#ModalLabel').text(btn.data('title'));

        if (btn.data('update') !== undefined) {
            updatedrow = btn.parents('tr');
        }

        $.get({
            url: btn.data('url'),
             
            success: function (form) {
               
                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
            },
            error: function () {
                showErrorMessage();
            }
        });

        modal.modal('show');
    });


     //Handle Toggle Status for books
    $('body').delegate('.js-toggle-status', 'click', function () {
        var btn = $(this);

        bootbox.confirm({
            message: "Are you sure that you need to toggle this item status?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },
            callback: function (result) {
                if (result) {
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function (lastUpdatedOn) {
                            var row = btn.parents('tr');
                            var status = row.find('.js-status');
                            var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                            status.text(newStatus).toggleClass('badge-light-success badge-light-danger');
                            row.find('.js-updated-on').html(lastUpdatedOn);
                            row.addClass('animate__animated animate__flash');

                            showSuccessMessage();
                        },
                        error: function () {
                            showErrorMessage();
                        }
                    });
                }
            }
        });
    });

 
})