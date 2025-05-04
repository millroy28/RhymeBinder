var formSubmit = false;
var hasUnsavedChanges = false;
var submitButton = document.getElementById("submitButton");
function SetEventListeners() {
    // We want the editable area texts to be copied to the input areas of the form
    let input = document.getElementById("edit_area");
    input.addEventListener('keyup', function () {
        CopyContentToForm();
        //ensureCursorVisible(input);
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
        window.addEventListener('keyup', function (e) {
            if (e.key === "Control") {
                isCtrl = false; // Reset flag when Control is released
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
            //document.insertLineBreak(e.target);
            document.execCommand('insertHTML', false, "\r\n");

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

    var formSubmitNotes = document.getElementsByClassName("editedTextNotes");
    var editedTextNotes = document.getElementsByClassName("sequence-notes");
    var savedTextNotes = document.getElementsByClassName("textNotes");

    var changedValues = document.getElementsByClassName("editedTextIsChanged");
    var unsavedCount = 0;

    for (let i = 0; i < formSubmitTitles.length; i++) {

        var editedTitlesBreakReplace = ReplaceBreakTags(editedTitles[i].innerHTML);
        var editedTextBreakReplace = ReplaceBreakTags(editedTexts[i].innerHTML);
        var editedNoteBreakReplace = ReplaceBreakTags(editedTextNotes[i].innerHTML);

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

        if (savedTextNotes[i].value != editedNoteBreakReplace) {
            formSubmitNotes[i].value = editedNoteBreakReplace;
            editedTextNotes[i].style.cssText += "border-left-color: goldenrod; ";
        } else {
            editedTextNotes[i].style.cssText += "border-left-color: darkseagreen; ";
        }

        if (savedTitles[i].value == editedTitlesBreakReplace
            && savedTexts[i].value == editedTextBreakReplace
            && savedTextNotes[i].value == editedNoteBreakReplace) {
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

function SetFormSubmit() {
    formSubmit = true;
}

function SubmitForm() {
    if (hasUnsavedChanges) {
        SetFormSubmit();
        RemoveUnchangedElements();
        GetCursorPosition();
        submitButton.click();
    }
}

function RemoveUnchangedElements() {
    // Running  up against an error when submitting a long (novel length) text
    // to address -- removing all entries that are unchanged and wouldn't need to be saved
    // to minimuze post space

    var changedValues = document.getElementsByClassName("editedTextIsChanged");

    for (let i = 0; i < changedValues.length; i++) {
        if (document.getElementById("isChanged[" + i + "]").value == "false") {
            document.getElementById("modelContainer[" + i + "]").remove();
        }
    }
}

function GetCursorPosition() {
    // Gets cursor position and current element in focus and updates hidden form inputs with the values

    var selection = window.getSelection();
    var range = selection.getRangeAt(0);
    let preCaretRange = range.cloneRange();

    var currentElement = document.activeElement;
    preCaretRange.selectNodeContents(currentElement);
    preCaretRange.setEnd(range.endContainer, range.endOffset);

    var cursorPosition = preCaretRange.toString().length;

    document.getElementById("cursorPosition").value = cursorPosition;
    document.getElementById("activeElementId").value = currentElement.id;
    document.getElementById("scrollPosition").value = document.getElementById("edit_area").scrollTop;

}
function SetCursorPosition() {
    var desiredActiveElementId = document.getElementById("activeElementId").value;
    var desiredCursorPosition = parseInt(document.getElementById("cursorPosition").value, 10);

    if (!desiredActiveElementId || isNaN(desiredCursorPosition)) return;

    var desiredActiveElement = document.getElementById(desiredActiveElementId);
    if (!desiredActiveElement) return;

    // if it's a note, open the note
    if (desiredActiveElementId.substring(0, 13) == "sequenceNote-") {
        var noteButtonIndex = desiredActiveElementId.replace("sequenceNote-", "");
        ToggleNotes(noteButtonIndex);
    }

    desiredActiveElement.focus(); // Ensure focus is set before modifying selection

    let range = document.createRange();
    let selection = window.getSelection();

    let textNode = FindTextNode(desiredActiveElement);
    if (!textNode) return;

    let pos = Math.min(desiredCursorPosition, textNode.length); // Prevent overflow
    range.setStart(textNode, pos);
    range.setEnd(textNode, pos);

    setTimeout(() => {
        selection.removeAllRanges();
        selection.addRange(range);

    }, 100);


}

function FindTextNode(element) {
    for (let child of element.childNodes) {
        if (child.nodeType === 3) { // Text node
            return child;
        }
    }
    return null; // Fallback if no text node exists
}

function SetAppendGroupFlag() {
    document.getElementById("appendNewGroup").value = 1;
    SubmitForm();
}