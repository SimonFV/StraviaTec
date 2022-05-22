import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { ApiService } from '../services/ApiService/api.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  public form!: FormGroup;//Formulario utilizado para capturar los datos requeridos
  constructor(private formBuilder: FormBuilder,
    private service: ApiService,
    private router: Router,
    private sanitizer: DomSanitizer
  ) {

  }

  alert: boolean = false;
  alertMessage: string = '';
  typeAlert: string = 'success';
  public token: any;//Tocken del usuario actual
  data: any = [];//Lista utilizada para enviar los datos del usuario
  student: boolean = false;//Flag para saber si el usuario actual es un estudiante
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      FirstName: ['', [Validators.required]],
      LastName1: ['', [Validators.required]],
      LastName2: ['', [Validators.required]],
      BirthDate: ['', [Validators.required]],
      Nationality: ['', [Validators.required]],
      Picture: ['', [Validators.required]],
      User: ['', [Validators.required]],
      Password: ['', [Validators.required]],

    });

  }
  //Funcion para capturar y enviar los datos introducidos en el formulario
  getData() {
    let formData: FormData = new FormData();

    formData.append('FirstName', this.form.get('FirstName')!.value);
    formData.append('LastName1', this.form.get('LastName1')!.value);
    formData.append('LastName2', this.form.get('LastName2')!.value);
    formData.append('BirthDate', this.form.get('BirthDate')!.value);
    formData.append('Nationality', this.form.get('Nationality')!.value);
    formData.append('Picture', this.form.get('Picture')!.value);
    formData.append('User', this.form.get('User')!.value);
    formData.append('Password', this.form.get('Password')!.value);

    this.service.registerUser(formData).subscribe({
      next: (response) => this.readResp(response),
      error: (error) => {
        console.log(error);
        this.riseAlert(error.error, 'danger');
      }
    })
  }

  //Funcion utilizada para leer la respuesta del API
  readResp(response: any) {
    this.data = <JSON>response;
    console.log(this.data);
    this.riseAlert(response.body, 'success');
  }

  getFile(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.form.get('Picture')!.setValue(file);
    }
  }

  selectCountryHandler(event: any) {
    this.form.get('Nationality')!.setValue(event.target.value);
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
