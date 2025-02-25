var button;
var showSequence = 0;

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
function show_sequence_inputs(savedViewId) {
    var inputs = document.getElementsByClassName("sequence-number-input");
    var numbers = document.getElementsByName("groupSequence");
    for (let i = 0; i < inputs.length; i++) {
        inputs[i].hidden = false;
        numbers[i].hidden = true;
    }
    let sequenceButton = document.getElementById("updateSequenceButton");
    sequenceButton.onclick = function () { selected_action_form_submit('UpdateGroupSequence', savedViewId); };
    sequenceButton.innerText = "Update Sequence";
}


//--------MODALS------------------------------------------------------------------------
function open_group_list_modal_with_id(elementId, view) {
    if (view == 'ListTexts') {
        populate_list_modal_footer_with_record_count_message();
        populate_group_selected_text_header_counts();
    }
    if (view == 'EditText') {
        populate_group_selected();
    }
    document.getElementById(elementId).style.display = "inline";
    document.body.style.pointerEvents = 'none';
    return;
}

function open_modal_with_Id(elementId) {
    document.getElementById(elementId).style.display = "inline";
    document.body.style.pointerEvents = "none";
}

function close_modal_with_id(elementId, view) {
    if (view == 'ListTexts') {
        populate_group_selected_text_header_counts();
    }
    if (view == 'EditText') {
        populate_group_selected(); // reverts check boxes to state on load
    }
    document.getElementById(elementId).style.display = "none";
    document.body.style.pointerEvents = 'all';
    return;
}
function close_modal_with_id(elementId) {
    document.getElementById(elementId).style.display = "none";
    document.body.style.pointerEvents = 'all';
}

function submit_modal(view) {
    set_group_selected_values_to_checkbox_states(); // updates model values to pass back to controller

    if (view == 'ListTexts') {
        selected_action_form_submit('GroupAddRemove', 1);
    }
    if (view == 'TransferBinder') {
        selected_action_form_submit('Transfer', 1);
    }
    if (view == 'EditText') {
        selected_action_form_submit('Save', 1);
    }
    return;
}

function set_modal_submit_button_availability(checkboxId, buttonId) {
    if (document.getElementById(checkboxId).checked == true) {
        document.getElementById(buttonId).disabled = false;
    } else {
        document.getElementById(buttonId).disabled = true;
    }
}

function populate_list_modal_footer_with_record_count_message() {
    var checkedBoxes = 0;
    var inputs = document.getElementsByTagName("input");

    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "checkbox" && inputs[i].checked == true && inputs[i].name.startsWith("TextHeaders")) {
            checkedBoxes++;
        }
    }

    var footerRecordCounts = document.getElementsByName("recordCountMessage");
    if (checkedBoxes == 0) {
        for (let i = 0; i < footerRecordCounts.length; i++) {
            footerRecordCounts[i].innerText = "No records selected!";
        }
        
    } else {
        for (let i = 0; i < footerRecordCounts.length; i++) {
            footerRecordCounts[i].innerText = "Apply changes to " + checkedBoxes + " selected records: ";
        }
    }


    document.GetElements
    return;
}

