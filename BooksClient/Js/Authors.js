const apiStart = 'https://localhost:7291/api';
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




$(document).ready(function () {

    //$('#GoHomeBtn').click(function () {
    //    localStorage.removeItem("courses");
    //    window.location.href = "index.html";
    //});

    getFromServer()

    //$(document).on('click', '.showMoreCoursesOfThisInstructor', function () {
    //    getCoursesByInstructorId($(this).data("instructor-id"));
    //});
});

function getFromServer() {
    console.log("Hi from getFromServer");
    let api = `${apiStart}/Authors`;
/*    let api = `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/Instructors`;*/
    ajaxCall("GET", api, "", getAuthorsSCB, getAuthorsECB);
}
function getAuthorsSCB(authors) {
    console.log(authors);
    renderAuthors(authors);
}

function getAuthorsECB(err) {
    console.log(err);
}


function renderAuthors(authors) {
    var instructorsList = $("#Authors-list");
    instructorsList.empty();
    authors.forEach(function (author) {
        var authorBox = `
        <div class="profile-card-container">
            <div class="profile-card">
                <div class="card-image">
                    <img src="${author.image}" alt="${author.name}">
                </div>
                <div class="card-details">
                    <button class="showMoreInfoAboutThisAuthor" data-instructor-id="${author.name}">Show</button>
                    <div class="details-container">
                        <p id="profile-display-name" class="italic-text">Display name: ${author.name}</p>
                    </div>
                </div>
            </div>
        </div>
    `;
        instructorsList.append(authorBox);
    });

}