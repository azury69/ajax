import { Component,OnInit } from '@angular/core';
import { TicketService } from '../ticket.service';
import { RouterLink } from '@angular/router';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-ticket-list',
  imports: [RouterLink,NgFor],
  templateUrl: './ticket-list.component.html',
  styleUrl: './ticket-list.component.css'
})
export class TicketListComponent implements OnInit {
  tickets: any[] = [];

  constructor(private ticketService: TicketService) {}

  ngOnInit(): void {
    this.ticketService.getTickets().subscribe((data) => {
      this.tickets = data;
    });
  }
}
