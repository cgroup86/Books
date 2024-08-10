$(document).ready(function() {
  // Load header.html into the header-container div
  $('#header-container').load('header.html', function() {
      // Add event listeners after loading the HTML
      LoginRegisterModalFunc();
  });

  // Physical Books: saleInfo.saleability != FREE && saleInfo.isEbook == false
  // Digital Books: saleInfo.isEbook == true
  var books = [];
  $.getJSON("../DataJson/books.json", function(data) {
      books = data;
      console.log("All:")
      console.log(books);
      insertBooksToDB(books);

      // Initialize an empty set to store unique author names
      let authorNames = new Set();
      let categoryNames = new Set();
      // Loop through each book in the data
      books.forEach(item => {
          // Check if volumeInfo and authors exist
          if (item.volumeInfo && item.volumeInfo.authors) {
              // Add each author to the set
              item.volumeInfo.authors.forEach(author => {
                  authorNames.add(author);
              });
          }
          // Check if volumeInfo and categories exist
          if (item.volumeInfo && item.volumeInfo.categories) {
            // Add each Category to the set
            item.volumeInfo.categories.forEach(category => {
              categoryNames.add(category);
            });
          }

      });
      
      // Convert the set to an array and log the unique author names
      let uniqueAuthors = Array.from(authorNames);
      console.log("Unique Author Names:");
      console.log(uniqueAuthors);
      
      // Convert the set to an array an log the unique category names.
      let uniqueCategories = Array.from(categoryNames);
      console.log("Unique category names");
      console.log(uniqueCategories);
      //insertCategoriesToDB(uniqueCategories);
      

      let categorizedBooks = {
          physical: [],
          digital: []
      };

      books.forEach(book => {
          const isEbook = book.saleInfo.isEbook;

          if (isEbook) {
              categorizedBooks.digital.push(book);
          } else {
              categorizedBooks.physical.push(book);
          }
      });

      console.log("Categorized Books:");
      console.log(categorizedBooks);

      
      const authors = uniqueAuthors;
      const authorDataArray = [];

      async function fetchAuthorData(authorName) {
          const searchUrl = `https://openlibrary.org/search/authors.json?q=${encodeURIComponent(authorName)}`;

          try {
              const searchResponse = await fetch(searchUrl);
              const searchResult = await searchResponse.json();

              if (searchResult.docs && searchResult.docs.length > 0) {
              const authorData = searchResult.docs[0];
              authorDataArray.push(authorData);
              //console.log(`Fetched data for ${authorName}:`, authorData);
            } else {
              console.log(`Author ${authorName} not found.`);
            }
          } catch (error) {
              console.error(`Failed to fetch data for author ${authorName}: ${error.message}`);
          }
      }

      async function fetchAllAuthors(authors) {
          for (const author of authors) {
              await fetchAuthorData(author);
          }
          console.log("All author data fetched and stored in array:", authorDataArray);
          insertAuthorsToDB(authorDataArray);
      }

      document.getElementById("fetchAuthorsButton").addEventListener("click", () => {
          fetchAllAuthors(authors);
      });
  });
});

//------------------------------------------------------------------------

function insertAuthorsToDB(data) {
  console.log("HI from insert author");
  let api = `${apiStart}Authors`;

  data.forEach(author => {
    const authorData = {
      Name: author.name,
      TopWork: author.top_work,
      WorkCount: author.work_count,
      Key: author.key,
    }
    ajaxCall("POST", api, JSON.stringify(authorData), insertAuthorsToDBSCB, insertAuthorsToDBECB)
  })
  //alert("Inserted authors to the data base successfully");
}

function insertAuthorsToDBSCB(stats) {
  console.log(stats);
}

function insertAuthorsToDBECB(err) {
  console.log(err);
  //alert("Failed to insert the authors to the data base");
}

//------------------------------------------------------------------------

function insertCategoriesToDB(data) {
  console.log("Hi from insert category");
  let api = `${apiStart}Categories`;

  data.forEach(category => {
    const categoryData = {
      Name: category
    }
    ajaxCall("POST", api, JSON.stringify(categoryData), insertCategoriesToDBSCB, insertCategoriesToDBECB)
  })
}

function insertCategoriesToDBSCB(stats) {
  console.log(stats);
}

function insertCategoriesToDBECB (err) {
  console.log(err);
}

//------------------------------------------------------------------------

function insertBooksToDB(data) {
  console.log("Hi from insert book");

  let api = `${apiStart}Books`;
  data.forEach(book => {
    
    const bookData = {
      Id: 0,
      Title: book.volumeInfo.title,
      Price: getRandomInt(25, 200),
      Authors: book.volumeInfo.authors,
      Publisher: book.volumeInfo.publisher,
      PublishedDate: book.volumeInfo.description,
      Description: book.volumeInfo.description,
      PageCount: book.volumeInfo.pageCount,
      Categories: book.volumeInfo.categories,
      AverageRating: parseInt(book.volumeInfo.averageRating),
      RatingsCount: book.volumeInfo.ratingsCount,
      SmallThumbnailUrl: book.volumeInfo.imageLinks.smallThumbnail,
      ThumbnailUrl: book.volumeInfo.imageLinks.thumbnail,
      Language: book.volumeInfo.language,
      PreviewLink: book.volumeInfo.previewLink,
      InfoLink: book.volumeInfo.infoLink,
      CanonicalVolumeLink: book.volumeInfo.canonicalVolumeLink,
      IsEbook: book.saleInfo.isEbook,
      Embeddable: book.accessInfo.embeddable,
      EpubIsAvailable: book.accessInfo.epub.isAvailable,
      EpubDownloadLink: book.accessInfo.epub.downloadLink,
      PdfIsAvailable: book.accessInfo.pdf.isAvailable,
      PdfDownloadLink: book.accessInfo.pdf.downloadLink,
      WebReaderLink: book.accessInfo.webReaderLink,
      TextReading: book.volumeInfo.readingModes.text,
      PhotoReading: book.volumeInfo.readingModes.photo,
      GoogleBooksId: book.id,
      Etag: book.etag,
      SelfLink: book.SelfLink
    }
    ajaxCall("POST", api, JSON.stringify(bookData), insertBooksToDBSCB, insertBooksToDBECB)

  });
}

function insertBooksToDBSCB(stats) {
  console.log(stats);
}

function insertBooksToDBECB (err) {
  console.log(err);
}

function getRandomInt(min, max) {
  min = Math.ceil(min);
  max = Math.floor(max);
  return Math.floor(Math.random() * (max - min + 1)) + min;
}
