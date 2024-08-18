// TODO - clear the forms when close/ move
// TODO - when logged in successfully remove the register and show logout button
// TODO - a 5 second loading page shows at the start of everypage, don't have to be part of the header
// TODO - NOT ON JS - ON SERVER SIDE - readAuthorsByPage FIX HOW ITS MADE IN DBservices.cs
const apiStart = `https://localhost:7291/api/`;

// Handles the login/register forms and buttons
function LoginRegisterModalFunc() {
  

  // Email Validation
  $("#registerEmail").on("input", function () {
    var pattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    var value = $(this).val();
    var message = "Please match the requested format: example123@example.com";
    this.setCustomValidity(value.match(pattern) ? "" : message);
  });

  // Name Validation
  $("#registerName").on("input", function () {
    var pattern = /^[a-zA-Z\s]*$/;
    var value = $(this).val();
    var message = "Please enter only letters and spaces.";
    this.setCustomValidity(value.match(pattern) ? "" : message);
  });

  // Opens the login modal
  $('#loginBtn').click(function() {
    $('#loginModal').show();
  });

  // Opens the registration modal
  $('#registerBtn').click(function() {
    $('#registerModal').show();
  });

  // Closes the modals
  $('.closeBtn').click(function() {
    $('#loginModal').hide();
    $('#registerModal').hide();
  });

  // Change form from login to register
  $('#openRegisterModalFooter').click(function() {
    $('#loginModal').hide();
    $('#registerModal').show();
  });

  // Change form from register to login
  $('#openLoginModalFooter').click(function() {
    $('#registerModal').hide();
    $('#loginModal').show();
  });

  // Login submission
  $("#loginForm").submit(function (e) {
    e.preventDefault();
    loginUser($('#loginEmail').val(), $('#loginPassword').val());
  });

  // Login function
  function loginUser(email, password) {
    let api = `${apiStart}Users/Login`;
    const loginData = {
      Id: 0,
      Name: "0",
      Email: email,
      Password: password,
      IsAdmin: false,
      IsActive: true,
    };

    ajaxCall("POST", api, JSON.stringify(loginData), loginSuccess, loginError);
  }

  // Success handler for login
  function loginSuccess(response) {
    if (response.isActive === false) {
      Swal.fire({
        icon: 'error',
        title: 'User is banned',
        text: response.error
      });
      return;
    }

    if (response.isAdmin === true) {
      // TODO: Redirect to admin panel
      return;
    }

    const userData = {
      userId: response.id,
      userName: response.name,
    };

    sessionStorage.setItem('userData', JSON.stringify(userData));

    Swal.fire({
      icon: 'success',
      title: 'Success!',
      text: 'Login successful'
    }).then(() => {
      // TODO: Close the modal - clean the modal
    });
  }

  // Error handler for login
  function loginError(err) {
    Swal.fire({
      icon: 'error',
      title: 'Login Failed',
      text: err.responseJSON.error
    });
    console.log(err);
  }

  // Register submission
  $("#registerForm").submit(function (e) {
    e.preventDefault();
    let api = `${apiStart}Users/Register`;

    const registerData = {
        Id: 0,
        Name: $('#registerName').val(),
        Email: $('#registerEmail').val(),
        Password: $('#registerPassword').val(),
        IsAdmin: false,
        IsActive: true,
    };

    ajaxCall("POST", api, JSON.stringify(registerData), registerSuccess, registerError);
  });

  // Success handler for registration
  function registerSuccess(response) {
    Swal.fire({
        icon: 'success',
        title: 'Success!',
        text: 'Registration successful'
    }).then(() => {
        // Automatically log in the user after successful registration
        loginUser($('#registerEmail').val(), $('#registerPassword').val());
    });
  }

  // Error handler for registration
  function registerError(err) {
      Swal.fire({
          icon: 'error',
          title: 'Registration Failed',
          text: err.responseJSON.error
      });
  }
}