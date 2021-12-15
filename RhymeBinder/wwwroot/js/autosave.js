var autosave_timer;
autosave_timer = 60; //seconds before autosaving

function autosave_counter() {
    if (autosave_timer > 0) {
        autosave_timer--;

        if (autosave_timer > 0) {
            setTimeout('autosave_counter()', 1000); //elapses one second
        }
        if (autosave_timer == 0) {
            document.getElementById('edit').submit();
        }
    }
}
autosave_counter();