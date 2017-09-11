$(document).ready(function (){
	var refreshValue = getUrlParameter('refresh');
	if (refreshValue === 'true') {
		startRefresh();
	}
});

function startRefresh(){
    setTimeout(refresh, 5000);

    function refresh(){
        window.location = location.href;
    }
}

function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1];
        }
    }
};
