import { Component } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

@Component({
  selector: 'app-form',
  templateUrl: './formulaire.component.html',
  styleUrls: ['./formulaire.component.css'],
})
export class FormulaireComponent {
  formulaire = 'Formulaire';
  id!: string;
  nom!: string;
  prenom!: string;
  
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

  onSubmit() {
    const formData = JSON.stringify({ nom: this.nom, prenom: this.prenom });
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    this.http.post('https://localhost:7167/api/Formulaire', formData, { headers: headers })
      .subscribe(response => {
        console.log(response);
        this.recupererNomsPrenoms();
      });
  }

  envoyer() {
    console.log('Nom : ' + this.nom);
    console.log('Prénom : ' + this.prenom);
  }

  nomsPrenoms: any[] = [];

  recupererNomsPrenoms() {
    this.http.get('https://localhost:7167/api/Formulaire').subscribe(
      (response: any) => {
        this.nomsPrenoms = response;
      },
      (error: any) => {
        console.log(error);
      }
    );
  }

  supprimerLigne(id: number) {
    this.http.delete(`https://localhost:7167/api/Formulaire/${id}`).subscribe(
      () => {
        console.log(`La ligne avec l'ID ${id} a été supprimée.`);
        this.recupererNomsPrenoms();
      },
      (error) => {
        console.error(`Une erreur est survenue lors de la suppression de la ligne avec l'ID ${id} :`, error);
      }
    );
  }
}