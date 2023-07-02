import {PermissionCheckerService} from 'abp-ng2-module';
import {AppSessionService} from '@shared/common/session/app-session.service';

import {Injectable} from '@angular/core';
import {AppMenu} from './app-menu';
import {AppMenuItem} from './app-menu-item';

@Injectable()
export class AppNavigationService {

    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService
    ) {

    }

    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu', [
            // new AppMenuItem('Dashboard', 'Pages.Administration.Host.Dashboard', 'flaticon-line-graph', '/app/admin/hostDashboard'),
            // new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'flaticon-line-graph', '/app/main/dashboard'),
            new AppMenuItem('ShareDirectory', 'SharedDriectory', 'flaticon-list-2', '', [], [
                new AppMenuItem('VenderManament', null, 'flaticon-menu-1', '/app/main/master/app-vender'),
                new AppMenuItem('ListAssetGroup', null, 'flaticon-list-1', '/app/main/master/app-asset-group'),
                new AppMenuItem('ListAsset', null, 'flaticon-list-1', '/app/main/master/app-asset'),
                // new AppMenuItem('CourceSafetyManament', null, 'flaticon-map', '/app/main/master/app-cource-safety')
                new AppMenuItem('LearnedSafetyManament', null, 'flaticon-map', '/app/main/master/MstEmployeesLearnedSafety'),
                new AppMenuItem('NoteText', null, 'flaticon-map', '/app/main/master/app-mst-note')
            ]),
            new AppMenuItem('MyRequest', 'Request', 'flaticon-paper-plane', '', [], [
                new AppMenuItem('WaitRequest', null, 'flaticon-statistics', '/app/main/asset-manament/app-request-asset-bring/0/m_waiting'),
                new AppMenuItem('Approved', null, 'flaticon-folder-1', '/app/main/asset-manament/app-request-asset-bring/0/m_approved'),
                new AppMenuItem('Rejected', null, 'flaticon-warning', '/app/main/asset-manament/app-request-asset-bring/0/m_rejected'),
                new AppMenuItem('OtherRequest', null, 'flaticon-warning', '/app/main/asset-manament/app-request-asset-bring/0/m_other_request')
            ]),
            new AppMenuItem('ApproveRequest', 'Request', 'flaticon-paper-plane', '', [], [
                new AppMenuItem('WaitRequest', null, 'flaticon-statistics', '/app/main/asset-manament/app-request-asset-bring/0/u_waittingme'),
                new AppMenuItem('Approved', null, 'flaticon-folder-1', '/app/main/asset-manament/app-request-asset-bring/0/u_approvedbyme'),
                new AppMenuItem('Rejected', null, 'flaticon-warning', '/app/main/asset-manament/app-request-asset-bring/0/u_rejectedbyme'),
                new AppMenuItem('OtherRequest', null, 'flaticon-warning', '/app/main/asset-manament/app-request-asset-bring/0/u_other_request')
            ]),
            //  new AppMenuItem('DraftRequest', 'Request', 'flaticon-warning', '/app/main/asset-manament/app-request-asset-bring/0/is_draft'),
            // // new AppMenuItem('RequestAssetBring', '', 'flaticon-paper-plane', '', [], [
            // //     // new AppMenuItem('WaitRequest', null, 'flaticon-statistics', '/app/main/asset-manament/app-request-asset-bring/CREATE_REQUEST'),
            // //     // new AppMenuItem('ManagerReject', null, 'flaticon-warning', '/app/main/asset-manament/app-request-asset-bring/MANAGER_REJECT'),
            // //     // new AppMenuItem('WaitAdm', null, 'flaticon-statistics', '/app/main/asset-manament/app-request-asset-bring/MANAGER_APPROVE'),
            // //     // new AppMenuItem('AdmApproved', null, 'flaticon-folder-1', '/app/main/asset-manament/app-request-asset-bring/ADM_APPROVE'),
            // //     // new AppMenuItem('AdmReject', null, 'flaticon-warning', '/app/main/asset-manament/app-request-asset-bring/ADM_REJECT'), 
            // // ]),
            new AppMenuItem('AssetManament', 'Security', 'flaticon-folder-3', '', [], [
                new AppMenuItem('AssetInManament', null, 'flaticon-book', '/app/main/asset-manament/app-asset-in-manament'),
                new AppMenuItem('AssetOutManament', null, 'flaticon-book', '/app/main/asset-manament/app-asset-out-manament'),
            ]),
            // new AppMenuItem('InOutManament', null, 'flaticon-folder-3', '/app/main/asset-manament/app-in-out-manament'),
             new AppMenuItem('Report', 'Report', 'flaticon-squares', '/app/main/report'),
            new AppMenuItem('Tenants', 'Pages.Tenants', 'flaticon-list-3', '/app/admin/tenants'),
            new AppMenuItem('Editions', 'Pages.Editions', 'flaticon-app', '/app/admin/editions'),
            new AppMenuItem('Administration', '', 'flaticon-interface-8', '', [], [
                new AppMenuItem('OrganizationUnits', 'Pages.Administration.OrganizationUnits', 'flaticon-map', '/app/admin/organization-units'),
                new AppMenuItem('Roles', 'Pages.Administration.Roles', 'flaticon-suitcase', '/app/admin/roles'),
                new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                new AppMenuItem('Languages', 'Pages.Administration.Languages', 'flaticon-tabs', '/app/admin/languages', ['/app/admin/languages/{name}/texts']),
                new AppMenuItem('AuditLogs', 'Pages.Administration.AuditLogs', 'flaticon-folder-1', '/app/admin/auditLogs'),
                new AppMenuItem('Maintenance', 'Pages.Administration.Host.Maintenance', 'flaticon-lock', '/app/admin/maintenance'),
                new AppMenuItem('Subscription', 'Pages.Administration.Tenant.SubscriptionManagement', 'flaticon-refresh', '/app/admin/subscription-management'),
                new AppMenuItem('VisualSettings', 'Pages.Administration.UiCustomization', 'flaticon-medical', '/app/admin/ui-customization'),
                new AppMenuItem('WebhookSubscriptions', 'Pages.Administration.WebhookSubscription', 'flaticon2-world', '/app/admin/webhook-subscriptions'),
                new AppMenuItem('DynamicProperties', 'Pages.Administration.DynamicProperties', 'flaticon-interface-8', '/app/admin/dynamic-property'),
                new AppMenuItem('Settings', 'Pages.Administration.Host.Settings', 'flaticon-settings', '/app/admin/hostSettings'),
                new AppMenuItem('Settings', 'Pages.Administration.Tenant.Settings', 'flaticon-settings', '/app/admin/tenantSettings')
            ]),
            new AppMenuItem('DemoUiComponents', 'Pages.DemoUiComponents', 'flaticon-shapes', '/app/admin/demo-ui-components')
        ]);
    }

    checkChildMenuItemPermission(menuItem): boolean {

        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName === '' || subMenuItem.permissionName === null) {
                if (subMenuItem.route) {
                    return true;
                }
            } else if (this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            }

            if (subMenuItem.items && subMenuItem.items.length) {
                let isAnyChildItemActive = this.checkChildMenuItemPermission(subMenuItem);
                if (isAnyChildItemActive) {
                    return true;
                }
            }
        }
        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' && this._appSessionService.tenant && !this._appSessionService.tenant.edition) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }

    /**
     * Returns all menu items recursively
     */
    getAllMenuItems(): AppMenuItem[] {
        let menu = this.getMenu();
        let allMenuItems: AppMenuItem[] = [];
        menu.items.forEach(menuItem => {
            allMenuItems = allMenuItems.concat(this.getAllMenuItemsRecursive(menuItem));
        });

        return allMenuItems;
    }

    private getAllMenuItemsRecursive(menuItem: AppMenuItem): AppMenuItem[] {
        if (!menuItem.items) {
            return [menuItem];
        }

        let menuItems = [menuItem];
        menuItem.items.forEach(subMenu => {
            menuItems = menuItems.concat(this.getAllMenuItemsRecursive(subMenu));
        });

        return menuItems;
    }
}
