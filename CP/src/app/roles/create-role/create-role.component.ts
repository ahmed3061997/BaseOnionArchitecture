import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import "jquery";
import { MultiLanguageInputComponent } from 'src/app/shared/multi-language-input/multi-language-input.component';
import { Role } from 'src/app/core/models/role';
import { RoleService } from 'src/app/core/services/roles/role.service';
import { Router } from '@angular/router';
import { PermissionGridComponent } from 'src/app/shared/permission-grid/permission-grid.component';

@Component({
  selector: 'app-create-role',
  templateUrl: './create-role.component.html',
  styleUrls: ['./create-role.component.scss']
})
export class CreateRoleComponent {

  @ViewChild(PermissionGridComponent) permissionGrid: PermissionGridComponent;
  @ViewChild(MultiLanguageInputComponent) nameInput: MultiLanguageInputComponent;

  submitted: boolean = false
  form = new FormGroup({
    isActive: new FormControl<boolean>(true)
  })

  constructor(
    private router: Router,
    private roleService: RoleService) { }

  save() {
    this.submitted = true

    if (this.form.invalid) return

    var role: Role = {
      isActive: this.form.value.isActive!,
      names: this.nameInput.getValue(),
      claims: this.permissionGrid.getSelectedClaims()
    }
    
    this.roleService.create(role)
      .subscribe(() => this.router.navigate(['/roles']))
  }
}
