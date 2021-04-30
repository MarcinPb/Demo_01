using System;

namespace WbEasyCalcRepository.Model
{
    public class PiSheet
    {
        private readonly EasyCalcSheet _data;

        public PiSheet(EasyCalcSheet data)
        {
            _data = data;
        }

        public double Pis_F9 { get => _data.IntermittentSupplySheet.SupplyTimeBestEstimate_H33; }
        public double Pis_H9 { get => _data.IntermittentSupplySheet.ErrorMargin_H26; }
        public double Pis_J9 { get => Pis_F9 * (1 - Pis_H9); }
        public double Pis_L9 { get => Pis_F9 * (1 + Pis_H9); }


        public double Pis_F11 { get => _data.PressureSheet.AveragePressureBestEstimate_F33; }
        public double Pis_H11 { get => _data.PressureSheet.Prs_ErrorMarg_F26; }
        public double Pis_J11 { get => Pis_F11 * (1 - Pis_H11); }
        public double Pis_L11 { get => Pis_F11 * (1 + Pis_H11); }

        public double Pis_F17 { get => _data.WaterBalanceDaySheet.PhysicalLossesM3_T34; }
        public double Pis_H17 { get => _data.WaterBalanceDaySheet.PhyscialLossesErrorMargin_AH35; }
        public double Pis_J17 { get => Pis_F17 * (1 - Pis_H17); }
        public double Pis_L17 { get => Pis_F17 * (1 + Pis_H17); }

        public double Pis_F19 { get => GetPis_F19(); }
        private double GetPis_F19()
        {
            if ((((_data.NetworkSheet.DistributionAndTransmissionMainsBestEstimate_D37 * 18 + _data.NetworkSheet.ServiceConnectionsBestEstimate_H30 * 0.8 + _data.NetworkSheet.LenOfServConnFromBoundToMeterKm_H39 * 25) * Pis_F11) / 24 * Pis_F9) * 365/1000 > 0) 
            {
                return (((_data.NetworkSheet.DistributionAndTransmissionMainsBestEstimate_D37 * 18 + _data.NetworkSheet.ServiceConnectionsBestEstimate_H30 * 0.8 + _data.NetworkSheet.LenOfServConnFromBoundToMeterKm_H39 * 25) * Pis_F11) / 24 * Pis_F9) / 1000;
            }
            else
            {
                return 0;
            }
        }
        public double Pis_H19 { get => GetPis_H19(); }
        private double GetPis_H19()
        {
            if (Pis_F19 == 0) 
            {
                return 0d;
            }
            else
            {
                var r17 = _data.NetworkSheet.ServiceConnectionsBestEstimate_H30;
                var s17 = r17 * 0.8 * Pis_F11 / 24 * Pis_F9 / 1000;
                var t17 = Math.Sqrt(Pis_H9 * Pis_H9 + Pis_H11 * Pis_H11 + Pis_H11 * Pis_H11 + _data.NetworkSheet.Network_ErrorMarg_J24 * _data.NetworkSheet.Network_ErrorMarg_J24);
                var u17 = Math.Pow(((s17 * t17) / 1.96), 2);
                var r19 = _data.NetworkSheet.LenOfServConnFromBoundToMeterKm_H39;
                var s19 = r19 * 25 * 365 * Pis_F11 / 24 * Pis_F9 / 1000 / 1000;
                var t19 = Math.Sqrt(Pis_H9 * Pis_H9 + Pis_H11 * Pis_H11 + Pis_H11 * Pis_H11 + _data.NetworkSheet.Network_ErrorMarg_J39 * _data.NetworkSheet.Network_ErrorMarg_J39);
                var u19 = Math.Pow(((s19 * t19) / 1.96), 2);
                var u20 = u17 + u19;
                return Math.Sqrt(u20) * 1.96 / Pis_F19;
            }
        }
        public double Pis_J19 { get => Pis_F19 * (1 - Pis_H19); }
        public double Pis_L19 { get => Pis_F19 * (1 + Pis_H19); }

        public double Pis_F25 { get => Pis_F19 == 0 ? 0 : Pis_F17 / Pis_F19; }
        public double Pis_H25 { get => Math.Sqrt(Pis_H17 * Pis_H17 + Pis_H19 * Pis_H19); }
        public double Pis_J25 { get => Pis_F25 * (1 - Pis_H25); }
        public double Pis_L25 { get => Pis_F25 * (1 + Pis_H25); }

