
//#region Common

var coursesArray = [];
var activeCoursesArray = [];
var usersArray = [];

function createGuid() {
    function s4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }

    return (s4().replace("-", "") + s4().replace("-", "") + s4().replace("-", "") + s4().replace("-", ""));
}

function checkNotEmpty(item) {
    if (item === "" || item === null || item === undefined || !item.length) {
        return true;
    }

    return false;
}

//#endregion

//#region Course Search

$("#Course-Display").on("change", function () {

    var courseId = $("#Course-Input").val();
    var courseTitle = $("#Course-Display").val();

    if (checkNotEmpty(courseId) || checkNotEmpty(courseTitle)) {
        return;
    }

    if ($(`#Course_${courseId}`).length) {
        ShowMessage("اعلان", "دوره مورد نظر از قبل انتخاب شده است .", "warning");

        return;
    }

    var courseObject = {
        Id: courseId,
        Title: courseTitle
    }

    coursesArray.push(courseObject);

    $("#CoursesTableDiv").removeClass("display-none");

    $("#CoursesTableBody").append(courseRowHtmlCode(courseObject));

    updateCoursesJsonInput();

});

function courseRowHtmlCode(courseObject) {
    var code = `
        <tr class="tc vm" id="Course_${courseObject.Id}" name="AddedCourseRow">
            <td class="tc vm ellipsis-style" style="max-width: 0">
                ${courseObject.Title}
            </td>
            <td class="tc vm">
                <button type="button" onclick="deleteCoursesRow('${courseObject.Id}')" class="btn btn-danger btn-circle btn-xs"><i class="glyphicon glyphicon-trash"></i></button>
            </td>
        </tr>
    `;

    return code;
}

function updateCoursesJsonInput() {
    var coursesArrayJson = JSON.stringify(coursesArray);

    $("#SelectedCoursesJson").val(coursesArrayJson);
}

function deleteCoursesRow(courseRowId) {
    if ($(`#Course_${courseRowId}`).length) {
        $(`#Course_${courseRowId}`).remove();
    }

    if ($('tr[name ="AddedCourseRow"]').length === 0) {
        $("#CoursesTableDiv").addClass("display-none");
    }

    var courseObject = coursesArray.find(function (object) {
        return object.Id.toString() === courseRowId.toString();
    });

    if (courseObject !== undefined) {
        var filteredArray = coursesArray.filter(function (value, index, arr) {
            return value.Id.toString() !== courseObject.Id.toString();
        });

        coursesArray = filteredArray;
    }

    updateCoursesJsonInput();
}

//#endregion

//#region User Search

$("#User-Display").on("change", function () {

    var userId = $("#User-Input").val();
    var userTitle = $("#User-Display").val();

    console.log(userId);
    console.log(userTitle);

    if (checkNotEmpty(userId) || checkNotEmpty(userTitle)) {
        return;
    }

    if ($(`#User_${userId}`).length) {
        ShowMessage("اعلان", "کاربر مورد نظر از قبل انتخاب شده است .", "warning");

        return;
    }

    var userObject = {
        Id: userId,
        Title: userTitle
    }

    usersArray.push(userObject);

    $("#UserTableDiv").removeClass("display-none");

    $("#UserTableBody").append(userRowHtmlCode(userObject));

    updateUsersJsonInput();

});

function userRowHtmlCode(userObject) {
    var code = `
        <tr class="tc vm" id="User_${userObject.Id}" name="AddedUserRow">
            <td class="tc vm ellipsis-style" style="max-width: 0">
                ${userObject.Title}
            </td>
            <td class="tc vm">
                <button type="button" onclick="deleteUsersRow('${userObject.Id}')" class="btn btn-danger btn-circle btn-xs"><i class="glyphicon glyphicon-trash"></i></button>
            </td>
        </tr>
    `;

    return code;
}

function updateUsersJsonInput() {
    var usersArrayJson = JSON.stringify(usersArray);

    $("#SelectedUsersJson").val(usersArrayJson);
}

