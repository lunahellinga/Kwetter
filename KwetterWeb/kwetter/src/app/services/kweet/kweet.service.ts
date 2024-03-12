import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {Kweet} from "../../model/kweet/kweet";
import {NewKweet} from "../../model/new-kweet/new-kweet";
import {environment} from "../../../environments/environment";
import {KeycloakService} from "keycloak-angular";

@Injectable({
  providedIn: 'root'
})
export class KweetService {
  constructor(private http: HttpClient) {
  }

  getRecentKweets(): Observable<Kweet[]> {
    return this.http.get<any>(environment.kwetter.gateway.recent).pipe(
      map(response => response['$values'] as Kweet[])
    );
  }

  postKweet(kweetText: string): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http.post(environment.kwetter.gateway.post, new NewKweet(kweetText)).subscribe({
        error: (e) => reject(e),
        complete: () => resolve(true)
      })
    });
  }

  gdprDeleteMe(): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http.delete(environment.kwetter.gateway.gdpr).subscribe({
        error: (e) => reject(e),
        complete: () => resolve(true)
      })
    });
  }

}

