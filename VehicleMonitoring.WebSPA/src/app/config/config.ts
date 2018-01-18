export let Config = {
  Urls: {
    customerUrl: 'http://localhost:5000/api/Customers',
    customerVehicleListUrl: 'http://localhost:5000/api/MonitoringBoard/GetCustomersVehicles',
    vehiclesByCutomerIdUrl: 'http://localhost:5000/api/MonitoringBoard/GetCustomerVehiclesByCustomerId',
    vehiclesByStatusUrl: 'http://localhost:5000/api/MonitoringBoard/GetCustomersVehiclesByStatus',
    signalrUrl:'http://localhost:4000/vehicleMonitoring'
  }
};
