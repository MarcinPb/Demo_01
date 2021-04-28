using System;

namespace WbEasyCalcRepository.Model
{
    public class MatOneOutSheet
    {
        private readonly EasyCalcSheet _data;

        public MatOneOutSheet(EasyCalcSheet data)
        {
            _data = data;
        }

        public string C11 { get => $"{_data.MatOneInSheet.C11.ToString()} < "; }
 
    }
}
