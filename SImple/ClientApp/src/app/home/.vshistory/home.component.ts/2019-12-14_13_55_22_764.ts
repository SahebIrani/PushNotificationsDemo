import { Component, OnInit } from '@angular/core';

import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {


  public _hubConnection: HubConnection;


  ngOnInit() {

    this._hubConnection = new HubConnectionBuilder().withUrl('/drinking').build();

    this._hubConnection.on('Group', (data: any) => {
      console.log(data);
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

}
