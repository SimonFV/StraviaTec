import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../services/ApiService/api.service';
import { SharedService } from '../services/SharedService/shared.service';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {
  friendImagesBlob = new Map<string, any>();

  isImageLoading = false;
  friend = [{
    "user": "",
    "firstName": "",
    "lastName1": "",
    "lastName2": "",
    "birthDate": Date,
    "picture": ""
  }]
  constructor(
    private service: ApiService,
    private sharedService: SharedService,
    private router: Router
  ) { }


  ngOnInit(): void {
    this.friend.splice(0, 1);

    this.service.getFriends(this.sharedService.getUserData().User).subscribe(resp => {
      this.loadFriends(resp.body);
    })
  }

  loadFriends(friends: any) {
    for (let i of friends) {
      this.friend.push({
        "user": i.user,
        "firstName": i.firstName,
        "lastName1": i.lastName1,
        "lastName2": i.lastName2,
        "birthDate": i.birthDate,
        "picture": i.picture
      });
    }
    for (let i of this.friend) {
      this.getImageFromService(i.picture, i.user);
    }
  }

  getImageFromService(imageUrl: string, user: string) {
    const path: any = { path: imageUrl };
    this.service.getUserImage(<JSON>path).subscribe({
      next: (data) => {
        this.createImageFromBlob(data, user);
      },
      error: (error) => {
        console.log(error);
        if (error.status == 401) {
          this.router.navigate(['/']);
          return;
        }
      }
    });
  }

  createImageFromBlob(image: Blob, user: string) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.friendImagesBlob.set(user, reader.result);
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }

  }

}
