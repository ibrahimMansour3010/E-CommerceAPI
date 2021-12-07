using Models.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProductImage
{
    public class GetEditProductImageViewModel
    {
        public string ID { get; set; }
        public string ImageURL { get; set; }
        public string ProductID { get; set; }
    }
    public static class GetEditProductImageViewModelExtensions
    {
        public static GetEditProductImageViewModel ToViewModel(this ProductImageEntity prouctImagEntity)
        {
            GetEditProductImageViewModel vm = new GetEditProductImageViewModel();
            vm.ImageURL = prouctImagEntity.ImageURL;
            vm.ProductID = prouctImagEntity.ProductID;
            vm.ID = prouctImagEntity.ID;
            return vm;
        }
    }
}
