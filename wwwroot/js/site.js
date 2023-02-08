var client;
function getMagnet(magnetLink) {
    console.log("Clicked");
    $(document).ready(function () {
        client = new WebTorrent();
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
            client = new WebTorrent();
            client.seed(torrentFile, function (torrent) {
                displayTorrentFiles(torrent);
            });
        }
    });
}

function displayTorrentFiles(torrent) {
    var tableBody = $('#torrent-table-body');
    tableBody.empty();

    if (torrent.files.length === 0) {
        tableBody.append("<tr><td colspan='3'>No Files in Torrent</td></tr>");
        return;
    }

    torrent.files.forEach(function (file) {
        var row = $("<tr></tr>");
        row.append("<td>" + file.name + "</td>");
        row.append("<td>" + file.length + "</td>");
        var actionsCol = $("<td></td>");
        var downloadBtn = $("<button class='btn btn-primary btn-sm mr-1'>Download</button>");
        var streamBtn = $("<button class='btn btn-primary btn-sm'>Stream</button>");
        downloadBtn.click(function () {
            downloadFile(file, torrent);
        });
        streamBtn.click(function () {
            streamFile(file, torrent);
        });
        actionsCol.append(downloadBtn);
        actionsCol.append(streamBtn);
        row.append(actionsCol);
        tableBody.append(row);
    });
}



function downloadFile(file, torrent) {
    console.log("Downloding: " + file.name + " is array:" + Array.isArray(torrent.files));
    file.getBlobURL(function (err, url) {
        if (err) throw err
        var a = document.createElement('a')
        a.download = file.name
        a.href = url
        a.click()
    });
}
 

function streamFile(file,torrent) {
    // Add code to stream the file here
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