import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/ApiService/api.service';
import { SharedService } from '../services/SharedService/shared.service';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {
  userImage: any;
  isImageLoading = false;
  friend=[{
    "user": "",
    "firstName":"",
    "lastName1":"",
    "lastName2":"",
    "birthDate":Date,
    "picture":""
  }]
  constructor(
    private service :ApiService,
    private sharedService: SharedService
  ) { }


  ngOnInit(): void {
    this.friend.splice(0,1);

    this.service.getFriends('SRC'/*this.sharedService.getUserData().User*/).subscribe(resp=>{
      console.log(resp);
      this.loadFriends(resp.body);
      })
      this.isImageLoading = false;
  }
  loadFriends(friends:any){
    for(let i of friends){
      this.getImageFromService(i.picture);

      this.friend.push({
        "user":i.user,
        "firstName":i.firstName,
        "lastName1":i.lastName1,
        "lastName2":i.lastName2,
        "birthDate":i.birthDate,
        "picture":this.userImage
      });
    }
    

  }

  getImageFromService(imageUrl: string) {
    this.isImageLoading = true;
    const path: any = { path: imageUrl };
    
    this.service.getUserImage(<JSON>path).subscribe({
      next: (data) => {
        this.isImageLoading = false;
        this.createImageFromBlob(data);
        
      },
      error: (error) => {
        this.isImageLoading = true;
        //console.log(error);
        if (error.status == 401) {
          
          return;
        }
      }
    });
  }

  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.userImage = reader.result;
      console.log(this.userImage);

    }, false);
    
    if (image) {
      reader.readAsDataURL(image);
    }
    console.log(this.isImageLoading);
    
  }

}
