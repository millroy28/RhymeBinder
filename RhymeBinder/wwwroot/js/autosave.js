﻿var autosave_timer;

let check_content_interval = 500;

var original_title_string;
var check_title_string;
var original_body_string;
var check_body_string;
var is_content_different;
var original_revision_status;
var check_revision_status;
var current_focus_element;
var previous_focus_element;
var current_body_cursor_position;
var previous_body_cursor_position;
var current_body_scroll_position;
var previous_body_scroll_position;
var current_note_cursor_position;
var previous_note_cursor_position;
var current_note_scroll_position;
var previous_note_scroll_position;
var current_title_cursor_position;
var previous_title_cursor_position;
var keyup_timer;
var timeout_timer;
var is_form_submitting = false;

//-----------Warn user before navigating away from page if there is unsaved changes
window.addEventListener("beforeunload", function (e) {
    if (is_content_different == 0  || is_form_submitting) {
        return undefined;
    }
    var confirmationMessage = "You have unsaved changes. If you continue, your changes will be lost!";

    (e || window.event).returnValue = confirmationMessage; //Gecko + IE
    return confirmationMessage; //Gecko + Webkit, Safari, Chrome etc.
});

function set_form_submitting_true() {
    is_form_submitting = true;
}


function reset_autosave_timer() {
    autosave_timer = 10; //seconds before autosaving
}

function reset_keyup_timer() {
    keyup_timer = 15;    //seconds wait for pause in typing before autosave
}

function reset_timeout_timer() {
    timeout_timer = 600;  // seconds to wait before timing out - returning to view instead of edit
}


function autosave_counter() {
    check_content_for_difference();

    if (is_content_different == 0)
    {
        reset_autosave_timer();
        timeout_timer--;

        if (timeout_timer == 0)
        {
            submit_timeout_action();
            return;
        }

    } else
    {
      //  autosave_timer--;
        reset_timeout_timer();
        //if (autosave_timer == 0)
        //{
            save_content();
            return;
        //}
    }
    setTimeout('autosave_counter()', 1000); //elapses one second before calling function again
}

function submit_timeout_action()
{   // going to submit form action with value of 'timeout' to controller

    button = document.getElementById('return');
    button.value = 'Timeout';
    button.click();
}

function save_content() {
    get_current_cursor_position_and_form_focus();
    console.log('preparing to autosave...');
    console.log('waiting for user to stop typing...');

    let input = document.getElementById(current_focus_element);
    input.addEventListener('keyup', function (e) {
        reset_keyup_timer();
    });

    wait_for_typing_to_finish_before_saving();
    
}

function prepare_to_submit() {
    //set cursor position and form focus values in form to be saved to server
    get_current_cursor_position_and_form_focus();
    // un-comment for debug console logging:
    //console.log('CURRENT FOCUS ELEMENT: ' + current_focus_element);
    //console.log('CURRENT TEXT CURSOR POSITION: ' + current_body_cursor_position);
    //console.log('CURRENT TEXT SCROLL POSITION: ' + current_body_scroll_position);
    //console.log('CURRENT NOTE CURSOR POSITION: ' + current_note_cursor_position);
    //console.log('CURRENT NOTE SCROLL POSITION: ' + current_note_scroll_position);
    //console.log('CURRENT TITLE CURSOR POSITION: ' + current_title_cursor_position);
    //setTimeout(3000);


    document.getElementById('body_cursor_position').value = current_body_cursor_position;
    document.getElementById('body_scroll_position').value = current_body_scroll_position;
    document.getElementById('note_cursor_position').value = current_note_cursor_position;
    document.getElementById('note_scroll_position').value = current_note_scroll_position;
    document.getElementById('title_cursor_position').value = current_title_cursor_position;
    document.getElementById('form_focus').value = current_focus_element;
    //document.getElementById('edit').submit();

    set_form_submitting_true();

    return;
}
function wait_for_typing_to_finish_before_saving() {
    keyup_timer--;
    if (keyup_timer % 5 == 0 || keyup_timer < 4) {
        console.log('no-typing detected... saving in ' + keyup_timer + ' seconds');       
    }    

    if (keyup_timer == 0) {
        prepare_to_submit();

        button = document.getElementById('save');
        button.click();
        return;
    } else {
        setTimeout('wait_for_typing_to_finish_before_saving()', 1000); //elapses one second before calling function again
    }

}

