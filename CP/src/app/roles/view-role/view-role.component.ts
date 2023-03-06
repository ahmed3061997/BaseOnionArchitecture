
import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import "jquery";
import { MultiLanguageInputComponent } from 'src/app/shared/multi-language-input/multi-language-input.component';
import { Role } from 'src/app/core/models/role';
import { RoleService } from 'src/app/core/services/roles/role.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PermissionGridComponent } from 'src/app/shared/permission-grid/permission-grid.component';

@Component({
  selector: 'app-view-role',
  templateUrl: './view-role.component.html',
  styleUrls: ['./view-role.component.scss']
})
export class ViewRoleComponent {

  @ViewChild(PermissionGridComponent) permissionGrid: PermissionGridComponent;
  @ViewChild(MultiLanguageInputComponent) nameInput: MultiLanguageInputComponent;

  role: Role = new Role()

  constructor(
    private activatedRoute: ActivatedRoute,
    private roleService: RoleService) { }

  ngOnInit() {
    this.roleService.get(this.activatedRoute.snapshot.params['id']).subscribe(result => {
      this.role = result
      this.nameInput.setValue(result.names!)
      this.permissionGrid.setSelectedClaims(result.claims!)
    })
  }
}