function populate_group_selected_text_header_counts() {
    /*
     * I want a modal of groups to be added/removed to the selected texts
     *  If all of the selected texts are in a group, that group will be checked
     *      If that check is un-selected, back end will remove all selected texts from that group
     *      If it remains checked, nothing changes
     *  If none of the selected texts are in a group, that group will be un-checked
     *      ...behavior obvious from here
     *  If SOME of the selected texts are in a group, that group check box will be indeterminate
     *      If the user does not check/uncheck the box, no changes are made
     *      If the users checks the box, all selected texts not already in group are added to the group
     *      If the user unchecks the box, all selected texts already in group are removed
     *      
     *      
     * This is some non-scalable very specific business using the names and ids from the ListTexts form
     * 
     * If I had an API I could query this would be necessary
     * 
     * As it is now:
     * Back end gets a list of texts header ids associated with a group
     * This function gets all selected text header IDs
     * ...gets all text header IDs associated with groups from a list of hidden div tags
     * compares the two, gives the user a count
     * sets the checkboxes accordingly
     * 
     * */
    var groupIds = document.getElementsByName("GroupId");


    // Get Selected Text Header IDs
    var inputs = document.getElementsByTagName("input");
    var selectedTextHeaderIds = [];
    
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "checkbox" && inputs[i].checked == true && inputs[i].name.startsWith("TextHeaders")) {
            var index = inputs[i].name.replace("TextHeaders[", "").replace("].Selected", "");

            //console.log("got index " + index);
            selectedTextHeaderIds.push(document.getElementById("TextHeaders[" + index + "].TextHeaderId").value);

        }
    }

    // If no headers, disable and move on
    if (selectedTextHeaderIds.length == 0) {

        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].type == "checkbox" && inputs[i].name == "GroupCheckbox") {
                inputs[i].disabled = true;
            }
        }

    } else {    
        for (var i = 0; i < groupIds.length; i++) {

            // Get text header IDs associated with group
            var elementName = "Group" + groupIds[i].innerHTML + "TextId";
            //console.log("getting text header ids from divs with name " + elementName);
            var groupTextHeaderIds = document.getElementsByName(elementName);

            // count number of matches between selected text headers and group text headers
            if (groupTextHeaderIds.length > 0) {

                elementName = "Group" + groupIds[i].innerHTML + "SelectedTextCount";
                //console.log("setting count value at div with id " + elementName);

                var matchCount = 0;

                for (var j = 0; j < groupTextHeaderIds.length; j++) {
               
                    for (var k = 0; k < selectedTextHeaderIds.length; k++) {
                        // console.log("comparing group text header id " + groupTextHeaderIds[j].innerHTML + "with selected text id " + selectedTextHeaderIds[k])
                        if (groupTextHeaderIds[j].innerHTML == selectedTextHeaderIds[k]) {
                            matchCount++;
                        }
                    }
                }

                // compare match count with total selected and set checkboxes accordingly
                if (selectedTextHeaderIds.length == matchCount) {
                    document.getElementById("Group" + groupIds[i].innerHTML).checked = true;
                } else if (matchCount == 0 || matchCount == null) {
                    document.getElementById("Group" + groupIds[i].innerHTML).checked = false;
                } else {
                    document.getElementById("Group" + groupIds[i].innerHTML).indeterminate = true;
                    document.getElementById(elementName).innerHTML = " (" + matchCount + ")  ";
                }
            }
        }
    }
    return;
}

function populate_group_selected() {
    var groupIds = document.getElementsByName("GroupId");

    for (var i = 0; i < groupIds.length; i++) {
        if (document.getElementById("Group" + groupIds[i].innerHTML + "Selected").value == "True") {
            document.getElementById("Group" + groupIds[i].innerHTML).checked = true;
        } else {
            document.getElementById("Group" + groupIds[i].innerHTML).checked = false;
        }
    }
      
    return;
}

function set_group_selected_values_to_checkbox_states() {
    var groupIds = document.getElementsByName("GroupId");

    for (var i = 0; i < groupIds.length; i++) {
        var checkbox = document.getElementById("Group" + groupIds[i].innerHTML);

        if (checkbox.checked && !checkbox.indeterminate) {
            document.getElementById("Group" + groupIds[i].innerHTML + "Selected").value = true;
        } else if (!checkbox.checked && !checkbox.indeterminate) { //specifically don't want indeterminate
            document.getElementById("Group" + groupIds[i].innerHTML + "Selected").value = false;
        } 
    }

    return; 
}

function set_group_selected_value_on_checkbox(groupId){
    var checkbox = document.getElementById("Group" + groupId);

    if (checkbox.checked) {
        document.getElementById("Group" + groupId + "Selected").value = true;
    } else {
        document.getElementById("Group" + groupId + "Selected").value = false;
    }
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
function selected_action_form_submit(actionValue, recordId) {
    console.log("I am being submitted!");
    // group Id value stored in form under id 'record_id'
    document.getElementById("record_id").value = recordId;
    sub_form(actionValue);
}

//--------SEARCH ACTIONS: SUBMITTING VALUE FOR SEARCH----------------------------------
function clear_search() {
    document.getElementById("View_SearchValue").value = "";
    sub_form('LastView');
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

function toggle_select_all_text_headers() {
    var selectAllCheckbox = document.getElementById("SelectAll");

    var inputs = document.getElementsByTagName("input");
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "checkbox" && inputs[i].name.startsWith("TextHeaders")) {
            inputs[i].checked = selectAllCheckbox.checked;
        }
    }
    return;
}