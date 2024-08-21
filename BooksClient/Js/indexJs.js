$(document).ready(function() {
  $('#header-container').load('header.html', function() {
      LoginRegisterModalFunc();
      checkUserStatus();
  });


    $('#search-form').on('submit', function (e) {
        e.preventDefault();

        var searchValue = $('#search-value').val();
        var searchType = $('#search-type').val();

        $.ajax({
            url: `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/Books/GetSearchedBooks/searchType/${searchType}/searchValue/${searchValue}`,
            method: 'GET',
            success: function (data) {
                var container = $('#search-results-container');
                container.empty();

                if (data.length === 0) {
                    container.append('<p>No data was found.</p>');
                } else {
                    data.forEach(function (book) {
                        console.log(book);
                        var bookItemHtml = `
                                            <div class="book-item">
                                                <img src="${book.smallThumbnailUrl}" alt="${book.title}" />
                                                <div class="details">
                                                    <div class="title">${book.title}</div>
                                                    <div class="price">Price: ₪${book.price}</div>
                                                    <a href="${book.previewLink}" class="preview-link" target="_blank">Preview</a>
                                                    <button class="add-book-button" data-book-id="${book.id}" data-is-ebook="${book.isEbook}" data-book-available="${book.isEbook || book.isAvailable}">Add book</button>
                                                </div>
                                            </div>`;
                        container.append(bookItemHtml);
                    });
                }

            },
            error: function (error) {
                console.error('Error getting search results:', error);
            }
        });
    });

    $.ajax({
        url: 'https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/Books/GetRandom5Books',
        method: 'GET',
        success: function (data) {
            var swiperWrapper = $('#swiper-wrapper');
            data.forEach(function (book) {
                var slideHtml = `
                                    <div class="swiper-slide">
                                        <img src="${book.smallThumbnailUrl}" alt="${book.title}" />
                                        <h3>${book.title}</h3>
                                        <p>Price: ₪${book.price}</p>
                                        <a href="${book.previewLink}" target="_blank">Preview</a>
                                        <button class="add-book-button" data-book-id="${book.id}" data-is-ebook="${book.isEbook}" data-book-available="${book.isEbook || book.isAvailable}">Add book</button>
                                    </div>`;
                swiperWrapper.append(slideHtml);
            });

            var TrendingSlider = new Swiper('.tranding-slider', {
                effect: 'coverflow',
                grabCursor: true,
                centeredSlides: true,
                loop: true,
                slidesPerView: 'auto',
                coverflowEffect: {
                    rotate: 0,
                    stretch: 0,
                    depth: 100,
                    modifier: 2.5,
                },
                pagination: {
                    el: '.swiper-pagination',
                    clickable: true,
                },
                navigation: {
                    nextEl: '.swiper-button-next',
                    prevEl: '.swiper-button-prev',
                }
            });
        },
        error: function (error) {
            console.error('Error getting data:', error);
        }
    });
});




// ---------------------------------------------------------------------------------------------------------------
let digitalPage = 1;
let physicalPage = 1;
const pageSize = 5;
let digitalTotalRecords = 0;
let physicalTotalRecords = 0;
const fetchTotalCountInterval = 10; 
let fetchTotalCountDigital = true;
let fetchTotalCountPhysical = true;
let digitalPageClicks = 0;
let physicalPageClicks = 0;

$(document).ready(function() {
    loadDigitalBooks(digitalPage, pageSize, true);
    loadPhysicalBooks(physicalPage, pageSize, true);
});

function loadDigitalBooks(pageNumber, pageSize, fetchTotalCount) {
    fetchBooksFromDB(true, pageNumber, pageSize, fetchTotalCount);
}

function loadPhysicalBooks(pageNumber, pageSize, fetchTotalCount) {
    fetchBooksFromDB(false, pageNumber, pageSize, fetchTotalCount);
}

// -------------------------------------------------------------------------------------------------------------


