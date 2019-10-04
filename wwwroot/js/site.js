// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function Helloworld() {
  console.log("Hello from Inspiration International!!");
}

$("div.nav_handle-container").click(function () {
  if ($(".toggled").is(":visible")) {
    $(".toggled").hide(500);
    console.log("Show clicked!");
    return;
  }
  $(".toggled").show(500);
  console.log("Hide clicked!");
});

function sendRSVPtoServer(callback) {
  $.ajax({
    url: "/RSVP/SubmitRSVP",
    type: "GET",
    success: function (response) {
      console.log("from success: ", JSON.stringify(response));
      callback(response);
    },
    error: function (response) {
      if (response.status == 401) {
        console.log("Redirecting to login page......");
        window.location.href =
          "/Identity/Account/Login?ReturnUrl=%2FRSVP%2FSubmitRSVPs";
      } else {
        console.log("From error: " + response);
        callback(response);
      }
      console.log("From error: " + response.responseText);
    }
  });
}

function setLocalStorage(data) {
  if (!data) {
    console.log("Please supply data to set session storage with.");
    return 1;
  }
  console.log("Saving data to local storage......");
  try {
    var localStorage = window["localStorage"];

    var usrData = {
      _rsvp: data.rsvp,
      _FN: data.firstName,
      _dateOfNextClass: data.dateOfNextClass,
      _hasPN: data.phoneNumber
    };
    localStorage.setItem("InspIntData", JSON.stringify(usrData));
    console.log("Data stored in localStorage.");
    // returns 0 if successful
    return 0;
  } catch (error) {
    console.error(error);
    //return 1 if not successful
    return 1;
  }
}

function setSession(data) {
  // Parameter check.
  if (!data) {
    console.log("Please supply data to set session storage with.");
    return 1;
  }
  console.log("Setting session storage instead.......");
  try {
    var usrData = {
      _rsvp: data.rsvp,
      _FN: data.firstName,
      _dateOfNextClass: data.dateOfNextClass,
      _hasPN: data.phoneNumber
    };
    sessionStorage.setItem("InspIntData", JSON.stringify(usrData));
    console.log("Data stored in session storage.");

    //returns 0 if successful
    return 0;
  } catch (error) {
    console.error("Error setting session data....." + error);
  }
}

function requestViewModelFromServer(callback) {
  try {
    $.ajax({
      url: "/Home/GetViewModel",
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
    console.error("My own error log: " + error);
  }
}

function getViewModel() {
  try {
    localStorage = window["localStorage"];
    if (localStorage.length !== 0) {
      var viewModel = JSON.parse(localStorage.getItem("InspIntData"));
      console.log("Retrieved from localStorage: " + viewModel._dateOfNextClass);
      return viewModel;
    } else if (sessionStorage.length !== 0) {
      var viewModel = JSON.parse(sessionStorage.getItem("InspIntData"));
      console.log(
        "Retrieved from sessionStorage: " + viewModel._dateOfNextClass
      );
      return viewModel;
    } else {
      return null;
    }
  } catch (error) {
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
    var salute = document.getElementsByClassName("salute")
    salute[0].innerText = "Hey " + firstName + ",";
    salute[1].innerText = "Hey " + firstName + ",";
  } catch (error) {
    console.error(error);
    return 1;
  }
}

function display(model) {
  if (!model) {
    console.log("Supply a valid model data.");
    return 1;
  }

  var willYouRSVP = document.getElementById("will-you-rsvp");
  var getPhoneNumber = document.getElementById("get-phoneNumber");
  var rsvpDone = document.getElementById("rsvp-done");

  try {
    if (model._rsvp === true) {
      console.log(model._rsvp);
      if (model._hasPN === "false") {
        // NOTE: model._hasPN is a string not a bool
        console.log("Does he have phoneNumber? " + model._hasPN);
        willYouRSVP.style.display = "none";
        rsvpDone.style.display = "none";
        getPhoneNumber.style.display = "block";
        return 0;
      }
      getPhoneNumber.style.display = "none";
      willYouRSVP.style.display = "none";
      rsvpDone.style.display = "block";
      return 0;
    } else {
      rsvpDone.style.display = "none";
      getPhoneNumber.style.display = "none";
      willYouRSVP.style.display = "block";
      return 1;
    }
  } catch (error) {
    console.error(error + model);
    return 1;
  }
}

$(".rsvp-cover").ready(function () {
  // Get view model from the browser local storage or session storage
  var viewModel = getViewModel();

  // If nothing is stored in session or local storage then request the data from server
  // and save them in the session or local storage
  if (viewModel === null) {
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
  appendDateOfNextClass(viewModel._dateOfNextClass);
  // Display appropriate rsvp view
  display(viewModel);
});


//Overwrite user data on sign out
$("#sign-out-btn").click(function () {
  var newModel = getViewModel();
  console.log("This is old rsvp status: " + newModel._rsvp);
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
  sendRSVPtoServer(function (response) {
    $(".rsvp-cover>#will-you-rsvp").replaceWith(response);
    // Get model from local storage
    var newModel = getViewModel();
    console.log("This is old rsvp status: " + newModel._rsvp);
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
});

// If user declines submitting her/his phoneNumber
$("#decline-submit").click(function ($event) {
  $event.preventDefault();
  // Get model from local storage
  var newModel = getViewModel();
  // Set the hasPhoneNumber value to declined
  newModel._hasPN = "declined";
  console.log("This is current rsvp status: " + JSON.stringify(newModel));
  try {
    var ls = window["localStorage"];
    ls.setItem("InspIntData", JSON.stringify(newModel));
  } catch (error) {
    ls.setItem("InspIntData", JSON.stringify(newModel));
  }
  display(newModel);
});