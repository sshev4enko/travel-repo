﻿define(['./module'], function (directives) {
    'use strict';
    directives.directive('route', [function () {
        var buildHtml = function (route) {
            return '<a class="list-group-item">' +
                        '<bold><h4 class="list-group-item-heading">Start place: ' + route.startPlace.name + ' (' + route.startPlace.formatted_address + ')' + '</h4></bold>' +
                        '<bold><h4 class="list-group-item-heading">Finish place: ' + route.finishPlace.name + ' (' + route.finishPlace.formatted_address + ')' + '</h4></bold>' +
                        '<p class="list-group-item-text">' + (route.Comment || '') + '</p>' +
                        '<p class="list-group-item-text">Spent money: ' + route.Cost + '</p>' +
                    '</a>';
        };

        return {
            restrict: 'E',
            replace: true,
            template: '<div></div>',
            link: function (scope, element, attrs) {
                var route = element.data('route').data;

                var map = new google.maps.Map(element.get()[0], {});
                var service = new google.maps.places.PlacesService(map);
                service.getDetails({
                    placeId: route.StartGPlaceId
                }, function (place, status) {
                    if (status == google.maps.places.PlacesServiceStatus.OK) {
                        route.startPlace = place;
                        if (route.finishPlace) {
                            element.append(buildHtml(route));
                        }
                    }
                });
                service.getDetails({
                    placeId: route.FinishGPlaceId
                }, function (place, status) {
                    if (status == google.maps.places.PlacesServiceStatus.OK) {
                        route.finishPlace = place;
                        if (route.startPlace) {
                            element.append(buildHtml(route));
                        }
                    }
                });
            }
        };
    }]);
});