        public double Pis_F27 { get => GetPis_F27(); }
        private double GetPis_F27()
        {
            if ( _data.NetworkSheet.ServiceConnectionsBestEstimate_H30 == 0 || Pis_F9 == 0)
            {
                return 0d;
            }
            else
            {
                return Pis_F17 / _data.NetworkSheet.ServiceConnectionsBestEstimate_H30 * 1000 / Pis_F9 * 24;
            }
        }
        public double Pis_H27 { get => Math.Sqrt(Pis_H9 * Pis_H9 + _data.NetworkSheet.Network_ErrorMarg_J24 * _data.NetworkSheet.Network_ErrorMarg_J24 + _data.WaterBalanceYearSheet.PhyscialLossesErrorMargin_AH35 * _data.WaterBalanceYearSheet.PhyscialLossesErrorMargin_AH35); }
        public double Pis_J27 { get => Pis_F27 * (1 - Pis_H27); }
        public double Pis_L27 { get => Pis_F27 * (1 + Pis_H27); }

        public double Pis_F29 { get => Pis_F11==0 ? 0 : Pis_F27 / Pis_F11; }
        public double Pis_H29 { get => Math.Sqrt(Pis_H27 * Pis_H27 + Pis_H11 * Pis_H11); }
        public double Pis_J29 { get => Pis_F29 * (1 - Pis_H29); }
        public double Pis_L29 { get => Pis_F29 * (1 + Pis_H29); }

        public double Pis_F31 { get => _data.NetworkSheet.DistributionAndTransmissionMainsBestEstimate_D37 == 0 || Pis_F9 == 0 ? 0d : Pis_F17 / Pis_F9 / _data.NetworkSheet.DistributionAndTransmissionMainsBestEstimate_D37; }
        public double Pis_H31 { get => Math.Sqrt(Pis_H9 * Pis_H9 + _data.NetworkSheet.DistributionAndTransmissionMainsPossibleUnderestimation_D30 * _data.NetworkSheet.DistributionAndTransmissionMainsPossibleUnderestimation_D30 + _data.WaterBalanceYearSheet.PhyscialLossesErrorMargin_AH35 * _data.WaterBalanceYearSheet.PhyscialLossesErrorMargin_AH35); }
        public double Pis_J31 { get => Pis_F31 * (1 - Pis_H31); }
        public double Pis_L31 { get => Pis_F31 * (1 + Pis_H31); }

        public double Pis_F37 { get => _data.WaterBalanceYearSheet.AuthorizedConsumption_K12 == 0 ? 0d : _data.WaterBalanceYearSheet.CommercialLosses_T26 / _data.WaterBalanceYearSheet.AuthorizedConsumption_K12; }
        public double Pis_H37 { get => GetPis_H37(); }
        private double GetPis_H37()
        {
            var s30 = _data.WaterBalanceYearSheet.AuthorizedConsumption_K12;
            var s34 = s30 * Pis_F37;
            if (s34 == 0)
            {
                return 0d;
            }
            else
            {
                var t30 = _data.WaterBalanceYearSheet.AuthorizedConsumptionErrorMargin_K15;
                var u30 = Math.Pow(((s30 * t30) / 1.96), 2);
                var t32 = _data.WaterBalanceYearSheet.CommercialLossesErrorMargin_T29;
                var s32 = _data.WaterBalanceYearSheet.CommercialLosses_T26;
                var u32 = Math.Pow(((s32 * t32) / 1.96), 2);
                var u34 = u30 + u32;
                return Math.Sqrt(u34) * 1.96 / s34;
            }
        }

        public double Pis_J37 { get => Pis_F37 * (1 - Pis_H37); }
        public double Pis_L37 { get => Pis_F37 * (1 + Pis_H37); }

        public double Pis_F39 { get => _data.NetworkSheet.ServiceConnectionsBestEstimate_H30 == 0 ? 0d : _data.WaterBalanceDaySheet.CommercialLosses_T26 / _data.NetworkSheet.ServiceConnectionsBestEstimate_H30 * 1000; }
        public double Pis_H39 { get => Math.Sqrt(_data.NetworkSheet.Network_ErrorMarg_J24 * _data.NetworkSheet.Network_ErrorMarg_J24 + _data.WaterBalanceYearSheet.CommercialLossesErrorMargin_T29 * _data.WaterBalanceYearSheet.CommercialLossesErrorMargin_T29); }
        public double Pis_J39 { get => Pis_F39 * (1 - Pis_H39); }
        public double Pis_L39 { get => Pis_F39 * (1 + Pis_H39); }

        public double Pis_F41 { get => _data.NetworkSheet.Network_NoCustomers_H7 == 0 ? 0d : _data.WaterBalanceDaySheet.CommercialLosses_T26 / _data.NetworkSheet.Network_NoCustomers_H7 * 1000; }
        public double Pis_H41 { get => Math.Sqrt(_data.NetworkSheet.Network_ErrorMargin_J7 * _data.NetworkSheet.Network_ErrorMargin_J7 + _data.WaterBalanceYearSheet.CommercialLossesErrorMargin_T29 * _data.WaterBalanceYearSheet.CommercialLossesErrorMargin_T29); }
        public double Pis_J41 { get => Pis_F41 * (1 - Pis_H41); }
        public double Pis_L41 { get => Pis_F41 * (1 + Pis_H41); }

