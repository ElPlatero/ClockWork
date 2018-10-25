import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { WorkerDetailComponent } from './worker-detail/worker-detail.component';

export const routes = [
  { path: '', component: HomeComponent },
  { path: 'worker/:id', component: WorkerDetailComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
