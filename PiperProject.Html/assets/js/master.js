
$(document).ready(main);

function main() {

    $("#hashSubmit").click(GetData);
    $('#hashBox').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $("#hashSubmit").click();
            return false;
        }
    });

    //TryHash();
}

var GetResource = "/api/document/";

function TryHash() {

    var url = window.location.hash;

    if (url != '') {

        $("#hashBox").val(url.replace("#", ""));
        $("#hashSubmit").click();
    }
}

function GetData() {

    var url = $("#hashBox").val();

    var host = "http://" + url.split("/")[0];
    var lookup = url.split("/")[1];

    // window.location.hash = url;

    var hash = Hash(lookup);

    Test(host, hash, lookup, function (data) {

        var body = data.Body;

        var size = formatSizeUnits(byteCount(body));

        log("Downloaded " + size + "");

        var plain = Decrypt(body, lookup);

        log("Body decrypted successfully");

        $("#view").val(plain);
    });
}


function byteCount(str) {
    // returns the byte length of an utf8 string
    var s = str.length;
    for (var i = str.length - 1; i >= 0; i--) {
        var code = str.charCodeAt(i);
        if (code > 0x7f && code <= 0x7ff) s++;
        else if (code > 0x7ff && code <= 0xffff) s += 2;
        if (code >= 0xDC00 && code <= 0xDFFF) i--; //trail surrogate
    }
    return s;
}

function formatSizeUnits(bytes) {
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return 'n/a';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    if (i == 0) return bytes + ' ' + sizes[i];
    return (bytes / Math.pow(1024, i)).toFixed(1) + ' ' + sizes[i];
};

function log(str) {

    $("#log").val($("#log").val() + str + ".");
    $('#log').scrollTop($('#log')[0].scrollHeight);
    $("#log").val($("#log").val() + "\n");
}

function Hash(lookup) {

    var md = forge.md.sha256.create();
    md.update(lookup);

    return md.digest().toHex();
}

function Test(host, hash, lookup, func) {

    $.ajax({
        dataType: "json",
        type: "OPTIONS",
        url: host + GetResource + hash,
        success: function (data) {

            log("Found " + lookup + " as " + hash.substr(0, 16));
            log("Asserting vector against '" + data + "'");

            var result = Decrypt(data, lookup);

            if (result == lookup) {

                log("Head is sane, lookup of " + lookup + " was OK");
                Get(host, hash, func);
            }
        },
        error: error
    });
}

function error(data) {

    log("An error has occured");
}

function Get(host, hash, func) {

    $.ajax({
        dataType: "json",
        url: host + GetResource + hash,
        success: func,
        error: error
    });
}

function Decrypt(data, password) {

    var salt = GetSalt(password);
    var key = GetPassword(password);

    var crypto = forge.util.decode64(data);

    var buffer = forge.util.createBuffer(crypto);

    var decipher = forge.cipher.createDecipher('AES-CBC', key);
    decipher.start({ iv: salt });
    decipher.update(buffer);
    decipher.finish();

    return decipher.output.data;
}

function GetBytes(str) {

    var bytes = [];

    for (var i = 0; i < str.length; ++i) {
        bytes.push(str.charCodeAt(i));
    }

    return bytes;
}

function GetPassword(password) {

    var hash = Hash(password).substring(0, 16);

    var key = forge.pkcs5.pbkdf2(password, hash, 1000, 32);

    return key;
}

function GetSalt(password) {

    var hash = Hash(password).substring(0, 16);

    return hash;
}

