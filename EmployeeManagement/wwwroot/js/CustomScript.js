function confirmdelete(uniqueId, isDeleteClicked) {
	var deleteSpan = 'DeleteSpan_' + uniqueId;
	var confirmDeleteSpan = 'ConfirmDeleteSpan_' + uniqueId;

	if (isDeleteClicked) {
		$('#' + deleteSpan).hide();
		$('#' + confirmDeleteSpan).show();
	} else {
		$('#' + deleteSpan).show();
		$('#' + confirmDeleteSpan).hide();
	}
}