function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 5000,
        theme: theme !== '' ? theme : 'success',
    })({
        title: title !== '' ? title : 'اعلان',
        message: text
    });
}

    $('#Search2').click( function() {
        $('#SearchForm2').submit();
});

    $('#Search1').click( function() {
        $('#SearchForm1').submit();
});


$(".toggle-password").click(function () {
    $(this).toggleClass("fa-eye fa-eye-slash");
    var input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});

function open_waiting(selector = 'body') {
    $(selector).waitMe({
        effect: 'win8',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255,255,255,0.7)',
        color: '#000'
    });
}

function close_waiting(selector = 'body') {
    $(selector).waitMe('hide');
}

function ChangeTicketStatus(ticketId, status) {
    if (ticketId !== "" && ticketId !== undefined && ticketId !== null && status !== "" && status !== undefined && status !== null) {
        $.get("/admin/Ticket/ChangeTicketStatus", { ticketId: ticketId, status: status }).then(res => {
            if (res.status === "Success") {
                location.reload();
            }
            else if (res.status === "Error") {
                ShowMessage("خطا", "در تغییر وضعیت تیکت خطایی رخ داده است", "error");
            }
        });
    }
    else {
        ShowMessage("خطا", "در تغییر وضعیت تیکت خطایی رخ داده است", "error");
    }
};

$('#AddArticleForm').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        return false;
    }
});

$('#DisableSubmit').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        return false;
    }
});

$('#AddExerciseForm').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        return false;
    }
});

$('[remove-admin-ajax-button]').on('click', function (e) {
    e.preventDefault();
    var url = $(this).attr('href');
    var removeElementId = $(this).attr('remove-admin-ajax-button');
    swal({
        title: 'اخطار',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید ؟",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "بله",
        cancelButtonText: "خیر",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((result) => {
        if (result.value) {
            $.get(url).then(res => {
                location.reload();
            });
        } else if (result.dismiss === swal.DismissReason.cancel) {
            swal('اعلان', 'عملیات لغو شد', 'error');
        }
    });
});

$('#change_user_avatar_button').on('click',
    function () {
        $('#change_user_avatar_input').trigger('click');
    });

$('#disabledEnter').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        return false;
    }
});

$("#change_user_avatar_input").on('change',
    function () {
        $('#change_user_avatar_form').submit();
    });

function ReturnNameForDropZone() {
    return "Files";
}

function FillPageId(id) {
    $("#PageId").val(id);
    $("#filter-search").submit();
}