function deleteUsersRow(userRowId) {
    if ($(`#User_${userRowId}`).length) {
        $(`#User_${userRowId}`).remove();
    }

    if ($('tr[name ="AddedUserRow"]').length === 0) {
        $("#UserTableDiv").addClass("display-none");
    }

    var userObject = usersArray.find(function (object) {
        return object.Id.toString() === userRowId.toString();
    });

    if (userObject !== undefined) {
        var filteredArray = usersArray.filter(function (value, index, arr) {
            return value.Id.toString() !== userObject.Id.toString();
        });

        usersArray = filteredArray;
    }

    updateUsersJsonInput();
}

//#endregion

//#region Course Search

$("#ActiveCourse-Display").on("change", function () {

    var activeCourseId = $("#ActiveCourse-Input").val();
    var activeCourseTitle = $("#ActiveCourse-Display").val();

    if (checkNotEmpty(activeCourseId) || checkNotEmpty(activeCourseTitle)) {
        return;
    }

    if ($(`#ActiveCourse_${activeCourseId}`).length) {
        ShowMessage("اعلان", "دوره فعال مورد نظر از قبل انتخاب شده است .", "warning");

        return;
    }

    var activeCourseObject = {
        Id: activeCourseId,
        Title: activeCourseTitle
    }

    activeCoursesArray.push(activeCourseObject);

    $("#ActiveCoursesTableDiv").removeClass("display-none");

    $("#ActiveCoursesTableBody").append(activeCourseRowHtmlCode(activeCourseObject));

    updateActiveCoursesJsonInput();

});

function activeCourseRowHtmlCode(activeCourseObject) {
    var code = `
        <tr class="tc vm" id="ActiveCourse_${activeCourseObject.Id}" name="AddedActiveCourseRow">
            <td class="tc vm ellipsis-style" style="max-width: 0">
                ${activeCourseObject.Title}
            </td>
            <td class="tc vm">
                <button type="button" onclick="deleteActiveCoursesRow('${activeCourseObject.Id}')" class="btn btn-danger btn-circle btn-xs"><i class="glyphicon glyphicon-trash"></i></button>
            </td>
        </tr>
    `;

    return code;
}

function updateActiveCoursesJsonInput() {
    var activeCoursesArrayJson = JSON.stringify(activeCoursesArray);

    $("#SelectedActiveCoursesJson").val(activeCoursesArrayJson);
}

function deleteActiveCoursesRow(activeCourseRowId) {
    if ($(`#ActiveCourse_${activeCourseRowId}`).length) {
        $(`#ActiveCourse_${activeCourseRowId}`).remove();
    }

    if ($('tr[name ="AddedActiveCourseRow"]').length === 0) {
        $("#ActiveCoursesTableDiv").addClass("display-none");
    }

    var activeCourseObject = activeCoursesArray.find(function (object) {
        return object.Id.toString() === activeCourseRowId.toString();
    });

    if (activeCourseObject !== undefined) {
        var filteredArray = activeCoursesArray.filter(function (value, index, arr) {
            return value.Id.toString() !== activeCourseObject.Id.toString();
        });

        activeCoursesArray = filteredArray;
    }

    updateActiveCoursesJsonInput();
}

//#endregion

//#region Document Ready

$(function () {

    var coursesJson = $("#SelectedCoursesJson").val();
    var activeCoursesJson = $("#SelectedActiveCoursesJson").val();
    var usersJson = $("#SelectedUsersJson").val();

    if (!checkNotEmpty(coursesJson)) {
        var selectedCourses = JSON.parse(coursesJson);

        coursesArray = selectedCourses;

        $("#CoursesTableDiv").removeClass("display-none");

        for (var item of selectedCourses) {
            $("#CoursesTableBody").append(courseRowHtmlCode(item));
        }
    }

    if (!checkNotEmpty(usersJson)) {
        var selectedUsers = JSON.parse(usersJson);

        usersArray = selectedUsers;

        $("#UserTableDiv").removeClass("display-none");

        for (var item of selectedUsers) {
            $("#UserTableBody").append(userRowHtmlCode(item));
        }
    }

    if (!checkNotEmpty(activeCoursesJson)) {
        var selectedActiveCourses = JSON.parse(activeCoursesJson);

        activeCoursesArray = selectedActiveCourses;

        $("#ActiveCoursesTableDiv").removeClass("display-none");

        for (var item of selectedActiveCourses) {
            $("#ActiveCoursesTableBody").append(activeCourseRowHtmlCode(item));
        }
    }
});

//#endregion