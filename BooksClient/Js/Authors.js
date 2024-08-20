//let authorsData = []; // Initialize the authorsData array

//async function fetchAuthors(query) {
//    const url = `https://openlibrary.org/search/authors.json?q=${encodeURIComponent(query)}`;
//    try {
//        const response = await fetch(url);
//        const data = await response.json();
//        return data.docs[0];
//    } catch (error) {
//        return null;
//    }
//}

//async function fetchImageDescriptionAuthors(query) {
//    const url = `https://api.wikimedia.org/core/v1/wikipedia/en/search/title?q=${encodeURIComponent(query)}&limit=1`;
//    try {
//        const response = await fetch(url);
//        const data = await response.json();
//        return data.pages[0];
//    } catch (error) {
//        return null;
//    }
//}

//function getRandomFallbackImage() {
//    const fallbackImages = [
//        'https://cdn.pixabay.com/photo/2021/07/07/04/54/edgar-allan-poe-6393223_960_720.png',
//        'https://cdn.pixabay.com/photo/2023/06/25/15/27/ai-generated-8087656_960_720.jpg',
//        'https://cdn.pixabay.com/photo/2023/04/11/22/44/ai-generated-7918380_1280.jpg',
//        'https://cdn.pixabay.com/photo/2023/05/21/20/24/ai-generated-8009384_640.jpg',
//        'https://cdn.pixabay.com/photo/2021/07/17/23/53/elizabeth-barrett-browning-6474338_640.png',
//        'https://cdn.pixabay.com/photo/2021/07/16/20/30/charlies-dickens-6471797_1280.png'
//    ];
//    const randomIndex = Math.floor(Math.random() * fallbackImages.length);
//    return fallbackImages[randomIndex];
//}

//async function getAuthorInfo(query) {
//    const author = await fetchAuthors(query);
//    if (!author) {
//        alert(`The author that was not found is: ${query}`);
//        return;
//    }

//    const wikiData = await fetchImageDescriptionAuthors(author.name);
//    let imageUrl;
//    let description = query;

//    if (wikiData && wikiData.thumbnail && wikiData.thumbnail.url) {
//        imageUrl = wikiData.thumbnail.url;
//        description = wikiData.description || description;
//    } else {
//        imageUrl = getRandomFallbackImage();
//    }

//    const authorData = {
//        Name: author.name,
//        Image: imageUrl,
//        Description: description,
//    };

//    authorsData.push(authorData);

//    // For debugging
//    //console.log(`Fetched data for ${author.name}:`, authorData);
//}

//function ajaxCall(method, url, data, successCallback, errorCallback) {
//    const xhr = new XMLHttpRequest();
//    xhr.open(method, url, true);
//    xhr.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');
//    xhr.onload = function () {
//        //console.log(`Response Status: ${xhr.status}`);
//        //console.log(`Response Text: ${xhr.responseText}`);
//        if (xhr.status >= 200 && xhr.status < 300) {
//            successCallback(xhr.responseText);
//        } else {
//            //console.error(`Error: ${xhr.status} - ${xhr.statusText}`);
//           // console.error(`Response: ${xhr.responseText}`);
//            errorCallback(xhr.statusText);
//        }
//    };
//    xhr.onerror = function () {
//        console.error(`Error: ${xhr.statusText}`);
//        errorCallback(xhr.statusText);
//    };
//    xhr.send(data);
//}

//function insertAuthorsToDBSCB(stats) {
//    console.log('Success:', stats);
//}

//function insertAuthorsToDBECB(err) {
//    console.error('Error:', err);
//}

//const authors = [
//    "Adolphus Alfred Jack", "Alexander Ellis", "Alexander Whyte", "Alexis Soyer",
//        "André Nusselder", "Armin Schwegler", "Arnold Bennett", "Arthur Edward Waite",
//        "Avery Hart", "Barney Simon", "Ben East", "Bert Greene", "Brian Falkner",
//        "Bryce Lease", "Carl E. Ricketts", "Charity Dye", "Charles Allston Collins",
//        "Charles Henry Webb", "Christene Thompson", "Clifford Blair", "Dan Rebellato",
//        "David Garrick", "Don Coldsmith", "Douglas Nicholas", "Edward Robins",
//        "Elizabeth Cleghorn Gaskell", "Enid Blyton", "Ernest William Hornung",
//        "Evelyn Martinengo Cesaresco", "Gerhart Hauptmann", "Grace Livingston Hill",
//        "Guy Boothby", "Henry James", "Henry Notaker", "Hergé", "Hugh Lofting",
//        "Iris Prouty O'Leary", "James Ballantyne", "James Edwin Thorold Rogers",
//        "Jane Garmey", "Janelle Jenstad", "Jean Anderson", "Jeannie B. Thomas",
//        "Jennifer Roberts-Smith", "Jill Ciment", "John Dryden", "Jonas Lie",
//        "Joseph Conrad", "Joseph Jacobs", "Joseph Planta", "Karen Mahoney",
//        "Kimberly Derting", "Knightley William Horlock", "Light Tuchihi",
//        "Louis DuPont Syle", "Ludwig Herrig", "Lynette Hunter", "Marc Monnier",
//        "Maria Edgeworth", "Maria M. Delgado", "Mariano Velázquez de la Cadena",
//        "Mark Kaethler", "Mbongeni Ngema", "Patricia Pellicane", "Paul Shipton",
//        "Percy Mtwa", "Peter Lichtenfels", "Philip Turner", "Richard Burton",
//        "Richard Green Moulton", "Robert Kemp Philp", "Roy", "Sandra Canfield",
//        "Saranne Dawson", "Seddon Johnson", "Shannon Messenger", "Soman Chainani",
//        "Tim Bowler", "Tom Tancin", "Ursula K. Le Guin", "Valerie Traub", "Vicki Page",
//        "Walter Scott", "William Henry Giles Kingston"
//];

