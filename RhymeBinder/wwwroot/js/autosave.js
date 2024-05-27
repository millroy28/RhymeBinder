var autosave_timer;

let check_content_interval = 500;

var original_title_string;
var check_title_string;
var original_body_string;
var check_body_string;
var is_content_different;
var original_revision_status;
var check_revision_status;
var current_cursor_position;
var previous_cursor_position;
var current_focus_element;
var previous_focus_element;
var current_scroll_position;
var previous_scroll_position;
var keyup_timer;
var timeout_timer;

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

function wait_for_typing_to_finish_before_saving() {
    keyup_timer--;
    if (keyup_timer % 5 == 0 || keyup_timer < 4) {
        console.log('no-typing detected... saving in ' + keyup_timer + ' seconds');
        // un-comment for debug console logging:
        // console.log('CURRENT FOCUS ELEMENT: ' + current_focus_element);
        // console.log('CURRENT CURSOR POSITION: ' + current_cursor_position);
        // console.log('CURRENT SCROLL POSITION: ' + current_scroll_position);
    }
    

    if (keyup_timer == 0) {
        //set cursor position and form focus values in form to be saved to server
        get_current_cursor_position_and_form_focus();
        document.getElementById('cursor_position').value = current_cursor_position;
        document.getElementById('form_focus').value = current_focus_element;
        document.getElementById('scroll_position').value = current_scroll_position;
        //document.getElementById('edit').submit();
        button = document.getElementById('save');
        button.click();
        return;
    } else {
        setTimeout('wait_for_typing_to_finish_before_saving()', 1000); //elapses one second before calling function again
    }

}

function get_current_cursor_position_and_form_focus() {
    current_focus_element = document.activeElement.id;

    if (current_focus_element == 'body_edit_field'
        || current_focus_element == 'title_edit_field'
        || current_focus_element == 'note_edit_field') {
        current_cursor_position = document.getElementById(current_focus_element).selectionEnd;
        current_scroll_position = document.getElementById(current_focus_element).scrollTop;
    } else {
        current_focus_element = 'body_edit_field'
        current_cursor_position = 0;
        current_scroll_position = 0;
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

    previous_cursor_position = document.getElementById('cursor_position').value;
    console.log('setting cursor to previous position: ' + previous_cursor_position);
    document.getElementById(previous_focus_element).selectionStart = previous_cursor_position;

    previous_scroll_position = document.getElementById('scroll_position').value;
    console.log('setting scrollbar to previous position: ' + previous_scroll_position);
    document.getElementById(previous_focus_element).scrollTop = previous_scroll_position;

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

