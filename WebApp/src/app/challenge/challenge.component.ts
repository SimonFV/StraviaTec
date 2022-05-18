import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/ApiService/api.service';

@Component({
  selector: 'app-challenge',
  templateUrl: './challenge.component.html',
  styleUrls: ['./challenge.component.css']
})
export class ChallengeComponent implements OnInit {

  constructor(private ApiService: ApiService) { }
  challenges = [{
    "userAdmin": "",
    "name": "",
    "class": "",
    "privacy": "",
    "startDate": "",
    "endDate": "",
    "activity_Type": ""
  }]
  ngOnInit(): void {
    this.challenges.splice(0, 1);
    this.ApiService.getChallenges().subscribe(resp => {
      console.log(resp.body);
      for (let challenge of resp.body!) {
        this.loadChallenges(challenge);
      }
    })
  }

  loadChallenges(challenge: any) {
    this.challenges.push({
      "userAdmin": challenge.userAdmin,
      "name": challenge.name,
      "class": challenge.class,
      "privacy": challenge.privacy,
      "startDate": challenge.startDate,
      "endDate": challenge.endDate,
      "activity_Type": challenge.activity_Type
    })
    console.log(this.challenges);
    
  }



}