function renderBooks(books, container) {
    container.empty();

    books.forEach(book => {
        let bookCard = `
        <div class="book-card">
            <img src="${book.smallThumbnailUrl}" alt="cover">
            <h3>
                <a href="${book.previewLink}" target="_blank">${book.title}</a>
            </h3>
            <p class="price-number">
                <b>Price: <span>${book.price}₪</span></b>
            </p>
            <button class="add-book-button" data-book-id="${book.id}" data-is-ebook="${book.isEbook}" data-book-available="${book.isEbook || book.isAvailable}">Add book</button>
        </div>`;
        
        container.append(bookCard);
    });
}

// -------------------------------------------------------------------------------------------------------------

function fetchBooksFromDB(isEbook, pageNumber, pageSize, fetchTotalCount) {
    const api = `${apiStart}Books?isEbook=${isEbook}&pageNumber=${pageNumber}&pageSize=${pageSize}&fetchTotalCount=${fetchTotalCount}`;
    ajaxCall("GET", api, "", fetchBooksSuccess, fetchBooksError);
}

function fetchBooksSuccess(response) {
  //console.log(response);
  const container = response.isEbook ? $('.main-container').first() : $('.main-container').last();
  const paginationElement = response.isEbook ? $('#digital-pagination') : $('#physical-pagination');
  
  if (response.isEbook) {
      if (digitalPageClicks % fetchTotalCountInterval === 0) {
          digitalTotalRecords = response.totalRecords !== undefined ? response.totalRecords : digitalTotalRecords;
          fetchTotalCountDigital = false;
      }
      digitalPageClicks++; 
  } else {
      if (physicalPageClicks % fetchTotalCountInterval === 0) {
          physicalTotalRecords = response.totalRecords !== undefined ? response.totalRecords : physicalTotalRecords;
          fetchTotalCountPhysical = false;
      }
      physicalPageClicks++; 
  }

    renderBooks(response.books, container);
    updatePaginationControls(response.isEbook ? digitalTotalRecords : physicalTotalRecords, response.pageSize, response.pageNumber, paginationElement);
}

function fetchBooksError(err) {
    console.error("Error fetching books:", err);
}

function updatePaginationControls(totalRecords, pageSize, currentPage, paginationElement) {
  const totalPages = Math.ceil(totalRecords / pageSize);

  let paginationHtml = `
      <button class="prevPage" ${currentPage <= 1 ? 'disabled' : ''}>Previous</button>
      <span>Page ${currentPage} of ${totalPages}</span>
      <button class="nextPage" ${currentPage >= totalPages ? 'disabled' : ''}>Next</button>
  `;
  paginationElement.html(paginationHtml);

  paginationElement.find('.prevPage').off('click').on('click', function () {
    if (currentPage > 1) {
        currentPage--;
        loadBooksBasedOnContainer(paginationElement, currentPage, pageSize);
    }
  });

  paginationElement.find('.nextPage').off('click').on('click', function () {
    if (currentPage < totalPages) {
        currentPage++;
        loadBooksBasedOnContainer(paginationElement, currentPage, pageSize);
    }
  });

}

function loadBooksBasedOnContainer(paginationElement, page, pageSize) {
  let fetchTotalCount;
  
  if (paginationElement.is('#digital-pagination')) {
    fetchTotalCount = (page === 1 || digitalPageClicks % fetchTotalCountInterval === 0);
    if (page % fetchTotalCountInterval === 0) {
        fetchTotalCountDigital = true; 
    }
    digitalPage = page;
    loadDigitalBooks(page, pageSize, fetchTotalCount);
  } else if (paginationElement.is('#physical-pagination')) {
      fetchTotalCount = (page === 1 || physicalPageClicks % fetchTotalCountInterval === 0);
      if (page % fetchTotalCountInterval === 0) {
          fetchTotalCountPhysical = true; 
      }
      physicalPage = page;
      loadPhysicalBooks(page, pageSize, fetchTotalCount);
  }

}

// -------------------------------------------------------------------------------------------------------------

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

