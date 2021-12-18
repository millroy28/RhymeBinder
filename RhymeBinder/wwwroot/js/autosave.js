var autosave_timer;

var check_content_interval;
check_content_interval = 500;

var original_title_string;
var check_title_string;
var original_body_string;
var check_body_string;
var is_content_different;
var original_revision_status;
var check_revision_status;
var current_cursor_position;
var original_cursor_position;

function reset_autosave_timer() {
    autosave_timer = 5; //seconds before autosaving
}


function autosave_counter() {
    check_content_for_difference();

    if (is_content_different == 0) {
        reset_autosave_timer();
    } else {
        autosave_timer--;
        if (autosave_timer == 0) {
            get_current_cursor_position_and_form_focus();
            document.getElementById('cursor_position').value = current_cursor_position;
            document.getElementById('edit').submit();
        }
    }

    setTimeout('autosave_counter()', 1000); //elapses one second before calling function again
}

function get_current_cursor_position_and_form_focus() {
    current_cursor_position = document.getElementById('body_edit_field').selectionEnd;
}

function get_initial_content_values() {
    original_body_string = document.getElementById('body_edit_field').value;
    original_title_string = document.getElementById('title_edit_field').value;
    original_revision_status = document.getElementById('revision_status').value;
}

function check_content_for_difference() {
    check_title_string = document.getElementById('title_edit_field').value;
    check_body_string = document.getElementById('body_edit_field').value;
    check_revision_status = document.getElementById('revision_status').value;

    if (   (check_title_string != original_title_string)
        || (check_body_string != original_body_string)
        || (check_revision_status != original_revision_status)
        ) {
        is_content_different = 1;
        document.getElementById('save').disabled = false;
        console.log('content changed');
    } else {
        is_content_different = 0;
        document.getElementById('save').disabled = true;
        console.log('content same');
    }
}


function set_focus_on_edit_field() {
    document.getElementById('body_edit_field').focus();
}

function set_cursor_to_previous_position() {
    original_cursor_position = document.getElementById('prev_cursor_position_value').value;
    document.getElementById('body_edit_field').selectionEnd = original_cursor_position;
}

//calling on load
set_focus_on_edit_field();
set_cursor_to_previous_position();
get_initial_content_values();
reset_autosave_timer();
autosave_counter();
