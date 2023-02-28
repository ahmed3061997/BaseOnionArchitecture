import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ActiveToast, ToastrService } from 'ngx-toastr';
import { CultureService } from '../culture/culture.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(
    private cultureService: CultureService,
    private translateService: TranslateService,
    private toastService: ToastrService) { }

  success(message: string = this.translateService.instant('shared.success_message')): ActiveToast<any> {
    return this.toastService.success(message,
      this.translateService.instant('shared.success'),
      {
        positionClass: this.cultureService.isRtl() ? 'toast-top-left' : 'toast-top-right'
      })
  }

  error(message: string = this.translateService.instant('shared.undefined_error')): ActiveToast<any> {
    return this.toastService.error(message,
      this.translateService.instant('shared.error'),
      {
        positionClass: this.cultureService.isRtl() ? 'toast-top-left' : 'toast-top-right'
      })
  }

  info(message: string): ActiveToast<any> {
    return this.toastService.info(message,
      this.translateService.instant('shared.info'),
      {
        positionClass: this.cultureService.isRtl() ? 'toast-top-left' : 'toast-top-right'
      })
  }

  warn(message: string): ActiveToast<any> {
    return this.toastService.warning(message,
      this.translateService.instant('shared.warning'),
      {
        positionClass: this.cultureService.isRtl() ? 'toast-top-left' : 'toast-top-right'
      })
  }
}
