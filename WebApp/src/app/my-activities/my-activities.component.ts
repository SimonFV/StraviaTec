import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../services/ApiService/api.service';
import { SharedService } from '../services/SharedService/shared.service';

@Component({
  selector: 'app-my-activities',
  templateUrl: './my-activities.component.html',
  styleUrls: ['./my-activities.component.css']
})
export class MyActivitiesComponent implements OnInit {
  public form!: FormGroup;
  alert: boolean = false;
  alertMessage: string = '';
  activity=[{
    "UserId":"",
    "Distance": 0,
    "Start":"",
    "Duration":"",
    "Type":"",
    "Route":"",
    "Altitude":0
  }]
  activityType='';
  typeAlert: string = 'success';
  data: any = [];//Lista utilizada para enviar los datos del usuario
  start;
  duration;
  constructor(
    private formBuilder: FormBuilder,
    private service: ApiService,
    private sharedService: SharedService
    ) { 
      this.start=new Date();
      this.duration=new Date();
    }

  ngOnInit(): void {
    this.activity.splice(0,1);
    this.form = this.formBuilder.group({
      UserId: [this.sharedService.getToken(), [Validators.required]],
      Distance: ['', [Validators.required]],
      Start: [Date, [Validators.required]],
      Duration: [Date, [Validators.required]],
      Type: ['', [Validators.required]],
      Route:['route.gpx', [Validators.required]]
    });
    
    this.service.getUserActivities(this.sharedService.getUserData().User).subscribe(resp=>{
      console.log(resp);
      this.loadActivities(resp.body);
      
    })
    

   
  }
  loadActivities(activities:any){
    for(let i of activities){
      this.activity.push({
        "UserId":i.userId,
        "Distance":i.distance,
        "Duration": i.duration.days+':'+i.duration.hours+':'+i.duration.minutes,
        "Route":i.route,
        "Altitude":i.altitude,
        "Start":i.start,
        "Type":i.type,
      })
    }
    console.log(this.activity);
    
  }



  getActivity(){
    
    this.form.get('Type')!.setValue(this.activityType);
    this.form.get('Start')!.setValue(this.start);
    this.form.get('Duration')!.setValue(this.duration);
    console.log(this.form.value);
    
    this.service.addActivity(this.form.value).subscribe(resp=>{
      console.log(resp);
      
    })
    
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

}
