import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoadingOverlayHelper } from 'src/app/core/helpers/loading-overlay/loading-overlay';
import { User } from 'src/app/core/models/user';
import { AuthService } from 'src/app/core/services/auth/auth.service';

@Component({
  selector: 'app-current-user',
  templateUrl: './current-user.component.html',
  styleUrls: ['./current-user.component.scss']
})
export class CurrentUserComponent {
  user: User = new User()
  roles: string

  constructor(private router: Router, private authService: AuthService) {
    this.user = authService.getUser()
    this.roles = this.user.roles?.map(x=>x.name).join(', ') ?? ''
  }

  logout() {
    LoadingOverlayHelper.showLoading()
    this.authService.logout().subscribe(() => {
      LoadingOverlayHelper.hideLoading()
      this.router.navigate(['/users/login'])
    })
  }
}
