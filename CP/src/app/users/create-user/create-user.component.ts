import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/core/models/user';
import { UserService } from 'src/app/core/services/users/user.service';
import { Router } from '@angular/router';
import { PermissionGridComponent } from 'src/app/shared/permission-grid/permission-grid.component';
import { Select2, Select2Data, Select2Option, Select2Value } from 'ng-select2-component';
import { MatchPasswordValidator } from 'src/app/core/validators/match-password.validator';
import { first, tap } from 'rxjs';
import { RoleService } from 'src/app/core/services/roles/role.service';
import { Role } from 'src/app/core/models/role';
import { AutoUnsubscribe } from 'src/app/core/decorators/auto-unsubscribe.decorator';
import { UserRole } from 'src/app/core/models/user-role';
import { CultureService } from 'src/app/core/services/culture/culture.service';

@AutoUnsubscribe()
@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.scss']
})
export class CreateUserComponent {
  @ViewChild(Select2) rolesElement: Select2
  @ViewChild(PermissionGridComponent) permissionGrid: PermissionGridComponent

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
  })
  submitted: boolean = false
  profileImage: File
  isActive: boolean = true
  selectedRoles: Select2Value
  roles: Select2Data = []
  onLangChange$: any;

  constructor(
    private router: Router,
    private userService: UserService,
    private roleService: RoleService,
    private cultureService: CultureService) { }

  get f() {
    return this.form.controls
  }

  ngOnInit() {
    this.load()
  }

  ngAfterViewInit() {
    this.onLangChange$ = this.cultureService.onCultureChange.subscribe(() => this.load())
  }

  load() {
    if (this.rolesElement != null) this.rolesElement.value = []
    this.roleService.getDrop().pipe(
      first(),
      tap(roles => {
        this.roles = roles.map((x: Role) => ({ label: x.name, value: x.id })) as Select2Option[]
      })
    ).subscribe()
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
    user.isActive = this.isActive
    user.emailConfirmed = true
    user.profileImageFile = this.profileImage
    user.roles = (this.rolesElement.value as string[]).map(x => ({ id: x })) as UserRole[]
    user.claims = this.permissionGrid.getSelectedClaims()

    this.userService.create(user)
      .subscribe(() => this.router.navigate(['/users']))
  }
}
