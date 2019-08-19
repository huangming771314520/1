using PreviewClient.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Serialization;

namespace PreviewClient.ViewModels
{
    public class BlankingRecordViewModel : ViewModelBase
    {
        private BLL.ExtraBLL.BlankingRecordBLL blankingRecordBLL = new BLL.ExtraBLL.BlankingRecordBLL();

        public BlankingRecordViewModel()
        {
            LeftData = ConvertToObservable(blankingRecordBLL.GetBlankingRecordLeftData().Data);

            ICode = @"03010102000720";

            BCode = @"NewBatchCode";

            ToRightCommand = new DelegateCommand(obj => ToRight(obj));

            ToLeftCommand = new DelegateCommand(obj => ToLeft(obj));

            //InputICodeCommand = new DelegateCommand(obj => InputICode(obj));

            //InputBCodeCommand = new DelegateCommand(obj => InputBCode(obj));

            InputCodeCommand = new DelegateCommand(obj => InputCode(obj));

        }


        private ObservableCollection<Model.BlankingRecordLeftModel.DataModel> leftData = new ObservableCollection<Model.BlankingRecordLeftModel.DataModel>();

        public ObservableCollection<Model.BlankingRecordLeftModel.DataModel> LeftData
        {
            get
            {
                return leftData;
            }
            set
            {
                leftData = value;
                OnPropertyChanged(nameof(LeftData));
            }
        }


        private int leftIndex = -1;

        public int LeftIndex
        {
            get
            {
                return leftIndex;
            }
            set
            {
                leftIndex = value;
                OnPropertyChanged(nameof(LeftIndex));
            }
        }


        private ObservableCollection<Model.BlankingRecordRightModel.DataModel> rightData = new ObservableCollection<Model.BlankingRecordRightModel.DataModel>();

        public ObservableCollection<Model.BlankingRecordRightModel.DataModel> RightData
        {
            get
            {
                return rightData;
            }
            set
            {
                rightData = value;
                OnPropertyChanged(nameof(RightData));
            }
        }


        private int rightIndex = -1;

        public int RightIndex
        {
            get
            {
                return rightIndex;
            }
            set
            {
                rightIndex = value;
                OnPropertyChanged(nameof(RightIndex));
            }
        }


        private string iCode;

        public string ICode
        {
            get
            {
                return iCode;
            }
            set
            {
                iCode = value;
                OnPropertyChanged(nameof(ICode));
            }
        }


        private string bCode;

        public string BCode
        {
            get
            {
                return bCode;
            }
            set
            {
                bCode = value;
                OnPropertyChanged(nameof(BCode));
            }
        }



        public DelegateCommand ToRightCommand { get; set; }

        public DelegateCommand ToLeftCommand { get; set; }


        public DelegateCommand InputICodeCommand { get; set; }

        public DelegateCommand InputBCodeCommand { get; set; }

        public DelegateCommand InputCodeCommand { get; set; }

        public void ToRight(object obj)
        {
            if (LeftIndex.Equals(-1))
            {
                return;
            }

            int index = LeftIndex;
            Model.BlankingRecordLeftModel.DataModel model = LeftData[LeftIndex];

            LeftData.RemoveAt(LeftIndex);

            if (!LeftData.Count.Equals(0))
            {
                LeftIndex = index >= LeftData.Count ? (LeftData.Count - 1) : index;
            }

            blankingRecordBLL.GetUpdateBlankingRecordLeftResult(ICode, BCode, model.ID);

            RightData.Add(new Model.BlankingRecordRightModel.DataModel()
            {
                ID = model.ID,
                BiankingSize = model.BiankingSize
            });

        }

        public void ToLeft(object obj)
        {
            if (RightIndex.Equals(-1))
            {
                return;
            }

            int index = RightIndex;
            Model.BlankingRecordRightModel.DataModel model = RightData[RightIndex];

            RightData.RemoveAt(RightIndex);

            if (!RightData.Count.Equals(0))
            {
                RightIndex = index >= RightData.Count ? (RightData.Count - 1) : index;
            }

            blankingRecordBLL.GetUpdateBlankingRecordRightResult(ICode, BCode, model.ID);
            LeftData = ConvertToObservable(blankingRecordBLL.GetBlankingRecordLeftData().Data);
        }


        public void InputICode(object obj)
        {

        }

        public void InputBCode(object obj)
        {

        }

        public void InputCode(object obj)
        {
            if (string.IsNullOrEmpty(ICode) || string.IsNullOrEmpty(BCode))
            {
                return;
            }

            RightData = ConvertToObservable(blankingRecordBLL.GetBlankingRecordRightData(ICode, BCode).Data);
        }

        public ObservableCollection<T> ConvertToObservable<T>(List<T> list) where T : class
        {
            ObservableCollection<T> result = new ObservableCollection<T>();
            list.ForEach(item => result.Add(item));
            return result;
        }

    }
}