        public double Pis_F47 { get => _data.WaterBalanceYearSheet.SystemInputVolume_B19 == 0 ? 0d : _data.WaterBalanceYearSheet.NonRevenueWaterM3_AY24 / _data.WaterBalanceYearSheet.SystemInputVolume_B19; }
        public double Pis_H47 { get => _data.WaterBalanceYearSheet.NonRevenueWaterErrorMargin_AY26; }
        public double Pis_J47 { get => Pis_F47 * (1 - Pis_H47); }
        public double Pis_L47 { get => Pis_F47 * (1 + Pis_H47); }

        public double Pis_F49 { get => _data.FinancialDataSheet.FinancData_G35 == 0 ? 0d : _data.FinancialDataSheet.FinancData_G31 / _data.FinancialDataSheet.FinancData_G35; }
        public double Pis_H49 { get => Pis_H47; }
        public double Pis_J49 { get => Pis_F49 * (1 - Pis_H49); }
        public double Pis_L49 { get => Pis_F49 * (1 + Pis_H49); }

        public double Pis_F51 { get => _data.NetworkSheet.ServiceConnectionsBestEstimate_H30 == 0 ? 0d : _data.WaterBalanceDaySheet.NonRevenueWaterM3_AY24 / _data.NetworkSheet.ServiceConnectionsBestEstimate_H30 * 1000 / Pis_F9 * 24; }
        public double Pis_H51 { get => Math.Sqrt(Pis_H9 * Pis_H9 + _data.NetworkSheet.Network_ErrorMargin_J10 * _data.NetworkSheet.Network_ErrorMargin_J10 + _data.WaterBalanceYearSheet.NonRevenueWaterErrorMargin_AY26 * _data.WaterBalanceYearSheet.NonRevenueWaterErrorMargin_AY26); }
        public double Pis_J51 { get => Pis_F51 * (1 - Pis_H51); }
        public double Pis_L51 { get => Pis_F51 * (1 + Pis_H51); }


        public string Pis_N27 => GetPis_N27();
        private string GetPis_N27()
        {
            string result = string.Empty;

            if (Pis_F25 == 0) { return result; }

            //else if (Pis_F25 <= 1.5) { result = "A1"; }
            //else if (Pis_F25 <= 2) { result = "A2"; }
            //else if (Pis_F25 <= 4) { result = "B"; }
            //else if (Pis_F25 <= 8) { result = "C"; }
            if (_data.MatOneInSheet.SelectedOption == 1)
            {
                     if (Pis_F25 <= _data.MatOneInSheet.C11) { result = "A1"; }
                else if (Pis_F25 <= _data.MatOneInSheet.C12) { result = "A2"; }
                else if (Pis_F25 <= _data.MatOneInSheet.C13) { result = "B"; }
                else if (Pis_F25 <= _data.MatOneInSheet.C14) { result = "C"; }
                else { result = "D"; }
            }
            if (_data.MatOneInSheet.SelectedOption == 3)
            { 
                     if (Pis_F25 <= _data.MatOneInSheet.E11) { result = "A1"; }
                else if (Pis_F25 <= _data.MatOneInSheet.E12) { result = "A2"; }
                else if (Pis_F25 <= _data.MatOneInSheet.E13) { result = "B"; }
                else if (Pis_F25 <= _data.MatOneInSheet.E14) { result = "C"; }
                else { result = "D"; }
            }
            if (_data.MatOneInSheet.SelectedOption == 4)
            { 
                     if (Pis_F25 <= _data.MatOneInSheet.F11) { result = "A1"; }
                else if (Pis_F25 <= _data.MatOneInSheet.F12) { result = "A2"; }
                else if (Pis_F25 <= _data.MatOneInSheet.F13) { result = "B"; }
                else if (Pis_F25 <= _data.MatOneInSheet.F14) { result = "C"; }
                else { result = "D"; }
            }
            if (_data.MatOneInSheet.SelectedOption == 5)
            { 
                     if (Pis_F25 <= _data.MatOneInSheet.G11) { result = "A1"; }
                else if (Pis_F25 <= _data.MatOneInSheet.G12) { result = "A2"; }
                else if (Pis_F25 <= _data.MatOneInSheet.G13) { result = "B"; }
                else if (Pis_F25 <= _data.MatOneInSheet.G14) { result = "C"; }
                else { result = "D"; }
            }
            if (_data.MatOneInSheet.SelectedOption == 6)
            { 
                     if (Pis_F25 <= _data.MatOneInSheet.H11) { result = "A1"; }
                else if (Pis_F25 <= _data.MatOneInSheet.H12) { result = "A2"; }
                else if (Pis_F25 <= _data.MatOneInSheet.H13) { result = "B"; }
                else if (Pis_F25 <= _data.MatOneInSheet.H14) { result = "C"; }
                else { result = "D"; }
            }

            return result;
        }

