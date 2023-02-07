
function getMagnet(magnetLink) {
    console.log("Cliecked");
    $(document).ready(function () {
        var client = new WebTorrent();
        if (magnetLink != '') {
            client.add(magnetLink, function (torrent) {
                displayTorrentFiles(torrent);
            });
        }
    }); 
   
}

function fetchTorrentFile(id) {
    $.ajax({
        url: "/Torrent/?id=" + id,
        type: "GET",
        success: function (data) {
            var torrentFile = new Blob([data], { type: 'application/x-bittorrent' });
            var client = new WebTorrent();
            client.add(torrentFile, function (torrent) {
                displayTorrentFiles(torrent);
            });
        }
    });
}

async function displayTorrentFiles(torrent) {
    console.log("Received torrent.");
    const files = await Promise.all(torrent.files.map(async file => {
        return {
            name: file.name,
            url: file.getBlobURL()
        };
    }));

    // Clear any existing files from the table
    const tableBody = document.getElementById("torrent-files-body");
    tableBody.innerHTML = "";

    // Add the new files to the table
    files.forEach(file => {
        const row = document.createElement("tr");

        const nameCell = document.createElement("td");
        nameCell.textContent = file.name;
        row.appendChild(nameCell);

        const urlCell = document.createElement("td");
        const link = document.createElement("a");
        link.href = file.url;
        link.textContent = file.url;
        urlCell.appendChild(link);
        row.appendChild(urlCell);

        tableBody.appendChild(row);
    });
}

async function displayTorrent(fileName) {

    $(document).ready(function () {
        fetchTorrentFile(fileName);
    }); 
}
function encode(input) {
    var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    var output = "";
    var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
    var i = 0;
    while (i < input.length) {
        chr1 = input[i++];
        chr2 = i < input.length ? input[i++] : Number.NaN;
        chr3 = i < input.length ? input[i++] : Number.NaN;
        enc1 = chr1 >> 2;
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
        enc4 = chr3 & 63;
        if (isNaN(chr2)) {
            enc3 = enc4 = 64;
        } else if (isNaN(chr3)) {
            enc4 = 64;
        }
        output += keyStr.charAt(enc1) + keyStr.charAt(enc2) +
            keyStr.charAt(enc3) + keyStr.charAt(enc4);
    }
    return output;
}