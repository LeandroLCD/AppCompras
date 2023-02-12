using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCompras.Triggers
{
    public partial class TBasketCounter : TriggerAction<VisualElement>
    {
        public bool Activate { get; set; }
        public int TranslateY { get; set; }
        protected override async void Invoke(VisualElement sender)
        {
            if (Activate)
            {
                await Task.WhenAll
                                (

                                sender.FadeTo(0, 500, Easing.BounceOut),
                                sender.TranslateTo(0, TranslateY, 500, Easing.Linear)
                                );
            }
            else
            {

                await Task.WhenAll
                               (

                                sender.FadeTo(1, 500, Easing.Linear),
                               sender.TranslateTo(0, TranslateY, 300, Easing.Linear)
                               );
            }
        }
    }
}
