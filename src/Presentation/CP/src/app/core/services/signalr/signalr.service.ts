import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { environment } from 'src/environments/environment.development';
import { AuthService } from '../auth/auth.service';
import { PushNotification } from '../../models/common/push-notification';
import { Subject, first } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private hubConnection: HubConnection;
  private connected$: Subject<boolean> = new Subject<boolean>();
  private pending: PushNotification[] = [];
  public onNotification: EventEmitter<PushNotification> = new EventEmitter<PushNotification>();

  constructor(private authService: AuthService) { }

  public connect() {
    return new Promise((resolve, reject) => {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(environment.API_Base_Url + "/api/hubs/notifications",
          {
            withCredentials: false,
            accessTokenFactory: () => this.authService.getToken()!
          })
        .configureLogging(LogLevel.None)
        .withAutomaticReconnect()
        .build();

      this.connected$
        .pipe(first())
        .subscribe(() => this.onConnected());

      this.hubConnection.start()
        .then(() => {
          console.log("connection established");
          this.connected$.next(true);
          return resolve(true);
        })
        .catch((err: any) => {
          // console.log("error occured" + err);
          // reject(err);
        });
    });
  }

  public disconnect() {
    return this.hubConnection.stop();
  }

  public push(notification: PushNotification) {
    return new Promise((resolve, reject) => {
      if (this.hubConnection.state != HubConnectionState.Connected) {

        this.pending.push(notification);
        resolve(true);
      }
      else {

        this.hubConnection.invoke('push', notification)
          .then(() => resolve(true))
          .catch((err: any) => reject(err));
      }
    })
  }

  private async onConnected() {

    this.hubConnection.on('notify', (data: PushNotification) => {
      console.log(data)
      this.onNotification.emit(data);
    });

    if (this.pending.length > 0) {
      for (const x of this.pending) {
        await this.push(x)
      }
      this.pending = [];
    }
  }
}
