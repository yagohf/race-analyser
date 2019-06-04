import { Injectable } from '@angular/core';
import { Observable,Subject} from 'rxjs';
import { EnumMessageType } from '../_models/enums/enum.messagetype';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private subject = new Subject<any>();

  constructor() { }

  sendMessage(message: string, messageType: EnumMessageType) {
      this.subject.next({ text: message, type: messageType});
  }

  getMessages(): Observable<any> {
      return this.subject.asObservable();
  }
}
