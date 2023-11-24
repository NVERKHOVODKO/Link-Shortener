function navigateToDatatable() {
    window.location.href = "../Pages/table.html";
}

function shortenUrl() {
    const longUrlInput = document.getElementById('longUrl');
    const shortUrlInput = document.getElementById('shortUrl');
    const errorMessageDiv = document.getElementById('errorMessage');

    const longUrl = longUrlInput.value;

    if (!longUrl.trim()) {
        errorMessageDiv.textContent = 'Url is empty';
        errorMessageDiv.classList.remove('hidden');
        return;
    } else {
        errorMessageDiv.classList.add('hidden');
    }

    fetch('http://localhost:5180/Link/shorten', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(longUrl),
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('Shortened URL:', data.redirectUrl);
            shortUrlInput.value = data.redirectUrl;

            var shortenUrlSection = document.getElementById('shortenUrlSection');
            shortenUrlSection.classList.add('visible'); // Use add to explicitly add the class
        })
        .catch(error => {
            // Error
            console.error('There was a problem with the fetch operation:', error);
        });
}


function copyUrl() {
    const shortUrlInput = document.getElementById('shortUrl');

    shortUrlInput.select();
    document.execCommand('copy');

    alert('Link copied to clipboard!');
}

function openEditLinkModal() {
    document.getElementById("id").value = "";
    document.getElementById("editLink").value = "";
    document.getElementById("shortLink").value = "";
    document.getElementById("clickCount").value = "";

    var modal = document.getElementById("editLinkModal");
    modal.style.display = "block";
}

function closeEditLinkModal() {
    var modal = document.getElementById("editLinkModal");
    modal.style.display = "none";
}

function editLink() {
    getLinkDetails();
}


function validateId() {
    const linkId = document.getElementById('id').value;
    const errorMessageEdit = document.getElementById('errorMessageEdit');

    if (!linkId) {
        errorMessageEdit.innerText = 'ID cannot be empty';
        errorMessageEdit.classList.remove('hidden');
    } else {
        errorMessageEdit.innerText = '';
        errorMessageEdit.classList.add('hidden');
    }
}


function getLinkDetails() {
    const linkId = document.getElementById('id').value;
    if (!linkId) {
        alert('Please enter a link ID');
        return;
    }
    fetch(`http://localhost:5180/Link/links/${linkId}`)
        .then(response => {
            if (!response.ok) {
                if (response.status === 400 || response.status === 404) {
                    alert('Link not found or invalid request');
                } else {
                    throw new Error('Network response was not ok');
                }
            }
            return response.json();
        })
        .then(data => {
            if (data) {
                document.getElementById('editLink').value = data.longUrl;
                document.getElementById('shortLink').value = data.shortUrl;
                document.getElementById('clickCount').value = data.clickCount;
            } else {
                alert('Link details not available');
            }
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

async function editLink() {
    const linkId = document.getElementById('id').value;
    const newLongUrl = document.getElementById('editLink').value;

    if (!linkId || !newLongUrl) {
        alert('ID or Full link cannot be empty');
        return;
    }

    const requestBody = {
        "id": linkId,
        "newLongUrl": newLongUrl
    };

    try {
        const response = await fetch('http://localhost:5180/Link/links/edit-link', {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestBody)
        });
        console.log(response);
        if (response.ok) {
            alert('Link edited successfully');
        } else if (response.status === 400 || response.status === 404) {
            alert('Failed to edit link. Invalid request or link not found.');
        } else {
            throw new Error('Network response was not ok');
        }
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
    }
}