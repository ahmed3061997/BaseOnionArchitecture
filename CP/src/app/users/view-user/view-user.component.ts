import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AutoUnsubscribe } from 'src/app/core/decorators/auto-unsubscribe.decorator';
import { User } from 'src/app/core/models/user';
import { UserService } from 'src/app/core/services/users/user.service';
import { PermissionGridComponent } from 'src/app/shared/permission-grid/permission-grid.component';
import { environment } from 'src/environments/environment';

@AutoUnsubscribe()
@Component({
  selector: 'app-view-user',
  templateUrl: './view-user.component.html',
  styleUrls: ['./view-user.component.scss']
})
export class ViewUserComponent {

  @ViewChild(PermissionGridComponent) permissionGrid: PermissionGridComponent;

  user: User | undefined
  roles: string = ''
  onLangChange$: any;

  constructor(
    private activatedRoute: ActivatedRoute,
    private userService: UserService,
    private translateService: TranslateService) { }

  ngOnInit() {
    this.load()
  }

  ngAfterViewInit() {
    this.onLangChange$ = this.translateService.onLangChange.subscribe(() => this.load())
  }

  load() {
    this.userService.get(this.activatedRoute.snapshot.params['id']).subscribe(result => {
      this.user = result;
      this.roles = (result.roles || []).map((x: any) => x.name).join(', ')
      this.permissionGrid.setSelectedClaims(result.claims!)
    })
  }

  sendResetPassword() {
    this.userService.sendResetPassword({
      username: this.user?.username!,
      resetUrl: environment.Base_Url + environment.Reset_Email_Url
    }).subscribe()
  }
}