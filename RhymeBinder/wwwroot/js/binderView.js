

function ShowListView() {
    document.getElementById("show_shelf_view").textContent = "  Shelf View";
    document.getElementById("show_list_view").textContent = "►  List View";
    document.getElementById("shelf_view").hidden = true;
    document.getElementById("list_view").hidden = false;
}

function ShowShelfView() {
    document.getElementById("show_shelf_view").textContent = "► Shelf View";
    document.getElementById("show_list_view").textContent = "  List View";
    document.getElementById("shelf_view").hidden = false;
    document.getElementById("list_view").hidden = true;
}