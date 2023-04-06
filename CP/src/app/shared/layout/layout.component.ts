import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NotificationMessages } from 'src/app/core/constants/notification-messages';
import { AutoUnsubscribe } from 'src/app/core/decorators/auto-unsubscribe.decorator';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { CultureService } from 'src/app/core/services/culture/culture.service';
import { NotificationService } from 'src/app/core/services/notification/notification.service';
import { SignalrService } from 'src/app/core/services/signalr/signalr.service';

@AutoUnsubscribe()
@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  onNotification: any;

  constructor(
    private router: Router,
    private authService: AuthService,
    private notificationService: NotificationService,
    private cultureService: CultureService,
    private signalr: SignalrService
  ) { }

  ngAfterViewInit() {
    this.onNotification = this.signalr.onNotification.subscribe(notification => {
      if (notification.subject == NotificationMessages.userBlocked) {

        this.notificationService.warn(this.cultureService.translate('users.account_locked'))
        this.authService.logout().subscribe(async () => {
          this.router.navigate(['/users/login'])
          await this.signalr.disconnect();
        })
      }
    })
  }

  ngOnInit(): void {
    init()
  }
}
