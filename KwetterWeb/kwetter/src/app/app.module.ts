import {APP_INITIALIZER, NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NavbarComponent} from './component/navbar/navbar.component';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatButtonModule} from '@angular/material/button';
import {MatMenuModule} from '@angular/material/menu';
import {MatIconModule} from '@angular/material/icon';
import {KeycloakAngularModule, KeycloakService} from 'keycloak-angular';
import {HttpClientModule} from '@angular/common/http';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {MatDialog, MatDialogModule} from '@angular/material/dialog';
import {CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MatNativeDateModule} from '@angular/material/core';
import {MaterialExampleModule} from 'src/material.module';
import {KweetListComponent} from './component/kweet-list/kweet-list.component';
import {PostKweetComponent} from './component/post-kweet/post-kweet.component';
import {environment} from "../environments/environment";

export function initializeKeycloak(keycloak: KeycloakService) {
  return () =>
    keycloak.init({
      config: {
        // Used dnsmasq to make this work. Needs different solution once inside the cluster
        url: environment.keycloak.url,
        realm: environment.keycloak.realm,
        clientId: environment.keycloak.clientId,
      },
      initOptions: {
        onLoad: 'login-required',
        silentCheckSsoRedirectUri:
          window.location.origin + '/assets/silent-check-sso.html'
      }
    });
}

@NgModule({
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  declarations: [
    AppComponent,
    NavbarComponent,
    KweetListComponent,
    PostKweetComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatMenuModule,
    MatIconModule,
    KeycloakAngularModule,
    HttpClientModule,
    MatDialogModule,
    FormsModule,
    MatDialogModule,
    MatNativeDateModule,
    MaterialExampleModule,
    ReactiveFormsModule,
    NgOptimizedImage,
  ],

  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initializeKeycloak,
      multi: true,
      deps: [KeycloakService],
    },
    MatDialog
  ],

  bootstrap: [AppComponent]
})
export class AppModule {
}
