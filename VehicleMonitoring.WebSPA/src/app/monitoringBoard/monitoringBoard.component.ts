import { Component, OnInit, NgZone } from '@angular/core';
import { HubConnection } from "@aspnet/signalr-client";
import { Config } from '../config/config'

import { CustomerVehicles } from '../customerVehicles/customerVehicles.component';

import { MonitoringBoardService } from '../services/monitoringBoard.service';

import 'rxjs/add/operator/map';
import $ from 'jquery';

@Component({
  selector: 'app-monitoringBoard',
  templateUrl: './monitoringBoard.component.html',
  styleUrls: ['./monitoringBoard.component.css']
})

export class MonitoringBoard implements OnInit{
  private customerVehicleList: any = null;
  private customers: any = null;

  private selectedCustomerId: string;
  private selectedStatus: boolean;
 
  private filteredCustomerVehicleList: any = null;
  private vehiclesList: any = null;
  private filteredVehicleList: any = null;

  private _hubConnection: HubConnection;
  private signalrUrl = Config.Urls.signalrUrl;
  constructor(private _monitoringBoardService: MonitoringBoardService) {

  }
  
  ngOnInit() {
    let that = this;
    this.vehiclesList = [];
    this.startHubConnection();

    this._monitoringBoardService.getCustomers().subscribe((customersList) => {
      that.customers = customersList;
    });

    this._monitoringBoardService.getCustomersVehicleList().subscribe((customerVehiclesList) => {
      that.customerVehicleList = that.filteredCustomerVehicleList = customerVehiclesList;
      $.each(that.customerVehicleList, (index, customer) => {
        that.vehiclesList = that.vehiclesList.concat(customer.vehicles);
      });

      that.filteredVehicleList = that.vehiclesList;
    });
  }
  
  applyFilter(data: any) {
    let that = this;
    if (data != null) {
      that.filteredVehicleList = [];
      that.filteredCustomerVehicleList = $.extend(true, [], that.customerVehicleList);

      this.selectedCustomerId = data.customerId;
      this.selectedStatus = data.status;

      if (data.customerId != null) {
        that.filteredCustomerVehicleList = that.filteredCustomerVehicleList.filter(customer => customer.id == data.customerId);
      }

      if (data.status != null) {
        that.filteredCustomerVehicleList.map(customer => customer.vehicles = customer.vehicles.filter(vehicle => vehicle.currentStatus == data.status));
      }      

      that.filteredCustomerVehicleList = that.filteredCustomerVehicleList.filter(customer => customer.vehicles.length > 0);
      

      $.each(that.filteredCustomerVehicleList, (index, customer) => {
        that.filteredVehicleList = that.filteredVehicleList.concat(customer.vehicles);
      });
    }
  }

  startHubConnection() {
    let that = this;

    this._hubConnection = new HubConnection(this.signalrUrl);

    this._hubConnection.on('vehicleStatusChanged', (data: any) => {
      that.syncVehicleStatus(data)
    });

    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started')
      })
      .catch(err => {
        console.log('Error while establishing connection')
      });
  }

  syncVehicleStatus(notificationData: any) {
    let updatedVehicle = this.vehiclesList.filter(vehicle => vehicle.id == notificationData.vehicleId);
    let updatedFilteredVehicle = this.filteredVehicleList.filter(vehicle => vehicle.id == notificationData.vehicleId);

    if (updatedVehicle != null && updatedVehicle.length > 0
      && updatedVehicle[0].currentStatus != notificationData.status) {
      updatedVehicle[0].currentStatus = notificationData.status;
    }

    if (updatedFilteredVehicle != null && updatedFilteredVehicle.length > 0
      && updatedFilteredVehicle[0].currentStatus != notificationData.status) {
      updatedFilteredVehicle[0].currentStatus = notificationData.status;
    }

    this.applyFilter({ status: this.selectedStatus, customerId: this.selectedCustomerId });

  }
  
}
