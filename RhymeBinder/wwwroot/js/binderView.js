

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


////Code from https://www.geeksforgeeks.org/create-a-drag-and-drop-sortable-list-using-html-css-javascript/

//const list = document.querySelector('.sortable-list');
//let draggingItem = null;


//function SetEventListeners() {
//    list.addEventListener('dragstart', (e) => {
//        draggingItem = e.target;
//        e.target.classList.add('dragging');
//    });

//    list.addEventListener('dragend', (e) => {
//        e.target.classList.remove('dragging');
//        document.querySelectorAll('.sortable-item').forEach(item => item.classList.remove('over'));
//        draggingItem = null;
//    });

//    list.addEventListener('dragover', (e) => {
//        e.preventDefault();
//        const draggingOverItem = getDragAfterElement(list, e.clientY);

//        // Remove .over from all items
//        document.querySelectorAll('.sortable-item').forEach(item => item.classList.remove('over'));

//        if (draggingOverItem) {
//            draggingOverItem.classList.add('over'); // Add .over to the hovered item
//            list.insertBefore(draggingItem, draggingOverItem);
//        } else {
//            list.appendChild(draggingItem); // Append to the end if no item below
//        }
//    });

//}

//function getDragAfterElement(container, y) {
//    const draggableElements = [...container.querySelectorAll('.sortable-item:not(.dragging)')];

//    return draggableElements.reduce((closest, child) => {
//        const box = child.getBoundingClientRect();
//        const offset = y - box.top - box.height / 2;
//        if (offset < 0 && offset > closest.offset) {
//            return { offset: offset, element: child };
//        } else {
//            return closest;
//        }
//    }, { offset: Number.NEGATIVE_INFINITY }).element;
//}