function get_current_cursor_position_and_form_focus() {
    current_body_cursor_position = document.getElementById('body_edit_field').selectionEnd;
    current_body_scroll_position = document.getElementById('body_edit_field').scrollTop; 
    current_note_cursor_position = document.getElementById('note_edit_field').selectionEnd;
    current_note_scroll_position = document.getElementById('note_edit_field').scrollTop;
    current_title_cursor_position = document.getElementById('title_edit_field').selectionEnd;

    current_focus_element = document.activeElement.id;

    if (current_focus_element != 'body_edit_field'
        && current_focus_element != 'title_edit_field'
        && current_focus_element != 'note_edit_field') {
        current_focus_element = 'body_edit_field'
    }
}


function get_initial_content_values() {
    original_body_string = document.getElementById('body_edit_field').value;
    original_title_string = document.getElementById('title_edit_field').value;
    original_revision_status = document.getElementById('revision_status').value;
    original_note_string = document.getElementById('note_edit_field').value;
}

function check_content_for_difference() {
    check_title_string = document.getElementById('title_edit_field').value;
    check_body_string = document.getElementById('body_edit_field').value;
    check_revision_status = document.getElementById('revision_status').value;
    check_note_string = document.getElementById('note_edit_field').value;

    if (   (check_title_string != original_title_string)
        || (check_body_string != original_body_string)
        || (check_revision_status != original_revision_status)
        || (check_note_string != original_note_string)
        ) {
        is_content_different = 1;
        document.getElementById('save').disabled = false;
        console.log('content changed - counting down to autosave');
    } else {
        is_content_different = 0;
        document.getElementById('save').disabled = true;
    }
}

function set_cursor_and_focus_to_previous() {
    previous_focus_element = document.getElementById('form_focus').value;
    console.log('setting focus to previously focused element: ' + previous_focus_element);
    document.getElementById(previous_focus_element).focus();

    previous_body_cursor_position = document.getElementById('body_cursor_position').value;
    console.log('setting text cursor to previous position: ' + previous_body_cursor_position);
    document.getElementById('body_edit_field').selectionStart = previous_body_cursor_position;

    previous_body_scroll_position = document.getElementById('body_scroll_position').value;
    console.log('setting texr scroll to previous position: ' + previous_body_scroll_position);
    document.getElementById('body_edit_field').scrollTop = previous_body_scroll_position;

    previous_note_cursor_position = document.getElementById('note_cursor_position').value;
    console.log('setting note cursor to previous position: ' + previous_note_cursor_position);
    document.getElementById('note_edit_field').selectionStart = previous_note_cursor_position;

    previous_note_scroll_position = document.getElementById('note_scroll_position').value;
    console.log('setting note scroll to previous position: ' + previous_note_scroll_position);
    document.getElementById('note_edit_field').scrollTop = previous_note_scroll_position;

    previous_title_cursor_position = document.getElementById('title_cursor_position').value;
    console.log('setting title cursor to previous position: ' + previous_title_cursor_position);
    document.getElementById('title_edit_field').selectionStart = previous_title_cursor_position;

}

function run_these_functions_on_page_load() {
//calling on load
set_cursor_and_focus_to_previous();
get_initial_content_values();
reset_autosave_timer();
reset_keyup_timer();
reset_timeout_timer();
autosave_counter();  //this is the one that runs continuously 
}

run_these_functions_on_page_load();


//-----------Listener to submit on ctrl+s
var isCtrl = false;
window.addEventListener('load', function () {
    window.addEventListener('keydown', function (e) {
        if (e.key === "Control") {
            e.preventDefault();
            isCtrl = true;
        }

        if (e.key === "s" && isCtrl) {
            e.preventDefault();

            prepare_to_submit();

            document.getElementById("save").click();
        }
    });
    window.addEventListener('keyup', function (e) {
        if (e.key === "Control") {
            isCtrl = false; // Reset flag when Control is released
        }
    });


});