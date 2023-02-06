import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingOverlayHelper } from 'src/app/core/helpers/loading-overlay/loading-overlay';
import { UserService } from 'src/app/core/services/users/user.service';
import { MatchPasswordValidator } from 'src/app/core/validators/match-password.validator';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {

  submitted = false;
  form = new FormGroup({
    firstName: new FormControl('',
      [
        Validators.required,
        Validators.maxLength(60)
      ]
    ),
    lastName: new FormControl('',
      [
        Validators.required,
        Validators.maxLength(60)
      ]
    ),
    username: new FormControl('',
      [
        Validators.required, Validators.maxLength(60)
      ]
    ),
    email: new FormControl('',
      [
        Validators.required,
        Validators.email,
        Validators.maxLength(80)
      ]
    ),
    password: new FormControl('',
      [
        Validators.required,
        Validators.minLength(6)
      ]
    ),
    confirmPassword: new FormControl('',
      [
        Validators.required,
        MatchPasswordValidator('password')
      ]
    ),
  })

  constructor(private router: Router, private userService: UserService) { }

  get f() {
    return this.form.controls
  }

  onSubmit() {
    this.submitted = true
    if (!this.form.valid) return

    var user = this.form.value as any
    user.confirmEmailUrl = environment.Base_Url + environment.Confirm_Email_Url

    LoadingOverlayHelper.showLoading()
    this.userService.register(user).subscribe(() => {
      LoadingOverlayHelper.hideLoading()
      this.router.navigate(['/'])
    })
  }
}
