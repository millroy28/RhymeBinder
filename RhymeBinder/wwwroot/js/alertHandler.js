// Alert information is passed via TempData with following format:
/*
    TempData["AlertMessage"] = "This is your message!"; // MEssage to be displayed
    TempData["AlertSeverity"] = "WARN";                 // Determines color of alert and perhaps behavior.

    AlertSeverity Behavior:
    SUCCESS     -   Greenish        -   Dissapear after 3 seconds
    WARN        -   Yellow/Orange   -   Linger until closed
    FAIL        -   Red             -   Linger until closed
*/

function ShowAlertOnLoad() {
    var severity = document.getElementById("alertSeverity").innerHTML;
    var message = document.getElementById("alertText").innerHTML;
    var timer = -1;

    if (!severity || severity.length > 0) {

        switch (severity) {
            case "SUCCESS":
                document.getElementById("alertText").classList.add("alert-success-text");
                timer = 5;
                break;
            case "WARN":
                document.getElementById("alertText").classList.add("alert-warn-text");
                break;
            case "FAIL":
                document.getElementById("alertText").classList.add("alert-fail-text");
                break;

        }
        document.getElementById("alertBox").style.display = "flex";
    }
}

function HideAlert() {
    document.getElementById("alertBox").style.display = "none";
}