//(async () => {
//    for (const author of authors) {
//        await getAuthorInfo(author);
//    }

//    authorsData.forEach(author => {
//        let i = 0;
//        if (author.Name == "Whyte, Alexander") {
//            author.Name = "Alexander Whyte";
//        }
//        const authorData1 = {
//            Name: author.Name,
//            Image: author.Image,
//            Description: author.Description,
//        };
//     //   console.log(authorData1);
//        const api1 = `${apiStart}/Authors/InsertImages/AuthorName/${encodeURIComponent(authorData1.Name)}/AuthorImage/${encodeURIComponent(authorData1.Image)}/Description/${encodeURIComponent(authorData1.Description)}`;
//        //console.log(`API URL: ${api1}`);
//        ajaxCall("PUT", api1, JSON.stringify(authorData1), insertAuthorsToDBSCB, insertAuthorsToDBECB);
//        //console.log(i++);
//    });
//})();

//////////////////////////////////////////////////////////////////////////////////////////


const apiStart = 'https://localhost:7291/api';
let currentPage = 1; // Start from the first page
const pageSize = 10; // Number of authors per page
const array = []; // Store fetched authors

$(document).ready(function () {
    getFromServer(currentPage, pageSize);

    $('#prevPageBtn').click(function () {
        if (currentPage > 1) {
            currentPage--;
            $('#numOfPages').text("Page " + currentPage);
            getFromServer(currentPage, pageSize);
        }
    });

    $('#nextPageBtn').click(function () {
        currentPage++;
        $('#numOfPages').text("Page " + currentPage);
        getFromServer(currentPage, pageSize);
    });

    // Close the modal when the user clicks on the close button
    $(document).on('click', '.btn-close', function () {
        $('#authorModal').modal('hide');
    });

    // Close the modal if the user clicks outside of the modal content
    $(window).click(function (event) {
        if ($(event.target).is('#authorModal')) {
            $('#authorModal').modal('hide');
        }
    });
});

function getFromServer(page, pageSize) {
    console.log("Fetching authors from server");
    let api = `${apiStart}/Authors/get10AuthorsPerPage/${page}/${pageSize}`;
    ajaxCall("GET", api, "", getAuthorsSCB, getAuthorsECB);
}

function getAuthorsSCB(authors) {
    console.log("Authors received:", authors);
    if (Array.isArray(authors)) {
        renderAuthors(authors);
    } else {
        console.error("Expected an array but got:", authors);
    }

    $('#prevPageBtn').prop('disabled', currentPage === 1);
    if (authors.length < pageSize) {
        $('#nextPageBtn').hide();
        alert("There are no more authors!");
    } else {
        $('#nextPageBtn').show();
    }

    // Update the global authors array
    array.length = 0; // Clear the array before pushing new data
    array.push(...authors);
}

function getAuthorsECB(err) {
    console.log(err);
}

function renderAuthors(authors) {
    const instructorsList = $('#Authors-list');
    instructorsList.empty();
    authors.forEach(function (author) {
        const authorBox = `
        <li class="item" draggable="true" data-author-name="${author.name}">
            <div class="details">
                <img src="${author.image || 'default-image-url'}" alt="${author.name || 'No Name'}">
                <p><strong>${author.name || 'No Name'}</strong></p>
            </div>
            <i class="uil uil-draggabledots"></i>
            <button class="show-Author-Books-button">Show books</button>
        </li>
        `;
        instructorsList.append(authorBox);
    });

    // Handle item click for fetching author details
    $(".item").click(function () {
        const authorName = $(this).data("author-name");
        fetchAuthorDetails(array, authorName);
    });

    // Handle "Show books" button click for each author
    $(".show-Author-Books-button").click(function (event) {
        event.stopPropagation();
        const authorName = $(this).closest(".item").data("author-name");
        console.log(authorName);
        localStorage.setItem("authorName", authorName);
        window.location.href = "http://localhost:60430/Html/AuthorsBooks.html";
    });
}

function fetchAuthorDetails(List, name) {
    const author = List.find(a => a.name === name);
    if (author) {
        showModal(author);
    }
}

function getAuthorName(authorName) {

}

function showModal(author) {
    $("#modal-author-name").text(author.name || 'No Name');
    $("#modal-author-image").attr("src", author.image || 'default-image-url');
    $("#modal-author-bio").text(author.description || 'No Bio Available');
    $("#modal-author-topWork").text(`Top Work: ${author.topWork || 'No Top Work'}`);
    $("#modal-author-workCount").text(`Work Count: ${author.workCount || 'No Work Count'}`);
    $("#authorModal").modal('show');
}