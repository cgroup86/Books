// Use the values
//document.getElementById("authorInfo").innerText = "Author Name: " + authorName;
//console.log(authorName);
const authorName = localStorage.getItem("authorName");
  $(document).ready(function () {
        //let digitalPage = 1;
        //let physicalPage = 1;
        //const pageSize = 5;

        //loadDigitalBooks(digitalPage, pageSize);
        //loadPhysicalBooks(physicalPage, pageSize);


      console.log(authorName);
      fetchBooksFromDB(authorName);
    });


    //function loadDigitalBooks(pageNumber, pageSize) {
    //    fetchBooksFromDB(true, pageNumber, pageSize);
    //}

    //function loadPhysicalBooks(pageNumber, pageSize) {
    //    fetchBooksFromDB(false, pageNumber, pageSize);
    //}

    // Function to render books in the container
    function renderBooks(books) {
        const instructorsList = $('.container');
        instructorsList.empty();
        let head = `<h1>${authorName}</h1>`
        instructorsList.append(head);
        books.forEach(book => {
            let bookCard = `
              <div class="book-card">
                  <img src="${book.smallThumbnailUrl}" alt="cover">
                  <h3>
                      <a href="${book.infoLink}" target="_blank">${book.title}</a>
                  </h3>
                  <p class="price-number">
                      <b>Price: <span>${book.price}₪</span></b>
                  </p>
                  <button class="add-book-button" data-book-id="${book.googleBooksId}">Add book</button>
              </div>`;

            instructorsList.append(bookCard);
        });
    }

    function fetchBooksFromDB(authorName) {
        console.log("hi from fetch books from data base");
        //https://localhost:7291/api/Authors/getBooksByAuthorsName/Walter%20Scott
        const api = `https://localhost:7291/api/Authors/getBooksByAuthorsName/${authorName}`;
        ajaxCall("GET", api, "", fetchBooksSuccess, fetchBooksError);
    }

    function fetchBooksSuccess(response) {
        console.log("Success response: ", response);
        renderBooks(response);
        //const container = response.isEbook ? $('.main-container').first() : $('.main-container').last();
        //const paginationElement = response.isEbook ? $('#digital-pagination') : $('#physical-pagination');
        //renderBooks(response.books, container);
        //updatePaginationControls(response.totalRecords, response.pageSize, response.pageNumber, paginationElement);
    }

    function fetchBooksError(err) {
        console.error("Error fetching books:", err);
    }
