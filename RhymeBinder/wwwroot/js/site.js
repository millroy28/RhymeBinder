var button;

//--------HIDING/SHOWING ELEMENTS-------------------------------------------------------
function toggle_hide_element(formElementID, clickElementID, hideableElementName, init) {
    /*takes in hidden form property id, clickable element id, and hideable area id.
     *will toggle the value of that form property to be saved                          ... formElementID flipped
     *will toggle the the label of the clickable element id to swap "hide" with "show" ...clickElementID value modified
     *will toggle hidden status of the hideable areas                                   ...hideableElementName hidden or un-hidden
     */
    var show = document.getElementById(formElementID).value;
    var clickElementText = document.getElementById(clickElementID).innerText;

    console.log('toogle_hide_element(' + formElementID + ', ' + clickElementID + ', ' + hideableElementName + ', ' + init + ')...');
    console.log('...formElementID value = ' + show);
    console.log('...clickElementID inner text = ' + clickElementText);

    var hideableElements = document.getElementsByName(hideableElementName);

    if (init == 'init') {   //intended for showing/hiding specific elements on start but not toggling status
        if (show == 0 || show == 'False') {
            let newClickElementText = clickElementText.replace('?', 'Show');
            document.getElementById(clickElementID).innerText = newClickElementText;

            hideableElements.forEach(hide_element);
        }
        if (show == 1 || show == 'True') {
            let newClickElementText = clickElementText.replace('?', 'Hide');
            document.getElementById(clickElementID).innerText = newClickElementText;
            document.getElementsByName(hideableElementName).hidden = false;

            hideableElements.forEach(show_element);
        }
        console.log('...initializing hide/show of ' + hideableElementName);

    } else {
        if (show == 0) {
            document.getElementById(formElementID).value = 1;  

            let newClickElementText = clickElementText.replace('Show', 'Hide');
            document.getElementById(clickElementID).innerText = newClickElementText;

            hideableElements.forEach(show_element);

        }
        if (show == 1) {
            document.getElementById(formElementID).value = 0;  

            let newClickElementText = clickElementText.replace('Hide', 'Show');
            document.getElementById(clickElementID).innerText = newClickElementText;

            hideableElements.forEach(hide_element);
        }
        if (show == 'False') {
            document.getElementById(formElementID).value = 'True';

            let newClickElementText = clickElementText.replace('Show', 'Hide');
            document.getElementById(clickElementID).innerText = newClickElementText;

            hideableElements.forEach(show_element);

        }
        if (show == 'True') {
            document.getElementById(formElementID).value = 'False';

            let newClickElementText = clickElementText.replace('Hide', 'Show');
            document.getElementById(clickElementID).innerText = newClickElementText;

            hideableElements.forEach(hide_element);
        }
        console.log('...toggle hide/show of ' + hideableElementName);
        return;
    }

}
function show_element(element) {
    element.hidden = false;
    return;
}
function hide_element(element){
    element.hidden = true;
    return;
}
//--------LIST VIEWS: BUTTON CLICKS ON VIEW CHANGES-------------------------------------
function hidden_view_form_submit(actionValue, formElementID, clickElementID, hideableElementName) {
    toggle_hide_element(formElementID, clickElementID, hideableElementName);
    sub_form(actionValue);
    return;
}
function grouping_view_form_submit(actionValue, formElementID, groupingElementID) {
    let groupingValue = document.getElementById(groupingElementID).innerHTML;
    document.getElementById(formElementID).value = groupingValue;
    sub_form(actionValue);
    return;
}
//--------SELECTED ACTIONS: SUBMITTING IDS FOR CHANGES----------------------------------
function selected_action_form_submit(actionValue, groupID) {
    // group Id value stored in form under id 'group_id'
    document.getElementById("group_id").value = groupID;
    sub_form(actionValue);
}

//--------MISC UTILITIES----------------------------------------------------------------
function sub_form(actionValue) {
    console.log("submitting form with value: " + actionValue);
    button.value = actionValue;
    button.click();
    return;
}
function col_header_sort_change(clickedHeaderID, currentSortValueID, currentSortOrderValueID) {
    /*All column header clicks will trigger this function.
     *clickedHeaderID is the ID of the element (column header) that started this function.
     * currentSortValueID - the SortValue is the name of the column we wish to sort by
     * currentSortOrderValueID -- the SortOrder a bool named Descending. If true, items are sorted in desc order, if false, etc
     * DESIRED BEHAVIOR:
     * If user clicks the same header that tables is currently sorted on, toggle the sort order
     * If user clicks on a different header, set the sort to that
     * BACK END NOTES:
     * Will submit the form using the button id grabbed on page load
     */
    let currentSort = document.getElementById(currentSortValueID);
    let currentSortValue = currentSort.value;
    let clickedSortValue = clickedHeaderID;
    let currentSortOrder = document.getElementById(currentSortOrderValueID);
    let currentSortOrderValue = currentSortOrder.value;
    let newSortOrderValue = currentSortOrderValue;
    let columnChange = 0;

    if (currentSortValue != clickedSortValue) {
        columnChange = 1;
    }

    if (columnChange == 0) {
        if (currentSortOrderValue == 'True') {
            newSortOrderValue = 'False';
        } else {
            newSortOrderValue = 'True';
        }
    }
    
    console.log('Changing sort from ' + currentSortValue + '/' + currentSortOrderValue + ' to ' + clickedSortValue + '/' + newSortOrderValue + '...');

    currentSort.value = clickedSortValue;
    currentSortOrder.value = newSortOrderValue;

    let defaultSubmitAction = button.value;
    sub_form(defaultSubmitAction);
    return;
}

function on_start_mark_sorted_column(sortValueID, sortOrderValueID) {
    // Mark the sort header with one of these to indicate to user which column is sorting and in which order: ▲/▼
    // Depends on columnHeader IDs being the identical to the SortValues stored in the table. This will break if they are not
    let sortValue = document.getElementById(sortValueID).value;
    let newSortValue = sortValue;
    let columnHeader = document.getElementById(sortValue);
    let sortOrderValue = document.getElementById(sortOrderValueID).value;

    if (sortOrderValue == 'True') {
        newSortValue = newSortValue + ' ▼';
    } else {
        newSortValue = newSortValue + ' ▲';
    }

    columnHeader.innerHTML = '<a>' + newSortValue + '</a>';
    console.log('Marking sort column :' + newSortValue);
    return;
}
function on_start_get_form_sub_button(buttonID) {
    button = document.getElementById(buttonID);
    return;
}