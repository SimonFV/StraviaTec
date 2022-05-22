import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  getChallenges() {
    let header = new HttpHeaders().set('Type-contet', 'aplication/json');
    return this.http.get<JSON[]>('http://localhost:5000/Challenge/challenges', { headers: header, observe: 'response' });
  }

  getRaces(){
    let header = new HttpHeaders().set('Type-contet', 'aplication/json');
    return this.http.get<JSON[]>('http://localhost:5000/Race/races', { headers: header, observe: 'response' });
  }

  registerUser(user: FormData) {
    let header = new HttpHeaders().set('Type-contet', 'multipart/form-data');
    return this.http.post('http://localhost:5000/User/register', user, { headers: header, observe: 'response' });
  }

  loginUser(user:JSON){
    let header = new HttpHeaders().set('Type-contet', 'aplication/json');
    return this.http.post('http://localhost:5000/User/login', user, { headers: header, observe: 'response' });
  }

  GetFriendsFrontPage(user: string){
    let header = new HttpHeaders().set('Type-contet', 'multipart/form-data');
    return this.http.get<JSON[]>('http://localhost:5000/User/friendsFrontPage/'+user, { headers: header, observe: 'response' });
  }
  
}
