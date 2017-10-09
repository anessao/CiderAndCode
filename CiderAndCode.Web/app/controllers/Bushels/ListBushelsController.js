app.controller("ListBushelsCtrl", ["$http", "$scope", function ($http, $scope) {
    $scope.bushels = [];
    $scope.pressedValue = false;


    let getBushels = () => {
        $http.get("/api/Bushels/")
            .then(function (result) {
                $scope.bushels = result.data;
            });
    };
    getBushels();

    let updateBushels = (newBushel) => {
        $http.put(`/api/Bushels/updatebushel`, {
            Id: newBushel.Id,
            Pressed: newBushel.Pressed,
            Quantity: newBushel.NumberOfBushels
        })
            .then(function (result) {
                console.log(result);
                getBushels();
            });
    }
    let createCider = (id) => {
        $http.post(`api/cider`, { BushelId: id }).then(function (results) {
            console.log(results);
            getBushels();
        }).catch(function (error) { console.log("there was an error with making cider", error.message)});
    }

    $scope.stealBushel = (bushel) => {
        var valueToSteal = document.getElementById("bushelInput-" + bushel.Id).value;
        var newBushelQuantity = bushel.NumberOfBushels - valueToSteal;
        bushel.NumberOfBushels = newBushelQuantity;
        if (bushel.NumberOfBushels == 0) {
            $http.delete(`/api/Bushels/${bushel.Id}`).then(function (result) {
                getBushels();
            });
        } else {
            updateBushels(bushel);
        }
    }

    $scope.pressBushel = (bushel) => {
        createCider(bushel.Id);
    }
    
}]);