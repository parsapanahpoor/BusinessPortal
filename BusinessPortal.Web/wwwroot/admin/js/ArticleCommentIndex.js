function ChangeReadStatusArticleCommentAjax(removeElementId, url) {
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
            $.get(url).then(res => {
                if (removeElementId !== null &&
                    removeElementId !== undefined &&
                    removeElementId !== '' &&
                    res.status === "Success") {
                    $('[remove-ajax-item=' + removeElementId + ']').removeClass('mr-8px label label-orange').addClass('mr-8px label label-success').html('خوانده شده');
                    location.reload();
                } else if (removeElementId !== null &&
                    removeElementId !== undefined &&
                    removeElementId !== '' &&
                    res.status === "Error") {
                    ShowMessage("اعلان", "عملیات با شکست مواجه شد", "error");
                }
            });
        }
    });
}


function ChangeArticleCommentDeleteStatus(removeElementId, url) {
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
            $.get(url).then(res => {
                if (removeElementId !== null &&
                    removeElementId !== undefined &&
                    removeElementId !== '' &&
                    res.status === "Success") {
                    location.reload();
                } else if (removeElementId !== null &&
                    removeElementId !== undefined &&
                    removeElementId !== '' &&
                    res.status === "Error") {
                    ShowMessage("اعلان", "عملیات با شکست مواجه شد", "error");
                }
            });
        }
    });
}

function LoadArticleCommentModalBody(ArcId) {
    $.ajax({
        url: "/Show-ArticleComment-Detail",
        type: "get",
        data: {
            articleCommentId: ArcId
        },
        success: function (response) {
            $("#NormalModalBody").html(response);

            $('#ArticleForm').data('validator', null);
            $.validator.unobtrusive.parse('#ArticleForm');

            $("#NormalModal").modal("show");
        }
    });
}

function ArticleFormSubmited(response) {
    if (response.status === "Success") {
        $("#NormalModal").modal("hide");
        $("#CommentDiv").load(location.href + " #CommentDiv");
        ShowMessage("اعلان", "عملیات با موفقیت انجام شده است ", "success");
    } else {
        ShowMessage("اعلان", "عملیات با شکست مواجه شد", "error");
    }
}


