import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from '../services/SharedService/shared.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(private sharedService: SharedService,
    private router: Router) { }

  ngOnInit(): void {
    /*if (this.sharedService.getUser() == '') {
      this.LogOut();
    }*/
  }

  LogOut() {
    this.sharedService.setUser('');
    this.sharedService.setToken('');
    this.router.navigate(['/']);
  }

}
