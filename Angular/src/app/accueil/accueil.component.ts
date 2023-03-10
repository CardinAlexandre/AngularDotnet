import { Component } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError } from 'rxjs';

@Component({
  selector: 'app-accueil',
  templateUrl: './accueil.component.html',
  styleUrls: ['./accueil.component.css']
})
export class AccueilComponent {
  title = 'Welcome';
  token: string | null = null;

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    this.token = localStorage.getItem('token');
    if (this.token !== null) {
      this.verifyToken(this.token).subscribe(
        (response) => {
          // le token est valide
        },
        (error) => {
          // le token n'est pas valide, rediriger vers la page de connexion
          this.router.navigate(['/login']);
        }
      );
    } else {
      // pas de token, rediriger vers la page de connexion
      this.router.navigate(['/login']);
    }
  }

  verifyToken(token: string) {
    const params = new HttpParams().set('token', token);
    return this.http.get('https://localhost:7167/api/VerifyToken', { params }).pipe(
        catchError((error) => {
            console.log('Error:', error.message);
            throw error;
        })
    );
  }
}
