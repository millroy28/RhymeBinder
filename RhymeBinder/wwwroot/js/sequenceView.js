

function setTextareaHeights() {
    document.querySelectorAll("textarea").forEach(function (textarea) {
        textarea.style.height = textarea.scrollHeight + "px";
        textarea.style.overflowY = "hidden";
    });

    
}