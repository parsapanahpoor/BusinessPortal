﻿//#region Document Ready

$(function () {
    if ($("input[name=Permissions]").length === $("input[name=Permissions]:checked").length) {
        $("#SelectAll").prop("checked", true);
    }
});

//#endregion

//#region Select All Button

$("#SelectAll").click(function (e) {
    if (this.checked) {
        $(".accordion-toggle").each(function (index, value) {
            if ($(value).hasClass("collapsed")) {
                $(value).removeClass("collapsed");
            }
        });
        $(".panel-collapse").each(function (index, value) {
            if (!$(value).hasClass("in")) {
                $(value).addClass("in");
            }
        });
        $(".accordion-toggle").attr("aria-expanded", true);
        $(".panel-collapse").attr("aria-expanded", true);
        $(".panel-collapse").css("height", "auto");
        $("input[name=Permissions]").prop("checked", true);
    } else {
        $(".accordion-toggle").each(function (index, value) {
            if (!$(value).hasClass("collapsed")) {
                $(value).addClass("collapsed");
            }
        });
        $(".panel-collapse").each(function (index, value) {
            if ($(value).hasClass("in")) {
                $(value).removeClass("in");
            }
        });
        $(".accordion-toggle").attr("aria-expanded", false);
        $(".panel-collapse").attr("aria-expanded", false);
        $(".panel-collapse").css("height", "0");
        $("input[name=Permissions]").prop("checked", false);
    }
});

//#endregion

//#region Check Box Click

$("input[name=Permissions]").click(function (e) {
    var id = $(this).attr("data-id");
    var parentId = $(this).attr("data-parentId");
    if (this.checked) {
        if (parentId === undefined) {
            $(`input[data-parentId=${id}]`).each(function (index, value) {
                $(this).prop("checked", true);
            });
        } else {
            $(`input[data-id=${parentId}]`).prop("checked", true);
        }
    } else {
        if (parentId === undefined) {
            $(`input[data-parentId=${id}]`).each(function (index, value) {
                $(this).prop("checked", false);
            });
        } else {
            if ($(`input[data-parentId=${parentId}]:checked`).length < 1) {
                $(`input[data-id=${parentId}]`).prop("checked", false);
            }
        }
        $("#SelectAll").prop("checked", false);
    }
});

//#endregion



