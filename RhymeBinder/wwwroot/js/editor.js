//Chaning tab behavior in text editor window so instead of changing focus we're insering a... tab character
let input = document.getElementById('body_edit_field');

input.addEventListener("keydown", (e) => {
    if (e.key == "Tab") {
        document.execCommand('insertHTML', false, '&#009');
        e.preventDefault();
    } 
});