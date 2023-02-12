using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace AppCompras.Triggers
{
    public partial class TPancakeView : TriggerAction<PancakeView>
    {
        public bool Activate { get; set; }
        protected override async void Invoke(PancakeView sender)
        {
            

            if(Activate)
            {
                await Task.WhenAll
                                (
                                sender.FadeTo(1, 500, Easing.Linear),
                                sender.TranslateTo(0,0,500,Easing.Linear)
                                ) ;
            }
            else
            {
                
                await Task.WhenAll
                               (
                               sender.FadeTo(0, 500, Easing.Linear),
                               sender.TranslateTo(0, 1000, 500, Easing.Linear)
                               );
            }
            
        }
    }
}
