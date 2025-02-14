

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
