import {Component} from '@angular/core';
import {KweetService} from "../../services/kweet/kweet.service";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-post-kweet',
  templateUrl: './post-kweet.component.html',
  styleUrls: ['./post-kweet.component.scss']
})
export class PostKweetComponent {
  kweetText: string = '';

  constructor(private kweetService: KweetService) {
  }

  postKweet(): void {
    this.kweetText = this.kweetText.trim()
    if (this.kweetText.length > environment.kwetter.conf.kweet_max_length || this.kweetText.length == 0) {
      console.error("Not postable!")
      return
    }
    console.log(this.kweetText)
    this.kweetService.postKweet(this.kweetText)
      .then(() => {
        // Kweet posted successfully
        this.kweetText = '';
      })
      .catch((error) => {
        console.error('Error posting kweet:', error);
      });
  }

}
