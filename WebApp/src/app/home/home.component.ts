import { Component, OnInit } from '@angular/core';
import { ApiMongoService } from '../services/ApiMongo/api-mongo.service';
import { ApiService } from '../services/ApiService/api.service';
import { SharedService } from '../services/SharedService/shared.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(
    private service: ApiService,
    private sharedService: SharedService,
    private mongoService: ApiMongoService
  ) { }

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
  comments = [{
    "id": 0,
    "user": "",
    "activityId": "",
    "postTime": "",
    "body": ""
  }]
  ngOnInit(): void {

    this.activities.splice(0, 1);
    this.service.GetFriendsFrontPage('andres').subscribe(resp => {
      console.log(resp.body);
      for (let i of resp.body!) {
        this.loadActivity(i);
      }
      console.log("token: " + this.sharedService.getToken());
    })

  }
  loadActivity(acts: any) {
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
  showComments(i: any) {
    console.log("comment");
    this.mongoService.getCommentsByActivity(0).subscribe(resp => {
      console.log(resp);

    })
  }
}
