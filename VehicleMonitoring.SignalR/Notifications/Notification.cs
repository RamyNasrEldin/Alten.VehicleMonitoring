namespace VehicleMonitoring.SignalR.Notifications
{
    public class VehicleStatusNotification
    {
        #region Data Memebrs
        public string VehicleId { get; set; }
        public bool Status { get; set; }
        #endregion
        #region CTOR
        public VehicleStatusNotification(string vehicleId, bool status)
        {
            this.VehicleId = vehicleId;
            this.Status = status;
        }
        #endregion

    }
}
