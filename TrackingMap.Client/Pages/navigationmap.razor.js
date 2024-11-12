var navMap;
var placemark;

// <button class="btn btn-danger" @onclick="@(async () => await DeleteItem(item.Id))">
async function initNavigationMap() {
    await ymaps.ready;
    const { YMap, YMapDefaultSchemeLayer } = ymaps;

    navMap = new ymaps.Map('map', {
        center: [58.172325, 59.8186],
        zoom: 15,
        type: 'yandex#satellite',

        controls: []
    });


    placemark = new ymaps.Placemark([58.172325, 59.8186], {},
        {
            preset: 'islands#circleDotIcon',
            iconColor: 'red',
            draggable: true
        });

    navMap.geoObjects.add(placemark);
    movePlacemark();
}

function movePlacemark() {
    var pt = placemark.geometry.getCoordinates();
    placemark.geometry.setCoordinates([pt[0] + 0.00001, pt[1] + 0.00001]);

    setTimeout("movePlacemark();", 1000);
}

function addLine(listCoordinate) {
    var polyline = new ymaps.Polyline([
        [37.64, 37.64], [49.64, 27.64], [57.64, 47.64], [67.64, 47.64]
    ], {
        hintContent: "Path"
    }, {
        draggable: true,
        strokeColor: '#FF0000',
        strokeWidth: 4
    });

    navMap.geoObjects.add(polyline);
}

function Point(latitude, longitude) {
    latitude: latitude;
    longitude: longitude;
}

function showPrompt(arrayCoordinates) {

    //alert("asjf;ajf;ajs;f" + arrayCoordinates[0].latitude + "  " + arrayCoordinates[0].longitude);
    //alert("asjf;ajf;ajs;f" + arrayCoordinates[1].latitude + "  " + arrayCoordinates[1].longitude);
    alert("Length = " + arrayCoordinates.length);
    alert("Value = " + arrayCoordinates[0].latitude + "  " + arrayCoordinates[0].longitude);

    var coords = [];
    for (var i = 0; i < arrayCoordinates.length; ++i) {

        coords[i] = [];
        coords[i][0] = arrayCoordinates[i].latitude;
        coords[i][1] = arrayCoordinates[i].longitude;
    }
    
    alert(coords);

    var Data = [
        [37.64, 37.64],
        [37.74, 37.64],
        [37.74, 37.74],
        [37.64, 37.74]
    ];

    alert(Data);

    var polyline = new ymaps.Polyline(coords, {
        hintContent: "Path"
    }, {
        draggable: true,
        strokeColor: '#FF0000',
        strokeWidth: 2
    });

    navMap.geoObjects.add(polyline);
}

