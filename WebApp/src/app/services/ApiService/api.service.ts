import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SharedService } from '../SharedService/shared.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient,
    private sharedService: SharedService) { }

  getChallenges() {
    let header = new HttpHeaders().set('Type-contet', 'aplication/json');
    header = header.set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.get<JSON[]>('http://localhost:5000/Challenge/challenges', { headers: header, observe: 'response' });
  }

  getInChallenge(user: string, challengeId:number){
    let header = new HttpHeaders().set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.post('http://localhost:5000/Challenge/getInChallenge/'+user+'/'+challengeId, { headers: header, responseType: 'blob' });
  }

  getChallengeVisibility(){
    let header = new HttpHeaders().set('Type-contet', 'aplication/json');
    header = header.set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.get<JSON[]>('http://localhost:5000/Challenge/ChallengeVisibility', { headers: header, observe: 'response' });
  }

  getRaces() {
    let header = new HttpHeaders().set('Type-contet', 'aplication/json');
    header = header.set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.get<JSON[]>('http://localhost:5000/Race/races', { headers: header, observe: 'response' });
  }

  getUser(user: string) {
    let header = new HttpHeaders().set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.get('http://localhost:5000/User/users/' + user, { headers: header, observe: 'response' });
  }

  getUserImage(imagePath: JSON): Observable<Blob> {
    let header = new HttpHeaders().set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.post('http://localhost:5000/User/userImage', imagePath, { headers: header, responseType: 'blob' });
  }

  registerUser(user: FormData) {
    let header = new HttpHeaders().set('Type-contet', 'multipart/form-data');
    return this.http.post('http://localhost:5000/Authentication/register', user, { headers: header, observe: 'response' });
  }

  loginUser(user: JSON) {
    let header = new HttpHeaders().set('Type-contet', 'aplication/json');
    return this.http.post('http://localhost:5000/Authentication/login', user, { headers: header, observe: 'response' });
  }

  GetFriendsFrontPage(user: string) {
    let header = new HttpHeaders().set('Type-contet', 'multipart/form-data');
    header = header.set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.get<JSON[]>('http://localhost:5000/User/friendsFrontPage/' + user, { headers: header, observe: 'response' });
  }
  getFriends(user: string) {
    let header = new HttpHeaders().set('Type-contet', 'multipart/form-data');
    header = header.set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.get<JSON[]>('http://localhost:5000/User/friends/' + user, { headers: header, observe: 'response' });
  }

  getFriendsAvailable(user: string) {
    let header = new HttpHeaders().set('Type-contet', 'multipart/form-data');
    header = header.set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.get<JSON[]>('http://localhost:5000/User/friendsAvailable/' + user, { headers: header, observe: 'response' });
  }
  addFriend(user: string, friend: string) {
    let header = new HttpHeaders().set('Type-contet', 'aplication/json');
    return this.http.post('http://localhost:5000/User/addFriend/' + user + '/' + friend, { headers: header, observe: 'response' });
  }

  delelteFriend(user: any, friend: any) {
    let header = new HttpHeaders().set('Type-contet', 'aplication/json');
    return this.http.delete('http://localhost:5000/User/friends/Delete/' + user + '/' + friend, { headers: header, observe: 'response' })
  }

  updateUser(user: FormData) {
    let header = new HttpHeaders().set('Type-contet', 'multipart/form-data');
    header = header.set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.put('http://localhost:5000/User/edit', user, { headers: header, observe: 'response' });
  }

  addActivity(activity: FormData) {
    let header = new HttpHeaders().set('Type-contet', 'multipart/form-data');
    header = header.set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.post('http://localhost:5000/User/addActivity', activity, { headers: header, observe: 'response' });
  }

  getUserActivities(user:string){
    let header = new HttpHeaders().set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.get('http://localhost:5000/User/userActivities/' + user, { headers: header, observe: 'response' });
  }

  addRace(race: string) {
    let header = new HttpHeaders().set('Type-contet', 'multipart/form-data');
    header = header.set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.post('http://localhost:5000/Race/races', race, { headers: header, observe: 'response' });
  }
  

  deleteUser(user: string) {
    let header = new HttpHeaders().set('Type-contet', 'aplication/json');
    header = header.set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.delete('http://localhost:5000/User/delete/' + user, { headers: header, observe: 'response' });
  }

  getGroups(user:string){
    let header = new HttpHeaders().set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.get('http://localhost:5000/User/groups/' + user, { headers: header, observe: 'response' });
  }

  getGroupsAvailable(user:string){
    let header = new HttpHeaders().set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.get('http://localhost:5000/User/groupsAvailable/' + user, { headers: header, observe: 'response' });
  }

  joinGroup(id:number, user:string){
    let header = new HttpHeaders().set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.post('http://localhost:5000/User/joinGroup/'+id+'/'+user, { headers: header, observe: 'response' });
  }

  quitGroup(id:number, user:string){
    let header = new HttpHeaders().set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.delete('http://localhost:5000/User/quitGroup/'+id+'/'+user, { headers: header, observe: 'response' });
  }

  createGroup(user:string, grpName:string){
    let header = new HttpHeaders().set('Authorization', 'Bearer ' + this.sharedService.getToken());
    return this.http.post('http://localhost:5000/User/createGroup/'+user+'/'+grpName, { headers: header, observe: 'response' });
  }

}
