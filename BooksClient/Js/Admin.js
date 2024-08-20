const apiStart = 'https://localhost:7291/api';
let currentPage = 1; // Start from the first page
const pageSize = 100; // Number of authors per page
$(document).ready(function () {

    getFromServer();
});

function getFromServer() {

    // Define the API URLs
    const booksApi = `${apiStart}/Books/get10BooksPerPage/${currentPage}/${pageSize}`;
    const authorsApi = `${apiStart}/Authors/get10AuthorsPerPage/${currentPage}/${pageSize}`;
    const usersApi = `${apiStart}/Users`;
    // Fetch Books 
    ajaxCall("GET", booksApi, "", function (books) {
        renderBooksTable('#booksTable', books, "Books");
    }, getBooksECB);



    //// Fetch Authors
    ajaxCall("GET", authorsApi, "", function (authors) {
        console.log("Authors:", authors);
        renderAuthorssTable('#authorsTable', authors, "Authors");
    }, getBooksECB);

    //// Fetch Books Purchased
    ajaxCall("GET", usersApi, "", function (users) {
        console.log("Users:", users);
        renderUsersTable('#usersTable', users, "Users");
    }, getBooksECB);
}

function getBooksECB(err) {
    console.log(err);
}

function renderBooksTable(tableId, tableData, title) {
    try {
        if (!$.fn.DataTable.isDataTable(tableId)) {
            $(tableId).DataTable({
                data: tableData,
                drawCallback: function () {
                    var table = this.api();
                    var pageInfo = table.page.info();
                    var pages = pageInfo.pages;

                    $('.paginate_button.next:not(.disabled)', table.table().container())
                        .on('click', function () {
                            currentPage++;
                            getFromServer(currentPage, pageSize);
                        });

                    $('.paginate_button.previous:not(.disabled)', table.table().container())
                        .on('click', function () {
                            if (currentPage > 1) {
                                currentPage--;
                                getFromServer(currentPage, pageSize);
                            }
                        });
                },
                columns: [

                    {
                        data: "smallThumbnailUrl",
                        render: function (data, type, row, meta) {
                            return '<img src="' + data + '" alt="Book Thumbnail" style="width: 50px; height: auto;">';
                        },
                        title: "Cover"
                    },
                    {
                        data: "id",
                        title: "ID"
                    },
                    {
                        data: "title",
                        title: "Title"
                    },
                    {
                        data: "price",
                        title: "Price"
                    },
                    {
                        data: "language",
                        title: "Language"
                    },
                    {
                        data: "isEbook",
                        render: function (data, type, row, meta) {
                            return data === 'ebook' ? 'eBook' : 'Physical';
                        },
                        title: "Type"
                    },
                    {
                        data: "isActive",
                        render: function (data, type, row, meta) {
                            return data ? 'Yes' : 'No';
                        },
                        title: "Active"
                    },
                    {
                        data: "isAvailable",
                        render: function (data, type, row, meta) {
                            return data ? 'Yes' : 'No';
                        },
                        title: "Available"
                    },
                    {
                        data: "numOfPrints",
                        title: "Prints"
                    },

                    {
                        data: null,
                        defaultContent: '<button class="btn DescriptionModalBtn">Description</button>',
                        title: "Description",
                        orderable: false
                    },
                    {
                        data: null,
                        defaultContent: '<button class="btn ChangeBookValuesModalBtn">Edit</button>',
                        title: "Change Values",
                        orderable: false
                    },
                ],
                columnDefs: [{
                    targets: 0,
                    orderable: false
                }]
            });

        } else {
            $(tableId).DataTable().clear().rows.add(tableData).draw();
        }
    } catch (err) {
        alert(err);
    }
}


$(document).on('click', 'button.DescriptionModalBtn', function () {///////////////// show books description
    var table = $('#booksTable').DataTable();
    var rowData = table.row($(this).parents('tr')).data();
    $('#modalLabel').text('Details for ID: ' + rowData.id); 
    $('#modalContent').html('<p><strong>Title:</strong> ' + rowData.title + '</p>' +
        '<p><strong>Prints:</strong> ' + rowData.description + '</p>');
    $('#actionModal').modal('show');
});



