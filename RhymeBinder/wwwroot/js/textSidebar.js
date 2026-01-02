//--------EDIT VIEW SIDEBAR-------------------------------------------------------------
function insert_new_text_in_sequence(groupId, value) {
    document.getElementById("SequenceGroupId").value = groupId;
    selected_action_form_submit('InsertNewTextInSequence', value);
}