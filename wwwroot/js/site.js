// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function Helloworld() {
  console.log("Hello from Inspiration International!!");
}

$("div.nav_handle-container").click(() => {
  if ($(".toggled").is(":visible")) {
    $(".toggled").hide(500);
    console.log("Show clicked!");
    return;
  }
  $(".toggled").show(500);
  console.log("Hide clicked!");
});

$(".rsvp-ok").click(() => {
  $(".rsvp-input-container").css("display", "flex");
  $(".rsvp-cover").hide();
  console.log("Show clicked!");
});


// If user declines submitting her/his phoneNumber
$("#decline-submit").click(
  () => {
    console.log("Decline button clicked....");
    alert("There would be no way to contact you without your phone number.");
    localStorage.setItem("_PNdeclined", "True");
    $(".rsvp-cover>fieldset").replaceWith(
      "Thanks for the RSVP. See you on Sunday."
    );
  }
);



// Submit RSVP
$("#rsvp-click").click(() => {
  var data = JSON.stringify({
    RSVP: true,
    PhoneNumber: 23456789123,
    FirstName: " sdsds",
    PictureData: "23456789123"
  });
  $.ajax({
    url: "/RSVP/SubmitRSVP",
    type: "POST",
    dataType: "html",
    data: data,
    contentType: "application/json; charset=utf-8",
    processData: false,
    success: (response) => {
      console.log("from success. %0", JSON.stringify(response));
      //alert(response);
      $(".rsvp-cover>p").replaceWith(response);
    },
    error: (response) => {
      console.log(response);
      $(".rsvp-cover").append(response.responseText);
      if (response.status == 401) {
        console.log("Redirecting to login page......");
        window.location.href =
          "/Identity/Account/Login?ReturnUrl=%2FRSVP%2FSubmitRSVPs";
        console.log(response.statusText);
      } else {
        alert("Error sending your RSVP. Please try again later.");
      }
      console.log(response.responseText);
    }
  });
});




// Submit rsvp form
// $("#submit-rsvp").click(function () {
//   // Hide the form view
//   $(".rsvp-input-container").css("display", "none");
//   //Show the rsvp-cover view.
//   $(".rsvp-cover").show();
//   console.log("Submitting the form....");
//   var obj = $("#rsvp-form").serializeToJSON();

//   var payload = JSON.stringify(obj);
//   console.log(payload);

//   $.ajax({
//     url: "/RSVP/SubmitRSVP",
//     type: "POST",
//     dataType: "text",
//     contentType: "application/json; charset=utf-8",
//     data: payload,
//     processData: false,
//     success: function (response) {
//       console.log("from success. %0", JSON.stringify(response));
//       alert(response);
//     },
//     error: function (response) {
//       console.log(response);
//       if (response.status == 401) {
//         console.log("Redirecting to login page......");
//         //window.location.href = "/Identity/Account/Login?ReturnUrl=%2FHome%2FIndex?RSVP=true";
//         console.log(response.statusText);
//       }
//       console.log(response.responseText);
//     }
//   });

//   console.error("Form submitted!");

//   return;
// });

// function onSignIn(googleUser) {
//   var vv = googleUser.getAuthResponse().id_token;
//   console.log("This is the google id_token: " + vv);
//   var id_token = {
//     "id_token": vv
//   }
//   var id_token_string = JSON.stringify(id_token)
//   console.log("This is data: %0", id_token_string);
//   $.ajax({
//     url: "/Identity/GoogleTokenSignIn",
//     type: "POST",
//     dataType: 'text/json; charset=utf-8 ',
//     data: id_token_string,
//     contentType: 'text/json; charset=utf-8',
//     processData: false,
//     success: function (response) {
//       console.log("from success. %0", JSON.stringify(response));
//     },
//     error: function (response) {
//       console.log("from error. %0", JSON.stringify(response));
//     }
//   });

//   return;
// }

function setLocalStorage(data) {
  if (!data) {

    console.log('Please supply data to set session storage with.')
    return 1;
  }
  console.log('Saving data to local storage......');
  try {
    var localStorage = window['localStorage'];

    var usrData = {
      _rsvp: data.rsvp,
      _FN: data.firstName,
      _dateOfNextClass: data.dateOfNextClass,
      _hasPN: data.phoneNumber
    };
    localStorage.setItem('InspIntData', JSON.stringify(usrData));
    console.log('Data stored in localStorage.');
    // returns 0 if successful
    return 0
  } catch (error) {
    console.error(error);
    //return 1 if not successful
    return 1
  }

}


function setSession(data) {
  // Parameter check.
  if (!data) {
    console.log('Please supply data to set session storage with.');
    return 1;
  }
  console.log('Setting session storage instead.......');
  try {

    var usrData = {
      _rsvp: data.rsvp,
      _FN: data.firstName,
      _dateOfNextClass: data.dateOfNextClass,
      _hasPN: data.phoneNumber
    };
    sessionStorage.setItem('InspIntData', JSON.stringify(usrData));
    console.log('Data stored in session storage.');

    //returns 0 if successful
    return 0;
  } catch (error) {
    console.error('Error setting session data.....' + error);
  }
}




function requestViewModelFromServer(callback) {
  try {
    $.ajax({
      url: '/Home/GetViewModel',
      type: "GET",
      success: function (response) {
        console.log("from success. %0", response);
        callback(response);
      },
      error: function (response) {
        console.log(response.responseText);
      }
    });
  } catch (error) {
    console.error('My own error log: ' + error);
  }
}



function getViewModel() {
  try {
    localStorage = window['localStorage'];
    if (localStorage.length !== 0) {
      var viewModel = JSON.parse(localStorage.getItem('InspIntData'));
      console.log('Retrieved from localStorage: ' + viewModel._FN);
      return viewModel;
    } else if (sessionStorage.length !== 0) {
      var viewModel = JSON.parse(sessionStorage.getItem('InspIntData'));
      console.log('Retrieved from sessionStorage: ' + viewModel._FN);
      return viewModel;
    } else {
      return null;
    }
  } catch (error) {
    return null;
  }
}




$(window).on('load', function () {

  var viewModel = getViewModel();

  if (viewModel === null) {

    requestViewModelFromServer(function (response) {
      console.log('response from the callback ' + response);
      if (setLocalStorage(response) !== 0) {
        setSession(response);
      }
      window.location.reload(true);
    });
  }
  console.log('ViewModel after every every: ' + viewModel._FN);
});