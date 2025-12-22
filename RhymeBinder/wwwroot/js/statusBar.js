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

