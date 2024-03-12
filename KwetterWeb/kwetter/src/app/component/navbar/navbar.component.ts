import {Component, OnInit} from '@angular/core';
import {KeycloakService} from 'keycloak-angular';
import {KweetService} from "../../services/kweet/kweet.service";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})


export class NavbarComponent implements OnInit {
  constructor(
    private readonly keycloak: KeycloakService,
    private readonly kweetService: KweetService
  ) {
  }

  public hasRealmRole: boolean = false;

  ngOnInit(): void {
    this.hasRealmRole = this.keycloak.getUserRoles().includes('realm-role');
  }

  public async logout() {
    await this.keycloak.logout();
  }

  public async gdpr() {
    if (confirm("Are you sure you want to delete your account? Kweets will remain but become anonymous.")) {
      await this.kweetService.gdprDeleteMe();
      await this.logout();
    }
  }
}
