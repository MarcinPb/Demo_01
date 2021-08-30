using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using AutoMapper;
using Database.DataModel;
using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using Database.DataRepository.WaterConsumption;
using GlobalRepository;
using Microsoft.WindowsAPICodePack.Dialogs;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WaterBalanceList.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.TableJunction
{
    public class EditedViewModel : ViewModelBase, IDialogViewModel, IDisposable
    {
        private RowViewModel _rowViewModel;
        public RowViewModel RowViewModel
        {
            get => _rowViewModel;
            set { _rowViewModel = value; RaisePropertyChanged(); }
        }

        public List<InfraDemandPattern> DemandPatternList => InfraRepo.GetInfraData().InfraChangeableData.DemandPatternDict;

        #region IDialogViewModel

        public string Title { get; set; } = "Junction";

        public bool Save()
        {
            try
            {
                var demandSettingObj = new DemandSettingObj()
                {
                    ObjId = RowViewModel.ObjModel.ObjId,
                    DemandBaseValue = RowViewModel.DemandBaseDmSet ?? 0,
                    DemandPatternId = RowViewModel.DemandPatternIdDmSet ?? -1,
                    IsExcluded = RowViewModel.IsExcluded,
                };

                InfraRepo.TableCustomerMeter.SaveItem(demandSettingObj);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public void Close()
        {
        }

        #endregion

        public EditedViewModel(RowViewModel rowViewModel)
        {
            RowViewModel = rowViewModel;
        }

        public void Dispose()
        {
            RowViewModel.Dispose();
        }
    }
}
