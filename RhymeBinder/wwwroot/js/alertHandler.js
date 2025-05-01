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

    if (severity === "") {
        return;
    }

    if (!severity || severity.length > 0) {

        switch (severity) {
            case "SUCCESS":
                document.getElementById("alertBox").classList.add("alert-success");
                document.getElementById("alertIcon").innerHTML = '<img src="/img/checkInCircle.svg">';
                timer = 3;
                break;
            case "INFO":
                document.getElementById("alertBox").classList.add("alert-info");
                document.getElementById("alertIcon").innerHTML = '<img src="/img/iInCircle.svg">';
                timer = 5;
                break;
            case "WARN":
                document.getElementById("alertBox").classList.add("alert-warn");
                document.getElementById("alertIcon").innerHTML = '<img src="/img/exclaimInTriangle.svg">';
                timer = 8;
                break;
            case "FAIL":
                document.getElementById("alertBox").classList.add("alert-fail");
                document.getElementById("alertIcon").innerHTML = '<img src="/img/exclaimInOctagon.svg">';
                break;
        }

        document.getElementById("alertBox").style.display = "flex";


        if (timer > 0) {
            CountdownToHide(timer);
        }
    }
}

function CountdownToHide(seconds) {
    seconds--;
    if (seconds > 0) {
        setTimeout(`CountdownToHide(${seconds})`, 1000);
    } else {
        SlideOutAlert();
    }
}

function SlideOutAlert() {
    document.getElementById("alertBox").classList.add("alert-hide");
}

function HideAlert() {
    document.getElementById("alertBox").style.display = "none";
}