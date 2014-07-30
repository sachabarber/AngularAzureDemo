angularAzureDemoServices.service('dialogService', ['$log', function ($log) {

    this.showPleaseWait = function () {
        $('#waitModal').modal('show');
    }
    
    
    this.hidePleaseWait = function () {
        $('#waitModal').modal('hide');
    }


    this.showAlert = function (title, content) {

        $('#alertModalTitle').text(title);
        $('#alertModalBody').text(content);

        $('#alertModal').modal('show');
    }

}]);
