$(document).ready(function () {
    // Parse URL query parameters to get token and email
    const urlParams = new URLSearchParams(window.location.search);
    const token = urlParams.get('token');
    const email = urlParams.get('email');

    // Show loading initially
    $("#loading-section").show();

    // In a real application, you would validate the token with your server
    // For demo purposes, we'll simulate API verification with a timeout
    // and show different statuses based on the token value

    setTimeout(function () {
        $("#loading-section").hide();

        // Demo logic: if token contains "expired", show expired view, otherwise show success
        if (token && token.includes("expired")) {
            // Show expired view
            $("#expired-section").show();
        } else {
            // Show success view
            $("#success-section").show();
        }
    }, 1500); // Simulate 1.5 seconds of verification time

    // Resend confirmation handler
    $("#resend-button").on("click", function () {
        // In a real app, this would call your API to resend the email
        $(this).prop("disabled", true).html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Sending...');

        // Simulate sending
        setTimeout(function () {
            $("#resend-button").prop("disabled", false).text("Resend Confirmation");
            $("#resend-message").show();
        }, 1500);
    });
});