import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../services/ApiService/api.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {
  public form!: FormGroup;//Formulario utilizado para capturar los datos requeridos
  data: any = [];//Lista utilizada guardar los datos del usuario
  public token: any;//Tocken del usuario
  alert: boolean = false;
  alertMessage: string = '';
  typeAlert: string = 'success';

  constructor(private formBuilder: FormBuilder,
    private service: ApiService,
    private router: Router
  ) {

  }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      User: ['', [Validators.required], Validators.maxLength(15)],
      Password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]]
    });
  }
  //Funcion para capturar y enviar los datos introducidos en el formulario
  getData() {
    console.log(this.form.value);
    this.service.loginUser(this.form.value).subscribe(resp => {
      console.log(resp);
      /*
        GUARDAR USUARIO
      */
      this.router.navigate(['/home'])
    }, err => {
      this.riseAlert(err.error, 'danger');

    })

  }

  riseAlert(message: string, type: string) {
    this.alertMessage = message;
    this.typeAlert = type;
    this.alert = true;
  }

  closeAlert() {
    this.alert = false
  }

  //Funcion para leer la respuesta del API
  readResp(response: any) {
  }

}
//#7742BE