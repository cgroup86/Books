$(document).ready(function() {
  getFromServer(); // Fetch data and render tables

});
//---------------------------------------------------------------------------------------------------------------------------------------

function getFromServer() {
  console.log("Hi from getFromServer");

  // Define the API URLs
  const userId = 1; // Replace with dynamic userId if needed
  const booksToReadApi = `https://localhost:7291/api/PersonalLibraries/BooksToRead/UserId/${userId}`;
  const booksReadApi = `https://localhost:7291/api/PersonalLibraries/BooksRead/UserId/${userId}`;
  const booksPurchasedApi = `https://localhost:7291/api/PersonalLibraries/BooksPurchased/UserId/${userId}`;
  
  // Fetch Books To Read
  // Fetch Books To Read
  ajaxCall("GET", booksToReadApi, "", function(booksToRead) {
      console.log("Books To Read:", booksToRead); // Log response
      renderTable('#booksToReadTable', booksToRead, "Books to Read");
  }, getBooksECB);
  
  // Fetch Books Read
  ajaxCall("GET", booksReadApi, "", function(booksRead) {
      console.log("Books Read:", booksRead); // Log response
      renderTable('#booksReadTable', booksRead, "Books Read");
  }, getBooksECB);
  
  // Fetch Books Purchased
  ajaxCall("GET", booksPurchasedApi, "", function(booksPurchased) {
      console.log("Books Purchased:", booksPurchased); // Log response
      renderTable('#booksPurchasedTable', booksPurchased, "Books Purchased");
  }, getBooksECB);
}
function getBooksECB(err) {
  console.log(err);
}

//---------------------------------------------------------------------------------------------------------------------------------------

function renderTable(tableId, tableData, title) {
  try
  {
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

                      // let dataBook = "data-bookId='" + row.id + "'";
                      // let status = row.status ? 'HaveRead' : 'ToRead';
                      // let toggleStatus = row.status ? 'Mark as ToRead' : 'Mark as HaveRead';
                      // let statusButton = "<button type='button' class='statusBtn btn btn-primary' " + dataBook + " data-status='" + row.status + "'>" + toggleStatus + "</button>";
                      // return statusButton;

                      // let dataBook = "data-bookId='" + row.bookId + "'";
                      // let toggleButton = row.status ? 
                      //     `<button type='button' class='btn btn-secondary' ${dataBook} data-action='markAsToRead'>Mark as To Read</button>` : 
                      //     `<button type='button' class='btn btn-primary' ${dataBook} data-action='markAsRead'>Mark as Read</button>`;
                      // return toggleButton;
                  },
                  title: "Actions"
              }
          ],
          
      });

      // Bind button events for the newly rendered table
      buttonEvents(tableId);
  } 
  }
  catch (err)
  {
    alert(err);
  }
  
}
// else {
//   // Update existing DataTable if needed
//   $(tableId).DataTable().clear().rows.add(tableData).draw();
// }

function buttonEvents(tableId) {
  $(document).on('click', '.statusBtn', function() {
    let bookId = $(this).data('bookid');
    let currentStatus = $(this).data('status') === 'true';
    toggleBookStatus(bookId, !currentStatus);
  });
}

// Function to toggle book status and update DataTables
function toggleBookStatus(bookId, newStatus) {
  // Find the book and update its status
  let allBooks = [...booksToReadData, ...booksReadData, ...booksPurchasedData];
  let book = allBooks.find(b => b.bookId == bookId);

  if (book) {
      book.status = newStatus;

      // Update the status button in the DataTables
      $('#booksToReadTable').DataTable().clear().rows.add(booksToReadData).draw();
      $('#booksReadTable').DataTable().clear().rows.add(booksReadData).draw();
      $('#booksPurchasedTable').DataTable().clear().rows.add(booksPurchasedData).draw();
      
      // Optionally, you may want to make an API call here to persist the status change
  }
}



// function toggleBookStatus(bookId, newStatus) {
//   // Define the API URL for updating the book status
//   const apiUrl = `https://localhost:7291/api/PersonalLibraries/UpdateBookStatus/UserId/1/BookId/${bookId}/NewStatus/${newStatus}`;
//   console.log(apiUrl);
//   // Make the API call to update the book status
//   $.ajax({
//     url: apiUrl,
//     type: 'put', // Assuming the API uses POST for updates
//     success: function(response) {
//       // Update the status in the DataTable
//       updateDataTables();
//     },
//     error: function(xhr, status, error) {
//       console.error('Error updating book status:', status, error);
//     }
//   });
// }

// function updateDataTables() {
//   // Re-fetch the data from the server and update the DataTables
//   getFromServer(); // This function will re-render the DataTables with updated data
// }

// function buttonEvents(tableId) {
//   $(document).on('click', '.btn', function() {
//     let bookId = $(this).data('bookid');
//     let currentStatus = $(this).data('action') === 'markAsRead';
//     let newStatus = !currentStatus;

//     // Toggle the book status
//     toggleBookStatus(bookId, newStatus);
//   });
// }

