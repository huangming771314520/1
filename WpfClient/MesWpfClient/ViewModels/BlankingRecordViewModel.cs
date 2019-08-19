using MesWpfClient.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MesWpfClient.ViewModels
{
    public class BlankingRecordViewModel : ViewModelBase
    {
        private BLL.ExtraBLL.BlankingRecordBLL blankingRecordBLL = new BLL.ExtraBLL.BlankingRecordBLL();

        public BlankingRecordViewModel()
        {
            Task.Run(() =>
            {
                LeftData = GetLeftData();

                ICode = @"03010102000720";
                BCode = @"NewBatchCode";

            });

            ToRightCommand = new DelegateCommand(obj => ToRight(obj));

            ToLeftCommand = new DelegateCommand(obj => ToLeft(obj));

            InputCodeCommand = new DelegateCommand(obj => InputCode(obj));
        }

        private ObservableCollection<Models.BlankingRecordLeftModel> leftData = new ObservableCollection<Models.BlankingRecordLeftModel>();

        public ObservableCollection<Models.BlankingRecordLeftModel> LeftData
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


        private ObservableCollection<Models.BlankingRecordRightModel> rightData = new ObservableCollection<Models.BlankingRecordRightModel>();

        public ObservableCollection<Models.BlankingRecordRightModel> RightData
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

        public DelegateCommand InputCodeCommand { get; set; }

        public void ToRight(object obj)
        {
            if (LeftIndex.Equals(-1))
            {
                return;
            }

            int index = LeftIndex;
            Models.BlankingRecordLeftModel model = LeftData[LeftIndex];

            LeftData.RemoveAt(LeftIndex);

            if (!LeftData.Count.Equals(0))
            {
                LeftIndex = index >= LeftData.Count ? (LeftData.Count - 1) : index;
            }

            blankingRecordBLL.GetUpdateBlankingRecordLeftResult(ICode, BCode, model.ID);

            RightData.Add(new Models.BlankingRecordRightModel()
            {
                ID = model.ID,
                BiankingSize = model.BiankingSize
            });

            UpdateLineNum();
        }

        public void ToLeft(object obj)
        {
            if (RightIndex.Equals(-1))
            {
                return;
            }

            int index = RightIndex;
            Models.BlankingRecordRightModel model = RightData[RightIndex];

            RightData.RemoveAt(RightIndex);

            if (!RightData.Count.Equals(0))
            {
                RightIndex = index >= RightData.Count ? (RightData.Count - 1) : index;
            }

            blankingRecordBLL.GetUpdateBlankingRecordRightResult(ICode, BCode, model.ID);
            LeftData = GetLeftData();

            UpdateLineNum();
        }


        public void InputCode(object obj)
        {
            RightData = GetRightData();
        }







        public ObservableCollection<Models.BlankingRecordLeftModel> GetLeftData()
        {
            ObservableCollection<Models.BlankingRecordLeftModel> result = new ObservableCollection<Models.BlankingRecordLeftModel>();
            var temp = blankingRecordBLL.GetBlankingRecordLeftData().Data;

            for (int i = 0; i < temp.Count; i++)
            {
                result.Add(new Models.BlankingRecordLeftModel()
                {
                    ID = temp[i].ID,
                    PartName = temp[i].PartName,
                    FigureCode = temp[i].FigureCode,
                    ContractCode = temp[i].ContractCode,
                    BiankingSize = temp[i].BiankingSize,
                    BlankingSize = temp[i].BlankingSize,
                    LineNum = i + 1
                });
            }

            return result;
        }

        public ObservableCollection<Models.BlankingRecordRightModel> GetRightData()
        {
            ObservableCollection<Models.BlankingRecordRightModel> result = new ObservableCollection<Models.BlankingRecordRightModel>();

            if (string.IsNullOrEmpty(ICode) || string.IsNullOrEmpty(BCode))
            {
                return result;
            }

            var temp = blankingRecordBLL.GetBlankingRecordRightData(ICode, BCode).Data;

            for (int i = 0; i < temp.Count; i++)
            {
                result.Add(new Models.BlankingRecordRightModel()
                {
                    ID = temp[i].ID,
                    BiankingSize = temp[i].BiankingSize,
                    LineNum = i + 1
                });
            }

            return result;
        }

        public void UpdateLineNum()
        {
            for (int i = 0; i < LeftData.Count; i++)
            {
                LeftData[i].LineNum = i + 1;
            }

            for (int i = 0; i < RightData.Count; i++)
            {
                RightData[i].LineNum = i + 1;
            }
        }

    }
}
