$(document).ready(function () {
    loaded();

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

function loaded() {
    // Set storage usage
    setStoragePercent(500, 1000);
    setStorageText(500, 1000);

    // Initialize files and folders
    appendFolder('My Folder');
    appendFile('Presentation.pptx');
    appendFile('Picture.jpg');
    appendFile('Document.doc');
    appendFile('Sheet.xlsx');
    appendFile('Document.pdf');

    // Set current folder text
    $('#currentfolder').text('Current Folder');

    // Set items count
    $('#itemscount').text(itemscount + ' items');
}

var itemscount = 0;

// Set storage usage text
function setStoragePercent(used, total) {
    // Set progress bar width
    $('#storage_percent').css('width', used / total * 100 + '%');
}

// Set storage text
function setStorageText(used, total) {
    // Set storage text
    $('#storage_text').text(used + ` MB` + ' / ' + total + ` MB`);
}

// Using jquery to append folder to filesandfolders
function appendFolder(name) {
    var template =
        `
                            <div class="storage-item p-2 mb-1 d-flex align-items-center">
                                <div class="me-3">
                                    <i class="bi bi-folder-fill fs-4 folder-icon"></i>
                                </div>
                                <div class="flex-grow-1">
                                    <div class="fw-medium">xfolder_name</div>
                                    <small class="text-muted">3 items - Modified 2 days ago</small>
                                </div>
                                <div class="dropdown">
                                    <button class="btn btn-sm btn-link text-muted" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                        <i class="bi bi-three-dots-vertical"></i>
                                    </button>
                                    <ul class="dropdown-menu dropdown-menu-end">
                                        <li><a class="dropdown-item" href="#"><i class="bi bi-pencil me-2"></i>Rename</a></li>
                                        <li><a class="dropdown-item" href="#"><i class="bi bi-share me-2"></i>Share</a></li>
                                        <li><a class="dropdown-item" href="#"><i class="bi bi-download me-2"></i>Download</a></li>
                                        <li><hr class="dropdown-divider"></li>
                                        <li><a class="dropdown-item text-danger" href="#"><i class="bi bi-trash me-2"></i>Delete</a></li>
                                    </ul>
                                </div>
                            </div>
        `;

    template = template.replace('xfolder_name', name);

    appendItem(template);
}

// Using jquery to append file to filesandfolders
function appendFile(name) {
    // Set the name, set the icon according to the file type
    var icon;
    var color;

    if (name.endsWith('.jpg') || name.endsWith('.jpeg') || name.endsWith('.png') || name.endsWith('.gif')) {
        icon = 'bi-file-earmark-image-fill';
        color = 'image';
    }
    if (name.endsWith('.pdf')) {
        icon = 'bi-file-earmark-pdf-fill';
        color = 'pdf';
    }
    if (name.endsWith('.doc') || name.endsWith('.docx')) {
        icon = 'bi-file-earmark-word-fill';
        color = 'document';
    }
    if (name.endsWith('.xls') || name.endsWith('.xlsx')) {
        icon = 'bi-file-earmark-excel-fill';
        color = 'spreadsheet';
    }
    if (name.endsWith('.ppt') || name.endsWith('.pptx')) {
        icon = 'bi-file-earmark-ppt-fill';
        color = 'presentation';
    }

    var template =
        `
                            <div class="storage-item p-2 mb-1 d-flex align-items-center">
                                <div class="me-3">
                                    <i class="bi xfile_icon fs-4 xcolor_icon-icon"></i>
                                </div>
                                <div class="flex-grow-1">
                                    <div class="fw-medium">xfile_name</div>
                                    <small class="text-muted">3.5 MB - Modified 1 week ago</small>
                                </div>
                                <div class="dropdown">
                                    <button class="btn btn-sm btn-link text-muted" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                        <i class="bi bi-three-dots-vertical"></i>
                                    </button>
                                    <ul class="dropdown-menu dropdown-menu-end">
                                        <li><a class="dropdown-item" href="#"><i class="bi bi-pencil me-2"></i>Rename</a></li>
                                        <li><a class="dropdown-item" href="#"><i class="bi bi-share me-2"></i>Share</a></li>
                                        <li><a class="dropdown-item" href="#"><i class="bi bi-download me-2"></i>Download</a></li>
                                        <li><hr class="dropdown-divider"></li>
                                        <li><a class="dropdown-item text-danger" href="#"><i class="bi bi-trash me-2"></i>Delete</a></li>
                                    </ul>
                                </div>
                            </div>
        `;
    template = template.replace('xfile_icon', icon);
    template = template.replace('xfile_name', name);
    template = template.replace('xcolor_icon', color);

    appendItem(template);
}

function appendItem(item) {
    $('#filesandfolders').append(item);
    itemscount++;
    $('#itemscount').text(itemscount + ' items');
}