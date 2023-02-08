var client;
function getMagnet(magnetLink) {  
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
    $("#table-header").text("Torrent: " + torrent.name);
    tableBody.empty();

    if (torrent.files.length === 0) {
        tableBody.append("<tr><td colspan='3'>No Files in Torrent</td></tr>");
        return;
    }

    torrent.files.forEach(function (file) {

        var row = $("<tr></tr>");
        row.append("<td>" + file.name + "</td>");
        row.append("<td>" + getFileSize(file.length) + "</td>");
        var actionsCol = $("<td></td>");
        var downloadBtn = $("<button class='btn btn-primary btn-sm mr-1'>Download</button>");
        var streamBtn = $("<button class='btn btn-danger btn-sm'>Stream</button>");        
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

function getFileSize(size) {
    var kbs = size / 1024;
    if (kbs >= 1000) {
        return (kbs / 1024).toFixed(1) + ' mb';
    }
    return kbs.toFixed(1) + ' kb';
}

function downloadFile(file, torrent) {
    console.log("Downloding: " + file.name + " is array:" + Array.isArray(torrent.files));
    const fileSize = file.length;
    torrent.on('download', function (bytes) {
        
        const percent = (bytes / fileSize * 100).toFixed(2);
        console.log(percent);
        $("#download-progress").css("width", percent + "%");
        $("#download-progress").attr("aria-valuenow", percent);
    });
    file.getBlobURL(function (err, url) {
        if (err) throw err
        var a = document.createElement('a')
        a.download = file.name
        a.href = url
        a.click()
    });

    $("#download-progress-container").show();
    $("#download-file-name").text(file.name);  
 
}
 

function streamFile(file,torrent) {
    // Add code to stream the file here
}


async function displayTorrent(fileName) {

    $(document).ready(function () {
        fetchTorrentFile(fileName);
    });
}
