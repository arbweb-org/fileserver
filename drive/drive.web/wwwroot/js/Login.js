$(document).ready(function () {
    // Form submission handler
    $("#login-form").on("submit", function (e) {
        e.preventDefault();

        const email = $("#email").val().trim();
        const password = $("#password").val().trim();

        // Simple validation
        if (email === "" || password === "") {
            $("#error-message").text("Please fill in all fields").show();
            return;
        }

        // Here you would typically send the data to your server
        // This is just a mockup to demonstrate the UI functionality

        // Simulate authentication (for demo purposes)
        // In a real application, you would send this data to your backend
        if (email === "user@example.com" && password === "password") {
            // Successful login
            $("#error-message").hide();
            alert("Login successful! Redirecting...");
            // Redirect to dashboard or home page would go here
        } else {
            // Failed login
            $("#error-message").text("Invalid email or password").show();
        }
    });

    // Clear error message when user starts typing again
    $("#email, #password").on("input", function () {
        $("#error-message").hide();
    });
});