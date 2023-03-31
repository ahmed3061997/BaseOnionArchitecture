import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import "jquery";
import { MultiLanguageInputComponent } from 'src/app/shared/multi-language-input/multi-language-input.component';
import { User } from 'src/app/core/models/user';
import { UserService } from 'src/app/core/services/users/user.service';
import { Router } from '@angular/router';
import { PermissionGridComponent } from 'src/app/shared/permission-grid/permission-grid.component';
import { Select2Data } from 'ng-select2-component';
import { MatchPasswordValidator } from 'src/app/core/validators/match-password.validator';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.scss']
})
export class CreateUserComponent {

  @ViewChild(PermissionGridComponent) permissionGrid: PermissionGridComponent;

  roles: Select2Data = [
    {
      value: '1',
      label: 'Admin'
    },
    {
      value: '2',
      label: 'Developer'
    },
    {
      value: '3',
      label: 'Employee'
    },
    {
      value: '4',
      label: 'Operator'
    },
    {
      value: '5',
      label: 'Director'
    },
  ]

  form = new FormGroup({
    fullName: new FormControl<string>('',
      [
        Validators.required,
        Validators.maxLength(60)
      ]
    ),
    username: new FormControl<string>('',
      [
        Validators.required,
        Validators.maxLength(60)
      ]
    ),
    email: new FormControl<string>('',
      [
        Validators.required,
        Validators.email,
        Validators.maxLength(60)
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
    phoneNumber: new FormControl<string>(''),
    isActive: new FormControl<boolean>(true),
  })
  submitted: boolean = false
  profileImage: File

  constructor(
    private router: Router,
    private userService: UserService) { }

  get f() {
    return this.form.controls
  }

  onImageSelected(event: any) {
    console.log(event)
    this.profileImage = event.target.files[0]
  }

  onUserLoaded(user: User) {
    this.permissionGrid.setSelectedClaims(user.claims!)
  }

  save() {
    this.submitted = true

    if (this.form.invalid) return

    var user = this.form.value as User
    user.profileImageFile = this.profileImage
    user.claims = this.permissionGrid.getSelectedClaims()
    console.log(user)
    // this.userService.create(user)
    //   .subscribe(() => this.router.navigate(['/users']))
  }
}
