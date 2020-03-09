// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("div.nav_handle-container").click(function () {
  if ($(".toggled").is(":visible")) {
    $(".toggled").hide(500);
    return;
  }
  $(".toggled").show(500);
});


function sendRSVPtoServer(callback) {
  try {
    $.ajax({
      url: "/RSVP/SubmitRSVP",
      type: "GET",
      success: function (response) {
        callback(0);
      },
      error: function (response) {
        if (response.status == 401) {
          //Redirect user to sign in page
          window.location.href =
            "/Identity/Account/Login?ReturnUrl=%2FRSVP%2FSubmitRSVPs";
        } else {
          callback(1);
          sendErrorReportToServer("sendRSVPtoServer()", response.responseText);
        }
      }
    });
  } catch (error) {
    callback(1);
    sendErrorReportToServer("sendRSVPtoServer()", error);
  }
}

function setLocalStorage(data) {
  if (!data) {
    return 1; // Returns 1 if not successful.
  }
  try {
    var localStorage = window["localStorage"];

    var usrData = {
      _rsvp: data.rsvp,
      _FN: data.firstName,
      _hasPN: data.phoneNumber
    };
    localStorage.setItem("InspIntData", JSON.stringify(usrData));
    // returns 0 if successful
    return 0;
  } catch (error) {
    sendErrorReportToServer("setLocalStorage()", error);
    return 1; // returns 1 if not successful.
  }
}

function setSession(data) {
  // Parameter check.
  if (!data) {
    return 1;
  }
  try {
    var usrData = {
      _rsvp: data.rsvp,
      _FN: data.firstName,
      _hasPN: data.phoneNumber
    };
    sessionStorage.setItem("InspIntData", JSON.stringify(usrData));
    //returns 0 if successful
    return 0;
  } catch (error) {
    sendErrorReportToServer("setSession()", error);
  }
}

function requestViewModelFromServer(callback) {
  try {
    $.ajax({
      url: "/Home/GetViewModel",
      type: "GET",
      success: function (response) {
        callback(response);
      },
      error: function (response) {
        sendErrorReportToServer(
          "requestViewModelFromServer()",
          response.responseText
        );
      }
    });
  } catch (error) {
    sendErrorReportToServer("requestViewModelFromServer()", error);
  }
}

function getViewModel() {
  try {
    if (localStorage.length !== 0) {
      var viewModel = JSON.parse(localStorage.getItem("InspIntData"));
      return viewModel;
    } else if (sessionStorage.length !== 0) {
      var viewModel = JSON.parse(sessionStorage.getItem("InspIntData"));
      return viewModel;
    } else {
      sendErrorReportToServer(
        "getViewModel()",
        "Returning null value as view model!"
      );
      return null;
    }
  } catch (error) {
    sendErrorReportToServer(
      "getViewModel()",
      error + " " + "Returning null value as view model!"
    );
    return null;
  }
}

function appendDateOfNextClass(dateOfNextClass) {
  // Check if parameter is a valid date data
  if (
    !dateOfNextClass ||
    new Date(dateOfNextClass).toString() === "Invalid Date"
  ) {
    return 1; //Returns 1 if error or unsuccessful
  }

  var showNextClass = document.getElementsByClassName("showNextClass");
  var nextClass = document.createElement("span");

  // Replace the time part so you can effectively compare both dates
  var splitModelDate = dateOfNextClass.split("T")[0];
  var splitLocalDate = new Date().toISOString().split("T")[0];

  var dateOfClass = new Date(splitModelDate + "T00:00:00Z").toLocaleDateString(
    undefined, // undefined allows the browser to set local timezone and calculate appropriate offsets
    {
      day: "numeric",
      month: "short",
      year: "numeric"
    }
  );
  var today = new Date(splitLocalDate + "T00:00:00Z").toLocaleDateString(
    undefined, // undefined allows the browser to set local timezone and calculate appropriate offsets
    {
      day: "numeric",
      month: "short",
      year: "numeric"
    }
  );

  if (dateOfClass === today) {
    nextClass.innerText = " today at 1:00pm."; // replace date with 'today' ....
    showNextClass[0].appendChild(nextClass.cloneNode(true));
    showNextClass[1].appendChild(nextClass);
    return 0; // Returns 0 if successful
  } else {
    nextClass.innerText = " on " + dateOfClass + ".";
    showNextClass[0].appendChild(nextClass.cloneNode(true));
    showNextClass[1].appendChild(nextClass);
    return 0; // Returns 0 if successful
  }
}

function appendUsersFirstName(firstName) {
  if (firstName === null) {
    return 1; //Returns 1 if error or unsuccessful
  }

  try {
    var salute = document.getElementsByClassName("salute");
    salute[0].innerText = "Hello " + firstName + ",";
    salute[1].innerText = "Hello " + firstName + ",";
  } catch (error) {
    console.error(error);
    return 1;
  }
}

