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
    private service: ApiService,
    public sharedService: SharedService,
    private router: Router) { }
  challenges = [{
    "Id":0,
    "userAdmin": "",
    "name": "",
    "class": "",
    "privacy": "",
    "startDate": "",
    "endDate": "",
    "activity_Type": ""
  }];

  userGroups=[0];
  visibility=[{
    "groupId":0,
    "challengeId": 0
  }];

  ngOnInit(): void {
    this.challenges.splice(0, 1);
    this.userGroups.splice(0, 1);
    this.visibility.splice(0, 1);
    this.service.getGroups(this.sharedService.getUserData().User).subscribe(resp=>{
      this.loadUserGroups(resp.body)
    });

    this.service.getChallengeVisibility().subscribe(resp=>{
      
      this.loadVisivility(resp.body);
    })

    this.service.getChallenges().subscribe({
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
    });
    

  }   

  loadUserGroups(grps: any){
    for(let i of grps){
      this.userGroups.push(i.id)
    }
    console.log(this.userGroups);
  }
  loadVisivility(groups:any){
    for(let i of groups){
      this.visibility.push({
        "groupId":i.groupId,
        "challengeId":i.challengeId
      });
    }
    console.log(this.visibility);
  }

  loadChallenges(challenge: any) {
    let challengeShown=[0];
    for(let vis of this.visibility){
      if(this.userGroups.includes(vis.groupId) && challenge.id==vis.challengeId && !challengeShown.includes(challenge.id)){
        this.challenges.push({
          "Id": challenge.id,
          "userAdmin": challenge.userAdmin,
          "name": challenge.name,
          "class": challenge.class,
          "privacy": challenge.privacy,
          "startDate": challenge.startDate,
          "endDate": challenge.endDate,
          "activity_Type": challenge.activity_Type
        })
        challengeShown.push(challenge.id)
      }
    }
    

    
  }

  getInChallenge(i: any){
    
    this.service.getInChallenge(this.sharedService.getUserData().User, i.Id).subscribe(resp=>{
      console.log(resp);
    })
  }



}
