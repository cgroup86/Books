$(document).ready(function() {
  $('#header-container').load('header.html', function() {
      LoginRegisterModalFunc();
      checkUserStatus();
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

$('.main-container').on('click', '.add-book-button', function() {
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




// // Physical Books: saleInfo.saleability != FREE && saleInfo.isEbook == false
  // // Digital Books: saleInfo.isEbook == true
  // var books = [];
  // $.getJSON("../DataJson/books.json", function(data) {
  //     books = data;
  //     console.log("All:")
  //     console.log(books);
  //     insertBooksToDB(books);

  //     // Initialize an empty set to store unique author names
  //     let authorNames = new Set();
  //     let categoryNames = new Set();
  //     // Loop through each book in the data
  //     books.forEach(item => {
  //         // Check if volumeInfo and authors exist
  //         if (item.volumeInfo && item.volumeInfo.authors) {
  //             // Add each author to the set
  //             item.volumeInfo.authors.forEach(author => {
  //                 authorNames.add(author);
  //             });
  //         }
          
  //         // Check if volumeInfo and categories exist
  //         if (item.volumeInfo && item.volumeInfo.categories) {
  //           // Add each Category to the set
  //           item.volumeInfo.categories.forEach(category => {
  //             categoryNames.add(category);
  //           });
  //         }

  //     });
      
  //     // Convert the set to an array and log the unique author names
  //     let uniqueAuthors = Array.from(authorNames);
  //     console.log("Unique Author Names:");
  //     console.log(uniqueAuthors);
      
  //     // Convert the set to an array an log the unique category names.
  //     let uniqueCategories = Array.from(categoryNames);
  //     console.log("Unique category names");
  //     console.log(uniqueCategories);
  //     //insertCategoriesToDB(uniqueCategories);
      

  //     let categorizedBooks = {
  //         physical: [],
  //         digital: []
  //     };

  //     books.forEach(book => {
  //         const isEbook = book.saleInfo.isEbook;

  //         if (isEbook) {
  //             categorizedBooks.digital.push(book);
  //         } else {
  //             categorizedBooks.physical.push(book);
  //         }
  //     });

  //     console.log("Categorized Books:");
  //     console.log(categorizedBooks);

      
  //     const authors = uniqueAuthors;
  //     const authorDataArray = [];

  //     async function fetchAuthorData(authorName) {
  //         const searchUrl = `https://openlibrary.org/search/authors.json?q=${encodeURIComponent(authorName)}`;

  //         try {
  //             const searchResponse = await fetch(searchUrl);
  //             const searchResult = await searchResponse.json();

  //             if (searchResult.docs && searchResult.docs.length > 0) {
  //             const authorData = {
  //               search: searchResult.docs[0],
  //               name: authorName
  //             }
  //             authorDataArray.push(authorData);
  //             //console.log(`Fetched data for ${authorName}:`, authorData);
  //           } else {
  //             console.log(`Author ${authorName} not found.`);
  //           }
  //         } catch (error) {
  //             console.error(`Failed to fetch data for author ${authorName}: ${error.message}`);
  //         }
  //     }

  //     async function fetchAllAuthors(authors) {
  //         for (const author of authors) {
  //             await fetchAuthorData(author);
  //         }
  //         console.log("All author data fetched and stored in array:", authorDataArray);
  //         insertAuthorsToDB(authorDataArray);
  //     }

  //     document.getElementById("fetchAuthorsButton").addEventListener("click", () => {
  //         fetchAllAuthors(authors);
  //     });
  // });






// //------------------------------------------------------------------------

// function insertAuthorsToDB(data) {
//   console.log("HI from insert author");
//   let api = `${apiStart}Authors`;

//   data.forEach(author => {
//     const authorData = {
//       Name: author.name,
//       TopWork: author.search.top_work,
//       WorkCount: author.search.work_count,
//       Key: author.search.key,
//     }
//     ajaxCall("POST", api, JSON.stringify(authorData), insertAuthorsToDBSCB, insertAuthorsToDBECB)
//   })
//   //alert("Inserted authors to the data base successfully");
// }

// function insertAuthorsToDBSCB(stats) {
//   console.log(stats);
// }

// function insertAuthorsToDBECB(err) {
//   console.log(err);
//   //alert("Failed to insert the authors to the data base");
// }

// //------------------------------------------------------------------------

// function insertCategoriesToDB(data) {
//   console.log("Hi from insert category");
//   let api = `${apiStart}Categories`;

//   data.forEach(category => {
//     const categoryData = {
//       Name: category
//     }
//     ajaxCall("POST", api, JSON.stringify(categoryData), insertCategoriesToDBSCB, insertCategoriesToDBECB)
//   })
// }

// function insertCategoriesToDBSCB(stats) {
//   console.log(stats);
// }

// function insertCategoriesToDBECB (err) {
//   console.log(err);
// }

// //------------------------------------------------------------------------

// // function insertBooksToDB(data) {
// //   console.log("Hi from insert book");

// //   let api = `${apiStart}Books`;
// //   data.forEach(book => {
    
// //     const bookData = {
// //       Id: 0,
// //       Title: book.volumeInfo.title,
// //       Price: getRandomInt(25, 200),
// //       Authors: book.volumeInfo.authors,
// //       Publisher: book.volumeInfo.publisher,
// //       PublishedDate: book.volumeInfo.description,
// //       Description: book.volumeInfo.description,
// //       PageCount: book.volumeInfo.pageCount,
// //       Categories: book.volumeInfo.categories,
// //       AverageRating: parseInt(book.volumeInfo.averageRating),
// //       RatingsCount: book.volumeInfo.ratingsCount,
// //       SmallThumbnailUrl: book.volumeInfo.imageLinks.smallThumbnail,
// //       ThumbnailUrl: book.volumeInfo.imageLinks.thumbnail,
// //       Language: book.volumeInfo.language,
// //       PreviewLink: book.volumeInfo.previewLink,
// //       InfoLink: book.volumeInfo.infoLink,
// //       CanonicalVolumeLink: book.volumeInfo.canonicalVolumeLink,
// //       IsEbook: book.saleInfo.isEbook,
// //       Embeddable: book.accessInfo.embeddable,
// //       EpubIsAvailable: book.accessInfo.epub.isAvailable,
// //       EpubDownloadLink: book.accessInfo.epub.downloadLink,
// //       PdfIsAvailable: book.accessInfo.pdf.isAvailable,
// //       PdfDownloadLink: book.accessInfo.pdf.downloadLink,
// //       WebReaderLink: book.accessInfo.webReaderLink,
// //       TextReading: book.volumeInfo.readingModes.text,
// //       PhotoReading: book.volumeInfo.readingModes.photo,
// //       GoogleBooksId: book.id,
// //       Etag: book.etag,
// //       SelfLink: book.SelfLink
// //     }
// //     ajaxCall("POST", api, JSON.stringify(bookData), insertBooksToDBSCB, insertBooksToDBECB)

// //   });
// // }


// function insertBooksToDB(data) {
//   console.log("Hi from insert book");

//   let api = `${apiStart}Books`;
//   let delay = 1000; // Delay in milliseconds (e.g., 1000 ms = 1 second)
  
//   function sendAjaxCall(index) {
//     if (index >= data.length) {
//       return; // Exit when all data has been processed
//     }

//     const book = data[index];
//     const bookData = {
//       Id: 0,
//       Title: book.volumeInfo.title,
//       Price: getRandomInt(25, 200),
//       Authors: book.volumeInfo.authors,
//       Publisher: book.volumeInfo.publisher,
//       PublishedDate: book.volumeInfo.publishedDate,
//       Description: book.volumeInfo.description,
//       PageCount: book.volumeInfo.pageCount,
//       Categories: book.volumeInfo.categories,
//       AverageRating: 0,
//       RatingsCount: 0,
//       SmallThumbnailUrl: book.volumeInfo.imageLinks.smallThumbnail,
//       ThumbnailUrl: book.volumeInfo.imageLinks.thumbnail,
//       Language: book.volumeInfo.language,
//       PreviewLink: book.volumeInfo.previewLink,
//       InfoLink: book.volumeInfo.infoLink,
//       CanonicalVolumeLink: book.volumeInfo.canonicalVolumeLink,
//       IsEbook: book.saleInfo.isEbook,
//       Embeddable: book.accessInfo.embeddable,
//       EpubIsAvailable: book.accessInfo.epub.isAvailable,
//       EpubDownloadLink: book.accessInfo.epub.downloadLink,
//       PdfIsAvailable: book.accessInfo.pdf.isAvailable,
//       PdfDownloadLink: book.accessInfo.pdf.downloadLink,
//       WebReaderLink: book.accessInfo.webReaderLink,
//       TextReading: book.volumeInfo.readingModes.text,
//       PhotoReading: book.volumeInfo.readingModes.photo,
//       GoogleBooksId: book.id,
//       Etag: book.etag,
//       SelfLink: book.selfLink,
//       IsActive: true,
//       IsAvailable: true,
//       NumOfPrints: getRandomInt(1, 4)
//     };

//     ajaxCall("POST", api, JSON.stringify(bookData), insertBooksToDBSCB, insertBooksToDBECB);

//     // Schedule the next call
//     setTimeout(() => sendAjaxCall(index + 1), delay);
//   }

//   // Start processing from the first item
//   sendAjaxCall(0);
// }

// function insertBooksToDBSCB(stats) {
//   console.log(stats);
// }

// function insertBooksToDBECB (err) {
//   console.log(err);
// }

// function getRandomInt(min, max) {
//   min = Math.ceil(min);
//   max = Math.floor(max);
//   return Math.floor(Math.random() * (max - min + 1)) + min;
// }