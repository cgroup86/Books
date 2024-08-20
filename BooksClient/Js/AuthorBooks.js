const authorName = localStorage.getItem("authorName");
  $(document).ready(function () {
      console.log(authorName);
      fetchBooksFromDB(authorName);
    });


    function renderBooks(books) {
        const instructorsList = $('.container');
        instructorsList.empty();
        let head = <h1>${authorName}</h1>
        instructorsList.append(head);
        books.forEach(book => {
            let bookCard = 
              <div class="book-card">
                  <img src="${book.smallThumbnailUrl}" alt="cover">
                  <h3>
                      <a href="${book.previewLink}" target="_blank">${book.title}</a>
                  </h3>
                  <p class="price-number">
                      <b>Price: <span>${book.price}₪</span></b>
                  </p>
                  <button class="add-book-button" data-book-id="${book.id}">Add book</button>
              </div>;

            instructorsList.append(bookCard);
        });
    }

    function fetchBooksFromDB(authorName) {
        console.log("hi from fetch books from data base");
        //https://localhost:7291/api/Authors/getBooksByAuthorsName/Walter%20Scott
        const api = 'https://localhost:7291/api/Authors/getBooksByAuthorsName/${authorName}';
        ajaxCall("GET", api, "", fetchBooksSuccess, fetchBooksError);
    }

    function fetchBooksSuccess(response) {
        console.log("Success response: ", response);
        renderBooks(response);
    }

    function fetchBooksError(err) {
        console.error("Error fetching books:", err);
    }