import { animate, keyframes, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { Toast, ToastrService, ToastPackage } from 'ngx-toastr';

@Component({
  selector: 'app-custom-toastr',
  templateUrl: './custom-toastr.component.html',
  styleUrls: ['./custom-toastr.component.scss'],
  // animations: [
  //   trigger('flyInOut', [
  //     state('inactive', style({
  //       opacity: 0,
  //     })),
  //     transition('inactive => active', animate('400ms ease-out', keyframes([
  //       style({
  //         transform: 'translate3d(100%, 0, 0) skewX(-30deg)',
  //         opacity: 0,
  //       }),
  //       style({
  //         transform: 'skewX(20deg)',
  //         opacity: 1,
  //       }),
  //       style({
  //         transform: 'skewX(-5deg)',
  //         opacity: 1,
  //       }),
  //       style({
  //         transform: 'none',
  //         opacity: 1,
  //       }),
  //     ]))),
  //     transition('active => removed', animate('400ms ease-out', keyframes([
  //       style({
  //         opacity: 1,
  //       }),
  //       style({
  //         transform: 'translate3d(100%, 0, 0) skewX(30deg)',
  //         opacity: 0,
  //       }),
  //     ]))),
  //   ]),
  // ],
  preserveWhitespaces: false,
})
export class CustomToastrComponent extends Toast {
  toastTypeClass: string;
  toastIconClass: string;

  constructor(
    protected override toastrService: ToastrService,
    public override toastPackage: ToastPackage,
  ) {
    super(toastrService, toastPackage);

    switch (toastPackage.toastType) {
      case 'toast-success':
        this.toastTypeClass = 'bg-success'
        this.toastIconClass = 'far fa-check-circle'
        break;

      case 'toast-error':
        this.toastTypeClass = 'bg-danger'
        this.toastIconClass = 'far fa-times-circle'
        break;

      case 'toast-warning':
        this.toastTypeClass = 'bg-warning'
        this.toastIconClass = 'bx bx-error'
        break;

      case 'toast-info':
        this.toastTypeClass = 'bg-primary'
        this.toastIconClass = 'bx bx-info-circle'
        break;
    }
  }

  ngOnInit() {
    console.log(this)
  }
}
