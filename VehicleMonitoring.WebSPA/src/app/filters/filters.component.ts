import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-filter',
  templateUrl: './filters.component.html',
  styleUrls:['./filters.component.css']
})

export class Filters {
  @Input() customers;

  private selectedCustomerName = "All";
  private selectedStatusName = "All";

  private selectedCustomerId: any = null;
  private selectedStatus: any = null;

  @Output() onFilterApplied: EventEmitter<any> = new EventEmitter<any>();

  addCustomerFilter(customer: any) {
    if (customer != null) {
      this.selectedCustomerName = customer.name;
      this.selectedCustomerId = customer.id;
    }
    else {
      this.selectedCustomerName = "All";
      this.selectedCustomerId = null;
    }

    this.onFilterApplied.emit({ customerId: this.selectedCustomerId, status: this.selectedStatus });
  }

  addStatusFilter(isConnected: any, statusName: any) {
    if (isConnected != null) {
      this.selectedStatusName = statusName;
      this.selectedStatus = isConnected;
    }
    else {
      this.selectedStatusName = "All";
      this.selectedStatus = isConnected;
    }

    this.onFilterApplied.emit({ customerId: this.selectedCustomerId, status: this.selectedStatus });
  }

}
