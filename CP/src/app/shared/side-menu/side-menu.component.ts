import { Component } from '@angular/core';
import { Claims } from 'src/app/core/constants/claims';
import { AuthService } from 'src/app/core/services/auth/auth.service';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss']
})
export class SideMenuComponent {
  claims: Claims = new Claims();
  private userClaims: string[] = []

  constructor(private authService: AuthService) {
    this.userClaims = authService.getUser().claims || []
    console.log(this.userClaims)
  }

  isInRole(role: string): boolean {
    return this.authService.isInRole(role);
  }

  hasClaim(claim: string): boolean {
    return this.userClaims.filter(x => x == claim).length != 0
  }

  hasClaimContains(claim: string): boolean {
    return this.userClaims.filter(x => x.indexOf(claim) != -1).length != 0
  }
}
