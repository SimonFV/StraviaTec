import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../services/ApiService/api.service';
import { SharedService } from '../services/SharedService/shared.service';

@Component({
  selector: 'app-race',
  templateUrl: './race.component.html',
  styleUrls: ['./race.component.css']
})


export class RaceComponent implements OnInit {

  public form!: FormGroup;
  alert: boolean = false;
  alertMessage: string = '';

  typeAlert: string = 'success';
  name = '';
  start;
  category = '';
  type = '';
  grps = '';
  isPrivate: boolean = false;

  races = [{
    "Id": 0,
    "userAdmin": "",
    "name": "",
    "route": "",
    "privacy": "",
    "cost": 0,
    "category": "",
    "startDate": "",
    "activity_Type": ""
  }];

  visibility = [{
    "groupId": 0,
    "raceId": 0
  }];
  userGroups = [0];

  participants = [{
    "FirstName": "",
    "LastName": "",
    "Age": 0,
    "CategoryName": ""
  }]

  records = [{
    "FirstName": "",
    "LastName": "",
    "Age": 0,
    "Duration": "",
    "CategoryName": ""
  }]


  constructor(
    private service: ApiService,
    private formBuilder: FormBuilder,
    private sharedService: SharedService) { this.start = new Date(); }




  ngOnInit(): void {

    this.visibility.splice(0, 1);
    this.userGroups.splice(0, 1);
    this.races.splice(0, 1);

    this.service.getGroups(this.sharedService.getUserData().User).subscribe(resp => {
      this.loadUserGroups(resp.body)
    });

    this.service.getRaceVisibility().subscribe(resp => {
      this.loadVisivility(resp.body);
    })

    this.service.getRaces().subscribe(resp => {
      console.log(resp.body);
      for (let race of resp.body!) {
        this.loadRaces(race);
      }

    })

    this.form = this.formBuilder.group({
      UserAdmin: [this.sharedService.getToken(), [Validators.required]],
      Name: ['', [Validators.required]],
      Cost: ['', [Validators.required]],
      StartDate: [Date, [Validators.required]],
      Type: ['', [Validators.required]],
      Route: ['route.gpx', [Validators.required]],
      Privacy: ['', [Validators.required]],
      Groups: ['', []],
      GroupsArray: this.formBuilder.array([]),
      Category: ['', [Validators.required]]
    });

    console.log(this.name);
    this.participants.splice(0, 1);
    this.service.getParticipants(this.name).subscribe(resp => {
      console.log(resp.body);
      for (let participant of resp.body!) {
        this.loadParticipants(participant);
      }
    })

    this.records.splice(0, 1);
    this.service.getRecord(this.name).subscribe(resp => {
      console.log(resp.body);
      for (let record of resp.body!) {
        this.loadRecords(record);
      }
    })


  }



  delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  async printDiv(divName: any, name: any) {
    this.name = name;
    this.ngOnInit();
    await this.delay(1000);
    var printContents = document.getElementById('printableArea' + divName.toString())!.innerHTML;
    var originalContents = document.body.innerHTML;

    document.body.innerHTML = printContents;

    window.print();

    document.body.innerHTML = originalContents;
    window.location.reload();
  }

  async printRecord(divName: any, name: any) {
    this.name = name;
    this.ngOnInit();
    await this.delay(1000);
    var printContents = document.getElementById('Area' + divName.toString())!.innerHTML;
    var originalContents = document.body.innerHTML;

    document.body.innerHTML = printContents;

    window.print();

    document.body.innerHTML = originalContents;
    window.location.reload();
  }

  loadRaces(race: any) {
    let raceShown = [0];
    for (let vis of this.visibility) {

      if (race.privacy) {
        if (this.userGroups.includes(vis.groupId) && race.id == vis.raceId && !raceShown.includes(race.id)) {
          this.races.push({
            "Id": race.id,
            "userAdmin": race.userAdmin,
            "name": race.name,
            "route": race.route,
            "privacy": race.privacy,
            "cost": race.cost,
            "category": race.category,
            "startDate": race.startDate,
            "activity_Type": race.activity_Type
          })
          raceShown.push(race.id)
          break;
        }
      } else {
        this.races.push({
          "Id": race.id,
          "userAdmin": race.userAdmin,
          "name": race.name,
          "route": race.route,
          "privacy": race.privacy,
          "cost": race.cost,
          "category": race.category,
          "startDate": race.startDate,
          "activity_Type": race.activity_Type
        })
        raceShown.push(race.id)
        break;
      }
    }
  }

  loadUserGroups(grps: any) {
    for (let i of grps) {
      this.userGroups.push(i.id)
    }

  }

  loadParticipants(participant: any) {
    this.participants.push({
      "FirstName": participant.firstName,
      "LastName": participant.lastName,
      "Age": participant.age,
      "CategoryName": participant.category
    })
    console.log(this.participants);
  }

  loadRecords(record: any) {
    console.log(this.records);
    this.records.push({
      "FirstName": record.firstName,
      "LastName": record.lastName,
      "Age": record.age,
      "Duration": record.duration,
      "CategoryName": record.category
    })
    console.log(this.records);
  }

  loadVisivility(groups: any) {
    for (let i of groups) {
      this.visibility.push({
        "groupId": i.groupId,
        "raceId": i.raceId
      });
    }
  }


  getRace() {
    this.setgroups();


    this.form.get('StartDate')!.setValue(this.start);
    this.form.get('Category')!.setValue(this.category);
    this.form.get('Type')!.setValue(this.type);

    delete this.form.value.GroupsArray;

    this.service.addRace(this.form.value).subscribe(resp => {
      console.log(resp);

    })
  }
  setgroups() {

    for (let i of this.form.get('GroupsArray')?.value) {

      this.grps = this.grps + i.GroupsArray + ',';
    }
    delete this.form.value.GroupsArray;
    this.form.get('Groups')?.setValue(this.grps);
  }

  getFile(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      //this.form.get('Route')!.setValue(file);
    }
  }

  riseAlert(message: string, type: string) {
    this.alertMessage = message;
    this.typeAlert = type;
    this.alert = true;
  }

  closeAlert() {
    this.alert = false
  }

  deleteGroup(i: number) {
    this.GroupsArray.removeAt(i);
  }

  get GroupsArray() {
    return this.form.get('GroupsArray') as FormArray;
  }
  addGroup() {
    const groupsFormGroup = this.formBuilder.group({
      GroupsArray: ''
    });
    this.GroupsArray.push(groupsFormGroup);
  }
  private() {
    this.isPrivate = !this.isPrivate;

    if (this.isPrivate) {
      this.addGroup();

    } else {
      this.GroupsArray.clear();
    }
  }
}