$(document).on('click', 'button.ChangeBookValuesModalBtn', function () {
    var table = $('#booksTable').DataTable();
    var rowData = table.row($(this).parents('tr')).data();

    // Update the modal label with the selected row's ID
    $('#modalLabel').text('Details for ID: ' + rowData.id);

    $('#modalContent').html(
        '<div class="form-group">' +
        '<label for="isActiveInput">Is Active:</label>' +
        '<input type="checkbox" class="form-control" id="isActiveInput" ' + (rowData.isActive ? 'checked' : '') + '>' +
        '</div>' +
        '<div class="form-group">' +
        '<label for="priceInput">Price:</label>' +
        '<input type="text" class="form-control" id="priceInput" value="' + rowData.price + '">' +
        '</div>' +
        '<div class="form-group">' +
        '<label for="printsInput">Number of Prints:</label>' +
        '<input type="text" class="form-control" id="printsInput" value="' + rowData.numOfPrints + '">' +
        '</div>' +
        '<button id="saveBookChangesBtn" class="btn btn-primary">Save</button>'
    );

    // Show the modal
    $('#actionModal').modal('show');
});

$(document).on('click', '#saveBookChangesBtn', function () {
    var bookId = $('#modalLabel').text().match(/\d+/)[0];
    var updatedIsActive = $('#isActiveInput').is(':checked');
    var updatedPrice = $('#priceInput').val();
    var updatedPrints = $('#printsInput').val();
    bookId = parseInt(bookId);

    putBooksChangesToServer(bookId, updatedIsActive, updatedPrice, updatedPrints);

    // Close the modal after saving changes (if applicable)
    $('#actionModal').modal('hide');
});


function putBooksChangesToServer(bookId, isActive,price,prints) {
    console.log("Fetching authors from server");
    let api = `${apiStart}/Books/UpdateBookValuesById/book Id/${bookId}/Is Active/${isActive}/price/${price}/NumberOfPrints/${prints}`;
    ajaxCall("PUT", api, "", updateBooksSCB, updateBooksECB);
}

function updateBooksSCB(status) {
    $('#booksTable').DataTable().clear();
    getFromServer();
}

