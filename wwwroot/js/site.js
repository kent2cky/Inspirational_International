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

$(".rsvp-ok").click(function () {
  $(".rsvp-input-container").css("display", "flex");
  $(".rsvp-cover").hide();
  console.log("Show clicked!");
});

// Submit rsvp form
$("#submit-rsvp").click(function () {
  // Hide the form view
  $(".rsvp-input-container").css("display", "none");
  //Show the rsvp-cover view.
  $(".rsvp-cover").show();
  console.log("Submitting the form....");
  var obj = $("#rsvp-form").serializeToJSON();

  var payload = JSON.stringify(obj);

  $.ajax({
    url: "/RSVP/SubmitRSVP",
    type: "POST",
    dataType: "text",
    contentType: "application/json; charset=utf-8",
    data: payload,
    processData: false,
    success: function (response) {
      console.log("from success. %0", JSON.stringify(response));
      alert(response);
    },
    error: function (response) {
      console.log(response);
      if (response.status == 401) {
        console.log("Redirecting to login page......");
        window.location.href = "/Identity/Account/Login?ReturnUrl=%2FHome?RSVP=true";
        console.log(response.statusText);
      }
      console.log(response.responseText);
    }

  });

  console.error("Form submitted!");

  return;
});


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