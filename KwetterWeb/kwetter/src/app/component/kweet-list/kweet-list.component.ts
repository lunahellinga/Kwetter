import {Component, OnInit} from '@angular/core';
import {Kweet} from "../../model/kweet/kweet";
import {KweetService} from "../../services/kweet/kweet.service";

@Component({
  selector: 'app-kweet-list',
  templateUrl: 'kweet-list.component.html',
  styleUrls: ['kweet-list.component.scss']
})
export class KweetListComponent implements OnInit {
  kweets: Kweet[] = [];

  constructor(private kweetService: KweetService) {
  }

  ngOnInit(): void {
    this.loadKweets();
  }

  loadKweets(): void {
    this.kweetService.getRecentKweets().subscribe({
      next: (kweets) => {
        this.kweets = kweets;
        console.log(kweets);
      },
      error: (error) => {
        console.error('Error fetching kweets:', error);
      },
      complete: () => {
        console.info('complete');
      }
    });
  }
}
