import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

interface LoginResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})

@Component({
  selector: 'app-form',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})

export class LoginComponent {
  loginTitle = 'Page de connexion';
  newAccountTitle = 'Créer un nouveau compte';

  username!: string;
  password!: string;
  email!: string;
  newUsername!: string;
  newPassword!: string;

  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string): Observable<LoginResponse> {
    const body = { username: username, password: password };
    return this.http.post<LoginResponse>('https://localhost:7167/api/Login', body);
  }

  onLoginSubmit() {
    this.login(this.username, this.password).subscribe(
      response => {
        console.log(response.token);
        localStorage.setItem('token', response.token);
        this.router.navigate(['/accueil']);
      },
      error => console.log(error)
    );
  }

  onNewUserSubmit() {
    const formData = JSON.stringify({ username: this.newUsername, password: this.newPassword, email: this.email });
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    this.http.post('https://localhost:7167/api/CreateAccount', formData, { headers: headers })
      .subscribe(response => {
        console.log(response);
      });
  }
}
