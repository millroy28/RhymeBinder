var autosave_timer;

var check_content_interval;
check_content_interval = 1000;

var original_title_string;
var check_title_string;
var original_body_string;
var check_body_string;
var is_content_different;

function reset_autosave_timer() {
    autosave_timer = 60; //seconds before autosaving
}


function autosave_counter() {
    check_content_for_difference();

    if (is_content_different == 0) {
        reset_autosave_timer();
    } else {
        autosave_timer--;
        if (autosave_timer == 0) {
            document.getElementById('edit').submit();
        }
    }

    setTimeout('autosave_counter()', 1000); //elapses one second before calling function again
}

function get_initial_content_values() {
    original_body_string = document.getElementById('body_edit_field').value;
    original_title_string = document.getElementById('title_edit_field').value;
}

function check_content_for_difference() {
    check_title_string = document.getElementById('title_edit_field').value;
    check_body_string = document.getElementById('body_edit_field').value;
    if ((check_title_string != original_title_string) || (check_body_string != original_body_string)) {
        is_content_different = 1;
        console.log('content changed');
    } else {
        is_content_different = 0;
        console.log('content same');
    }
}


function set_focus_on_edit_field() {
    document.getElementById('body_edit_field').focus();
}

//calling on load
set_focus_on_edit_field();
get_initial_content_values();
reset_autosave_timer();
autosave_counter();
