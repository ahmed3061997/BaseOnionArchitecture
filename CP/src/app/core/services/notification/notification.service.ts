import { Injectable } from '@angular/core';
import { ActiveToast, ToastrService } from 'ngx-toastr';
import { CultureService } from '../culture/culture.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(
    private cultureService: CultureService,
    private toastService: ToastrService) { }

  success(message: string = this.cultureService.translate('shared.success_message')): ActiveToast<any> {
    return this.toastService.success(message,
      this.cultureService.translate('shared.success'),
      {
        positionClass: this.cultureService.isRtl() ? 'toast-top-left' : 'toast-top-right'
      })
  }

  error(message: string = this.cultureService.translate('shared.undefined_error')): ActiveToast<any> {
    return this.toastService.error(message,
      this.cultureService.translate('shared.error'),
      {
        positionClass: this.cultureService.isRtl() ? 'toast-top-left' : 'toast-top-right'
      })
  }

  info(message: string): ActiveToast<any> {
    return this.toastService.info(message,
      this.cultureService.translate('shared.info'),
      {
        positionClass: this.cultureService.isRtl() ? 'toast-top-left' : 'toast-top-right'
      })
  }

  warn(message: string): ActiveToast<any> {
    return this.toastService.warning(message,
      this.cultureService.translate('shared.warning'),
      {
        positionClass: this.cultureService.isRtl() ? 'toast-top-left' : 'toast-top-right'
      })
  }
}
