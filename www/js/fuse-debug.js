$(document).ready(function (){
	startRefresh();
});

function startRefresh(){
    setTimeout(refresh, 5000); 
	
    function refresh(){
        window.location = location.href;
    }
}
