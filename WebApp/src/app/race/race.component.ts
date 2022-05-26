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
  start;
  category='';
  type='';
  grps='';
  isPrivate:boolean=false;

  races=[{
    "userAdmin": "",
    "name": "",
    "route": "",
    "cost": 0,
    "category": "",
    "startDate": "",
    "activity_Type": ""
  }]


  constructor(
    private service : ApiService,
    private formBuilder: FormBuilder,
    private sharedService: SharedService) 
    {this.start=new Date(); }

  

  ngOnInit(): void {
    
    this.form = this.formBuilder.group({
      UserAdmin: ['SRC'/*this.sharedService.getToken()*/, [Validators.required]],
      Name: ['', [Validators.required]],
      Cost: ['', [Validators.required]],
      StartDate: [Date, [Validators.required]],
      Type: ['', [Validators.required]],
      Route:['route.gpx', [Validators.required]],
      Privacy: ['', [Validators.required]],
      Groups: ['', []],
      GroupsArray: this.formBuilder.array([]),
      Category: ['', [Validators.required]]
    });

    
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

  
  getRace(){
    this.setgroups();

    
    this.form.get('StartDate')!.setValue(this.start);
    this.form.get('Category')!.setValue(this.category);
    this.form.get('Type')!.setValue(this.type);

    delete this.form.value.GroupsArray;
    console.log(this.form.value);

    this.service.addRace(this.form.value).subscribe(resp=>{
      console.log(resp);
      
    })
  }
  setgroups(){
    
    for(let i of this.form.get('GroupsArray')?.value){
      
      this.grps=this.grps+i.GroupsArray+',';
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

  deleteGroup(i: number){
    this.GroupsArray.removeAt(i);
  }

  get GroupsArray(){
    return this.form.get('GroupsArray') as FormArray; 
  }
  addGroup(){
    const groupsFormGroup= this.formBuilder.group({
      GroupsArray: ''
    });
    this.GroupsArray.push(groupsFormGroup);
  }
  private(){
    this.isPrivate=!this.isPrivate;
    console.log(this.isPrivate);
    
    if(this.isPrivate){
      this.addGroup();
      
    }else{
      this.GroupsArray.clear();
    }
  }
}
