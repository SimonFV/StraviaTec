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
    this.form = this.formBuilder.group({
      UserId: ['SRC'/*this.sharedService.getToken()*/, [Validators.required]],
      Distance: ['', [Validators.required]],
      Start: [Date, [Validators.required]],
      Duration: [Date, [Validators.required]],
      Type: ['', [Validators.required]],
      Route:['route.gpx', [Validators.required]]
    });
    

   
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
