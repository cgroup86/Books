const urlParams = new URLSearchParams(window.location.search);
const authorName = urlParams.get('authorName');
const apiStart = `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/`;
$(document).ready(function () {
      fetchBooksFromDB(authorName);
    });


    function renderBooks(books) {
        const instructorsList = $('.container');
        instructorsList.empty();
        let head = `<h1 style="width:100vw;">${authorName}</h1>`
        instructorsList.append(head);
        books.forEach(book => {
            let bookCard = `   <div class="book-card">
                  <img src="${book.smallThumbnailUrl}" alt="cover">
                  <h3>
                      <a href="${book.previewLink}" target="_blank">${book.title}</a>
                  </h3>
                  <p class="price-number">
                      <b>Price: <span>${book.price}₪</span></b>
                  </p>
                <button class="add-book-button" data-book-id="${book.id}" data-is-ebook="${book.isEbook}" data-book-available="${book.isEbook || book.isAvailable}">Add book</button>
             </div>`;

            instructorsList.append(bookCard);
        });
    }
$(document).on('click', '.add-book-button', function () {
    const user = JSON.parse(sessionStorage.getItem('userData'));
    if (!user) {
        alert("You need to log in to add a book");
    }
    else {
        const button = $(this);
        const bookId = button.data('book-id');
        const isEbook = button.data('is-ebook');
        const isAvailable = button.data('book-available');

        if (!isEbook && !isAvailable) {
            alert('This book is out of stock.');
            return;
        }
        addToLibrary(bookId, isEbook);
    }
});

function addToLibrary(bookId, isEbook) {
    //console.log("addToLibrary  bookId:", bookId, "isEbook:", isEbook);
    const user = JSON.parse(sessionStorage.getItem('userData'));
    const personalLibrary = {
        UserId: user.userId,
        BookId: bookId,
        Status: false,
        IsPurchased: false
    };

    const api = `${apiStart}PersonalLibraries/AddToLibrary/${isEbook}`;
    ajaxCall("POST", api, JSON.stringify(personalLibrary), addBookSuccess, addBookError);
}

function addBookSuccess(response) {
    if (response.success) {
        alert('Book added to your library successfully.');
        if (response.isEbook) {
            loadDigitalBooks(digitalPage, pageSize, fetchTotalCountDigital);
        } else {
            loadPhysicalBooks(physicalPage, pageSize, fetchTotalCountPhysical);
        }
    } else {
        alert("Book already exists on your library");
    }
}

function addBookError(err) {
    alert("Book already exists on your library");
}

    function fetchBooksFromDB(authorName) {
    console.log("hi from fetch books from data base");
    let api = `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/Authors/getBooksByAuthorsName/${authorName}`;
    ajaxCall("GET", api, "", fetchBooksSuccess, fetchBooksError);
    }


    function fetchBooksSuccess(response) {
        console.log("Success response: ", response);
        renderBooks(response);
    }

    function fetchBooksError(err) {
        console.error("Error fetching books:", err);
}


////////////////////////////////////////////////////////////////////////////
$(document).on('click', '.add-book-button', function () {
    const user = JSON.parse(sessionStorage.getItem('userData'));
    if (!user) {
        alert("You need to log in to add a book");
    } else {
        const button = $(this);
        const bookId = button.data('book-id');
        const isEbook = button.data('is-ebook');
        const isAvailable = button.data('book-available');
        const userId = user.userId;

        if (!isEbook && !isAvailable) {
            $.ajax({
                url: `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/PersonalLibraries/GetWhoOwnTheBook/bookId/${bookId}`,
                type: 'GET',
                success: function (response) {
                    const sellerId = response[0].sellerId;
                    console.log("Seller ID for the book purchase: ", sellerId);

                    $('#modalLabel').text('This book is out of stock');

                    let modalContent = '<div class="form-group">';
                    modalContent += `<h3>You can ${sellerId === -1 ? "not" : ""} make or declare a request for purchase for this book</h3>`;
                    modalContent += '<p>This book is in the hands of another user</p>';

                    if (sellerId !== -1) {
                        modalContent += `<button id="saveUserChangesBtn" class="btn btn-primary" ` +
                            `data-user-id="${userId}" data-book-id="${bookId}" data-seller-id="${sellerId}">Request or declare</button>`;
                    }

                    modalContent += '</div>';

                    $('#modalContent').html(modalContent);
                    $('#actionModal').modal('show');
                },
                error: function (xhr, status, error) {
                    alert('An error occurred while retrieving seller information.');
                }
            });
            return;
        }
        addToLibrary(bookId, isEbook);
    }
});


$(document).on('click', '#saveUserChangesBtn', function () {
    const button = $(this);
    const userId = button.data('user-id');
    const bookId = button.data('book-id');
    const sellerId = button.data('seller-id');

    $.ajax({
        url: `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/PersonalLibraries/AddRemoveRequestToBuy/sellerId/${sellerId}/buyerId/${userId}/bookId/${bookId}`, 
        type: 'PUT',
        contentType: 'application/json',
        success: function (response) {
            alert('Your request has been submitted successfully.');
        },
        error: function (xhr, status, error) {
            alert('An error occurred while submitting your request.');
        }
    });
});


function addToLibrary(bookId, isEbook) {
  //console.log("addToLibrary  bookId:", bookId, "isEbook:", isEbook);
    const user = JSON.parse(sessionStorage.getItem('userData'));
    const personalLibrary = {
        UserId: user.userId,
        BookId: bookId,
        Status: false, 
        IsPurchased: false
    };

    const api = `${apiStart}PersonalLibraries/AddToLibrary/${isEbook}`;
    ajaxCall("POST", api, JSON.stringify(personalLibrary), addBookSuccess, addBookError);
}

function addBookSuccess(response) {
    if (response.success) {
        alert('Book added to your library successfully.');
        if (response.isEbook) {
            loadDigitalBooks(digitalPage, pageSize, fetchTotalCountDigital);
        } else {
            loadPhysicalBooks(physicalPage, pageSize, fetchTotalCountPhysical);
        }
    } else {
      alert("Book already exists on your library");
    }
}

function addBookError(err) {
    alert("Book already exists on your library");
}