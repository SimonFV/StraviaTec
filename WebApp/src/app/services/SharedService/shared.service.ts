import { Injectable } from '@angular/core';

@Injectable()
export class SharedService {
    globalToken;
    globalUser;

    constructor() {
        this.globalToken = '';
        this.globalUser = '';
    }

    setToken(val: string) {
        this.globalToken = val;
    }

    getToken() {
        return this.globalToken;
    }

    setUser(val: string) {
        this.globalUser = val;
    }

    getUser() {
        return this.globalUser;
    }
}