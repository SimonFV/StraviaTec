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
  
  public token: any;//Tocken del usuario actual
  data:any=[];//Lista utilizada para enviar los datos del usuario
  student:boolean=false;//Flag para saber si el usuario actual es un estudiante
  ngOnInit(): void {
    this.form= this.formBuilder.group({
      FirstName: ['',[Validators.required]],
      LastName1: ['',[Validators.required]],
      LastName2: ['',[Validators.required]],
      BirthDay: ['',[Validators.required]],
      Nationality: ['',[Validators.required]],
      Photo: ['',[Validators.required]],
      User: ['',[Validators.required, Validators.email]],
      Password: ['',[Validators.required, Validators.minLength(6)]],
      
    });
    
  }
  //Funcion para capturar y enviar los datos introducidos en el formulario
  getData(){
    console.log(this.form.value);
  }
  
//Funcion que introduce una alerta dentro de la vista
  alert(message:string, type: string){
    const alertPlaceholder = document.getElementById('alertDiv')!
    var wrapper = document.createElement('div')
    wrapper.innerHTML = '<div class="alert alert-' + type + ' alert-dismissible" role="alert">' + message +
      '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>'
    alertPlaceholder.appendChild(wrapper)
  }

  //Funcion utilizada para leer la respuesta del API
  readResp(response:any){
    
  }

  getFile(event:any){
    const userPhoto=event.target.files[0];
  }
    
  
}
