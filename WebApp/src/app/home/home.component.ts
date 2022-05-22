import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/ApiService/api.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private service: ApiService) { }

  activities = [{
    "User": "sfv",
    "FirstName": "sfv",
    "LastName1": "sfv",
    "LastName2": "sfv",
    "Type": "Running",
    "Start": "f",
    "Route": "ff",
    "Distance": 10
  }];
  ngOnInit(): void {

    this.activities.splice(0, 1);
    this.service.GetFriendsFrontPage('andres').subscribe(resp=>{
      console.log(resp.body);
      for(let i of resp.body!){
        this.loadActivity(i);
      }
    })

  }
  loadActivity(acts:any){
    console.log(acts);
    
    this.activities.push({
      "User": acts.user,
      "FirstName": acts.firstName,
      "LastName1": acts.lastName1,
      "LastName2": acts.lastName2,
      "Type": acts.type,
      "Start": acts.start,
      "Route": acts.route,
      "Distance": acts.distance
    })
  }
  showComments(i:any){
    console.log("comment");
  }
}
