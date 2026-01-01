//-----------Chaning tab behavior in text editor window so instead of changing focus we're insering a... tab character
let input = document.getElementById('body_edit_field');

input.addEventListener("keydown", (e) => {
    if (e.key == "Tab") {
        document.execCommand('insertHTML', false, '&#009');
        e.preventDefault();
    } 
});

//-----------Code for LineCounter
function get_characters_per_line() {
    var textBox = document.getElementById('body_edit_field');
    var checkBox = document.getElementById('textWidthSample');
    var textBoxWidth = textBox.scrollWidth;

    checkBox.hidden = false;
    var textCharacterWidth = checkBox.clientWidth;
    checkBox.hidden = true;

    var estCharactersInLine = Math.floor(textBoxWidth / textCharacterWidth);
    console.log('Estimated Characters per Line: ' + estCharactersInLine);
    return estCharactersInLine;
}
function populate_count_rulers() {
    var charsInLine = get_characters_per_line();
    var text = document.getElementById('body_edit_field').value;

    var splitTextArray = text.split("\n");
    var splitTextArrayCount = splitTextArray.length;
    var lineDisplayString = "";
    var paraDisplayString = "";
    var textLineNumber = 1;
    var textParaNumber = 1;
    var midPara = 0;

    for (let i = 0; i < splitTextArrayCount; i++) {
        
        let arrayLine = splitTextArray[i];
        let arrayLineLength = arrayLine.length;
        //If arrayline has no characters besides spaces, it is not a line
        //do with regex at some point?

        //console.log("textarea row: " + i + "; row length: " + arrayLineLength + "; max row length: " + charsInLine );

        //if text overruns line, don't count subsequent lines
        if (arrayLineLength > charsInLine) {
            //console.log('overrun line at ' + i);
            let overRunFactor = Math.floor(arrayLineLength / charsInLine);
            lineDisplayString += textLineNumber.toString();
            textLineNumber++;

            if (midPara == 1) {
                paraDisplayString += "|"
            }
            if (midPara == 0) {
                paraDisplayString += textParaNumber.toString();
                midPara = 1;
            }
            
            //add extra line breaks so next line number matches next populated line in text area
            for (let j = 0; j < overRunFactor; j++) {
                lineDisplayString += ' \r\n';
                paraDisplayString += ' \r\n|';
            }

        }
        if (arrayLineLength > 0 && arrayLineLength <= charsInLine) {
            //console.log('line exists at ' + i);
            lineDisplayString += textLineNumber.toString();
            textLineNumber++;

            if (midPara == 1) {
                paraDisplayString += "|"
            }
            if (midPara == 0) {
                paraDisplayString += textParaNumber.toString();
                midPara = 1;
            }

        }
        if (arrayLineLength == 0 && midPara == 1) {
            midPara = 0;
            textParaNumber++;
        }
        

        lineDisplayString += ' \r\n';
        paraDisplayString += ' \r\n';
    };

    document.getElementById('line_count').value = lineDisplayString;
    document.getElementById('paragraph_count').value = paraDisplayString;
}

populate_count_rulers(); //do it once on load

window.addEventListener("resize", populate_count_rulers);

let textboxTyping = document.getElementById('body_edit_field');
textboxTyping.addEventListener('keyup', function (e) {
    populate_count_rulers();
});


//-----------Sync scroll of rulers with main text area
function sync_scroll() {
    var rulerA = document.getElementById('paragraph_count');
    var rulerB = document.getElementById('line_count');
    var textArea = document.getElementById('body_edit_field');

    rulerA.scrollTop = textArea.scrollTop;
    rulerB.scrollTop = textArea.scrollTop;
}


//--------EDIT VIEW SIDEBAR-------------------------------------------------------------
function insert_new_text_in_sequence(groupId, value) {
    console.log("TESTING GROUP ID : " + groupId + "   VALUE: " + value);
    document.getElementById("SequenceGroupId").value = groupId;
    selected_action_form_submit('InsertNewTextInSequence', value);
}

//-----------Initializing auto-hide elements
toggle_hide_element('show_line_count', 'toggle_line_count', 'line_count', 'init');
toggle_hide_element('show_paragraph_count', 'toggle_paragraph_count', 'paragraph_count', 'init');