using System;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;

namespace WbEasyCalcModel
{
    public class EasyCalcModel : ICloneable
    {
        public StartModel StartModel { get; set; } = new StartModel();
        public SysInputModel SysInputModel { get; set; }
        public BilledConsModel BilledConsModel { get; set; }

        public UnbilledConsModel UnbilledConsModel { get; set; }
        public UnauthConsModel UnauthConsModel { get; set; }
        public MetErrorsModel MetErrorsModel { get; set; }
        public NetworkModel NetworkModel { get; set; }
        public PressureModel PressureModel { get; set; }
        public IntermModel IntermModel { get; set; } = new IntermModel();
        public FinancDataModel FinancDataModel { get; set; } = new FinancDataModel();

        public WaterBalanceModel WaterBalanceDay { get; set; } = new WaterBalanceModel();
        public WaterBalanceModel WaterBalancePeriod { get; set; } = new WaterBalanceModel();
        public WaterBalanceModel WaterBalanceYear { get; set; } = new WaterBalanceModel();
        public PisModel Pis { get; set; } = new PisModel();
        public MatrixOneInModel MatrixOneIn { get; set; } = new MatrixOneInModel();
        public MatrixOneInModel MatrixTwoIn { get; set; } = new MatrixOneInModel();
        public MatrixOneOutModel MatrixOneOut { get; set; } = new MatrixOneOutModel();
        public MatrixOneOutModel MatrixTwoOut { get; set; } = new MatrixOneOutModel();


        public object Clone()
        {
            return new EasyCalcModel()
            {
                StartModel = (StartModel)StartModel.Clone(),
                SysInputModel = (SysInputModel)SysInputModel.Clone(),
                BilledConsModel = (BilledConsModel)BilledConsModel.Clone(),
                UnbilledConsModel = (UnbilledConsModel)UnbilledConsModel.Clone(),
                UnauthConsModel = (UnauthConsModel)UnauthConsModel.Clone(),
                MetErrorsModel = (MetErrorsModel)MetErrorsModel.Clone(),
                NetworkModel = (NetworkModel)NetworkModel.Clone(),
                PressureModel = (PressureModel)PressureModel.Clone(),
                IntermModel = (IntermModel)IntermModel.Clone(),
                FinancDataModel = (FinancDataModel)FinancDataModel.Clone(),
                WaterBalanceDay = (WaterBalanceModel)WaterBalanceDay.Clone(),
                WaterBalancePeriod = (WaterBalanceModel)WaterBalancePeriod.Clone(),
                WaterBalanceYear = (WaterBalanceModel)WaterBalanceYear.Clone(),
                Pis = (PisModel)Pis.Clone(),
                MatrixOneIn = (MatrixOneInModel)MatrixOneIn.Clone(),
                MatrixTwoIn = (MatrixOneInModel)MatrixTwoIn.Clone(),
                MatrixOneOut = (MatrixOneOutModel)MatrixOneOut.Clone(),
                MatrixTwoOut = (MatrixOneOutModel)MatrixTwoOut.Clone(),
            };
        }
    }
}
