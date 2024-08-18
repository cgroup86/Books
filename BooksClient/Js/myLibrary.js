let booksToReadData = [];
let booksReadData = [];
let booksPurchasedData = [];

$(document).ready(function() {
  getFromServer(); // Fetch data and render tables
});

function getFromServer() {
  console.log("Hi from getFromServer");

  // Define the API URLs
  const userId = 1; // Replace with dynamic userId if needed
  const booksToReadApi = `https://localhost:7291/api/PersonalLibraries/BooksToRead/UserId/${userId}`;
  const booksReadApi = `https://localhost:7291/api/PersonalLibraries/BooksRead/UserId/${userId}`;
  const booksPurchasedApi = `https://localhost:7291/api/PersonalLibraries/BooksPurchased/UserId/${userId}`;
  
  // Fetch Books To Read
  ajaxCall("GET", booksToReadApi, "", function(booksToRead) {
      console.log("Books To Read:", booksToRead); 
      booksToReadData = booksToRead;
      renderTable('#booksToReadTable', booksToRead, "Books to Read");
  }, getBooksECB);
  
  // Fetch Books Read
  ajaxCall("GET", booksReadApi, "", function(booksRead) {
      console.log("Books Read:", booksRead); 
      booksReadData = booksRead;
      renderTable('#booksReadTable', booksRead, "Books Read");
  }, getBooksECB);
  
  // Fetch Books Purchased
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

      // Bind button events for the newly rendered table
      buttonEvents(tableId);
    } else {
      // Update existing DataTable if needed
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
    
    // Update the status on the server
    updateBookStatus(bookId, newStatus);
  });
}

function updateBookStatus(bookId, newStatus) {
  const apiUrl = `https://localhost:7291/api/PersonalLibraries/UpdateBookStatus/UserId/1/BookId/${bookId}/NewStatus/${newStatus}`;

  $.ajax({
    url: apiUrl,
    type: 'PUT', // Use 'PUT' for updating status
    success: function(response) {
      // Update DataTables based on the current book status
      if (newStatus) {
        // Move book from ToRead to Read
        moveBook('#booksToReadTable', '#booksReadTable', bookId, newStatus);
      } else {
        // Move book from Read to ToRead
        moveBook('#booksReadTable', '#booksToReadTable', bookId, newStatus);
      }
      
      // Optionally update the Purchased table if needed
      updatePurchasedTable(bookId, newStatus);
    },
    error: function(xhr, status, error) {
      console.error('Error updating book status:', status, error);
    }
  });
}

function moveBook(fromTableId, toTableId, bookId, newStatus) {
  let fromTable = $(fromTableId).DataTable();
  let toTable = $(toTableId).DataTable();

  // Find the book in the source table
  let bookData = fromTable.row(function(idx, data, node) {
    return data.bookId === bookId;
  }).data();

  if (bookData) {
    // Remove the book from the source table
    fromTable.row(function(idx, data, node) {
      return data.bookId === bookId;
    }).remove().draw();

    // Add the book to the target table
    toTable.row.add(bookData).draw();

    // Update the button text and data-status attribute
    updateButtonStatus(bookId, newStatus);
  }
}

function updateButtonStatus(bookId, newStatus) {
  let buttonText = newStatus ? 'Mark as ToRead' : 'Mark as HaveRead';
  let buttonStatus = newStatus ? 'true' : 'false';

  // Update the button text and status in both tables
  ['#booksToReadTable', '#booksReadTable'].forEach(tableId => {
    $(tableId).find(`button[data-bookid='${bookId}']`)
      .text(buttonText)
      .data('status', buttonStatus);
  });
}

function updatePurchasedTable(bookId, newStatus) {
  // Fetch the updated data if needed or handle changes locally
  // Example: if needed, you could fetch and update the Purchased table similarly
}
