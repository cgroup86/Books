$(document).ready(function () {

  $('.showScoresBtn').click(function() {
    $.ajax({
        url: 'https://localhost:7291/api/UsersScores/GetUserAndTopScores/userId/0',
        type: 'GET',
        dataType: 'json',
        success: function(data) {
            $('#showUserScores').html(`
                <p><strong>User Score:</strong> ${data.score}</p>
            `);

            $('#showTopScoresList').empty();
            data.topUserScores.forEach(function(score) {
                $('#showTopScoresList').append(`
                    <li>${score.userName}: ${score.score}</li>
                `);
            });

            $('#showScoresModal').modal('show');
        },
        error: function(err) {
            console.error('Error fetching data:', err);
        }
    });
  });

  // Close the "Show Scores" modal
  $('#closeShowScoresBtn').click(function() {
      $('#showScoresModal').modal('hide');
  });


// ------------------------------------------------------------------------------------------------

function submitScore(userId, score) {
    //console.log("HIHIHIHIHIHIHI");
    const api = 'https://localhost:7291/api/UsersScores/GetUserAndTopScores/userId/0';

    ajaxCall("GET", api, null, submitScoreSCB, submitScoreECB);
}

function submitScoreSCB(data) {
    $('#userScores').html(`
          <p><strong>User Score:</strong> ${data.score}</p>
        `);

        $('#topScoresList').empty();
        data.topUserScores.forEach(function(score) {
          $('#topScoresList').append(`
            <li>${score.userName}: ${score.score}</li>
          `);
        });

        $('#scoresModal').modal('show');
}

function submitScoreECB(error) {
  console.error('Error fetching data:', error);
}
// ------------------------------------------------------------------------------------------------

let currentQuestionIndex = 0;
let score = 0;
let timer;
const totalQuestions = 5;
const questions = []; 
const userId = 1; 

function startTimer() {
    let time = 59;
    $('#timer').text(`Time: ${time}s`);
    timer = setInterval(() => {
        time--;
        $('#timer').text(`Time: ${time}s`);
        if (time <= 0) {
            clearInterval(timer);
            submitAnswer();
        }
    }, 1000);
}

function loadQuestion(index) {
    $.ajax({
        url: `https://localhost:7291/api/questions/GetQuestion${index + 1}`,
        method: 'GET',
        success: function (data) {
            questions[index] = data; 
            renderQuestion(data);
            startTimer();
        },
        error: function () {
            alert('Failed to load question.');
        }
    });
}

// ------------------------------------------------------------------------------------------------

function renderQuestion(question) {
    $('#questionContainer').html(`
        <h3>${question.questionText}</h3>
        ${question.answers.map((answer, idx) => {
            if (typeof answer === 'object') {
                return `
                    <div class="form-check">
                        <input class="form-check-input" type="radio" name="answer" value="${answer.wrongAnswerTitle}" id="answer${idx}">
                        <label class="form-check-label" for="answer${idx}">
                            <img src="${answer.wrongAnswerImage}" class="answer-image" alt="Answer Image">
                            ${answer.wrongAnswerTitle}
                        </label>
                    </div>
                `;
            } else {
                return `
                    <div class="form-check">
                        <input class="form-check-input" type="radio" name="answer" value="${answer}" id="answer${idx}">
                        <label class="form-check-label" for="answer${idx}">
                            ${answer}
                        </label>
                    </div>
                `;
            }
        }).join('')}
    `);
}

// ------------------------------------------------------------------------------------------------

function submitScore(userId, score) {
    //console.log("HIHIHIHIHIHIHI");
    const api = `https://localhost:7291/api/UsersScores`;

    const score20Times = score * 20; 
    const userScore = {
        userId: userId,
        score: score20Times,
        topUserScores: [] 
    };

    ajaxCall("PUT", api, JSON.stringify(userScore), submitScoreSCB, submitScoreECB);
}

function submitScoreSCB(stats) {
    //console.log('Score submitted successfully.');
}

function submitScoreECB(err) {
    console.error('Failed to submit score.');
}

// ------------------------------------------------------------------------------------------------

function submitAnswer() {
    clearInterval(timer);
    const selectedAnswer = $('input[name="answer"]:checked').val();
    if (selectedAnswer) {
        if (questions[currentQuestionIndex] && questions[currentQuestionIndex].correctAnswer) {
            const correctAnswer = questions[currentQuestionIndex].correctAnswer;
            if (selectedAnswer === correctAnswer) {
                score++;
            }
        } else {
            console.error('Current question data is not available.');
        }
    }
    currentQuestionIndex++;
    if (currentQuestionIndex < totalQuestions) {
        loadQuestion(currentQuestionIndex);
    } else {
        $('#quizPage').addClass('d-none');
        $('#scorePage').removeClass('d-none');
        $('#score').text(`Your score: ${score}/${totalQuestions}`);
        submitScore(userId, score); 
    }
}

$('#startQuiz').on('click', function () {
    $('#homePage').addClass('d-none');
    $('#quizPage').removeClass('d-none');
    $('#exitQuiz').show();
    loadQuestion(currentQuestionIndex);
});

$('#submitAnswer').on('click', function () {
    submitAnswer();
});

$('#startNewQuiz').on('click', function () {
    $('#scorePage').addClass('d-none');
    $('#quizPage').removeClass('d-none');
    currentQuestionIndex = 0;
    score = 0;
    loadQuestion(currentQuestionIndex);
});

$('#exitQuiz').on('click', function () {
    clearInterval(timer);
    currentQuestionIndex = 0; 
    score = 0; 
    $('#quizPage').addClass('d-none'); 
    $('#homePage').removeClass('d-none');
    $('#exitQuiz').hide(); 
});

$('#exitQuiz').hide();
});