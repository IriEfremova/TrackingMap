function showPrompt(message) {
    return prompt(message, 'Type your name here showPrompt');
}
async function initNavigationMap11() {
    await ymaps.ready;
    const { YMap, YMapDefaultSchemeLayer } = ymaps;

    navMap = new ymaps.Map('map', {
        center: [37.64, 37.64],
        zoom: 15,
        type: 'yandex#satellite',

        controls: []
    });


    placemark = new ymaps.Placemark([37.64, 37.64], {},
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