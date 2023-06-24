import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingOverlayHelper } from 'src/app/core/helpers/loading-overlay/loading-overlay';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { SignalrService } from 'src/app/core/services/signalr/signalr.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  submitted = false;
  form = new FormGroup({
    username: new FormControl('',
      [
        Validators.required
      ]
    ),
    password: new FormControl('',
      [
        Validators.required
      ]
    ),
  })

  constructor(
    private router: Router,
    private authService: AuthService,
    private signalr: SignalrService
  ) { }

  get f() {
    return this.form.controls
  }

  onSubmit() {
    this.submitted = true
    if (!this.form.valid) return

    LoadingOverlayHelper.showLoading()
    this.authService.login(this.form.value.username, this.form.value.password).subscribe(() => {
      LoadingOverlayHelper.hideLoading()
      this.router.navigate(['/'])
      this.signalr.connect()
    })
  }
}
