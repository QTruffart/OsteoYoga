(function () {
    var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
    po.src = 'https://apis.google.com/js/client:plusone.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
})();

function signinCallback(authResult) {
    if (authResult['access_token']) {
        console.log(authResult);
        document.getElementById('signinButton').setAttribute('style', 'display: none');
        $.ajax({
            url: 'https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=ya29.bAFQNowK0J3rObIgmY4cnwemHk-u7V9qxihumfwBoDuMn8z8w40_323KHIwjmQdzbgOmxKM6nzgIvA'
        })
          .done(function (html) {
              console.log(html);
          });
    } else if (authResult['error']) {
    }
}