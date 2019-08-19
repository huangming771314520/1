namespace MesClient.Entitys
{
    public class UserDetailInfoEntity
    {
        public string Token { get; set; }

        public string BarCode { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

        public ExtraInfoModel ExtraInfo { get; set; }

        public class ExtraInfoModel
        {
            public string TeamCode { get; set; }

            public string TeamName { get; set; }

            public string DepartID { get; set; }

            public string WarehouseCode { get; set; }

            public string WarehouseName { get; set; }
        }

    }

    public class UserInfoEntity
    {
        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string BarCode { get; set; }
    }
}
