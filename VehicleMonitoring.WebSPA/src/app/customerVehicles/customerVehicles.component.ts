import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-customerVehicles',
  templateUrl: './customerVehicles.component.html',
  styleUrls:['./customerVehicles.component.css']
})

export class CustomerVehicles {
  @Input() customer;
}
