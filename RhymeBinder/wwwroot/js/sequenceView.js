//Toggle sidebar navigation links and auto scroll left sidebar links
document.addEventListener("DOMContentLoaded", function () {
    let sections = document.querySelectorAll(".sequence-title");
    let sidebarLinks = document.querySelectorAll(".sequence-sidebar-link");
    let sidebar = document.getElementById("left-sidebar");

    let observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            if (entry.isIntersecting) {
                let index = entry.target.id.split("-")[1]; // Get index from ID

                sidebarLinks.forEach(link => link.classList.remove("active"));
                let activeLink = document.getElementById(`nav-${index}`);
                activeLink.classList.add("active");

                // **Ensure Active Link is in View**
                let sidebarOffset = activeLink.offsetTop - sidebar.clientHeight / 2;
                sidebar.scrollTo({ top: sidebarOffset, behavior: "smooth" });
            }
        });
    }, { threshold: 0.5 });

    sections.forEach(section => observer.observe(section));
});



// Show/Hide Notes
function ToggleNotes(index) {
    var notesPanel = document.getElementById("notePanel[" + index + "]");
    var notesPanelButton = document.getElementById("notePanelButton[" + index + "]");
    var notesContent = document.getElementById("sequenceNote[" + index + "]").textContent;
    var expandButton = "▼"
    if (notesContent.length > 0) {
        expandButton += " (Notes...)";
    }

    if (notesPanel.style.display === "grid") {
        notesPanel.style.display = "none";
        notesPanelButton.textContent = "▼ (Notes...)";
    } else {
        notesPanel.style.display = "grid";
        notesPanelButton.textContent = "▲";

    }


}
