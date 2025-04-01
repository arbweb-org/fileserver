async function MailSent() {
    $("#resend-button").prop("disabled", true);

    CountDown();
}

async function CountDown() {
    let timeLeft = 60; // Set countdown time in seconds
    const countdownElement = $("#resend-timer");
    const cooldownNotice = $("#cooldown-notice");

    $(cooldownNotice).show();
    const countdown = setInterval(() => {
        timeLeft--;
        $(countdownElement).text(timeLeft);

        if (timeLeft <= 0) {
            clearInterval(countdown);
            $("#resend-button").prop("disabled", false);
            $(cooldownNotice).hide();
        }
    }, 1000);
}