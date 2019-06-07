import { Component, OnInit } from '@angular/core';
import { MessageService } from '../_services/message.service';
import { Subscription } from 'rxjs';
import { EnumMessageType } from '../_models/enums/enum.messagetype';
import { Guid } from '../_models/infrastructure/guid';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  subscription: Subscription;
  private messageTypes = EnumMessageType; //Associar membro ao ENUM para poder bindar no template.
  messages: any[] = [];

  constructor(private mensagensService: MessageService) { }

  ngOnInit() {
    this.subscription = this.mensagensService.getMessages().subscribe(msg => {
      while (msg.text.indexOf(';') > -1) {
        msg.text = msg.text.replace(';', '<br />');
      }
      if (!this.messages.find(x => x.text == msg.text)) {
        let newMessage = { id: Guid.newGuid(), text: msg.text, type: msg.type };
        this.messages.push(newMessage);
        var this$ = this;
        setTimeout(function () {
          this$.removeMessage(newMessage.id);
        }, 5000);
      }
    });
  }

  removeMessage(id: string) {
    var messageToRemove = this.messages.find(x => x.id === id);
    if (messageToRemove) {
      this.messages.splice(this.messages.indexOf(messageToRemove), 1);
    }
  }
}
