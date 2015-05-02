function ajaxWithResultInModal(targetUrl, targetData, targetTitle, funcOnSuccess) {
    $.ajax({
        url: targetUrl,
        data: targetData,
        type: 'POST',
        success: function (result) {
            new Messi(result, { title: targetTitle, modal: true, titleClass: 'info' });
            if(funcOnSuccess != null) funcOnSuccess();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            new Messi(errorThrown, { title: 'Error', titleClass: 'anim error' });
            return false;
        }
    });
}