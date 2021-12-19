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

function reset_autosave_timer() {
    autosave_timer = 5; //seconds before autosaving
}


// Get the input box
let input = document.getElementById('my-input');


function autosave_counter() {
    check_content_for_difference();

    if (is_content_different == 0) {
        reset_autosave_timer();
    } else {
        autosave_timer--;
        if (autosave_timer == 0) {
            save_content();
        }
    }
    setTimeout('autosave_counter()', 1000); //elapses one second before calling function again
}

function save_content() {

    get_current_cursor_position_and_form_focus();
    document.getElementById('cursor_position').value = current_cursor_position;
    document.getElementById('form_focus').value = current_focus_element;
    document.getElementById('edit').submit();
}

function get_current_cursor_position_and_form_focus() {
    current_focus_element = document.activeElement.id;
    console.log('preparing to autosave... current focus element: ' + current_focus_element);
   // current_cursor_position = document.getElementById('body_edit_field').selectionEnd;
    if (current_focus_element == 'body_edit_field' || current_focus_element == 'title_edit_field') {
        current_cursor_position = document.getElementById(current_focus_element).selectionEnd;
    } else {
        current_cursor_position = 0;
    }
    console.log('preparing to autosave... current cursor position: ' + current_cursor_position);
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
}

function run_these_functions_on_page_load() {
//calling on load
set_cursor_and_focus_to_previous();
get_initial_content_values();
reset_autosave_timer();
autosave_counter();  //this is the one that runs continuously 
}

run_these_functions_on_page_load();

