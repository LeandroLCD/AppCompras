using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCompras.Triggers
{
    public partial class TLabel : TriggerAction<Label> 
    {
        public bool Activate { get; set; }
        public double TrasnlateY { get; set; }
        public double Scale { get; set; }

        protected override async void Invoke(Label sender)
        {
            if (Activate)
            {

                    var TrasnlateX = (sender.Width / 2) * (1 - Scale) * -1;
                await Task.WhenAll(
                                        sender.TranslateTo(TrasnlateX, TrasnlateY, 10, Easing.Linear),
                                         sender.ScaleTo(Scale, 10, Easing.Linear)
                                    );


            }
            else if (!Activate)
            {
                await Task.WhenAll
                     (
                     sender.TranslateTo(0, 0, 10, Easing.Linear),
                       sender.ScaleTo(1, 10, Easing.CubicOut)
                     );
            }
        }
    }
}
