function navigateToDatatable() {
    window.location.href = "./Contacts.html";
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