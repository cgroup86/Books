$(document).ready(function () {
    const apiStart = `https://localhost:7291/api/`;
    getInstructorsFromServer(apiStart);
 
});

function getInstructorsFromServer(apiStart) {/// Get Authors List From Server
    let api = `${apiStart}Authors`;
    $.ajax({
        url: api,
        method: "GET",
        success: getAuthorsSCB,
        error: getAuthorsECB
    });
}

function getAuthorsSCB(instructorsList) {
    console.log(instructorsList);
    displayInstructors(instructorsList);
}

function getAuthorsECB(xhr, status, error) {
    console.log(error);
}
