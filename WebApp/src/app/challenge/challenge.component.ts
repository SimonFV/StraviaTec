import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../services/ApiService/api.service';
import { SharedService } from '../services/SharedService/shared.service';

@Component({
  selector: 'app-challenge',
  templateUrl: './challenge.component.html',
  styleUrls: ['./challenge.component.css']
})
export class ChallengeComponent implements OnInit {

  constructor(
    private ApiService: ApiService,
    public sharedService: SharedService,
    private router: Router) { }
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
    this.ApiService.getChallenges().subscribe({
      next: (resp) => {
        for (let challenge of resp.body!) {
          this.loadChallenges(challenge);
        }
      }, error: (error) => {
        console.log(error);
        if (error.status == 401) {
          this.router.navigate(['/']);
        }
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
  }



}
