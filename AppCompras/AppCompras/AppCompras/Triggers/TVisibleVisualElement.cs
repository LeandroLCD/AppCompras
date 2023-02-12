using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCompras.Triggers
{
    public partial class TVisibleVisualElement : TriggerAction<VisualElement>
    {
        public bool Activate { get; set; }
        protected override async void Invoke(VisualElement sender)
        {

            if(Activate) 
            {
                sender.IsVisible = true;
                await sender.FadeTo(1, 500, Easing.Linear);
            }
            else
            {
                await sender.FadeTo(1, 1000, Easing.Linear);
                sender.IsVisible = false;

            }
        }
    }
}
