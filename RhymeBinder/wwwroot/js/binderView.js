

function ShowListView() {
    document.getElementById("show_shelf_view").textContent = "  Shelf View";
    document.getElementById("show_list_view").textContent = "►  List View";
    document.getElementById("shelf_view").style.display = "none";
    document.getElementById("list_view").hidden = false;
    document.getElementById("show_shelf_order_modal").style.display = "none";
}

function ShowShelfView() {
    document.getElementById("show_shelf_view").textContent = "► Shelf View";
    document.getElementById("show_list_view").textContent = "  List View";
    document.getElementById("shelf_view").style.display = "grid";
    document.getElementById("list_view").hidden = true;
    document.getElementById("show_shelf_order_modal").style.display = "block";

}

function OpenModal(elementId) {
    document.getElementById(elementId).style.display = "inline";
    document.body.style.pointerEvents = "none";
}
function CloseModal(elementId) {
    document.getElementById(elementId).style.display = "none";
    document.body.style.pointerEvents = 'all';
}

function SetEventListeners() {

    document.addEventListener('DOMContentLoaded', () => {
        let binders = document.getElementsByClassName('shelf-view-binder');
        let hoverCards = document.getElementsByClassName('shelf-view-binder-hover');
        let hoverTimeout;

        for (let i = 0; i < binders.length; i++) {
            binders[i].addEventListener('mouseover', (event) => {
                hoverTimeout = setTimeout(() => {
                    // Set the hover card position based on mouse coordinates
                    hoverCards[i].style.left = (event.pageX + 10) + 'px'; // Add 10px offset to the left
                    hoverCards[i].style.top = (event.pageY + 10) + 'px'; // Add 10px offset to the top
                    hoverCards[i].style.display = 'block';
                }, 1000);
            });

            binders[i].addEventListener('mouseout', () => {
                clearTimeout(hoverTimeout);
                hoverCards[i].style.display = 'none';
            });
        }
    });

}
