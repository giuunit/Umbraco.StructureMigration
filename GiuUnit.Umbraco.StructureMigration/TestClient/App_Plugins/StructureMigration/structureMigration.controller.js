'use strict';

angular.module("umbraco").controller("StructureMigrationController",
    function ($scope, $http) {
        $http.get("/umbraco/backoffice/api/structuremigration/renderviewdata").then(function (response) {

            if (response.status === 200) {
                var results = response.data;

                $scope.data = results;

                $scope.selectedMigration = results.migrations[0];
                $scope.selectedProperty = results.propertyTypes[0];
            }
        });

        $scope.addTransitionProp = function () {

            $http.post("/umbraco/backoffice/api/structuremigration/AddTransitionProperty",
                {
                    selectedMigration: $scope.selectedMigration,
                    selectedProperty: $scope.selectedProperty
                })
                .then(function (response) {
                    //success
                    if (response.status == 200) {
                        var transitionProperty = response.data;

                        $scope.step1Success = true;
                        $scope.transitionProperty = transitionProperty;
                    }
                });
        }

        $scope.migrateData = function () {
            $http.post("/umbraco/backoffice/api/structuremigration/migrateData",
                {
                    selectedMigration: $scope.selectedMigration,
                    selectedProperty: $scope.selectedProperty,
                    transitionProperty: $scope.transitionProperty
                })
                .then(function (response) {
                    //success
                    if (response.status == 200) {
                        $scope.step2Success = true;
                    }
                });
        }

        $scope.renameAndDelete = function () {
            $http.post("/umbraco/backoffice/api/structuremigration/renameDelete",
                {
                    selectedMigration: $scope.selectedMigration,
                    selectedProperty: $scope.selectedProperty,
                    transitionProperty: $scope.transitionProperty
                })
                .then(function (response) {
                    //success
                    if (response.status == 200) {
                        $scope.step3Success = true;
                    }
                });
        }

        $scope.refresh = function () {
            //possible dependency here but I am tired rn
            window.location.reload();
        }
    });