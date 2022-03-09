//#region Branch Master

function ShowBranchMasterModal(branchId, masterId) {
    $.ajax({
        url: "/Admin/Branch/LoadBranchMasterModal",
        type: "post",
        data: {
            branchId: branchId,
            masterId: masterId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = masterId === 0 ? "افزودن مدرس جدید" : "ویرایش مدرس";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#BranchMasterForm').data('validator', null);
            $.validator.unobtrusive.parse('#BranchMasterForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function BranchMasterFormDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", response.data, "success");
        $("#NormalModal").modal("hide");
        $("#BranchMasters").load(location.href + " #BranchMasters");
    }
    else {
        ShowMessage("اعلان", response.data, "error");
    }
}

//#endregion

//#region Branch Manager

function ShowBranchManagerModal(branchId, managerId) {
    $.ajax({
        url: "/Admin/Branch/LoadBranchManagerModal",
        type: "post",
        data: {
            branchId: branchId,
            managerId: managerId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = managerId === 0 ? "افزودن مدیر جدید" : "ویرایش مدیر";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#BranchManagerForm').data('validator', null);
            $.validator.unobtrusive.parse('#BranchManagerForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function BranchManagerFormDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", response.data, "success");
        $("#NormalModal").modal("hide");
        $("#BranchManagers").load(location.href + " #BranchManagers");
    }
    else {
        ShowMessage("اعلان", response.data, "error");
    }
}

//#endregion

//#region Branch Class

function ShowBranchClassModal(branchId, classId) {
    $.ajax({
        url: "/Admin/Branch/LoadBranchClassModal",
        type: "post",
        data: {
            branchId: branchId,
            classId: classId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = classId === 0 ? "افزودن کلاس جدید" : "ویرایش کلاس";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#BranchClassForm').data('validator', null);
            $.validator.unobtrusive.parse('#BranchClassForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function BranchClassFormDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", "عملیات با موفقیت انجام شد .", "success");
        $("#NormalModal").modal("hide");
        $("#BranchClasses").load(location.href + " #BranchClasses");
    }
    else {
        ShowMessage("اعلان", "خطایی رخ داده است لطفا مجدد تلاش کنید .", "error");
    }
}

//#endregion