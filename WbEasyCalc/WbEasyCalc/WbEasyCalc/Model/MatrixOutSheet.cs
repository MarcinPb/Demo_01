using System;

namespace WbEasyCalcRepository.Model
{
    public class MatrixOutSheet
    {
        private readonly EasyCalcSheet _data;

        public MatrixOutSheet(EasyCalcSheet data)
        {
            _data = data;
        }

        public string C11 { get => $"<= {_data.MatOneInSheet.C11}"; }
        public string C12 { get => $"{_data.MatOneInSheet.C11} - {_data.MatOneInSheet.C12}"; }
        public string C13 { get => $"{_data.MatOneInSheet.C12} - {_data.MatOneInSheet.C13}"; }
        public string C14 { get => $"{_data.MatOneInSheet.C13} - {_data.MatOneInSheet.C14}"; } 
        public string C15 { get => $" > {_data.MatOneInSheet.C14}"; } 
        public string C21 { get => $"<= {_data.MatOneInSheet.C21}"; }
        public string C22 { get => $"{_data.MatOneInSheet.C21} - {_data.MatOneInSheet.C22}"; }
        public string C23 { get => $"{_data.MatOneInSheet.C22} - {_data.MatOneInSheet.C23}"; }
        public string C24 { get => $"{_data.MatOneInSheet.C23} - {_data.MatOneInSheet.C24}"; } 
        public string C25 { get => $" > {_data.MatOneInSheet.C24}"; } 


        public string D21 { get => $"<= {_data.MatOneInSheet.D21}"; }
        public string D22 { get => $"{_data.MatOneInSheet.D21} - {_data.MatOneInSheet.D22}"; }
        public string D23 { get => $"{_data.MatOneInSheet.D22} - {_data.MatOneInSheet.D23}"; }
        public string D24 { get => $"{_data.MatOneInSheet.D23} - {_data.MatOneInSheet.D24}"; } 
        public string D25 { get => $" > {_data.MatOneInSheet.D24}"; } 

        public string E11 { get => $"<= {_data.MatOneInSheet.E11}"; }
        public string E12 { get => $"{_data.MatOneInSheet.E11} - {_data.MatOneInSheet.E12}"; }
        public string E13 { get => $"{_data.MatOneInSheet.E12} - {_data.MatOneInSheet.E13}"; }
        public string E14 { get => $"{_data.MatOneInSheet.E13} - {_data.MatOneInSheet.E14}"; } 
        public string E15 { get => $" > {_data.MatOneInSheet.E14}"; } 
        public string E21 { get => $"<= {_data.MatOneInSheet.E21}"; }
        public string E22 { get => $"{_data.MatOneInSheet.E21} - {_data.MatOneInSheet.E22}"; }
        public string E23 { get => $"{_data.MatOneInSheet.E22} - {_data.MatOneInSheet.E23}"; }
        public string E24 { get => $"{_data.MatOneInSheet.E23} - {_data.MatOneInSheet.E24}"; } 
        public string E25 { get => $" > {_data.MatOneInSheet.E24}"; } 

        public string F11 { get => $"<= {_data.MatOneInSheet.F11}"; }
        public string F12 { get => $"{_data.MatOneInSheet.F11} - {_data.MatOneInSheet.F12}"; }
        public string F13 { get => $"{_data.MatOneInSheet.F12} - {_data.MatOneInSheet.F13}"; }
        public string F14 { get => $"{_data.MatOneInSheet.F13} - {_data.MatOneInSheet.F14}"; } 
        public string F15 { get => $" > {_data.MatOneInSheet.F14}"; } 
        public string F21 { get => $"<= {_data.MatOneInSheet.F21}"; }
        public string F22 { get => $"{_data.MatOneInSheet.F21} - {_data.MatOneInSheet.F22}"; }
        public string F23 { get => $"{_data.MatOneInSheet.F22} - {_data.MatOneInSheet.F23}"; }
        public string F24 { get => $"{_data.MatOneInSheet.F23} - {_data.MatOneInSheet.F24}"; } 
        public string F25 { get => $" > {_data.MatOneInSheet.F24}"; } 

        public string G11 { get => $"<= {_data.MatOneInSheet.G11}"; }
        public string G12 { get => $"{_data.MatOneInSheet.G11} - {_data.MatOneInSheet.G12}"; }
        public string G13 { get => $"{_data.MatOneInSheet.G12} - {_data.MatOneInSheet.G13}"; }
        public string G14 { get => $"{_data.MatOneInSheet.G13} - {_data.MatOneInSheet.G14}"; } 
        public string G15 { get => $" > {_data.MatOneInSheet.G14}"; } 
        public string G21 { get => $"<= {_data.MatOneInSheet.G21}"; }
        public string G22 { get => $"{_data.MatOneInSheet.G21} - {_data.MatOneInSheet.G22}"; }
        public string G23 { get => $"{_data.MatOneInSheet.G22} - {_data.MatOneInSheet.G23}"; }
        public string G24 { get => $"{_data.MatOneInSheet.G23} - {_data.MatOneInSheet.G24}"; } 
        public string G25 { get => $" > {_data.MatOneInSheet.G24}"; } 

        public string H11 { get => $"<= {_data.MatOneInSheet.H11}"; }
        public string H12 { get => $"{_data.MatOneInSheet.H11} - {_data.MatOneInSheet.H12}"; }
        public string H13 { get => $"{_data.MatOneInSheet.H12} - {_data.MatOneInSheet.H13}"; }
        public string H14 { get => $"{_data.MatOneInSheet.H13} - {_data.MatOneInSheet.H14}"; } 
        public string H15 { get => $" > {_data.MatOneInSheet.H14}"; } 
        public string H21 { get => $"<= {_data.MatOneInSheet.H21}"; }
        public string H22 { get => $"{_data.MatOneInSheet.H21} - {_data.MatOneInSheet.H22}"; }
        public string H23 { get => $"{_data.MatOneInSheet.H22} - {_data.MatOneInSheet.H23}"; }
        public string H24 { get => $"{_data.MatOneInSheet.H23} - {_data.MatOneInSheet.H24}"; } 
        public string H25 { get => $" > {_data.MatOneInSheet.H24}"; } 
    }
}
