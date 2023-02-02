import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthService } from "../core/services/auth/auth.service";

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(public authService: AuthService, public router: Router) {
    }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        if (this.authService.isAuthenticated() || state.url.indexOf('login') != -1) return true;
        console.log(route.url);
        this.router.navigate(['/users/login'])
        return false;
    }
}