        public string Pis_P27 => GetPis_P27();
        private string GetPis_P27()
        {
            string result = string.Empty;

            if (Pis_F25 == 0) { return result; }

            //else if (Pis_F25 <= 1.5) { result = "A1"; }
            //else if (Pis_F25 <= 2) { result = "A2"; }
            //else if (Pis_F25 <= 4) { result = "B"; }
            //else if (Pis_F25 <= 8) { result = "C"; }
            if (_data.MatOneInSheet.SelectedOption == 1)
            {
                if (Pis_F25 <= _data.MatOneInSheet.C21) { result = "A1"; }
                else if (Pis_F25 <= _data.MatOneInSheet.C22) { result = "A2"; }
                else if (Pis_F25 <= _data.MatOneInSheet.C23) { result = "B"; }
                else if (Pis_F25 <= _data.MatOneInSheet.C24) { result = "C"; }
                else { result = "D"; }
            }
            if (_data.MatOneInSheet.SelectedOption == 2)
            {
                if (Pis_F25 <= _data.MatOneInSheet.D21) { result = "A1"; }
                else if (Pis_F25 <= _data.MatOneInSheet.D22) { result = "A2"; }
                else if (Pis_F25 <= _data.MatOneInSheet.D23) { result = "B"; }
                else if (Pis_F25 <= _data.MatOneInSheet.D24) { result = "C"; }
                else { result = "D"; }
            }
            if (_data.MatOneInSheet.SelectedOption == 3)
            {
                if (Pis_F25 <= _data.MatOneInSheet.E21) { result = "A1"; }
                else if (Pis_F25 <= _data.MatOneInSheet.E22) { result = "A2"; }
                else if (Pis_F25 <= _data.MatOneInSheet.E23) { result = "B"; }
                else if (Pis_F25 <= _data.MatOneInSheet.E24) { result = "C"; }
                else { result = "D"; }
            }
            if (_data.MatOneInSheet.SelectedOption == 4)
            {
                if (Pis_F25 <= _data.MatOneInSheet.F21) { result = "A1"; }
                else if (Pis_F25 <= _data.MatOneInSheet.F22) { result = "A2"; }
                else if (Pis_F25 <= _data.MatOneInSheet.F23) { result = "B"; }
                else if (Pis_F25 <= _data.MatOneInSheet.F24) { result = "C"; }
                else { result = "D"; }
            }
            if (_data.MatOneInSheet.SelectedOption == 5)
            {
                if (Pis_F25 <= _data.MatOneInSheet.G21) { result = "A1"; }
                else if (Pis_F25 <= _data.MatOneInSheet.G22) { result = "A2"; }
                else if (Pis_F25 <= _data.MatOneInSheet.G23) { result = "B"; }
                else if (Pis_F25 <= _data.MatOneInSheet.G24) { result = "C"; }
                else { result = "D"; }
            }
            if (_data.MatOneInSheet.SelectedOption == 6)
            {
                if (Pis_F25 <= _data.MatOneInSheet.H21) { result = "A1"; }
                else if (Pis_F25 <= _data.MatOneInSheet.H22) { result = "A2"; }
                else if (Pis_F25 <= _data.MatOneInSheet.H23) { result = "B"; }
                else if (Pis_F25 <= _data.MatOneInSheet.H24) { result = "C"; }
                else { result = "D"; }
            }

            return result;
        }

        public string Pis_N47 => GetPis_N47();
        private string GetPis_N47()
        {
            string result;

            if (Pis_F51 == 0) { result = string.Empty; }
            else if (Pis_F51 <= 75) { result = "A1"; }
            else if (Pis_F51 <= 150) { result = "A2"; }
            else if (Pis_F51 <= 300) { result = "B"; }
            else if (Pis_F51 <= 550) { result = "C"; }
            else { result = "D"; }

            return result;
        }

        public string Pis_P47 => GetPis_P47();
        private string GetPis_P47()
        {
            string result;

            if (Pis_F51 == 0) { result = string.Empty; }
            else if (Pis_F51 <= 130) { result = "A1"; }
            else if (Pis_F51 <= 260) { result = "A2"; }
            else if (Pis_F51 <= 520) { result = "B"; }
            else if (Pis_F51 <= 1000) { result = "C"; }
            else { result = "D"; }

            return result;
        }
 
    }
}
