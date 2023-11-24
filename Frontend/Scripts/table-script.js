document.addEventListener('DOMContentLoaded', function() {
    fetchAndDisplayLinks();
});

function navigateToExample() {
    window.location.href = "../Pages/main-menu.html";
}

function fetchAndDisplayLinks() {
    fetch('http://localhost:5180/Link/links')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            if (Array.isArray(data)) {
                displayLinks(data);
            } else {
                throw new Error('Invalid data format');
            }
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function displayLinks(links) {
    const tableBody = document.querySelector('#linksTableBody');

    tableBody.innerHTML = '';

    links.forEach(link => {
        const row = tableBody.insertRow();
        const cellId = row.insertCell(0);
        const cellLongUrl = row.insertCell(1);
        const cellShortUrl = row.insertCell(2);
        const cellClickCount = row.insertCell(3);
        const cellDateCreated = row.insertCell(4);
        const cellDateUpdated = row.insertCell(5);
        const cellAction = row.insertCell(6);

        cellId.textContent = link.id;
        cellLongUrl.textContent = link.longUrl;
        cellShortUrl.textContent = link.shortUrl;
        cellClickCount.textContent = link.clickCount;
        cellDateCreated.textContent = link.dateCreated;
        cellDateUpdated.textContent = link.dateUpdated || 'N/A';

        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Delete';
        deleteButton.addEventListener('click', function() {
            deleteLink(link.id);
        });

        cellAction.appendChild(deleteButton);
    });
}

function deleteLink(linkId) {
    const deleteUrl = `http://localhost:5180/Link/links/${linkId}`;
    fetch(deleteUrl, {
            method: 'DELETE'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            alert("Link has been deleted successfully");
            fetchAndDisplayLinks();
        })
        .catch(error => {
            console.error('There was a problem with the delete operation:', error);
        });
}