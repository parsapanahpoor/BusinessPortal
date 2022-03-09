//#region Edit User Wallet

function ShowEditUserWalletModal(walletId) {
    $.ajax({
        url: "/Admin/UserWallet/EditUserWallet",
        type: "Get",
        data: {
            walletId: walletId
        },
        beforeSend: function () {
            open_waiting();
        },
        success: function (response) {
            close_waiting();
            var modalTitle = "ویرایش تراکنش";
            $("#NormalModalTitle").html(modalTitle);
            $("#NormalModalBody").html(response);
            $('#EditUserWalletForm').data('validator', null);
            $.validator.unobtrusive.parse('#EditUserWalletForm');
            $("#NormalModal").modal("show");
        },
        error: function () {
            close_waiting();
            ShowMessage("خطا", "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .", "error");
        }
    });
}

function EditUserWalletDone(response) {
    close_waiting();
    if (response.status === "Success") {
        ShowMessage("اعلان", "عملیات با موفقیت انجام شد", "success");
        $("#NormalModal").modal("hide");
        location.reload();
    }
    else {
        ShowMessage("اعلان", response.data, "error");
    }
}

//#endregion