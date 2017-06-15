$(document).ready(function () {
    function requestStatus() {
        $.ajax({
            url: '?type=status',
            success: function (data) {
                if (data.connection) {
                    if (data.isInput) {
                        $("body").removeClass();
                        $("body").addClass(data.status);
                        $("div#labelArea").text(data.shortName + ': ' + data.longName);
                        $("div#textArea").text(data.status.toUpperCase());
                    }
                    else {
                        $("body").removeClass();
                        $("div#textArea").text('Gekozen input bestaat niet');
                        $("div#labelArea").text('');
                    }
                }
                else {
                    $("body").removeClass();
                    $("div#textArea").text('Er is geen verbinding tussen server en ATEM');
                    $("div#labelArea").text('');
                }
            },
            timeout: 4*1000,
            cache: false,
            dataType: 'json'
        }).fail(function (data) {
            $("body").removeClass();
            $("div#labelArea").text('');
            $("div#textArea").text("Geen verbinding met server...");
        }).always(function() {
    });
    setTimeout(requestStatus, 100);
    }

    requestStatus();
});