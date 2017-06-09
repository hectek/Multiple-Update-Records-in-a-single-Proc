// ========================================= NAVIGATION 'orderNavigationBarController' ======================================//
(function () {
    "use strict";

    angular.module(APPNAME).controller('orderNavigationBarController', OrderNavController);
    OrderNavController.$inject = ['$scope', '$baseController', '$cmsService', '$serverModel', '$location', '$anchorScroll'];

    function OrderNavController($scope, $baseController, $cmsService, $serverModel, $location, $anchorScroll) {

        var vm = this;
        vm.$scope = $scope;
        $baseController.merge(vm, $baseController);
        vm.cmsService = $cmsService;
        vm.serverModel = $serverModel;
        vm.cmsPageId = vm.$routeParams.id;

        vm.notify = vm.cmsService.getNotifier($scope);

        //FUNCTIONS
        vm.saveChanges = _saveChanges;

        _render();

        // START UP FUNCTION
        function _render() {
             
            _setUp();
            _toggleMainOrSubMenu();

        }

        // SETUP VARIABLES
        function _setUp() {
            vm.cmsPage;
            vm.mainTabs = [];
            vm.subTabs = [];
            vm.mainMenu = false;
            vm.subMenu = false;
            vm.updateList = {
                pairs: []
            }

        }

        //  TOGGLE BETWEEN MAIN MENU OR SUBMENU
        function _toggleMainOrSubMenu() {
            if (vm.cmsPageId > 0) {
                _scrollTop();
                vm.cmsService.getById(vm.cmsPageId, _onGetbyId, _getOnError);
                vm.cmsService.getBranch(vm.cmsPageId, _onGetBranch, _getOnError)
                vm.subMenu = true;
            }
            else {
                vm.cmsService.getNavigationPages(_onGetNavBarTabs, _getOnError);
                vm.mainMenu = true;
            }

        }

        // -------------------------- MAIN FUNCTIONS  -----------------------------//

        // ON GET NAV BAR SUCCESS
        function _onGetNavBarTabs(data) {
            if (data && data.items) {
                vm.notify(function () {
                    vm.tabs = data.items;
                    for (var i = 0; i < vm.tabs.length; i++) {
                        if (vm.tabs[i].parentId == null) {
                            vm.mainTabs.push(vm.tabs[i]);
                        }
                    }
                });
            }
        }

        //  ON GET SUBMENU ITEMS
        function _onGetBranch(data) {
            if (data) {
                vm.notify(function () {
                    vm.subTabs = data.items;
                });
            }
        }

        // HANDLER FOR SAVE CHANGES
        function _saveChanges() {
            if (vm.cmsPageId > 0) {
                _assignNaVOrder(vm.subTabs);
            }
            else {
                _assignNaVOrder(vm.mainTabs);
            }
            window.location.href = '/admin/cms/pages/manage/#!';
        }

        // UPDATE PAGES WITH NEW PAGE ORDER
        function _assignNaVOrder(tabs) {
            var updateList = [];
            for (var i = 0; i < tabs.length; i++) {
                var curTab = tabs[i];
                var pair = {
                    One: null
                    , Two: null
                };
                curTab.pageOrder = tabs.indexOf(curTab) + 1;
                pair.One = curTab.id;
                pair.Two = curTab.pageOrder;
                vm.updateList.pairs.push(pair);
            }
            vm.cmsService.updateMultiPageOrder(JSON.stringify(vm.updateList), _onPageOrderSuccess, _updateCmsOnError);
        }

        // ON GET BY ID
        function _onGetbyId(data) {
            if (data) {
                vm.notify(function () {
                    vm.tabName = data.item.name;
                    vm.currentTab = data.item;
                });
            }
        }

        // ON UPDATE SUCCESS
        function _onPageOrderSuccess(jqXHR) {
        }

        // SCROLL TOP FUNCTION
        function _scrollTop() {
            document.getElementById('top').scrollIntoView();
        }


        //ERROR EVENT FUNCTIONS
        function _updateCmsOnError(jqXHR) {
            vm.$alertService.error(jqXHR.responseText, "Update CMS failed");
        }
        function _getOnError(jqXHR) {
            vm.$alertService.error(jqXHR.responseText, "Get CMS failed");
        }

    }


})();