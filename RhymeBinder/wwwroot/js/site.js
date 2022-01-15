﻿var button;

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
     * Mark the sort header with one of these to indicate to user which column is sorting and in which order: ▲/▼
     * BACK END NOTES:
     * Will submit the form 
     */
}

function on_start_get_form_sub_button(buttonID) {
    button = document.getElementById(buttonID);
    return;
}