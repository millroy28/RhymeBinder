var formSubmit = false;
var hasUnsavedChanges = false;
var submitButton = document.getElementById("submitButton");
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
                SubmitForm();
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

function SubmitForm() {
    if (hasUnsavedChanges)
    {
        SetFormSubmit();
        RemoveUnchangedElements();
        submitButton.click();
    }
}

function RemoveUnchangedElements() {
    // Running  up against an error when submitting a long (novel length) text
    // to address -- removing all entries that are unchanged and wouldn't need to be saved
    // to minimuze post space

    var changedValues = document.getElementsByClassName("editedTextIsChanged");
    
    for (let i = 0; i < changedValues.length; i++)
    {
        if (document.getElementById("isChanged[" + i + "]").value == "false")
            {
            document.getElementById("modelContainer[" + i + "]").remove();
        } 
    }
}

function ToggleNotes(index) {
    var notesPanel = document.getElementById("notePanel[" + index + "]");
    var notesPanelButton = document.getElementById("notePanelButton[" + index + "]");
    var notesContent = document.getElementById("sequenceNote[" + index + "]").textContent;
    var expandButton = "▼"
    if (notesContent.length > 0) {
        expandButton += " (Notes...)";
    }

    if (notesPanel.style.display === "grid") {
        notesPanel.style.display = "none";
        notesPanelButton.textContent = "▼ (Notes...)";
    } else {
        notesPanel.style.display = "grid";
        notesPanelButton.textContent = "▲";

    } 


}
