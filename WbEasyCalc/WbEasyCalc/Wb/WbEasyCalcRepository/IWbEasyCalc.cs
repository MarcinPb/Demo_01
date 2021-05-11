using WbEasyCalcModel;

namespace WbEasyCalcRepository
{
    public interface IWbEasyCalc
    {
        EasyCalcDataOutput Calculate(EasyCalcDataInput easyCalcDataInput);
    }
}
