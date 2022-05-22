import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/ApiService/api.service';

@Component({
  selector: 'app-race',
  templateUrl: './race.component.html',
  styleUrls: ['./race.component.css']
})
export class RaceComponent implements OnInit {

  constructor(private service : ApiService) { }
  races=[{
    "userAdmin": "",
    "name": "",
    "route": "",
    "cost": 0,
    "category": "",
    "startDate": "",
    "activity_Type": ""
  }]
  ngOnInit(): void {
    this.races.splice(0, 1);
    this.service.getRaces().subscribe(resp=>{
      console.log(resp.body);
      for (let race of resp.body!) {
        this.loadRaces(race);
      }
      
    })
  }
  loadRaces(race: any){
    this.races.push({
      "userAdmin": race.userAdmin,
      "name": race.name,
      "route":race.route,
      "cost": race.cost,
      "category": race.category,
      "startDate": race.startDate,
      "activity_Type": race.type
    })
    console.log(this.races);
  }
}
