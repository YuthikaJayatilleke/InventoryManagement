function applyDiscounts(griddata, grid, otherPartyId, srcLocationId) {
    //debugger;
    var data = griddata.dataSource.data();
    var dataToSend = JSON.stringify(data);
    var t = grid.dataSource.transport.options.read.url;
    // grid.dataSource.transport.options.read.url = "../NewInvoice/CalDiscount?data=" + btoa(unescape(encodeURIComponent(dataToSend))) + "&OtherPartyId=" + otherPartyId;
    ///grid.dataSource.transport.options.read.url = "../NewInvoice/CalDiscount?&OtherPartyId=" + otherPartyId;
    //grid.dataSource.transport.options.read.data = "data=" + btoa(unescape(encodeURIComponent(dataToSend))); 
    //grid.dataSource.read();

    $.ajax({
        type: "POST",
        dataType: 'json',
        url: "../NewInvoice/CalDiscount?&OtherPartyId=" + otherPartyId,
        data: "data=" + btoa(unescape(encodeURIComponent(dataToSend))),
        async: false,

        success: function (msg) {
          //  debugger;
            grid.dataSource.data(msg);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            kendo.alert("Error !");
        }
    });

   // debugger;
   // dataSource.read();
    //debugger;
    //griddata.dataSource = dataSource;
    //griddata.dataSource.data(dataSource._data);
   
    }

    function applyCategoryDiscounts(griddata, grid) {
        debugger;
        var formatedvalue = 0;
        var data = griddata.dataSource.data();

        var productData = [];
        for (var i = 0; i < griddata.dataSource.data().length; i++) {

            var line = data[i];
            var item = {};
            item.Id = line.Product.Id;
            item.Qty = line.Qty;
            item.Rate = line.Rate;
            item.Discount = line.DiscPct;
            item.UoM = line.UoM;
            item.Lineid = i;

            productData.push(item);

        }

        var dataToSend = JSON.stringify(productData);

    }

function applyRate(griddata, grid, expartyDropdown, ordersDropDown) {

    var data = griddata.dataSource.data();
    var dataToSend = JSON.stringify(data);
    var linkOrder = ordersDropDown.value();
            if (linkOrder == "") {
                linkOrder = 0;
            }
    $.ajax({
        type: "POST",
        dataType: 'json',
        url: "../NewInvoice/CalRate?&OtherPartyId=" + expartyDropdown.value() + "&linkOrderId=" + linkOrder,
        data: "data=" + btoa(unescape(encodeURIComponent(dataToSend))),
        async: false,

        success: function (msg) {
             debugger;
            grid.dataSource.data(msg);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            kendo.alert("Error !");
        }
    });

}

//    function applyRate(griddata, grid, expartyDropdown, ordersDropDown) {

//        var formatedvalue = 0;
//        var data = griddata.dataSource.data();
//        var productData = [];
//        for (var i = 0; i < griddata.dataSource.data().length; i++) {

//            var line = data[i];
//            var item = {};
//            item.Id = line.Product.Id;
//            item.Qty = line.Qty;
//            item.Rate = line.Rate;
//            item.Discount = line.DiscPct;
//            item.UoM = line.UoM;
//            item.Lineid = line.LinkPoLineId;

//            productData.push(item);

//        }

//        var dataToSend = JSON.stringify(productData);
//        debugger;
//        var linkOrder = ordersDropDown.value();
//        if (linkOrder == "") {
//            linkOrder = 0;
//        }
//        $.ajax({
//            type: "POST",
//            dataType: 'json',
//            url: "../NewInvoice/CalRate?otherpartyId=" + expartyDropdown.value() + "&linkOrderId=" + linkOrder,
//            data: "data=" + btoa(unescape(encodeURIComponent(dataToSend))),
//            async: false,

//            success: function (msg) {
//                debugger;

//                if (msg.Success) {

//                    objs = msg.data;
//                    for (var i = 0; i < griddata.dataSource.data().length; i++) {

//                        var item = data[i];
//                        item.Rate = objs[i].Rate;
//                        item.DiscPct = objs[i].Discount;
//                        item.LinkPoLineId = objs[i].LinkPoLineId;
//                        formatedvalue = parseFloat(Math.round(objs[i].Rate).toFixed(2));
//                        grid.find("tr[data-uid='" + line.uid + "'] td:eq(4)").text(formatedvalue.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
//                        grid.find("tr[data-uid='" + line.uid + "'] td:eq(5)").text(item.DiscPct.toString());
//                    }

//                    if (objs.length > 0) {

//                        ordersDropDown.value(objs[0].Lineid);
//                        ordersDropDown.trigger("change");
//                    }

//                } else {

//                    kendo.alert(msg.msg);
//                }
//            },
//            error: function (XMLHttpRequest, textStatus, errorThrown) {

//                kendo.alert("Error !");
//            }
//        });
//    }