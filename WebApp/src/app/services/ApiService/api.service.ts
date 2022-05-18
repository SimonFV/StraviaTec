import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  getChallenges(){
    let header = new HttpHeaders().set('Type-contet', 'aplication/json');
    return this.http.get<JSON[]>('http://localhost:5000/Challenge/challenges',{headers: header, observe:'response'});
  }

}
