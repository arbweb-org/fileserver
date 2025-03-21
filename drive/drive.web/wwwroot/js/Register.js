$(document).ready(function () {
    let cooldownActive = false;
    let cooldownTimer = 60;
    let intervalId;

    // Form submission handler
    $("#register-form").on("submit", function (e) {
        e.preventDefault();

        const email = $("#email").val().trim();
        const password = $("#password").val().trim();
        const confirmPassword = $("#confirm-password").val().trim();

        // Reset error messages
        $("#error-message").hide();

        // Validate email format
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            $("#error-message").text("Please enter a valid email address").show();
            return;
        }

        // Check password length
        if (password.length < 8) {
            $("#error-message").text("Password must be at least 8 characters").show();
            return;
        }

        // Check if passwords match
        if (password !== confirmPassword) {
            $("#error-message").text("Passwords do not match").show();
            return;
        }

        // Simulate registration success (in a real app, this would be an API call)
        $("#register-form").hide();
        $("#registered-email").text(email);
        $("#confirmation-section").show();

        // In a real application, you would send an email here
        $("#success-message").text("Registration successful! Please check your email.").show();

        // In this demo, we're simulating the email sending
        console.log("Confirmation email sent to: " + email);
    });

    // Resend confirmation email
    $("#resend-button").on("click", function () {
        if (cooldownActive) {
            return;
        }

        const email = $("#registered-email").text();

        // Simulate resending confirmation email
        console.log("Resending confirmation email to: " + email);
        $("#success-message").text("Confirmation email resent! Please check your inbox.").show();

        // Start cooldown
        startCooldown();
    });

    function startCooldown() {
        cooldownActive = true;
        cooldownTimer = 60;
        $("#resend-button").prop("disabled", true);
        $("#cooldown-notice").show();
        $("#resend-timer").text(cooldownTimer);

        intervalId = setInterval(function () {
            cooldownTimer--;
            $("#resend-timer").text(cooldownTimer);

            if (cooldownTimer <= 0) {
                clearInterval(intervalId);
                cooldownActive = false;
                $("#resend-button").prop("disabled", false);
                $("#cooldown-notice").hide();
            }
        }, 1000);
    }

    // Clear error message when user starts typing again
    $("#email, #password, #confirm-password").on("input", function () {
        $("#error-message").hide();
    });
});