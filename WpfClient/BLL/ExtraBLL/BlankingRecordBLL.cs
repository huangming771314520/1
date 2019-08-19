using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ExtraBLL
{
    public class BlankingRecordBLL
    {
        public BlankingRecordLeftModel GetBlankingRecordLeftData()
        {
            try
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetBlankingRecordLeftData?fCode={1}", ConfigInfoModel.API, StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.FigureCode);
                var result = Helpers.HttpHelper.GetTByUrl<BlankingRecordLeftModel>(url);
                return result;
            }
            catch (Exception ex)
            {
                return new BlankingRecordLeftModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        public BlankingRecordRightModel GetBlankingRecordRightData(string iCode, string bNumber)
        {
            try
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetBlankingRecordRightData?iCode={1}&bNumber={2}", ConfigInfoModel.API, iCode, bNumber);
                var result = Helpers.HttpHelper.GetTByUrl<BlankingRecordRightModel>(url);
                return result;
            }
            catch (Exception ex)
            {
                return new BlankingRecordRightModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }


        public ResultModel GetUpdateBlankingRecordLeftResult(string iCode, string bNumber, long id)
        {
            try
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetUpdateBlankingRecordLeftData?iCode={1}&bNumber={2}&id={3}", ConfigInfoModel.API, iCode, bNumber, id);
                var result = Helpers.HttpHelper.GetTByUrl<ResultModel>(url);
                return result;
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        public ResultModel GetUpdateBlankingRecordRightResult(string iCode, string bNumber, long id)
        {
            try
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetUpdateBlankingRecordRightData?iCode={1}&bNumber={2}&id={3}", ConfigInfoModel.API, iCode, bNumber, id);
                var result = Helpers.HttpHelper.GetTByUrl<ResultModel>(url);
                return result;
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

    }
}