function display(model) {
  if (!model) {
    console.error("Supply a valid viewModel as parameter.");
    return 1;
  }

  var willYouRSVP = document.getElementById("will-you-rsvp");
  var getPhoneNumber = document.getElementById("get-phoneNumber");
  var rsvpDone = document.getElementById("rsvp-done");
  var preloader = document.getElementById("preloader");

  try {
    if (model._rsvp === true) {
      if (model._hasPN === "false") {
        // NOTE: model._hasPN is a string not a bool
        willYouRSVP.style.display = "none";
        rsvpDone.style.display = "none";
        preloader.style.display = "none";
        getPhoneNumber.style.display = "block";
        return 0;
      }
      getPhoneNumber.style.display = "none";
      willYouRSVP.style.display = "none";
      preloader.style.display = "none";
      rsvpDone.style.display = "block";
      if (model._hasPN === "declined") {
        // Add button to collect the phone number in case
        // the user changes their mind.
        var submitPhoneNumber = document.createElement("button");
        submitPhoneNumber.classList.add("submitPhoneNumber");
        submitPhoneNumber.innerText = "Submit PhoneNumber";
        submitPhoneNumber.addEventListener("click", function () {
          rsvpDone.style.display = "none";
          getPhoneNumber.style.display = "block";
          rsvpDone.removeChild(submitPhoneNumber);
        });
        rsvpDone.appendChild(submitPhoneNumber);
      }
      return 0;
    } else {
      rsvpDone.style.display = "none";
      getPhoneNumber.style.display = "none";
      preloader.style.display = "none";
      willYouRSVP.style.display = "block";
      return 1;
    }
  } catch (error) {
    return 1;
  }
}

$(".rsvp-cover").ready(function () {
  // Get view model from the browser local storage or session storage
  var viewModel = getViewModel();
var dateOfNextClass = sessionStorage.getItem("_dateOfNextClass");
// Replace the time part so you can effectively compare both dates
  var splitModelDate = dateOfNextClass.split("T")[0];
  var splitLocalDate = new Date().toISOString().split("T")[0];
  console.log(dateOfNextClass);
  
  var dateOfClass = new Date(splitModelDate + "T00:00:00Z").toLocaleDateString(
    undefined, // undefined allows the browser to set local timezone and calculate appropriate offsets
    {
      day: "numeric",
      month: "short",
      year: "numeric"
    }
  );
  var today = new Date(splitLocalDate + "T00:00:00Z").toLocaleDateString(
    undefined, // undefined allows the browser to set local timezone and calculate appropriate offsets
    {
      day: "numeric",
      month: "short",
      year: "numeric"
    }
  );
  
  
  // If nothing is stored in session or local storage then request the data from server
  // and save them in the session or local storage
  if (!viewModel || dateOfClass < today) {
    requestViewModelFromServer(function (response) {
      // Set response to local storage
      if (setLocalStorage(response) !== 0) {
        // Set response to session storage if not successfully set in local storage
        setSession(response);
      }
      // Reload the page so we can access the stored data.
      window.location.reload();
    });
  }

  //Dynamically append users firstName
  appendUsersFirstName(viewModel._FN);
  
  // Dynamically append date of next class.
  appendDateOfNextClass(dateOfNextClass);
  // Display appropriate rsvp view
  display(viewModel);
});

//Overwrite user data on sign out
$("#sign-out-btn").click(function () {
  var newModel = getViewModel();
  // Set all data to default except dateOfNextClass
  newModel._rsvp = false;
  newModel._FN = null;
  newModel._hasPN = null;
  // Save it back to local storage
  try {
    localStorage.setItem("InspIntData", JSON.stringify(newModel));
  } catch (error) {
    sessionStorage.setItem("InspIntData", JSON.stringify(newModel));
  }
});

//Submit RSVP
$("#rsvp-ok").click(function () {
  var willYouRSVP = document.getElementById("will-you-rsvp");
  var preloader = document.getElementById("preloader");

  willYouRSVP.style.display = "none";
  preloader.style.display = "block";


  setTimeout(function () {
    sendRSVPtoServer(function (response) {
      // Return early if there is error.
      if (response !== 0) {
        alert("Something went wrong while submiting your rsvp. Please try again later.");
        willYouRSVP.style.display = "block";
        preloader.style.display = "none";
        return;
      }
      // Get model from local storage
      var newModel = getViewModel();
      // Set rsvp to true
      newModel._rsvp = true;
      // Save it back to local storage
      try {
        localStorage.setItem("InspIntData", JSON.stringify(newModel));
      } catch (error) {
        sessionStorage.setItem("InspIntData", JSON.stringify(newModel));
      }
      display(newModel);
    });
  }, 1000);

});

