const apiStart = `https://localhost:7291/api/`;



$(document).ready(function() {
  LoginRegisterModalFunc();
});


function checkUserStatus() {
  const userData = sessionStorage.getItem('userData');

  if (userData) {
    // logged
    $('#loginBtn').hide();
    $('#registerBtn').hide();
    $('#logoutBtn').show();
  } else {
    // not logged
    $('#loginBtn').show();
    $('#registerBtn').show();
    $('#logoutBtn').hide();
  }
}

function LoginRegisterModalFunc() {

  $("#registerEmail").on("input", function () {
    var pattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    var value = $(this).val();
    var message = "Please match the requested format: example123@example.com";
    this.setCustomValidity(value.match(pattern) ? "" : message);
  });

  $("#registerName").on("input", function () {
    var pattern = /^[a-zA-Z\s]*$/;
    var value = $(this).val();
    var message = "Please enter only letters and spaces.";
    this.setCustomValidity(value.match(pattern) ? "" : message);
  });

  $('#loginBtn').click(function() {
    $('#loginModal').show();
  });

  $('#registerBtn').click(function() {
    $('#registerModal').show();
  });

  $('#closeLoginBtn').click(function() {
    $('#loginModal').hide();
    $('#loginForm')[0].reset(); 
  });

  $('#closeRegisterBtn').click(function() {
    $('#registerModal').hide();
    $('#registerForm')[0].reset(); 
  });

  $('#openRegisterModalFooter').click(function() {
    $('#loginModal').hide();
    $('#registerModal').show();
    $('#loginForm')[0].reset();
  });

  $('#openLoginModalFooter').click(function() {
    $('#registerModal').hide();
    $('#loginModal').show();
    $('#registerForm')[0].reset(); 
  });

  $("#loginForm").submit(function (e) {
    e.preventDefault();
    loginUser($('#loginEmail').val(), $('#loginPassword').val());
  });

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

  function loginSuccess(response) {
    //console.log(response);
    if (response.isActive === false) {
      Swal.fire({
        icon: 'error',
        title: 'User is banned',
        text: 'User is banned'
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
      $('#loginModal').hide();
      $('#loginForm')[0].reset();
      $('#registerBtn').hide();
      $('#loginBtn').hide();
      $('#logoutBtn').show();
    });
  }

  function loginError(err) {

    if (err.responseJSON.message === 'User does not exist') {
      Swal.fire({
        icon: 'error',
        title: 'User does not exist',
        text: err.responseJSON.message
      });
      return;
    }

    if (err.responseJSON.message === 'Incorrect password') {
      Swal.fire({
        icon: 'error',
        title: 'Incorrect password',
        text: err.responseJSON.message
      });
      return;
    }
    
    Swal.fire({
      icon: 'error',
      title: 'Login Failed',
      text: err.responseJSON.error
    });
    console.log(err);
  }

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

  function registerSuccess(response) {
    Swal.fire({
        icon: 'success',
        title: 'Success!',
        text: 'Registration successful'
    }).then(() => {
        loginUser($('#registerEmail').val(), $('#registerPassword').val());
        $('#registerModal').hide();
        $('#registerForm')[0].reset();
    });
   
  }

  function registerError(err) {
      Swal.fire({
          icon: 'error',
          title: 'Registration Failed',
          text: err.responseJSON.error
      });
  }
  
  $('#logoutBtn').click(function() {
    sessionStorage.removeItem('userData');
    $('#loginBtn').show();
    $('#registerBtn').show();
    $('#logoutBtn').hide();
  });
}



