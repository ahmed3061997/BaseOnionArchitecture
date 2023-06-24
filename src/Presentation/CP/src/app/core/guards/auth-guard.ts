import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthService } from "../services/auth/auth.service";

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    private allowedUrls: string[] = [
        '/users/login',
        '/users/register',
        '/users/forget-password',
        '/users/reset-password',
        '/users/confirm-email'
    ]

    constructor(public authService: AuthService, public router: Router) {
    }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        if (this.authService.isAuthenticated() || this.allowedUrls.indexOf(state.url) != -1) return true;
        console.log(route.url);
        this.router.navigate(['/users/login'])
        return false;
    }
}