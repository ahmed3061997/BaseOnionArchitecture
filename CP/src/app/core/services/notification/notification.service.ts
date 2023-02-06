import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ActiveToast, ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private translateService: TranslateService, private toastService: ToastrService) { }

  success(message: string = this.translateService.instant('shared.success_message')): ActiveToast<any> {
    return this.toastService.success(message, this.translateService.instant('shared.success'))
  }

  error(message: string = this.translateService.instant('shared.undefined_error')): ActiveToast<any> {
    return this.toastService.error(message, this.translateService.instant('shared.error'))
  }

  info(message: string): ActiveToast<any> {
    return this.toastService.info(message, this.translateService.instant('shared.info'))
  }

  warn(message: string): ActiveToast<any> {
    return this.toastService.warning(message, this.translateService.instant('shared.warning'))
  }
}
