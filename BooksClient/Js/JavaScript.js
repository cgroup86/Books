$(document).ready(function () {
    getFromServer();
});

function getFromServer() {
    const booksApi = `https://localhost:7291/api/Books/get10BooksPerPage/1/10`;

    ajaxCall("GET", booksApi, "", function (data) {
        // Initialize the array
        console.log("data", data);
        //let arr = [];

        //// Check if data is an array
        //if (Array.isArray(data)) {
        //    // Add each item to the array
        //    for (var i = 0; i < data.length; i++) {
        //        arr.push(data[i]);
        //    }
        //    // Render the table with the fetched data
        //    renderTable('#booksTable', arr);
        //} else {
        //    console.error("Data is not an array:", data);
        //}

    }, getBooksECB);
}
//if (Array.isArray(data)) {
//  renderTable('#booksTable', data);
//} else {
//    console.error("Data is not an array:", data);
//}
function getBooksECB(err) {
    console.error("Error fetching books:", err);
}

function renderTable(tableId, data) {
    console.log("Rendering table with data:", data);

    if ($.fn.DataTable.isDataTable(tableId)) {
        $(tableId).DataTable().clear().rows.add(data).draw();
    } else {
        $(tableId).DataTable({
            data: data,
            pageLength: 5,
            columns: [
                {
                    data: "smallThumbnailUrl",
                    render: function (data) {
                        return '<img src="' + data + '" alt="Book Thumbnail" style="width: 50px; height: auto;">';
                    },
                    title: "Cover"
                },
                {
                    data: "bookTitle",
                    title: "Title"
                },
                {
                    data: "price",
                    title: "Price"
                },
                {
                    data: "lang",
                    title: "Language"
                },
                {
                    data: "isEbook",
                    render: function (data) {
                        return data === 'ebook' ? 'eBook' : 'Physical';
                    },
                    title: "Type"
                },
                {
                    data: "isActive",
                    render: function (data) {
                        return data ? 'Yes' : 'No';
                    },
                    title: "Active"
                },
                {
                    data: "isAvailable",
                    render: function (data) {
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
                    render: function (data, type, row) {
                        return `
                            <button type="button" class="btn btn-warning" onclick="editBook('${row.bookId}')">Edit</button>
                            <button type="button" class="btn btn-danger" onclick="deleteBook('${row.bookId}')">Delete</button>
                        `;
                    },
                    title: "Actions"
                }
            ]
        });
    }
}

function editBook(bookId) {
    console.log("Edit book with ID:", bookId);
    // Implement edit functionality here
}

function deleteBook(bookId) {
    console.log("Delete book with ID:", bookId);
    // Implement delete functionality here
}






