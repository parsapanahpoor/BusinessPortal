//#region Active Course Link

function ShowAddOrEditActiveCourseLink(activeCourseId) {
    $.ajax({
        url: "/Admin/ActiveCourse/LoadAddOrEditActiveCourseLinkModal",
        type: "post",
        data: {
            activeCourseId: activeCourseId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = "لینک دوره ی فعال ";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#ActiveCourseLinkForm').data('validator', null);
            $.validator.unobtrusive.parse('#ActiveCourseLinkForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function ActiveCourseLinkFormDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", response.data, "success");
        $("#NormalModal").modal("hide");
    }
    else if (response.status === "Warning") {
        ShowMessage("اعلان", response.data, "warning");
    }
    else if (response.status === "Error") {
        ShowMessage("اعلان", response.data, "error");
        $("#NormalModal").modal("hide");
    }
}

//#endregion

//#region Pricing

function ShowActiveCoursePricingModal(pricingId, activeCourseId) {
    $.ajax({
        url: "/Admin/ActiveCourse/LoadActiveCoursePricingModal",
        type: "post",
        data: {
            pricingId: pricingId,
            activeCourseId: activeCourseId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = pricingId === 0 ? "افزودن قیمت جدید" : "ویرایش قیمت";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#ActiveCoursePricingForm').data('validator', null);
            $.validator.unobtrusive.parse('#ActiveCoursePricingForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function ActiveCoursePricingFormDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", response.data, "success");
        $("#NormalModal").modal("hide");
        $("#ActiveCoursePricing").load(location.href + " #ActiveCoursePricing");
    }
    else if (response.status === "Warning") {
        ShowMessage("اعلان", response.data, "warning");
    }
    else if (response.status === "Error") {
        ShowMessage("اعلان", response.data, "error");
        $("#NormalModal").modal("hide");
    }
}

//#endregion

//#region Active Course Timing Crud

var timingArray = [];

function timeValidation(time) {
    var timeFormat = /([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]/g;

    return timeFormat.test(time);
}

function createGuid() {
    function s4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }

    return (s4().replace("-", "") + s4().replace("-", "") + s4().replace("-", "") + s4().replace("-", ""));
}

function addActiveCourseTiming() {

    //#region check day validation

    var dayId = $("#Day option:selected").val();
    var dayName = $("#Day option:selected").text();

    if (checkNotEmpty(dayId)) {
        ShowMessage("اعلان", "لطفا روز مورد نظر را انتخاب کنید .", "warning");
        return;
    }

    //#endregion

    //#region check start time

    var startTime = $("#StartTime").val();

    if (checkNotEmpty(startTime)) {
        ShowMessage("اعلان", "لطفا ساعت شروع را انتخاب کنید .", "warning");
        return;
    }

    if (!timeValidation(startTime)) {
        ShowMessage("اعلان", "زمان وارد شده معتبر نمی باشد .", "warning");
        return;
    }

    //#endregion

    //#region check end time

    var endTime = $("#EndTime").val();

    if (checkNotEmpty(endTime)) {
        ShowMessage("اعلان", "لطفا ساعت پایان را انتخاب کنید .", "warning");
        return;
    }

    if (!timeValidation(endTime)) {
        ShowMessage("اعلان", "زمان وارد شده معتبر نمی باشد .", "warning");
        return;
    }

    //#endregion

    //#region create object and array

    var timingObject = {
        StartTime: startTime,
        EndTime: endTime,
        DayId: dayId,
        DayName: dayName,
        Id: createGuid()
    }

    timingArray.push(timingObject);

    updateTimingJsonInput();

    //#endregion

    //#region add timing html code

    $("#TimingTable").removeClass("display-none");

    $("#TimingTableTr").append(timingRowHtmlCode(timingObject));

    //#endregion

    emptyTimingInputs();
}

function checkNotEmpty(item) {
    if (item === "" || item === null || item === undefined || !item.length) {
        return true;
    }

    return false;
}

function emptyTimingInputs() {
    $("#Day").val(null);
    $("#StartTime").val(null);
    $("#EndTime").val(null);
}

function deleteTimingRow(timingRowId) {
    if ($(`#${timingRowId}`).length) {
        $(`#${timingRowId}`).remove();
    }

    if ($('tr[name ="AddedTimingRow"]').length === 0) {
        $("#TimingTable").addClass("display-none");
    }

    var timingObject = timingArray.find(function (object) {
        return object.Id.toString() === timingRowId.toString();
    });

    if (timingObject !== undefined) {
        var filteredArray = timingArray.filter(function (value, index, arr) {
            return value.Id.toString() !== timingObject.Id.toString();
        });

        timingArray = filteredArray;
    }

    updateTimingJsonInput();
}

function timingRowHtmlCode(timingObject) {
    var code = `
        <tr class="tc vm" id="${timingObject.Id}" name="AddedTimingRow">
            <td class="tc vm">
                ${timingObject.DayName} از ${timingObject.StartTime} تا ${timingObject.EndTime}
            </td>
            <td class="tc vm">
                <button type="button" onclick="deleteTimingRow('${timingObject.Id}')" class="btn btn-danger btn-circle btn-xs"><i class="glyphicon glyphicon-trash"></i></button>
            </td>
        </tr>
    `;

    return code;
}

function updateTimingJsonInput() {
    var timingArrayJson = JSON.stringify(timingArray);

    $("#SelectedTimingsJson").val(timingArrayJson);
}

function FillClassSelect(branchId) {
    if (branchId === "" || !branchId.length) {
        return;
    }

    $.ajax({
        url: "/Admin/Home/LoadClassesByBranchId",
        type: "post",
        data: {
            branchId: branchId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();

            $('#ClassId').prop("disabled", false);
            $('#ClassId option:not(:first)').remove();

            $.each(response.data, function () {
                $("#ClassId").append(
                    '<option value=' + this.id + '>' + this.title + '</option>'
                );
            });
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

$(function () {

    var timingJson = $("#SelectedTimingsJson").val();

    if (!checkNotEmpty(timingJson)) {
        var selectedTimings = JSON.parse(timingJson);

        timingArray = selectedTimings;

        $("#TimingTable").removeClass("display-none");

        for (var item of selectedTimings) {
            $("#TimingTableTr").append(timingRowHtmlCode(item));
        }
    }

});

//#endregion

//#region Sessions

function ShowAddOrEditSession(sessionId, activeCourseId) {
    $.ajax({
        url: "/Admin/ActiveCourse/LoadActiveCourseSessionModal",
        type: "post",
        data: {
            sessionId: sessionId,
            activeCourseId: activeCourseId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = sessionId === 0 ? "افزودن جلسه جدید" : "ویرایش جلسه";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#ActiveCourseSessionForm').data('validator', null);
            $.validator.unobtrusive.parse('#ActiveCourseSessionForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function ActiveCourseSessionFormDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", response.data, "success");
        $("#NormalModal").modal("hide");
        $("#ActiveCourseSessions").load(location.href + " #ActiveCourseSessions");
    }
    else if (response.status === "Error") {
        ShowMessage("اعلان", response.data, "error");
        $("#NormalModal").modal("hide");
    }
}

//#endregion

//#region Change Register State

function RegisterStateChanged(registerId, stateId) {
    $.ajax({
        url: "/Admin/ActiveCourse/ChangeRegisterState",
        type: "post",
        data: {
            registerId: registerId,
            stateId: stateId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            if (response.status === "Success") {
                ShowMessage("اعلان", response.data, "success");
                $("#ActiveCourseStudents").load(location.href + " #ActiveCourseStudents");
            }
            else {
                ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
            }
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

//#endregion

//#region Delete Discount Use

function DeleteDiscountUse(registerId, userId) {
    Swal.fire({
        title: 'اعلان',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید ؟",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'بله',
        cancelButtonText: 'خیر',
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Admin/ActiveCourse/DeleteDiscountUse",
                type: "post",
                data: {
                    registerId: registerId,
                    userId: userId
                },
                beforeSend: function () {
                    open_waiting();
                },
                success: function (response) {
                    close_waiting();
                    if (response.status === "Success") {
                        ShowMessage("اعلان", response.data, "success");
                        $("#ActiveCourseStudents").load(location.href + " #ActiveCourseStudents");
                    }
                    else {
                        ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
                    }
                },
                error: function () {
                    close_waiting();
                    ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
                }
            });
        }
    });
}

//#endregion

//#region Open Register Description Modal

function OpenRegisterDescriptionModal(registerId) {
    $.ajax({
        url: "/Admin/ActiveCourse/LoadRegisterDescriptionModal",
        type: "post",
        data: {
            registerId: registerId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = "توضیحات ثبت نام";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#RegisterDescriptionForm').data('validator', null);
            $.validator.unobtrusive.parse('#RegisterDescriptionForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function RegisterDescriptionFormDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", response.data, "success");
        $("#NormalModal").modal("hide");
    }
    else if (response.status === "Error") {
        ShowMessage("اعلان", response.data, "error");
        $("#NormalModal").modal("hide");
    }
}

//#endregion

//#region Sessions Files

function ShowSessionFileManagementModal(sessionId) {
    $.ajax({
        url: "/Admin/ActiveCourse/SessionFilesManagement",
        type: "post",
        data: {
            sessionId: sessionId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = "مدیریت جلسات";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#SessionFileForm').data('validator', null);
            $.validator.unobtrusive.parse('#SessionFileForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function SessionFileFormDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", response.data, "success");
        $("#NormalModal").modal("hide");
    }
    else if (response.status === "Error") {
        ShowMessage("اعلان", response.data, "error");
        $("#NormalModal").modal("hide");
    }
}

//#endregion

//#region Sessions Presence

function ShowPresenceStudentsModal(sessionId, activeCourseId) {
    $.ajax({
        url: "/Admin/ActiveCourse/ShowSessionPresenceModal",
        type: "post",
        data: {
            sessionId: sessionId,
            activeCourseId: activeCourseId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = "حضور و غیاب";
            $("#LargeModalTitle").html(modalTitle);
            $("#LargeModalBody").html(response);
            $("#LargeModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function ChangeRegisterPresenceState(registerId, sessionId) {
    $.ajax({
        url: "/Admin/ActiveCourse/ChangeRegisterPresenceStatus",
        type: "post",
        data: {
            registerId: registerId,
            sessionId: sessionId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            if (response.status === "Success") {
                ShowMessage("اعلان", response.data, "success");
            }
            else if (response.status === "Error") {
                ShowMessage("اعلان", response.data, "error");
            }
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

//#endregion

//#region Chat

function ShowChatRoomModal(activeCourseId) {
    $.ajax({
        url: "/Admin/ActiveCourse/ManageChatRoom",
        type: "post",
        data: {
            activeCourseId: activeCourseId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = "مدیریت چت روم";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#ManageChatRoomForm').data('validator', null);
            $.validator.unobtrusive.parse('#ManageChatRoomForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function ManageChatRoomFormDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", response.data, "success");
        $("#NormalModal").modal("hide");
    }
    else if (response.status === "Error") {
        ShowMessage("اعلان", response.data, "error");
        $("#NormalModal").modal("hide");
    }
}

function AddUserCheckBox() {

    var userId = $("#User-Input").val();
    var userTitle = $("#User-Display").val();

    if (!$(`label[data-id='${userId}']`).length) {
        $("#SelectedUsersBox").append(`
            <div class="form-group">
                <div class="checkbox">
                    <label data-id="${userId}">
                        <input checked="checked" value="${userId}" type="checkbox" name="SelectedUsers">
                        <span class="text"> ${userTitle} </span>
                    </label>
                </div>
            </div>
        `);
    }

    $('.modal').css("overflow-y", "auto");
}

//#endregion

//#region Add Price Modal

function OpenAddPriceModal(registerId) {
    $.ajax({
        url: "/Admin/ActiveCourse/LoadAddPriceModal",
        type: "post",
        data: {
            registerId: registerId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = "پرداخت شهریه";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#AddPriceForm').data('validator', null);
            $.validator.unobtrusive.parse('#AddPriceForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function AddPriceFormDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", response.data, "success");
        $("#NormalModal").modal("hide");
        $("#ActiveCourseStudents").load(location.href + " #ActiveCourseStudents");
    }
    else if (response.status === "Error") {
        ShowMessage("اعلان", response.data, "error");
        $("#NormalModal").modal("hide");
    }
}

//#endregion

//#region Register Payment List

function OpenRegisterPayedList(registerId) {
    $.ajax({
        url: "/Admin/ActiveCourse/StudentsPaymentList",
        type: "post",
        data: {
            registerId: registerId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = "لیست پرداختی ها";
            $("#LargeModalTitle").html(modalTitle);
            $("#LargeModalBody").html(response);
            $("#LargeModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

//#endregion

//#region Open Online Class Info Modal

function OpenOnlineClassInfoModal(registerId) {
    $.ajax({
        url: "/Admin/ActiveCourse/StudentOnlineClassInfo",
        type: "post",
        data: {
            registerId: registerId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = "اطلاعات کلاس آنلاین";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

//#endregion

//#region Change User Access To Sessions

function ChangeUserAccessToSessions(registerId) {
    Swal.fire({
        title: 'اعلان',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید ؟",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'بله',
        cancelButtonText: 'خیر',
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Admin/ActiveCourse/ChnageStudentAccessToSession",
                type: "post",
                data: {
                    registerId: registerId
                },
                beforeSend: function () {
                    open_waiting();
                },
                success: function (response) {
                    close_waiting();
                    if (response.status === "Success") {
                        ShowMessage("اعلان", response.data, "success");
                        $("#ActiveCourseStudents").load(location.href + " #ActiveCourseStudents");
                    }
                    else {
                        ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
                    }
                },
                error: function () {
                    close_waiting();
                    ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
                }
            });
        }
    });
}

//#endregion

//#region Transfer Student

function TransferStudent(registerId) {
    $.ajax({
        url: "/Admin/ActiveCourse/TransferStudent",
        type: "post",
        data: {
            registerId: registerId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = "انتقال دانشجو";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#TransferStudentForm').data('validator', null);
            $.validator.unobtrusive.parse('#TransferStudentForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function TransferStudentFormDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", response.data, "success");
        $("#NormalModal").modal("hide");
        $("#ActiveCourseStudents").load(location.href + " #ActiveCourseStudents");
    }
    else if (response.status === "Error") {
        ShowMessage("اعلان", response.data, "error");
        $("#NormalModal").modal("hide");
    }
}

function ActiveCourseInputChanged() {
    var activeCourseId = $("#ActiveCourse-Input").val();

    $.ajax({
        url: "/Admin/ActiveCourse/GetPricingSelectList",
        type: "post",
        data: {
            activeCourseId: activeCourseId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            $("#PricingType").empty();
            for (var price of response.data) {
                $("#PricingType").append(`<option value=${price.id}>${price.title}</option>`);
            }
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

//#endregion

//#region Students Numbers

function CopyStudentsNumbers() {
    if ($("#NumbersTabContent .active").length) {
        var $temp = $("<input>");
        $("body").append($temp);
        var el = $("#NumbersTabContent .active div div p");
        $temp.val($(el).text()).select();
        document.execCommand("copy");
        $temp.remove();
        ShowMessage('عملیات موفق', 'اطلاعات مورد نظر با موفقیت کپی شد');
    }
}

//#endregion