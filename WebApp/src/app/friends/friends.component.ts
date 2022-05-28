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
  userToShow=[{
    "user": "",
  "firstName":"",
  "lastName1":"",
  "lastName2":"",
  "birthDate":Date,
  "picture":""}];
  friend=[{
    "user": "",
    "firstName":"",
    "lastName1":"",
    "lastName2":"",
    "birthDate":Date,
    "picture":""
  }]
  userFound=[{
    "user": "",
    "firstName":"",
    "lastName1":"",
    "lastName2":"",
    "birthDate":Date,
    "picture":""
  }];
  userToFind="";

  constructor(
    private service :ApiService,
    private sharedService: SharedService
  ) { }


  ngOnInit(): void {
    this.friend.splice(0,1);
    this.userFound.splice(0,1);
    this.userToShow.splice(0,1);
    
    this.service.getFriends('SRC'/*this.sharedService.getUserData().User*/).subscribe(resp=>{
      console.log(resp);
      this.loadFriends(resp.body);
      });

      this.service.getFriendsAvailable('SRC'/*this.sharedService.getUserData().User*/).subscribe(resp=>{
        this.loadUserFound(resp.body);

      });

    this.isImageLoading = false;
  }
  loadFriends(friends:any){
    
    for(let i of friends){
      
      this.friend.push({
        "user":i.user,
        "firstName":i.firstName,
        "lastName1":i.lastName1,
        "lastName2":i.lastName2,
        "birthDate":i.birthDate,
        "picture":""//this.getImageFromService(i.picture)
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
    }, false);
    
    if (image) {
      reader.readAsDataURL(image);
    }
  }


  searchUser(){
    this.userToShow.splice(0,1);
    
    for(let i of this.userFound){
      
      if(this.userToFind.toLowerCase()==i.user.substr(0,this.userToFind.length).toLowerCase()&&this.userToFind.length!=0 ){
        
        this.userToShow.push({
          "user":i.user,
          "firstName":i.firstName,
          "lastName1":i.lastName1,
          "lastName2":i.lastName2,
          "birthDate":i.birthDate,
          "picture":""//this.getImageFromService(i.picture)
        });
      }
      
    }
  }

  loadUserFound(user:any){
    
    for(let i of user){
      this.userFound.push({
        "user":i.user,
        "firstName":i.firstName,
        "lastName1":i.lastName1,
        "lastName2":i.lastName2,
        "birthDate":i.birthDate,
        "picture":""//this.getImageFromService(i.picture)
      });
    }
  
  }
  follow(i:any){
    
    this.service.addFriend('SRC'/*this.sharedService.getUserData().User*/, i.user).subscribe(resp=>{
      console.log(resp);
      this.friend.push({
        "user":i.user,
        "firstName":i.firstName,
        "lastName1":i.lastName1,
        "lastName2":i.lastName2,
        "birthDate":i.birthDate,
        "picture":""//this.getImageFromService(i.picture)
      });
      
    })

  }
  deleteFriend(i:any){
    this.service.delelteFriend('SRC'/*this.sharedService.getUserData().User*/,i.user).subscribe(resp=>{
      console.log(resp);
      for(let u in this.friend){
        if(this.friend[u].user==i.user){
          this.friend.splice(Number(u),1);
        }
      }
    })
  }

  
}
