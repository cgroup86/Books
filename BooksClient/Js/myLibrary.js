let booksToReadData = [];
let booksReadData = [];
let booksPurchasedData = [];

$(document).ready(function() {
  $('#header-container').load('header.html', function() {
    LoginRegisterModalFunc();
    checkUserStatus();
  });
  getFromServer(); 
});


function getFromServer() {
  console.log("Hi from getFromServer");
    const user = JSON.parse(sessionStorage.getItem('userData'));
    const userId = user.userId;
    const booksToReadApi = `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/PersonalLibraries/BooksToRead/UserId/${userId}`;
    const booksReadApi = `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/PersonalLibraries/BooksRead/UserId/${userId}`;
    const booksPurchasedApi = `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/PersonalLibraries/BooksPurchased/UserId/${userId}`;
  
  ajaxCall("GET", booksToReadApi, "", function(booksToRead) {
      //console.log("Books To Read:", booksToRead); 
      booksToReadData = booksToRead;
      renderTable('#booksToReadTable', booksToRead, "Books to Read");
  }, getBooksECB);
  
  ajaxCall("GET", booksReadApi, "", function(booksRead) {
      //console.log("Books Read:", booksRead); 
      booksReadData = booksRead;
      renderTable('#booksReadTable', booksRead, "Books Read");
  }, getBooksECB);
  
  ajaxCall("GET", booksPurchasedApi, "", function(booksPurchased) {
      //console.log("Books Purchased:", booksPurchased); 
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
                    return `<button type='button' class='statusBtn btn btn-primary' ${dataBook} data-status='${row.status}' data-isEbook='${row.isEbook}'>${toggleStatus}</button>`;
                  },
                  title: "Actions"
              },
              {
                  render: function (data, type, row, meta) {
                      if (row.isEbook && row.embeddable) {
                          return `<a href='explore.html?bookId=${row.googleBooksId}' class='btn btn-info'>Explore</a>`;
                      }
                      return 'N/A';
                  },
                  title: "Explore"
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
    let isEbook = $(this).data('isEbook');

    updateBookStatus(bookId, newStatus, isEbook);
  });
}

function updateBookStatus(bookId, newStatus) {
    const user = JSON.parse(sessionStorage.getItem('userData'));
    const userId = user.userId;
    const apiUrl = `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/PersonalLibraries/UpdateBookStatus/UserId/${userId}/BookId/${bookId}/NewStatus/${newStatus}/IsEbook/${isEbook}`;

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

