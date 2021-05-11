using System;

namespace WbEasyCalcRepository.Model
{
    public class MatrixOutSheet
    {
        private readonly MatrixInSheet _data;

        public MatrixOutSheet(MatrixInSheet data)
        {
            _data = data;
        }

        public string C11 { get => $"<= {_data.C11}"; }
        public string C12 { get => $"{_data.C11} - {_data.C12}"; }
        public string C13 { get => $"{_data.C12} - {_data.C13}"; }
        public string C14 { get => $"{_data.C13} - {_data.C14}"; } 
        public string C15 { get => $" > {_data.C14}"; } 
        public string C21 { get => $"<= {_data.C21}"; }
        public string C22 { get => $"{_data.C21} - {_data.C22}"; }
        public string C23 { get => $"{_data.C22} - {_data.C23}"; }
        public string C24 { get => $"{_data.C23} - {_data.C24}"; } 
        public string C25 { get => $" > {_data.C24}"; } 


        public string D21 { get => $"<= {_data.D21}"; }
        public string D22 { get => $"{_data.D21} - {_data.D22}"; }
        public string D23 { get => $"{_data.D22} - {_data.D23}"; }
        public string D24 { get => $"{_data.D23} - {_data.D24}"; } 
        public string D25 { get => $" > {_data.D24}"; } 

        public string E11 { get => $"<= {_data.E11}"; }
        public string E12 { get => $"{_data.E11} - {_data.E12}"; }
        public string E13 { get => $"{_data.E12} - {_data.E13}"; }
        public string E14 { get => $"{_data.E13} - {_data.E14}"; } 
        public string E15 { get => $" > {_data.E14}"; } 
        public string E21 { get => $"<= {_data.E21}"; }
        public string E22 { get => $"{_data.E21} - {_data.E22}"; }
        public string E23 { get => $"{_data.E22} - {_data.E23}"; }
        public string E24 { get => $"{_data.E23} - {_data.E24}"; } 
        public string E25 { get => $" > {_data.E24}"; } 

        public string F11 { get => $"<= {_data.F11}"; }
        public string F12 { get => $"{_data.F11} - {_data.F12}"; }
        public string F13 { get => $"{_data.F12} - {_data.F13}"; }
        public string F14 { get => $"{_data.F13} - {_data.F14}"; } 
        public string F15 { get => $" > {_data.F14}"; } 
        public string F21 { get => $"<= {_data.F21}"; }
        public string F22 { get => $"{_data.F21} - {_data.F22}"; }
        public string F23 { get => $"{_data.F22} - {_data.F23}"; }
        public string F24 { get => $"{_data.F23} - {_data.F24}"; } 
        public string F25 { get => $" > {_data.F24}"; } 

        public string G11 { get => $"<= {_data.G11}"; }
        public string G12 { get => $"{_data.G11} - {_data.G12}"; }
        public string G13 { get => $"{_data.G12} - {_data.G13}"; }
        public string G14 { get => $"{_data.G13} - {_data.G14}"; } 
        public string G15 { get => $" > {_data.G14}"; } 
        public string G21 { get => $"<= {_data.G21}"; }
        public string G22 { get => $"{_data.G21} - {_data.G22}"; }
        public string G23 { get => $"{_data.G22} - {_data.G23}"; }
        public string G24 { get => $"{_data.G23} - {_data.G24}"; } 
        public string G25 { get => $" > {_data.G24}"; } 

        public string H11 { get => $"<= {_data.H11}"; }
        public string H12 { get => $"{_data.H11} - {_data.H12}"; }
        public string H13 { get => $"{_data.H12} - {_data.H13}"; }
        public string H14 { get => $"{_data.H13} - {_data.H14}"; } 
        public string H15 { get => $" > {_data.H14}"; } 
        public string H21 { get => $"<= {_data.H21}"; }
        public string H22 { get => $"{_data.H21} - {_data.H22}"; }
        public string H23 { get => $"{_data.H22} - {_data.H23}"; }
        public string H24 { get => $"{_data.H23} - {_data.H24}"; } 
        public string H25 { get => $" > {_data.H24}"; } 
    }
}