// If user declines submitting her/his phoneNumber
$("#decline-submit").click(function ($event) {
  $event.preventDefault();
  $("#phoneNumber-input").val("");
  $("#phoneNumber-input-error").css("display", "none");
  // Get model from local storage
  var newModel = getViewModel();
  // Set the hasPhoneNumber value to declined
  newModel._hasPN = "declined";
  try {
    var ls = window["localStorage"];
    ls.setItem("InspIntData", JSON.stringify(newModel));
  } catch (error) {
    ls.setItem("InspIntData", JSON.stringify(newModel));
  }
  display(newModel);
});

$("#phoneNumber-form").submit(function ($event) {
  $event.preventDefault();
  var phoneNumber = $("#phoneNumber-input").val();

  if (!/^[0-9]{11,15}$/.test(phoneNumber)) {
    $("#phoneNumber-input").val("");
    return;
  }

  var getPhoneNumber = document.getElementById("get-phoneNumber");
  var preloader = document.getElementById("preloader");

  getPhoneNumber.style.display = "none";
  preloader.style.display = "block";

  setTimeout(function () {
    sendUserPhoneNumberToServer(phoneNumber, function (response) {
      // Return early if there is error.
      if (response !== 0) {
        alert("Something went wrong while submiting your phone number. Please try again later.");
        willYouRSVP.style.display = "block";
        preloader.style.display = "none";
        return;
      }

      var newModel = getViewModel();
      newModel._hasPN = "true";
      // Save it back to local storage
      try {
        localStorage.setItem("InspIntData", JSON.stringify(newModel));
      } catch (error) {
        sessionStorage.setItem("InspIntData", JSON.stringify(newModel));
      }
      display(newModel);
    });
  }, 1000);
});

function sendErrorReportToServer(sender, errorMessage) {
  if (!sender || !errorMessage) {
    console.error(
      "Please pass a valid parameter to the sendErrorReportToServer function."
    );
    return;
  }
  try {
    var error = JSON.stringify({
      Sender: sender,
      ErrorMessage: errorMessage
    });
    $.ajax({
      url: "/Home/sendErrorReports",
      type: "POST",
      contentType: "application/json, charset utf8",
      dataType: "text",
      data: error,
      success: function (response) {
        console.log(response);
      },
      error: function (response) {
        console.log("Error from sendErrorReportToServer: " + response);
      }
    });
  } catch {
    console.error("Error from sendErrorReportToServer");
    return;
  }
}

function sendUserPhoneNumberToServer(phoneNumber, callback) {
  // Regex to match at least 11 digits
  if (!/^[0-9]{11,15}$/.test(phoneNumber)) {
    $("#phoneNumber-input").val("");
    return;
  }

  try {
    var data = {
      phoneNumber: phoneNumber
    };
    $.ajax({
      url: "/RSVP/saveUserPhoneNumber",
      type: "POST",
      contentType: "application/json, charset utf8",
      dataType: "text",
      data: JSON.stringify(data),
      success: function (response) {
        callback(0);
      },
      error: function (response) {
        if (response.status == 401) {
          //Redirect user to sign in page
          window.location.href = "/Identity/Account/Login?ReturnUrl=%2FHome";
        } else {
          sendErrorReportToServer(
            "sendUserPhoneNumberToServer()",
            response.responseText
          );
          callback(1);
          return;
        }
      }
    });
  } catch (error) {
    callback(1);
    console.error("Error from submitButton" + error);
  }
}


// the slideshow functionality

// function showSlides(n) {
//   var i;
//   var slides = document.getElementsByClassName("mySlides");
//   var dots = document.getElementsByClassName("dot");
//   if (n > slides.length) {slideIndex = 1}
//   if (n < 1) {slideIndex = slides.length}
//   for (i = 0; i < slides.length; i++) {
//       slides[i].style.display = "none";
//   }
//   for (i = 0; i < dots.length; i++) {
//       dots[i].className = dots[i].className.replace(" active", "");
//   }
//   slides[slideIndex-1].style.display = "block";
//   dots[slideIndex-1].className += " active";
// }

var slideIndex = 0;

// Next/previous controls
function plusSlides(n) {
  showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
  showSlides(slideIndex = n);
}

showSlides();

function showSlides() {
  var i;
  var slides = document.getElementsByClassName("mySlides");
  console.log('this is the number of slides: ', slides);
  for (i = 0; i < slides.length; i++) {
    slides[i].style.display = "none";
  }
  slideIndex++;
  if (slideIndex > slides.length) {slideIndex = 1}
  slides[slideIndex-1].style.display = "block";
  setTimeout(showSlides, 5000); // Change image every 5 seconds
}
