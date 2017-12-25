import {Injectable} from "@angular/core";
import { Http, Response, URLSearchParams } from "@angular/http";
import { Config } from "../config/config";

import 'rxjs/add/operator/map';

@Injectable()

export class MonitoringBoardService {
  //Getting URLs from Config
  private customerVehicleListUrl = Config.Urls.customerVehicleListUrl;
  private customersUrl = Config.Urls.customerUrl;
  private customerByIdUrl = Config.Urls.vehiclesByCutomerIdUrl;

  constructor(private _http: Http) { }

  getCustomersVehicleList() {
    let that = this;
    return this._http.get(this.customerVehicleListUrl).map((response) => {
      let data;
      if (response.status == 200)
        data = response.json();
      else
        console.log('No data found');

      return data;
    });
  }
  getVehiclesByCustomerId(customerId: string) {
    let params: URLSearchParams = new URLSearchParams();
    params.set('customerID', customerId);
    let that = this;
    return this._http.get(this.customerByIdUrl, {
      search: params
    }).map((response) => {
      let data;
      if (response.status == 200) {
        data = response.json();
      }
      else
        console.log('No data found');

      return data;
    });
  }
  getCustomers() {
    let that = this;
    return this._http.get(this.customersUrl).map((response) => {
      let data;
      if (response.status == 200)
        data = response.json();
      else
        console.log('No data found');

      return data;

    });
  }

}
