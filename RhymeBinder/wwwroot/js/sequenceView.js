﻿var formSubmit = false;
var hasUnsavedChanges = false;
function SetEventListeners() {
    // We want the editable area texts to be copied to the input areas of the form
    let input = document.getElementById("edit_area");
    input.addEventListener('keyup', function () {
        CopyContentToForm();
    });

    // Listener to submit on ctrl+s
    var isCtrl = false;
    window.addEventListener('load', function () {
        window.addEventListener('keydown', function (e) {
            if (e.key === "Control") {
                e.preventDefault();
                isCtrl = true;
            }

            if (e.key === "s" && isCtrl) {
                e.preventDefault();
                document.getElementById("save").click();
            }
        });

    });

    // Warn user before navigating away from page if there is unsaved changes
    window.addEventListener("beforeunload", function (e) {
        if (formSubmit || !hasUnsavedChanges) {
            return undefined;
        }

        var confirmationMessage = "You have unsaved changes. If you continue, your changes will be lost!";

        (e || window.event).returnValue = confirmationMessage; //Gecko + IE
        return confirmationMessage; //Gecko + Webkit, Safari, Chrome etc.
    });

    input.addEventListener("keydown", (e) => {
        // Tab tabs instead of tabbing
        if (e.key == "Tab") {
            e.preventDefault();
            document.execCommand('insertHTML', false, '&#009;');
        }
        // Enter behaves like Shift+Enter, to insert line breaks instead of divs
        //if (e.key == "Enter") {
        //    e.preventDefault();
        //    document.execCommand('insertHTML', false, '&#013;&#010;');
        //}

        if (e.key === "Enter") {
            e.preventDefault();
            insertLineBreak(e.target);
        }
    });

}  


function insertLineBreak(target) {
    const selection = window.getSelection();
    const range = selection.getRangeAt(0);
    const br = document.createElement("br");
    range.deleteContents();
    range.insertNode(br);
    range.setStartAfter(br);
    range.setEndAfter(br);
    selection.removeAllRanges();
    selection.addRange(range);
}

function ReplaceBreakTags(input) {    
    return input.replace(/<br>/g, "\r\n");
}


function CopyContentToForm() {
    var formSubmitTitles = document.getElementsByClassName("editedTextTitles");
    var editedTitles = document.getElementsByClassName("sequence-title");
    var savedTitles = document.getElementsByClassName("textTitles");
    var formSubmitTexts = document.getElementsByClassName("editedTextBodies");
    var editedTexts = document.getElementsByClassName("sequence-text");
    var savedTexts = document.getElementsByClassName("textBodies");
    var changedValues = document.getElementsByClassName("editedTextIsChanged");
    var unsavedCount = 0;

    for (let i = 0; i < formSubmitTitles.length; i++) {

        var editedTitlesBreakReplace = ReplaceBreakTags(editedTitles[i].innerHTML);
        var editedTextBreakReplace = ReplaceBreakTags(editedTexts[i].innerHTML);

        if (savedTitles[i].value != editedTitlesBreakReplace) {
            formSubmitTitles[i].value = editedTitlesBreakReplace;
            editedTitles[i].style.cssText += "border-left-color: goldenrod; ";
        } else {
            editedTitles[i].style.cssText += "border-left-color: darkseagreen; ";
        }

        if (savedTexts[i].value != editedTextBreakReplace) {
            formSubmitTexts[i].value = editedTextBreakReplace;
            editedTexts[i].style.cssText += "border-left-color: goldenrod; ";
        } else {
            editedTexts[i].style.cssText += "border-left-color: darkseagreen; ";
        }

        if (savedTitles[i].value == editedTitlesBreakReplace
            && savedTexts[i].value == editedTextBreakReplace) {
            changedValues[i].value = false;
        } else {
            changedValues[i].value = true;
            unsavedCount++;
        }
    }

    if (unsavedCount > 0) {
        hasUnsavedChanges = true;
        document.getElementById('save').disabled = false;

    } else {
        hasUnsavedChanges = false;
        document.getElementById('save').disabled = true;
    }

}

function SetFormSubmit(){
    formSubmit = true;
}