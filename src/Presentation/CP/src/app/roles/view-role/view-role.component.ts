
import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import "jquery";
import { MultiLanguageInputComponent } from 'src/app/shared/multi-language-input/multi-language-input.component';
import { Role } from 'src/app/core/models/users/role';
import { RoleService } from 'src/app/core/services/roles/role.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PermissionGridComponent } from 'src/app/shared/permission-grid/permission-grid.component';
import { CultureLookup } from 'src/app/core/models/common/culture-lookup';

@Component({
  selector: 'app-view-role',
  templateUrl: './view-role.component.html',
  styleUrls: ['./view-role.component.scss']
})
export class ViewRoleComponent {

  @ViewChild(PermissionGridComponent) permissionGrid: PermissionGridComponent;

  roleId: string
  isActive: boolean
  names: CultureLookup[] = []

  constructor(
    private activatedRoute: ActivatedRoute,
    private roleService: RoleService) { }

  ngOnInit() {
    this.roleService.get(this.activatedRoute.snapshot.params['id']).subscribe(result => {
      this.roleId = result.id!
      this.names = result.names!
      this.isActive = result.isActive
      this.permissionGrid.setSelectedClaims(result.claims!)
    })
  }
}
