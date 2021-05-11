using System;
using System.Collections.Generic;
using System.Linq;

namespace WbEasyCalcRepository.Model
{
    public class NetworkSheet
    {
        private readonly EasyCalcSheet _data;

        public NetworkSheet(EasyCalcSheet data)
        {
            this._data = data;
            this.DistributionAndTransmissionMainsEntries_D7_D26 = new List<double>();
        }

        public List<double> DistributionAndTransmissionMainsEntries_D7_D26 { get; set; }
        public double DistributionAndTransmissionMainsPossibleUnderestimation_D30 { get; set; }
        public double DistributionAndTransmissionMainsTotalKm_D28 { get => this.DistributionAndTransmissionMainsEntries_D7_D26.Sum(); }
        public double DistributionAndTransmissionMainsMinimum_D33 { get => this.DistributionAndTransmissionMainsTotalKm_D28; }
        public double DistributionAndTransmissionMainsMaximum_D35
        {
            get => Math.Round(this.DistributionAndTransmissionMainsTotalKm_D28 * (1 + this.DistributionAndTransmissionMainsPossibleUnderestimation_D30), 1);
        }
        public double DistributionAndTransmissionMainsBestEstimate_D37
        {
            get => (this.DistributionAndTransmissionMainsMinimum_D33 + this.DistributionAndTransmissionMainsMaximum_D35) / 2;
        }

        public double Network_NoCustomers_H7 { get; set; }
        public double NumberOfConnectionsOfRegsteredCustomers_H10 { get; set; }
        public double NumberOfInactiveAccountsWServiceConnections_H18 { get; set; }
        public double AvgLenOfServiceConnectionFromBoundaryToMeterM_H32 { get; set; }
        public double Network_ErrorMargin_J7 { get; set; }
        public double Network_ErrorMargin_J10 { get; set; }
        public double Network_ErrorMargin_J18 { get; set; }
        public double Network_ErrorMargin_J32 { get; set; }

        public double EstimatedNumberOfIllegalConnections_H21
        {
            get => this._data.UnauthorizedConsumptionSheet.IllegalConnectionsDomesticEstimatedNumber_D6
                + this._data.UnauthorizedConsumptionSheet.IllegalConnectionsOthersEstimatedNumber_D10;
        }
        public double ServiceConnectionsBestEstimate_H30
        {
            get => this.NumberOfConnectionsOfRegsteredCustomers_H10
                + this.EstimatedNumberOfIllegalConnections_H21
                + this.NumberOfInactiveAccountsWServiceConnections_H18;
        }
        public double LenOfServConnFromBoundToMeterKm_H39
        {
            get => this.AvgLenOfServiceConnectionFromBoundaryToMeterM_H32
                * this.ServiceConnectionsBestEstimate_H30 / 1000;
        }
        public double Network_ErrorMarg_J21 { get => _data.UnauthorizedConsumptionSheet.IllegalConnectionsDomesticErrorMargin_F6; }
        
        public double Network_ErrorMarg_J24 { get => GetNetwork_ErrorMarg_J24(); }
        private double GetNetwork_ErrorMarg_J24()
        {
            var m7 = (NumberOfConnectionsOfRegsteredCustomers_H10 * Network_ErrorMargin_J10) / Constants.StandardDistributionFactor;
            var m15 = (NumberOfInactiveAccountsWServiceConnections_H18 * Network_ErrorMargin_J18) / Constants.StandardDistributionFactor;
            var m18 = (EstimatedNumberOfIllegalConnections_H21 * Network_ErrorMarg_J21) / Constants.StandardDistributionFactor;
            var n7 = m7 * m7;
            var n15 = m15 * m15;
            var n18 = m18 * m18;
            var n22 = n7 + n15 + n18;
            var m24 = Math.Sqrt(n22);

            return ServiceConnectionsBestEstimate_H30==0 ? 0 : m24 * Constants.StandardDistributionFactor / ServiceConnectionsBestEstimate_H30;
        }
        public double Network_ErrorMarg_J39 { get => Math.Sqrt(Network_ErrorMarg_J24*Network_ErrorMarg_J24 + Network_ErrorMargin_J32*Network_ErrorMargin_J32); }

        public double Network_Min_H26 { get => this.GetNetwork_Min_H26(); }
        private double GetNetwork_Min_H26()
        {
            return ServiceConnectionsBestEstimate_H30 * (1 - Network_ErrorMarg_J24);
        }
        public double Network_Max_H28 { get => this.GetNetwork_Max_H28(); }
        private double GetNetwork_Max_H28()
        {
            return ServiceConnectionsBestEstimate_H30 * (1 + Network_ErrorMarg_J24);
        }

    }
}
