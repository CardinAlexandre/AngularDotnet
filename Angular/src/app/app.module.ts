import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { FormulaireComponent } from './formulaire/formulaire.component';
import { AccueilComponent } from './accueil/accueil.component';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    FormulaireComponent,
    AccueilComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'login', component: LoginComponent }, 
      { path: 'accueil', component: AccueilComponent }, 
      { path: 'formulaire', component: FormulaireComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

// Commande pour eviter les erreurs CORS pendant le developpement
// à executer dans le répertoire de chrome.exe
// chrome.exe --user-data-dir="C:/Chrome dev session" --disable-web-security