function updateBooksECB(err) {
    console.log(err);
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function renderAuthorssTable(tableId, tableData, title) {
    console.log("data To Read:", tableData);
    try {
        if (!$.fn.DataTable.isDataTable(tableId)) {
            $(tableId).DataTable({
                data: tableData,
                drawCallback: function () {
                    var table = this.api();
                    var pageInfo = table.page.info();

                    $('.paginate_button.next:not(.disabled)', table.table().container())
                        .on('click', function () {
                            var nextPageIndex = pageInfo.page;
                            currentPage++;
                            $('#numOfPages').text("Page " + currentPage);
                            getFromServer(currentPage, pageSize);
                        });

                    // Event handler for page number buttons
                    $('.paginate_button:not(.next, .previous)', table.table().container())
                        .on('click', function () {
                            var clickedPageIndex = $(this).text() - 1; // Adjust to zero-based index

                        });

                    // Event handler for the "Previous" button
                    $('.paginate_button.previous:not(.disabled)', table.table().container())
                        .on('click', function () {
                            if (currentPage > 1) {
                                currentPage--;
                                $('#numOfPages').text("Page " + currentPage);
                                getFromServer(currentPage, pageSize);
                            }
                        });
                },
                columns: [
                    {
                        data: "image",
                        render: function (data, type, row, meta) {
                            return '<img src="' + data + '" alt="Book Thumbnail" style="width: 50px; height: auto;">';
                        },
                        title: "Cover"
                    },
                    {
                        data: "name",
                        title: "Name"
                    },
                    {
                        data: "topWork",
                        title: "Top Work"
                    },
                    {
                        data: "workCount",
                        title: "Work Count"
                    },
                    {
                        data: "description",
                        title: "Description"
                    }, 

                ],
            });

            //// Bind button events for the newly rendered table
            //buttonEvents(tableId);
        } else {
            // Update existing DataTable if needed
            $(tableId).DataTable().clear().rows.add(tableData).draw();
        }
    } catch (err) {
        alert(err);
    }
}



///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function renderUsersTable(tableId, tableData, title) {
    console.log("data To Read:", tableData);
    try {
        if (!$.fn.DataTable.isDataTable(tableId)) {
            $(tableId).DataTable({
                data: tableData,
                drawCallback: function () {
                    var table = this.api();
                    var pageInfo = table.page.info();

                    $('.paginate_button.next:not(.disabled)', table.table().container())
                        .on('click', function () {
                            var nextPageIndex = pageInfo.page;
                            currentPage++;
                            $('#numOfPages').text("Page " + currentPage);
                            getFromServer(currentPage, pageSize);
                        });

                    // Event handler for page number buttons
                    $('.paginate_button:not(.next, .previous)', table.table().container())
                        .on('click', function () {
                            var clickedPageIndex = $(this).text() - 1; // Adjust to zero-based index

                        });

                    // Event handler for the "Previous" button
                    $('.paginate_button.previous:not(.disabled)', table.table().container())
                        .on('click', function () {
                            if (currentPage > 1) {
                                currentPage--;
                                $('#numOfPages').text("Page " + currentPage);
                                getFromServer(currentPage, pageSize);
                            }
                        });
                },
                columns: [
                    {
                        data: "id",
                        title: "ID"
                    },
                    {
                        data: "email",
                        title: "Email"
                    },
                    {
                        data: "name",
                        title: "Name"
                    },
                    {
                        data: "isActive",
                        title: "Is Active"
                    },
                    {
                        data: null,
                        defaultContent: '<button class="btn ChangeUserStatusModalBtn">Edit</button>',
                        title: "Change Values",
                        orderable: false
                    }, 
                ],
            });

            //// Bind button events for the newly rendered table
            //buttonEvents(tableId);
        } else {
            // Update existing DataTable if needed
            $(tableId).DataTable().clear().rows.add(tableData).draw();
        }
    } catch (err) {
        alert(err);
    }
}
$(document).on('click', 'button.ChangeUserStatusModalBtn', function () {
    var table = $('#usersTable').DataTable();
    var rowData = table.row($(this).parents('tr')).data();

    // Update the modal label with the selected row's ID
    $('#modalLabel').text('Details for ID: ' + rowData.id);

    $('#modalContent').html(
        '<div class="form-group">' +
        '<label for="isActiveInput">Is Active:</label>' +
        '<input type="checkbox" class="form-control" id="isActiveInput" ' + (rowData.isActive ? 'checked' : '') + '>' +
        '</div>' +
        '<button id="saveUserChangesBtn" class="btn btn-primary">Save</button>'
    );

    // Show the modal
    $('#actionModal').modal('show');
});

$(document).on('click', '#saveUserChangesBtn', function () {
    var userId = $('#modalLabel').text().match(/\d+/)[0];
    var updatedIsActive = $('#isActiveInput').is(':checked');
    bookId = parseInt(userId);

    putUserChangesToServer(userId, updatedIsActive);

    // Close the modal after saving changes (if applicable)
    $('#actionModal').modal('hide');
});


function putUserChangesToServer(userId, isActive) {
    console.log("Fetching authors from server");
    let api = `${apiStart}/Users/UpdateUserValuesById/user Id/${userId}/Is Active/${isActive}`;
    ajaxCall("PUT", api, "", updateUsersSCB, updateUsersECB);
}

function updateUsersSCB(status) {
    $('#usersTable').DataTable().clear();
    getFromServer();
}

function updateUsersECB(err) {
    console.log(err);
}