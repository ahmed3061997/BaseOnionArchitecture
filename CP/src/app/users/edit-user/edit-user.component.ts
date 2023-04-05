import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/core/models/user';
import { UserService } from 'src/app/core/services/users/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PermissionGridComponent } from 'src/app/shared/permission-grid/permission-grid.component';
import { Select2, Select2Data, Select2Option, Select2Value } from 'ng-select2-component';
import { Subscription, tap } from 'rxjs';
import { RoleService } from 'src/app/core/services/roles/role.service';
import { Role } from 'src/app/core/models/role';
import { UserRole } from 'src/app/core/models/user-role';
import { environment } from 'src/environments/environment';
import { AutoUnsubscribe } from 'src/app/core/decorators/auto-unsubscribe.decorator';
import { CultureService } from 'src/app/core/services/culture/culture.service';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss']
})
@AutoUnsubscribe()
export class EditUserComponent {
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
    phoneNumber: new FormControl<string>(''),
  })
  submitted: boolean = false
  profileImage: File
  onLangChange: Subscription
  isActive: boolean = true
  user: User = new User()
  selectedRoles: Select2Value
  roles: Select2Data = []

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
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
    this.onLangChange = this.cultureService.onCultureChange.subscribe(() => this.load())
  }

  load() {
    if (this.rolesElement != null) this.rolesElement.value = []
    this.roleService.getDrop().pipe(
      tap(roles => {
        this.roles = roles.map((x: Role) => ({ label: x.name, value: x.id })) as Select2Option[]
        this.userService.get(this.activatedRoute.snapshot.params['id']).subscribe(user => {
          this.user = user
          this.form.patchValue(user)
          this.rolesElement.value = user.roles!.map((x: any) => x.id)
          this.permissionGrid.setSelectedClaims(user.claims!)
        })
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

  sendResetPassword() {
    this.userService.sendResetPassword({
      username: this.user?.username!,
      resetUrl: environment.Base_Url + environment.Reset_Email_Url
    }).subscribe()
  }

  save() {
    this.submitted = true

    if (this.form.invalid) return

    var user = this.form.value as User
    user.id = this.user.id
    user.isActive = this.isActive
    user.profileImageFile = this.profileImage
    user.roles = (this.rolesElement.value as string[]).map(x => ({ id: x })) as UserRole[]
    user.claims = this.permissionGrid.getSelectedClaims()

    this.userService.edit(user)
      .subscribe(() => this.router.navigate(['/users']))
  }
}