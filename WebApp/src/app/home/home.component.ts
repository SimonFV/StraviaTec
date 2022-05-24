import { DatePipe, formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ApiMongoService } from '../services/ApiMongo/api-mongo.service';
import { ApiService } from '../services/ApiService/api.service';
import { SharedService } from '../services/SharedService/shared.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [DatePipe]
})
export class HomeComponent implements OnInit {

  constructor(
    private service: ApiService,
    private sharedService: SharedService,
    private mongoService: ApiMongoService,
    private datePipe: DatePipe
  ) { }

  userImage: any;
  userImagePath: string = '';
  isImageLoading = false;
  data: any = [];

  activities = [{
    "id": 0,
    "User": "sfv",
    "FirstName": "sfv",
    "LastName1": "sfv",
    "LastName2": "sfv",
    "Type": "Running",
    "Start": "f",
    "Route": "ff",
    "Distance": 10
  }];
  comments = [{
    "id": 0,
    "user": "",
    "activityId": "",
    "postTime": "",
    "body": ""
  }];
  activityNumber = -1;
  ngOnInit(): void {
    this.activities.splice(0, 1);
    this.comments.splice(0, 1);
    //Check de usuario logeado
    /*if (this.sharedService.getUser() == '') {
      return;
    }else{
      this.service.GetFriendsFrontPage(this.sharedService.getUser()).subscribe(resp => {
        console.log(resp.body);
        for (let i of resp.body!) {
          this.loadActivity(i);
        }
      })
  }*/
    this.service.GetFriendsFrontPage('sfv').subscribe(resp => {
      console.log(resp.body);
      for (let i of resp.body!) {
        this.loadActivity(i);
      }
    });

    // solicita y carga la informacion del usuario
    // y luego carga la imagen
    this.service.getUser('sfv').subscribe(resp => {
      console.log(resp.body);
      this.data = <JSON>resp.body;
      this.userImagePath = this.data.picture;
      this.getImageFromService(this.userImagePath);
    });

  }

  loadActivity(acts: any) {

    this.activities.push({
      "id": acts.id,
      "User": acts.user,
      "FirstName": acts.firstName,
      "LastName1": acts.lastName1,
      "LastName2": acts.lastName2,
      "Type": acts.type,
      "Start": acts.start,
      "Route": acts.route,
      "Distance": acts.distance
    })
  }

  getCommentsByActivity(i: number) {


    this.activityNumber = this.activities[i].id;
    console.log("ACT NUM: " + this.activityNumber);
    this.mongoService.getCommentsByActivity(this.activities[i].id).subscribe(resp => {
      console.log(resp);

      for (let i of resp.body!) {
        this.showComments(i);
      }
    })
  }
  showComments(comment: any) {
    this.comments.push({
      "id": comment.id,
      "user": comment.user,
      "activityId": comment.activityId,
      "postTime": comment.postTime,
      "body": comment.body
    })
    var postTime = new Date();
    const cValue = formatDate(postTime, 'yyyy-MM-ddTHH:mm:SS', 'en-US');
    console.log(cValue + 'Z');

  }
  hideComments() {
    this.comments.splice(0, this.comments.length);
  }


  addComment(comment: any) {
    var postTime = new Date();
    const cValue = formatDate(postTime, 'yyyy-MM-ddTHH:mm:SS', 'en-US');
    var newComment = {
      "user": 'SRC'/*this.sharedService.getUser()*/,
      "activityId": this.activityNumber,
      "postTime": cValue + "Z",
      "body": comment
    }

    if (comment == '') {
      return;
    } else {
      this.mongoService.postComments(newComment).subscribe(resp => {
        console.log(resp);
      })
      this.showComments(newComment);
    }
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

  getImageFromService(imageUrl: string) {
    this.isImageLoading = true;
    const path: any = { path: imageUrl };
    this.service.getUserImage(<JSON>path).subscribe({
      next: (data) => {
        this.createImageFromBlob(data);
        this.isImageLoading = false;
      },
      error: (error) => {
        this.isImageLoading = true;
        console.log(error);
      }
    });
  }
}
