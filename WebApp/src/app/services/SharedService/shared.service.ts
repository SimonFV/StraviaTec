import { Injectable } from '@angular/core';

@Injectable()
export class SharedService {
    globalToken;
    globalUserData;

    constructor() {
        this.globalToken = '';
        this.globalUserData = {
            "User": 'src',
            "FirstName": '',
            "LastName1": '',
            "LastName2": '',
            "BirthDate": '',
            "Picture": '',
            "Nationality": ''
        }
    }

    setToken(val: string) {
        this.globalToken = val;
    }

    getToken() {
        return this.globalToken;
    }

    getUserData() {
        return this.globalUserData;
    }
}
