import { Injectable } from '@angular/core';

@Injectable()
export class SharedService {
    globalToken;

    constructor() {
        this.globalToken = '';
    }

    setToken(val: string) {
        this.globalToken = val;
    }

    getToken() {
        return this.globalToken;
    }
}