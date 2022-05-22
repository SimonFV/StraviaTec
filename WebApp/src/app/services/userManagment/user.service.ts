import { EventEmitter, Injectable, Output } from '@angular/core';


@Injectable({
    providedIn: 'root'
})
export class UserService {
    @Output() trigger: EventEmitter<any> = new EventEmitter();
    constructor() { }
}