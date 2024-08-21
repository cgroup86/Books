const apiStart = 'https://proj.ruppin.ac.il/cgroup86/test2/tar1/api';
let currentPage = 1; 
const pageSize = 10; 
const array = []; 

$(document).ready(function () {
    localStorage.clear();
    getFromServer(currentPage, pageSize);

    $('#prevPageBtn').click(function () {
        if (currentPage > 1) {
            currentPage--;
            $('#numOfPages').text("Page " + currentPage);
            getFromServer(currentPage, pageSize);
        }
    });

    $('#nextPageBtn').click(function () {
        currentPage++;
        $('#numOfPages').text("Page " + currentPage);
        getFromServer(currentPage, pageSize);
    });

    $(document).on('click', '.btn-close', function () {
        $('#authorModal').modal('hide');
    });

    $(window).click(function (event) {
        if ($(event.target).is('#authorModal')) {
            $('#authorModal').modal('hide');
        }
    });
});

function getFromServer(page, pageSize) {
    console.log("Fetching authors from server");
    let api = `${apiStart}/Authors/get10AuthorsPerPage/${page}/${pageSize}`;
    ajaxCall("GET", api, "", getAuthorsSCB, getAuthorsECB);
}

function getAuthorsSCB(authors) {
    console.log("Authors received:", authors);
    if (Array.isArray(authors)) {
        renderAuthors(authors);
    } else {
        console.error("Expected an array but got:", authors);
    }

    $('#prevPageBtn').prop('disabled', currentPage === 1);

    if (authors.length < pageSize) {
        $('#nextPageBtn').prop('disabled', true); 
    } else {
        $('#nextPageBtn').prop('disabled', false); 
    }

    array.length = 0;
    array.push(...authors);
}

function getAuthorsECB(err) {
    console.log(err);
}

function renderAuthors(authors) {
    const instructorsList = $('#Authors-list');
    instructorsList.empty();
    authors.forEach(function (author) {
        const authorBox = `
        <li class="item" draggable="true" data-author-name="${author.name}">
            <div class="details">
                <img src="${author.image || 'default-image-url'}" alt="${author.name || 'No Name'}">
                <p><strong>${author.name || 'No Name'}</strong></p>
            </div>
            <i class="uil uil-draggabledots"></i>
            <button class="show-Author-Books-button">Show books</button>
        </li>
        `;
        instructorsList.append(authorBox);
    });


    $(".item").click(function () {
        const authorName = $(this).data("author-name");
        fetchAuthorDetails(array, authorName);
    });

    $(".show-Author-Books-button").click(function (event) {
        event.stopPropagation();
        const authorName = $(this).closest(".item").data("author-name");
        console.log(authorName);
        window.location.href = `AuthorsBooks.html?authorName=${encodeURIComponent(authorName)}`;
    });

}

function fetchAuthorDetails(List, name) {
    const author = List.find(a => a.name === name);
    if (author) {
        showModal(author);
    }
}


function showModal(author) {
    $("#modal-author-name").text(author.name || 'No Name');
    $("#modal-author-image").attr("src", author.image || 'default-image-url');
    $("#modal-author-bio").text(author.description || 'No Bio Available');
    $("#modal-author-topWork").text(`Top Work: ${author.topWork || 'No Top Work'}`);
    $("#modal-author-workCount").text(`Work Count: ${author.workCount || 'No Work Count'}`);
    $("#authorModal").modal('show');
}