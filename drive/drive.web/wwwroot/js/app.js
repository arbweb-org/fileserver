async function SetBusy(isBusy) {
    if (isBusy) {
        $("#spinner").show();
    }
    else {
        $("#spinner").hide();
    }
}