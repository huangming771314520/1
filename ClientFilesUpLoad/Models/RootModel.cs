using System.Collections.Generic;

namespace ClientFilesUpLoad.Models
{
    public class RootNewModel
    {
        public RootNewModel()
        {
            Project = new ProjectModel();

            Product = new List<ProductModel>();
        }

        public ProjectModel Project { get; set; }

        public List<ProductModel> Product { get; set; }

        public class ProjectModel
        {
            public string Projectid { get; set; }

            public string Projectname { get; set; }
        }

        public class ProductModel
        {
            public ProductModel()
            {
                Doc = new List<DocModel>();

                Part = new List<PartModel>();
            }

            public List<DocModel> Doc { get; set; }

            public List<PartModel> Part { get; set; }

            public string Code { get; set; }

            public string Code1 { get; set; }

            public string Version { get; set; }

            public string Name { get; set; }

            public string Quantity { get; set; }

            public string Material { get; set; }

            public string Sigweight { get; set; }

            public string TotWeight { get; set; }

            public string Remark { get; set; }

            public string FaHCode { get; set; }


            public class DocModel
            {
                public string Code { get; set; }

                public string Code1 { get; set; }

                public string Name { get; set; }

                public string Version { get; set; }

                public string Gcname { get; set; }

                public string Page { get; set; }

                public string Totpage { get; set; }

                public string Filename { get; set; }

            }

            public class PartModel
            {
                public PartModel()
                {
                    Doc = new List<DocModel>();

                    Part = new List<PartModel>();
                }

                public List<DocModel> Doc { get; set; }

                public List<PartModel> Part { get; set; }

                public string Code { get; set; }

                public string Code1 { get; set; }

                public string Version { get; set; }

                public string Name { get; set; }

                public string Quantity { get; set; }

                public string Material { get; set; }

                public string Sigweight { get; set; }

                public string TotWeight { get; set; }

                public string Remark { get; set; }

                public string FaHCode { get; set; }

                public string PartType { get; set; }
            }

        }
    }

    public class RootChangeModel
    {
        public RootChangeModel()
        {
            Doc = new List<DocModel>();
        }

        public FileModel ChangeQ { get; set; }

        public FileModel Change { get; set; }

        public List<DocModel> Doc { get; set; }

        public class FileModel
        {
            public string FileName { get; set; }
        }

        public class DocModel
        {
            public DocModel()
            {
                Part = new List<PartModel>();
            }

            public List<PartModel> Part { get; set; }

            public string Code { get; set; }

            public string Code1 { get; set; }

            public string Name { get; set; }

            public string Version { get; set; }

            public string Gcname { get; set; }

            public string Page { get; set; }

            public string Totpage { get; set; }

            public string Filename { get; set; }


            public class PartModel
            {
                public string Code { get; set; }

                public string Code1 { get; set; }

                public string Version { get; set; }

                public string Name { get; set; }

                public string Quantity { get; set; }

                public string Material { get; set; }

                public string Sigweight { get; set; }

                public string TotWeight { get; set; }

                public string Remark { get; set; }

                public string FaHCode { get; set; }

                public string PartType { get; set; }
            }

        }
    }

}
