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
        GetCursorAndScrollPosition();
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

function GetCursorAndScrollPosition() {
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
function SetCursorAndScrollPosition() {
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

// window.addEventListener("load", setCaretPosition);

// Helper function to find first text node within a contenteditable element
function FindTextNode(element) {
    for (let child of element.childNodes) {
        if (child.nodeType === 3) { // Text node
            return child;
        }
    }
    return null; // Fallback if no text node exists
}



    // On Load sets cursor position



    //var desiredActiveElementId = document.getElementById("activeElementId").value;
    //console.log("Set focus on " + desiredActiveElementId);
    //var desiredCursorPosition = document.getElementById("cursorPosition").value;
    //console.log("Set cursor to position " + desiredCursorPosition);
    ////var desiredScrollPosition = document.getElementById("scrollPosition").value;
    ////if (scrollPosition != null) {
    ////    document.getElementById("edit_area").scrollTop = scrollPosition;
    ////}

    //if (desiredActiveElementId == null || desiredCursorPosition == null) {
    //    return;
    //}

    //var desiredActiveElement = document.getElementById(desiredActiveElementId);
    //desiredActiveElement.focus();


    //var selection = window.getSelection();
    //var range = selection.getRangeAt(0);
    //let preCaretRange = range.cloneRange();

    //preCaretRange.selectNodeContents(desiredActiveElement);
    //preCaretRange.setEnd(range.endContainer, desiredCursorPosition);

  


    ////approach 3
    //var range = window.getSelection().getRangeAt(0);
    //window.getSelection().removeAllRanges();
    //window.getSelection().addRange(range);






    // approach 1
    //range.selectNodeContents(desiredActiveElement);
    //range.collapse(true);

    //let charCount = 0, found = false;
    //range.setStart(desiredActiveElement, desiredCursorPosition);
    //range.setEnd(desiredActiveElement, desiredCursorPosition + 1);

    //function traverseNodes(node) {
    //    if (node.nodeType === 3) { // Text node
    //        let nextCharCount = charCount + node.length;
    //        if (!found && desiredCursorPosition <= nextCharCount) {
    //            range.setStart(node, desiredCursorPosition - charCount);
    //            range.setEnd(node, desiredCursorPosition - charCount);
    //            found = true;
    //        }
    //        charCount = nextCharCount;
    //    } else {
    //        for (let child of node.childNodes) traverseNodes(child);
    //    }
    //}


    //// approach 2
    //let range = document.createRange();
    //let selection = window.getSelection();
    //function findTextNode(element) {
    //    for (let child of element.childNodes) {
    //        if (child.nodeType === 3) { // Look for first text node
    //            console.log(child.id);
    //            return child;
    //        }
    //    }
    //    return element; // Fallback if no text node found
    //}
    //let textNode = findTextNode(desiredActiveElement);
    //if (textNode.nodeType === 3) {
    //    range.setStart(textNode, Math.min(desiredCursorPosition, textNode.length));
    //    range.setEnd(textNode, Math.min(desiredCursorPosition, textNode.length));
    //} else {
    //    range.selectNodeContents(desiredActiveElement);
    //    range.collapse(true);
    //}


    ////traverseNodes(node);

    //selection.removeAllRanges();
    //selection.addRange(range);


//}



//function ensureCursorVisible(element) {
//    let selection = window.getSelection();
//    let range = selection.getRangeAt(0);
//    let rect = range.getBoundingClientRect(); // Get cursor position

//    let parentRect = element.getBoundingClientRect();
//    let offset = rect.top - parentRect.top; // Cursor position relative to the div

//    if (offset < 0 || offset > element.clientHeight) {
//        element.scrollTop += offset - element.clientHeight / 2; // Adjust scroll
//    }
//}
