// Navigation bar toggle functionality.
$("div.nav_handle-container").click(function () {
    if ($(".toggled").is(":visible")) {
        $(".toggled").hide(500);
        console.log("Show clicked!");
        return;
    }
    $(".toggled").show(500);
    console.log("Hide clicked!");
});

//Remove and rewrite saved viewModel on sign in
$(".external-login").click(function () {
    localStorage.removeItem("InspIntData");
    sessionStorage.removeItem("InspIntData");
});

//Remove and rewrite saved viewModel on sign in
$("form").submit(function () {
    localStorage.removeItem("InspIntData");
    sessionStorage.removeItem("InspIntData");
});