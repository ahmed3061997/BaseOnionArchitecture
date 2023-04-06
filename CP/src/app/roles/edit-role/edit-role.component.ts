import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import "jquery";
import { MultiLanguageInputComponent } from 'src/app/shared/multi-language-input/multi-language-input.component';
import { Role } from 'src/app/core/models/users/role';
import { RoleService } from 'src/app/core/services/roles/role.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PermissionGridComponent } from 'src/app/shared/permission-grid/permission-grid.component';

@Component({
  selector: 'app-edit-role',
  templateUrl: './edit-role.component.html',
  styleUrls: ['./edit-role.component.scss']
})
export class EditRoleComponent {

  @ViewChild(PermissionGridComponent) permissionGrid: PermissionGridComponent;
  @ViewChild(MultiLanguageInputComponent) nameInput: MultiLanguageInputComponent;

  role: Role = new Role()
  submitted: boolean = false
  form = new FormGroup({
    isActive: new FormControl<boolean>(true)
  })

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private roleService: RoleService) { }

  ngOnInit() {
    this.roleService.get(this.activatedRoute.snapshot.params['id']).subscribe(result => {
      this.role = result
      this.nameInput.setValue(result.names!)
      this.permissionGrid.setSelectedClaims(result.claims!)
    })
  }

  onRoleLoaded(role: Role) {
    this.permissionGrid.setSelectedClaims(role.claims!)
  }

  save() {
    this.submitted = true

    if (this.form.invalid) return

    this.role.isActive = this.form.value.isActive!
    this.role.names = this.nameInput.getValue()
    this.role.claims = this.permissionGrid.getSelectedClaims()

    this.roleService.edit(this.role)
      .subscribe(() => this.router.navigate(['/roles']))
  }
}
