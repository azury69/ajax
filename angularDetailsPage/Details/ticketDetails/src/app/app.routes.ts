import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TicketListComponent } from './ticket-list/ticket-list.component';
import { TicketDetailComponent } from './ticket-detail/ticket-detail.component';

export const routes: Routes = [
  { path: 'ticket-list', component: TicketListComponent },
  { path: 'ticket-detail/:id', component: TicketDetailComponent },
  { path: '', redirectTo: '/ticket-list', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
