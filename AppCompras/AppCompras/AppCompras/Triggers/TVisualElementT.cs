using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AppCompras.Triggers
{
    public partial class TVisualElementT : TriggerAction<VisualElement>
    {
        public bool Activate { get; set; }

        public int TranslateX { get; set; }
        public int TranslateY { get; set; }
        protected override async void Invoke(VisualElement sender)
        {
            if (Activate)
            {
                await sender.TranslateTo(TranslateX, TranslateY, 300, Easing.Linear);
            }
            else
            {

                await sender.TranslateTo(TranslateX, TranslateY, 300, Easing.Linear);
            }
        }
        double CustomEase(double t)
        {
            return t == 0 || t == 1 ? t : (int)(5 * t) / 5.0;
        }
    }
}
