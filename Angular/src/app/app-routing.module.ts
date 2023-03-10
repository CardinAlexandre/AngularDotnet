import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormulaireComponent } from './formulaire/formulaire.component';
import { AppComponent } from './app.component';
import { AccueilComponent } from './accueil/accueil.component';

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
