import { DialogRef } from '@angular/cdk/dialog';
import { Component, ElementRef, EventEmitter, Output, TemplateRef, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { Role } from 'src/app/core/models/users/role';
import { DialogService } from 'src/app/core/services/dialogs/dialog.service';
import { RoleService } from 'src/app/core/services/roles/role.service';

@Component({
  selector: 'app-copy-from-role',
  templateUrl: './copy-from-role.component.html',
  styleUrls: ['./copy-from-role.component.scss']
})
export class CopyFromRoleComponent {
  @Output() roleLoaded: EventEmitter<Role> = new EventEmitter<Role>()
  @ViewChild('roleDropDown') roleDropDown: ElementRef
  @ViewChild('dialog') dialogTemplate: TemplateRef<any>

  dialogRef: MatDialogRef<any>
  roles: Observable<Role[]>

  constructor(
    private dialog: DialogService,
    private roleService: RoleService
  ) { }

  ngOnInit() {
  }

  showDialog() {
    this.roles = this.roleService.getDrop()
    this.dialogRef = this.dialog.open(this.dialogTemplate)
  }

  loadRole() {
    this.roleService.get(this.roleDropDown.nativeElement.value)
      .subscribe(result => {
        this.roleLoaded.emit(result)
        this.dialogRef.close()
      })
  }
}
