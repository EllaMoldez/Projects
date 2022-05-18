function stateDetails(productCode) {
    $.get('/States/Details' + StateCode, function (data) {
        $('#stateDetails').modal('show');
        $(".modal-body").html(data);
    });
}

//function createPaging(page, top, totalProducts, baseUrl) {

//    var _HTMLPaging = [];
//    var i = 0;
//    var pages = parseInt(totalProducts / top);
//    if (totalProducts % top != 0) {
//        pages++;
//    }
//    for (i = 0; i < pages; i++) {

//        var activeClass = '';
//        if (page == (i + 1)) {
//            activeClass = 'active';
//        }

//        _HTMLPaging.push('<li class="page-item' + activeClass + '"><a class="page-link" href="' + baseUrl + '?top=' + top + '&page=' + (i + 1).toString() + '">' + (i + 1).toString() + '</a ></li > ');
//    }
//    $(".pagination").html(_HTMLPaging.join(''));

//}