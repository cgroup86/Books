let booksToReadData = [];
let booksReadData = [];
let booksPurchasedData = [];

$(document).ready(function() {
  getFromServer(); 
});

function getFromServer() {
  console.log("Hi from getFromServer");

  const userId = 1; 
  const booksToReadApi = `https://localhost:7291/api/PersonalLibraries/BooksToRead/UserId/${userId}`;
  const booksReadApi = `https://localhost:7291/api/PersonalLibraries/BooksRead/UserId/${userId}`;
  const booksPurchasedApi = `https://localhost:7291/api/PersonalLibraries/BooksPurchased/UserId/${userId}`;
  
  ajaxCall("GET", booksToReadApi, "", function(booksToRead) {
      console.log("Books To Read:", booksToRead); 
      booksToReadData = booksToRead;
      renderTable('#booksToReadTable', booksToRead, "Books to Read");
  }, getBooksECB);
  
  ajaxCall("GET", booksReadApi, "", function(booksRead) {
      console.log("Books Read:", booksRead); 
      booksReadData = booksRead;
      renderTable('#booksReadTable', booksRead, "Books Read");
  }, getBooksECB);
  
  ajaxCall("GET", booksPurchasedApi, "", function(booksPurchased) {
      console.log("Books Purchased:", booksPurchased); 
      booksPurchasedData = booksPurchased;
      renderTable('#booksPurchasedTable', booksPurchased, "Books Purchased");
  }, getBooksECB);
}

function getBooksECB(err) {
  console.log(err);
}

function renderTable(tableId, tableData, title) {
  try {
    if (!$.fn.DataTable.isDataTable(tableId)) {
      $(tableId).DataTable({
          data: tableData,
          pageLength: 5,
          columns: [
              {
                  data: "smallThumbnailUrl",
                  render: function (data, type, row, meta) {
                      return '<img src="' + data + '" alt="Book Thumbnail" style="width: 50px; height: auto;">';
                  },
                  title: "Cover"
              },
              {
                  data: "bookTitle",
                  title: "Title"
              },
              {
                  data: "isEbook",
                  render: function (data, type, row, meta) {
                      return data === true ? 'eBook' : 'Physical';
                  },
                  title: "Type"
              },
              {
                  data: "webReaderLink",
                  render: function (data, type, row, meta) {
                    return row.isEbook && data ? '<a href="' + data + '" target="_blank">Read online</a>' : 'N/A';
                  },
                  title: "Read Online"
              },
              {
                  render: function (data, type, row, meta) {
                    let dataBook = "data-bookId='" + row.bookId + "'";
                    let status = row.status ? 'HaveRead' : 'ToRead';
                    let toggleStatus = row.status ? 'Mark as ToRead' : 'Mark as HaveRead';
                    return `<button type='button' class='statusBtn btn btn-primary' ${dataBook} data-status='${row.status}'>${toggleStatus}</button>`;
                  },
                  title: "Actions"
              }
          ],
      });

      buttonEvents(tableId);
    } else {
      $(tableId).DataTable().clear().rows.add(tableData).draw();
    }
  } catch (err) {
    alert(err);
  }
}

function buttonEvents(tableId) {
  $(document).on('click', '.statusBtn', function() {
    let bookId = $(this).data('bookid');
    let currentStatus = $(this).data('status') === 'true';
    let newStatus = !currentStatus;
    
    updateBookStatus(bookId, newStatus);
  });
}

function updateBookStatus(bookId, newStatus) {
  const apiUrl = `https://localhost:7291/api/PersonalLibraries/UpdateBookStatus/UserId/1/BookId/${bookId}/NewStatus/${newStatus}`;

  $.ajax({
    url: apiUrl,
    type: 'PUT', 
    success: function(response) {
      if (newStatus) {
        moveBook('#booksToReadTable', '#booksReadTable', bookId, newStatus);
      } else {
        moveBook('#booksReadTable', '#booksToReadTable', bookId, newStatus);
      }
      
      updatePurchasedTable(bookId, newStatus);
    },
    error: function(err) {
      console.error('Error updating book:', err);
    }
  });
}

function moveBook(fromTableId, toTableId, bookId, newStatus) {
  let fromTable = $(fromTableId).DataTable();
  let toTable = $(toTableId).DataTable();

  let bookData = fromTable.row(function(idx, data, node) {
    return data.bookId === bookId;
  }).data();

  if (bookData) {
    fromTable.row(function(idx, data, node) {
      return data.bookId === bookId;
    }).remove().draw();

    toTable.row.add(bookData).draw();

    updateButtonStatus(bookId, newStatus);
  }
}

function updateButtonStatus(bookId, newStatus) {
  let buttonText = newStatus ? 'Mark as ToRead' : 'Mark as HaveRead';
  let buttonStatus = newStatus ? 'true' : 'false';

  ['#booksToReadTable', '#booksReadTable', '#booksPurchasedTable'].forEach(tableId => {
    $(tableId).find(`button[data-bookid='${bookId}']`)
      .text(buttonText)
      .data('status', buttonStatus);
  });
}

function updatePurchasedTable(bookId, newStatus) {
  let purchasedTable = $('#booksPurchasedTable').DataTable();

  let bookData = purchasedTable.row(function(idx, data, node) {
    return data.bookId === bookId;
  }).data();

  if (bookData) {
    bookData.status = newStatus;

    purchasedTable.row(function(idx, data, node) {
      return data.bookId === bookId;
    }).data(bookData).draw();

    updateButtonStatus(bookId, newStatus);
  }
}

