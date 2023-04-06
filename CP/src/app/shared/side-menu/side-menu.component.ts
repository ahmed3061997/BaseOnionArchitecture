import { Component } from '@angular/core';
import { tap } from 'rxjs';
import { Claims } from 'src/app/core/constants/claims';
import { NotificationMessages } from 'src/app/core/constants/notification-messages';
import { AutoUnsubscribe } from 'src/app/core/decorators/auto-unsubscribe.decorator';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { SignalrService } from 'src/app/core/services/signalr/signalr.service';

@AutoUnsubscribe()
@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss']
})
export class SideMenuComponent {
  system: boolean
  manageUsers: boolean
  users: boolean
  roles: boolean
  private onNotification: any;

  constructor(
    private authService: AuthService,
    private signalr: SignalrService
  ) { }

  ngOnInit() {
    this.loadClaims()
  }

  ngAfterViewInit() {
    this.onNotification = this.signalr.onNotification.subscribe(notification => {

      if (notification.subject == NotificationMessages.roleUpdated) {
        this.authService.refreshClaims().subscribe(() => this.loadClaims())
      }
    })
  }

  loadClaims() {
    this.system = this.authService.isInRole(Claims.developer)
    this.manageUsers = this.authService.hasClaimContains(Claims.manageUsers)
    this.roles = this.authService.hasClaim(Claims.roles.view)
    this.users = this.authService.hasClaim(Claims.users.view)
  }
}
