using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LearnSchool
{
    public partial class Service
    {
        public decimal CostDiscount
        {
            get => (Discount == 0 || Discount == null) ? Cost : Cost - Cost * (decimal)Discount / 100;
        }
        public string StrDiscount
        {
            get => (Discount == 0 || Discount == null) ? "" : $"* скидка {Discount} %";
        }
        public string CostDuration
        {
            get => (Discount == 0 || Discount == null) ? $"{Cost:F} рублей за  {DurationInSeconds / 60} минут" :
                    $" {(double)Cost - (double)Cost * (double)Discount / 100:F} рублей за {DurationInSeconds / 60} минут";
        }
        public Visibility BtnVisible
        {
            get => (MainWindow.User.RoleId == 2) ? Visibility.Collapsed : Visibility.Visible;
        }
        public Visibility DiscountVisability
        {
            get => (Discount == 0 || Discount == null) ? Visibility.Collapsed : Visibility.Visible;
        }
        public string ColorDis
        {
            get => (Discount == 0 || Discount == null) ? "#ffffff" : "#D1FFD1";
        }
    }
}
