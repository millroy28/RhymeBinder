//-----------Status Bar functions

function update_word_count() {
    var wordCount = get_word_count_from_textbox();
    set_word_count_indicator(wordCount);
}
function get_word_count_from_textbox() {
    var text = document.getElementById('body_edit_field').value;
    if (text == null) {
        return 0;
    }

    // Split on any whitespace (spaces, newlines, tabs, etc.)
    return text.trim().split(/\s+/).filter(function (word) {
        return word.length > 0;
    }).length;
}
function set_word_count_indicator(wordCount) {
    if (wordCount) {
        document.getElementById('word_count').innerHTML = wordCount;
    } else {
        document.getElementById('word_count').innerHTML = "-";
    }

}
function initialize_word_count() {
    update_word_count();
    document.getElementById('body_edit_field')
        .addEventListener("input", (event) => update_word_count());
}


//-----------Sidebar Functions

function insert_new_text_in_sequence(groupId, value) {
    document.getElementById("SequenceGroupId").value = groupId;
    selected_action_form_submit('InsertNewTextInSequence', value);
}


//-----------Font Size Functions
const maxFontSize = 40;
const minFontSize = 8;
function adjustFontSize(direction) {
    var titleBox = document.getElementById("title_edit_field");
    var editorBox = document.getElementById("body_edit_field");
    var notesBox = document.getElementById("note_edit_field");
    var paragraphRuler = document.getElementById("paragraph_count");
    var lineRuler = document.getElementById("line_count");

    var currentFontSizeString = titleBox.style.fontSize;

    var currentFontSizeInt = parseInt(currentFontSizeString.replace("px", ""));

    if (direction == "increase" && currentFontSizeInt < maxFontSize) {
        currentFontSizeInt++;
    } else if (direction == "decrease" && currentFontSizeInt > minFontSize) {
        currentFontSizeInt--;
    } else {
        return;
    }

    newFontSizeString = currentFontSizeInt.toString() + "px";

    titleBox.style.fontSize = newFontSizeString;
    editorBox.style.fontSize = newFontSizeString;
    notesBox.style.fontSize = newFontSizeString;

    if (paragraphRuler) {
        paragraphRuler.style.fontSize = newFontSizeString;
    }
    if (lineRuler) {
        lineRuler.style.fontSize = newFontSizeString;
    }

    toggleFontSizeButtons(currentFontSizeInt);

    const userId = document.getElementById("userId").value;
    saveFontSizePreference(userId, currentFontSizeInt);

    return;
}

function toggleFontSizeButtons(fontSize) {
    var fontIncrease = document.getElementById("fontIncrease");
    var fontDecrease = document.getElementById("fontDecrease");

    if (fontSize <= minFontSize) {
        fontDecrease.classList.add("menu-bar-button-disabled");
        fontDecrease.classList.remove("menu-bar-button");
    }
    else if (fontSize >= maxFontSize) {
        fontIncrease.classList.add("menu-bar-button-disabled");
        fontIncrease.classList.remove("menu-bar-button");
    } else {
        fontDecrease.classList.remove("menu-bar-button-disabled");
        fontIncrease.classList.remove("menu-bar-button-disabled");
        fontDecrease.classList.add("menu-bar-button");
        fontIncrease.classList.add("menu-bar-button");
    }
    return;
}

function saveFontSizePreference(userId, fontSize) {
    $.ajax({
        url: '/BinderCore/SaveUserFontSize',
        type: 'POST',
        data: {
            userId: userId,
            fontSize: fontSize
        },
        success: function (response) {
            console.log('Font size saved successfully');
        },
        error: function (xhr, status, error) {
            console.error('Error saving font size:', error);
        }
    });
}

//-----------Window Size Functions

const minwidthSize = 0; // refer to EditWindowWidthLevel enum
const maxwidthSize = 2;
function adjustWidth(direction) {
    var currentWidth = document.getElementById("ViewWidth").value;

    if (direction == "increase" && currentWidth < maxwidthSize) {
        currentWidth++; 
    } else if (direction == "decrease" && currentWidth > minwidthSize) {
        currentWidth--;
    }
    setWindowWidth(currentWidth);
    const userId = document.getElementById("userId").value;
    saveUserWindowWidthPreference(userId, currentWidth);

    document.getElementById("ViewWidth").value = currentWidth;
}

function setWindowWidth(level) {
    var editWindow = document.getElementById("edit");
    var leftSidebar = document.getElementById("leftSidebar");
    var rightSidebar = document.getElementById("rightSidebar");

    if (level == 0) {
        editWindow.classList.remove("editor-view-wrapper-wideFull");
        editWindow.classList.remove("editor-view-wrapper-wideMed");

        editWindow.classList.add("editor-view-wrapper-withSidebars");

        leftSidebar.classList.remove("hidden");
        rightSidebar.classList.remove("hidden");
        
    } else if (level == 1) {
        editWindow.classList.remove("editor-view-wrapper-wideFull");
        editWindow.classList.remove("editor-view-wrapper-withSidebars");

        editWindow.classList.add("editor-view-wrapper-wideMed");
        leftSidebar.classList.add("hidden");
        rightSidebar.classList.remove("hidden");
    } else if (level == 2) {
        editWindow.classList.remove("editor-view-wrapper-wideMed");
        editWindow.classList.remove("editor-view-wrapper-withSidebars");

        editWindow.classList.add("editor-view-wrapper-wideFull");
        leftSidebar.classList.add("hidden");
        rightSidebar.classList.add("hidden");
    }

    toggleWidthButtons(level);
}

function toggleWidthButtons(widthSize) {
    var widthIncrease = document.getElementById("widthIncrease");
    var widthDecrease = document.getElementById("widthDecrease");

    if (widthSize <= minwidthSize) {
        widthDecrease.classList.add("menu-bar-button-disabled");
        widthDecrease.classList.remove("menu-bar-button");
    }
    else if (widthSize >= maxwidthSize) {
        widthIncrease.classList.add("menu-bar-button-disabled");
        widthIncrease.classList.remove("menu-bar-button");
    } else {
        widthDecrease.classList.remove("menu-bar-button-disabled");
        widthIncrease.classList.remove("menu-bar-button-disabled");
        widthDecrease.classList.add("menu-bar-button");
        widthIncrease.classList.add("menu-bar-button");
    }
    return;
}
function saveUserWindowWidthPreference(userId, widthLevel) {
    $.ajax({
        url: '/BinderCore/SaveUserWindowWidth',
        type: 'POST',
        data: {
            userId: userId,
            widthLevel: widthLevel
        },
        success: function (response) {
            console.log('Width level saved successfully');
        },
        error: function (xhr, status, error) {
            console.error('Error saving width level:', error);
        }
    });
}