$('[remove-ajax-button]').on('click', function (e) {
    e.preventDefault();
    var url = $(this).attr('href');
    var removeElementId = $(this).attr('remove-ajax-button');
    swal({
        title: 'اخطار',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید ؟",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "بله",
        cancelButtonText: "خیر",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((result) => {
        if (result.value) {
            $.get(url).then(res => {
                if (removeElementId !== null && removeElementId !== undefined && removeElementId !== '' && res.status === "Success") {
                    ShowMessage("اعلان", "عملیات با موفقیت انجام شد", "success");
                    $('[remove-ajax-item=' + removeElementId + ']').fadeOut(1000);
                }
                else if (removeElementId !== null && removeElementId !== undefined && removeElementId !== '' && res.status === "Error") {
                    ShowMessage("اعلان", "عملیات با شکست مواجه شد", "error");
                }
                else {
                    location.reload();
                }
            });
        } else if (result.dismiss === swal.DismissReason.cancel) {
            swal('اعلان', 'عملیات لغو شد', 'error');
        }
    });
});

function countdown(Id, minutes, seconds, functionName) {
    function tick() {
        var counter = document.getElementById(Id);
        try {
            counter.innerHTML =
                (minutes < 10 ? "0" : "") + minutes.toString() + ":" + (seconds < 10 ? "0" : "") + String(seconds);
            seconds--;
            if (seconds >= 0) {
                setTimeout(tick, 1000);
            } else {
                if (minutes >= 1) {
                    setTimeout(function () {
                        countdown(minutes - 1, 59);
                    }, 1000);
                }
                else if (minutes == 0) {
                    functionName();
                }
            }
        } catch (e) {
            // Do Nothing 
        }
    }
    tick();
}

$("[ImageInput]").change(function () {
    var x = $(this).attr("ImageInput");
    if (this.files && this.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("[ImageFile=" + x + "]").attr('src', e.target.result);
        };
        reader.readAsDataURL(this.files[0]);
    }
});

$(document).ready(function () {

    var datePickers = $("[DatePicker]");
    if (datePickers.length > 0) {
        $('<link/>', {
            rel: 'stylesheet',
            type: 'text/css',
            href: '/common/kamadatepicker/style/kamadatepicker.min.css'
        }).appendTo('head');
        $.getScript("/common/kamadatepicker/src/kamadatepicker.min.js", function (script, textStatus, jqXHR) { });
    }

    var timePickers = $("[TimePicker]");
    if (timePickers.length > 0) {
        $('<link/>', {
            rel: 'stylesheet',
            type: 'text/css',
            href: '/common/material-time-picker/bootstrap-material-datetimepicker.css'
        }).appendTo('head');
        $.getScript("/common/material-time-picker/bootstrap-material-datetimepicker.js", function (script, textStatus, jqXHR) {
            $(timePickers).each(function (index, value) {
                $(value).bootstrapMaterialDatePicker({
                    format: 'HH:mm',
                    time: true,
                    date: false
                });
            });
        });
    }

    var tagifys = $("[tagify]");
    if (tagifys.length > 0) {
        $('<link/>', {
            rel: 'stylesheet',
            type: 'text/css',
            href: '/common/tagify-master/dist/tagify.css'
        }).appendTo('head');
        $.getScript("/common/tagify-master/dist/jQuery.tagify.min.js", function (script, textStatus, jqXHR) {
            $(tagifys).each(function (index, value) {
                var id = $(value).attr('tagify');
                new Tagify(document.querySelector('[tagify="' + id + '"]'), {
                    duplicates: false,
                    trim: true,
                    delimiters: ",",
                    originalInputValueFormat: valueArr => valueArr.map(item => item.value).join(', ')
                });
            });
        });
    }

    var dropZones = $("[dropZone]");
    if (dropZones.length > 0) {
        $('<link/>', {
            rel: 'stylesheet',
            type: 'text/css',
            href: '/common/dropzone/dropzone.css'
        }).appendTo('head');
        $.getScript("/common/dropzone/dropzone.js",
            function (script, textStatus, jqXHR) {
                $(dropZones).each(function (index, value) {
                    var id = $(value).attr('dropZone');
                    new Dropzone(document.querySelector('[dropZone="' + id + '"]'), {
                        paramName: ReturnNameForDropZone,
                        uploadMultiple: true,
                        parallelUploads: 5,
                        maxFiles: 5,
                        acceptedFiles: ".jpg, .jpeg, .png",
                        init: function () {
                            this.on("errormultiple", function (file) {
                                swal({
                                    title: 'اعلان',
                                    text: "در بارگذاری تصاویر خطایی رخ داده است",
                                    type: "error",
                                    confirmButtonClass: "btn-danger",
                                    confirmButtonText: "بستن",
                                    closeOnConfirm: true,
                                }).then((result) => {
                                    if (result.value) {
                                        location.reload();
                                    }
                                });
                            });
                            this.on("successmultiple", function (file) {
                                swal({
                                    title: 'اعلان',
                                    text: "تصاویر شما با موفقیت بارگذاری شد",
                                    type: "success",
                                    confirmButtonClass: "btn-success",
                                    confirmButtonText: "بستن",
                                    closeOnConfirm: true,
                                }).then((result) => {
                                    if (result.value) {
                                        location.reload();
                                    }
                                });
                            });
                            this.on("maxfilesreached", function (file) {
                                swal({
                                    title: 'اعلان',
                                    text: "تعداد مجاز بارگذاری در هر بار تلاش 5 تصویر می باشد",
                                    type: "error",
                                    confirmButtonClass: "btn-danger",
                                    confirmButtonText: "بستن",
                                    closeOnConfirm: true,
                                }).then((result) => {
                                    if (result.value) {
                                        location.reload();
                                    }
                                });
                            });
                        }
                    });
                });
            });
    }

    var numericInputs = $("[NumericInput]");
    if (numericInputs.length > 0) {
        $.getScript("/common/AutoNumeric/AutoNumeric.js",
            function (script, textStatus, jqXHR) {
                $(numericInputs).each(function (index, value) {
                    var id = $(value).attr('NumericInput');
                    new AutoNumeric(document.querySelector('[NumericInput="' + id + '"]'), {
                        currencySymbol: '  تومان  ',
                        outputFormat: "number",
                        allowDecimalPadding: false,
                        currencySymbolPlacement: "s",
                        digitGroupSeparator: ',',
                        unformatOnSubmit: true,
                        wheelStep: "10000"
                    });
                });
            });
    }

    var percentInputs = $("[PercentInput]");
    if (percentInputs.length > 0) {
        $.getScript("/common/AutoNumeric/AutoNumeric.js",
            function (script, textStatus, jqXHR) {
                $(numericInputs).each(function (index, value) {
                    var id = $(value).attr('NumericInput');
                    new AutoNumeric(document.querySelector('[NumericInput="' + id + '"]'), {
                        currencySymbol: '  %  ',
                        outputFormat: "number",
                        allowDecimalPadding: false,
                        currencySymbolPlacement: "s",
                        digitGroupSeparator: ',',
                        unformatOnSubmit: true,
                        wheelStep: "10000"
                    });
                });
            });
    }

    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        $.getScript("/common/ckeditor/build/ckeditor.js",
            function (script, textStatus, jqXHR) {
                $(editors).each(function (index, value) {
                    var id = $(value).attr('ckeditor');
                    ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                        {
                            toolbar: {
                                items: [
                                    'heading',
                                    '|',
                                    'fontSize',
                                    'fontColor',
                                    'fontBackgroundColor',
                                    'alignment',
                                    'pageBreak',
                                    '|',
                                    'link',
                                    'blockQuote',
                                    'bold',
                                    'underline',
                                    'italic',
                                    'removeFormat',
                                    'specialCharacters',
                                    'subscript',
                                    'superscript',
                                    'strikethrough',
                                    'findAndReplace',
                                    '|',
                                    'numberedList',
                                    'bulletedList',
                                    'indent',
                                    'outdent',
                                    'horizontalLine',
                                    '|',
                                    'codeBlock',
                                    'htmlEmbed',
                                    'sourceEditing',
                                    '|',
                                    'imageInsert',
                                    'imageUpload',
                                    'insertTable',
                                    '|',
                                    'undo',
                                    'redo'
                                ]
                            },
                            image: {
                                toolbar: [
                                    'imageTextAlternative',
                                    'imageStyle:inline',
                                    'imageStyle:block',
                                    'imageStyle:side',
                                    'linkImage'
                                ]
                            },
                            table: {
                                contentToolbar: [
                                    'tableColumn',
                                    'tableRow',
                                    'mergeTableCells',
                                    'tableCellProperties',
                                    'tableProperties'
                                ]
                            },
                            language: 'fa',
                            simpleUpload: {
                                uploadUrl: '/Uploader/UploadImage'
                            },
                            licenseKey: '',
                        })
                        .then(editor => {
                            window.editor = editor;
                        })
                        .catch(error => {
                            console.error(error);
                        });
                });
            });
    }
});

// Replace Persion Number to English In Input

$(function () {
    $('input').keyup(function (e) {
        var ctrlKey = 67, vKey = 86;
        if (e.keyCode != ctrlKey && e.keyCode != vKey) {
            $(this).val(persianToEnglish($(this).val()));
        }
    });
    $('textarea').keyup(function (e) {
        var ctrlKey = 67, vKey = 86;
        if (e.keyCode != ctrlKey && e.keyCode != vKey) {
            $(this).val(persianToEnglish($(this).val()));
        }
    });
});
function persianToEnglish(input) {
    var inputstring = input;
    var persian = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"]
    var english = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"]
    for (var i = 0; i < 10; i++) {
        inputstring = inputstring.toString().replace(persian[i], english[i]);
    }
    return inputstring;
}