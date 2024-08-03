// function LoginRegisterModalFunc() {
  // const loginModal = $('#loginModal');
  // const registerModal = $('#registerModal');
  // const loginBtn = $('#loginBtn');
  // const registerBtn = $('#registerBtn');
  // const closeBtns = $('.closeBtn');

  // $(window).on('click', function(event) {
  //     if ($(event.target).is(loginModal)) {
  //         loginModal.hide();
  //     } else if ($(event.target).is(registerModal)) {
  //         registerModal.hide();
  //     }
  // });

  // $('#loginForm').on('submit', function(e) {
  //     e.preventDefault();
  //     console.log('Login form submitted');
  //     loginModal.hide();
  // });

  // $('#registerForm').on('submit', function(e) {
  //     e.preventDefault();
  //     console.log('Register form submitted');
  //     registerModal.hide();
  // });
//}

// Handles the login/register forms and buttons
function LoginRegisterModalFunc() {

  // Opens the login modal
  $('#loginBtn').click(function() {
    $('#loginModal').show();
  });

  // Opens the registeration modal
  $('#registerBtn').click(function() {
    $('#registerModal').show();
  });

  // Closes the modals
  $('.closeBtn').click(function() {
    $('#loginModal').hide();
    $('#registerModal').hide();
  })

  // $('#openRegisterModalFooter').on('click', function(e) {
  //   // e.preventDefault(); check if its needed
  //   // To do - clear the form 
  //   $('#loginModal').hide();
  //   $('#registerModal').show();
  // });

  $('#openRegisterModalFooter').click(function() {
    // e.preventDefault(); check if its needed
    // To do - clear the form 
    $('#loginModal').hide();
    $('#registerModal').show();
  });   

  // $('#openLoginModalFooter').on('click', function(e) {
  //   // e.preventDefault(); check if its needed
  //   // To do - clear the form 
  //   e.preventDefault();
  //   registerModal.hide();
  //   loginModal.show();
  // });

  $('#openLoginModalFooter').click(function() {
    $('#registerModal').hide();
    $('#loginModal').show();
  });
}