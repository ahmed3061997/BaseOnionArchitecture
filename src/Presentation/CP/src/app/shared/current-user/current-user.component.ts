import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AutoUnsubscribe } from 'src/app/core/decorators/auto-unsubscribe.decorator';
import { LoadingOverlayHelper } from 'src/app/core/helpers/loading-overlay/loading-overlay';
import { User } from 'src/app/core/models/users/user';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { CultureService } from 'src/app/core/services/culture/culture.service';
import { SignalrService } from 'src/app/core/services/signalr/signalr.service';

@AutoUnsubscribe()
@Component({
  selector: 'app-current-user',
  templateUrl: './current-user.component.html',
  styleUrls: ['./current-user.component.scss']
})
export class CurrentUserComponent {
  user: User = new User()
  roles: string
  onCultureChange: any;

  constructor(
    private router: Router,
    private authService: AuthService,
    private signalr: SignalrService,
    private cultureService: CultureService
  ) { }

  ngOnInit() {
    this.loadUser()
  }

  ngAfterViewInit() {
    this.onCultureChange = this.cultureService.onCultureChange
      .subscribe(() => this.authService.refreshRoles().subscribe(() => this.loadUser()))
  }

  loadUser() {
    this.user = this.authService.getUser()
    this.roles = this.user.roles?.map(x => x.name).join(', ') ?? ''
  }

  logout() {
    this.authService.logout().subscribe(async () => {
      this.router.navigate(['/users/login'])
      await this.signalr.disconnect();
    })
  }
}
