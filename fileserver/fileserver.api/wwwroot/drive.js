$(document).ready(function () {
    // Initialize upload modal
    const uploadModal = new bootstrap.Modal(document.getElementById('uploadModal'));

    // Show upload modal when upload button is clicked
    $('#uploadButton').click(function () {
        uploadModal.show();
    });

    // Handle item selection
    $('.storage-item').click(function (e) {
        // Don't trigger if clicking on dropdown or dropdown button
        if ($(e.target).hasClass('btn-link') || $(e.target).hasClass('bi-three-dots-vertical') ||
            $(e.target).parents('.dropdown-menu').length) {
            return;
        }

        // Toggle selection class
        $(this).toggleClass('selected');

        // Count selected items
        const selectedCount = $('.storage-item.selected').length;

        // Enable/disable action buttons based on selection
        if (selectedCount > 0) {
            $('#shareButton, #downloadButton, #moreButton').prop('disabled', false);
        } else {
            $('#shareButton, #downloadButton, #moreButton').prop('disabled', true);
        }
    });

    // Handle folder double-click (navigation)
    $('.storage-item').dblclick(function () {
        // Check if this is a folder (has folder-icon)
        if ($(this).find('.folder-icon').length) {
            // In a real app, this would navigate to the folder
            console.log('Navigating to folder: ' + $(this).find('.fw-medium').text());
            // For this demo, do nothing
        }
    });
});