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
  private: boolean= false;
  alertMessage: string = '';
  typeAlert: string = 'success';
  public token: any;//Tocken del usuario actual
  data: any = [];//Lista utilizada para enviar los datos del usuario
  constructor(
    private formBuilder: FormBuilder,
    private service: ApiService,
    private sharedService: SharedService
    ) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      UserId: ['SRC'/*this.sharedService.getToken()*/, [Validators.required]],
      Distance: ['', [Validators.required]],
      Start: ['', [Validators.required]],
      Duration: ['', [Validators.required]],
      Type: ['', [Validators.required]],
      Route:['route.gpx', [Validators.required]]
    });
   
  }
  getActivity(){
    //this.form.get('Duration')!.setValue(this.form.get('Duration')!.value+'.000Z');
    console.log(this.form.value);
    console.log(this.form.get('Duration')!.value);
    
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
