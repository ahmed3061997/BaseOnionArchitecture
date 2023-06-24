import { Component } from '@angular/core';
import { SignalrService } from './core/services/signalr/signalr.service';
import { AutoUnsubscribe } from './core/decorators/auto-unsubscribe.decorator';

@AutoUnsubscribe()
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  onNotification: any;

  constructor(private signalr: SignalrService) { }

  async ngAfterViewInit() {
    await this.signalr.connect()
  }

  async ngOnDestroy() {
    await this.signalr.disconnect()
  }
}