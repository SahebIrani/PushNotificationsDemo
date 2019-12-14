import { Component, OnInit } from '@angular/core';
import { md5 } from './md5';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  selector: 'drinking',
  templateUrl: './drinking.component.html',
  styleUrls: ['./drinking.component.css']
})

export class HomeComponent implements OnInit {

  public group: any;
  public _hubConnection: HubConnection;
  public groupKey: string = '';
  public email: string = '';

  ngOnInit() {

    this._hubConnection = new HubConnectionBuilder().withUrl('/drinking').build();

    this._hubConnection.on('Group', (data: any) => {
      console.log(data);
      this.group = data;
    });

    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started')
      })
      .catch(err => {
        console.log('Error while establishing connection')
      });

    console.log(this._hubConnection);

  }

  createOrJoin() {
    this._hubConnection.invoke('CreateOrJoin', this.groupKey, this.email);
  }

  drink() {
    this._hubConnection.invoke('Drink');
  }

  start() {
    this._hubConnection.invoke('Start');
  }

  clear() {
    console.log('test');
    console.log(this.group);
    this.group = null;
    console.log(this.group);
  }

  getGravatarByEmail(email: string): string {
    if (!email) email = "___";
    return 'http://www.gravatar.com/avatar/' + md5(email) + '?s=210&d=mm';
  }

}
