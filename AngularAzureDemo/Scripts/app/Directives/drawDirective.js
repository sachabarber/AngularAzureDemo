angularAzureDemoDirectives.directive("draw", ['$timeout', function ($timeout) {
    return {
        scope: {
            ngModel: '=',
            strokeColor: '=?',
            strokeWidth: '=?',
            updateController: '&updateController'
        },
        link: function (scope, element, attrs) {
            scope.strokeWidth = scope.strokeWidth || 3;
            scope.strokeColor = scope.strokeColor || '#343536';

            var canvas = element[0];
            var ctx = canvas.getContext('2d');

            // variable that decides if something should be drawn on mousemove
            var drawing = false;

            // the last coordinates before the current move
            var lastX;
            var lastY;

            element.bind('mousedown', function (event) {
                lastX = event.offsetX;
                lastY = event.offsetY;

                // begins new line
                ctx.beginPath();

                drawing = true;
            });

            element.bind('mouseup', function (event) {
                // stop drawing
                drawing = false;
                exportImage();
            });

            element.bind('mousemove', function (event) {
                if (!drawing) {
                    return;
                }

                draw(lastX, lastY, event.offsetX, event.offsetY);

                // set current coordinates to last one
                lastX = event.offsetX;
                lastY = event.offsetY;
            });

            scope.$watch('ngModel', function (newVal, oldVal) {
                if (!newVal && !oldVal) {
                    return;
                }

                if (!newVal && oldVal) {
                    reset();
                }
            });

            // canvas reset
            function reset() {
                element[0].width = element[0].width;
            }

            function draw(lX, lY, cX, cY) {
                // line from
                ctx.moveTo(lX, lY);

                // to
                ctx.lineTo(cX, cY);

                ctx.lineCap = 'round';

                // stroke width
                ctx.lineWidth = scope.strokeWidth;

                // color
                ctx.strokeStyle = scope.strokeColor;

                // draw it
                ctx.stroke();
            }

            function exportImage() {
                $timeout(function () {
                    scope.ngModel = canvas.toDataURL();
                    scope.updateController({ canvasArg: canvas.toDataURL() });
                });
            }
        }
    };
}]);