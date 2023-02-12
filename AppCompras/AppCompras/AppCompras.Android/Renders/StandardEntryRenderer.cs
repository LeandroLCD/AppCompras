using Android.Content;
using Android.Graphics.Drawables;
using AppCompras.Controls;
using AppCompras.Droid.Renders;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(StandardEntry), typeof(StandardEntryRenderer))]

namespace AppCompras.Droid.Renders
{

    public class StandardEntryRenderer : EntryRenderer
    {
        private Context _context;
        public StandardEntryRenderer(Context context) : base(context)
        {
            _context = context;
        }

        public StandardEntry ElementV2 => Element as StandardEntry;
        protected override FormsEditText CreateNativeControl()
        {

            var control = base.CreateNativeControl();
            UpdateBackground(control);
            control.Focusable = false;
            control.FocusableInTouchMode = false;
            return control;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == StandardEntry.CornerRadiusProperty.PropertyName)
            {
                UpdateBackground(Control);
            }
            else if (e.PropertyName == StandardEntry.BorderThicknessProperty.PropertyName)
            {
                UpdateBackground(Control);
            }
            else if (e.PropertyName == StandardEntry.BorderColorProperty.PropertyName)
            {
                UpdateBackground(Control);
            }

            base.OnElementPropertyChanged(sender, e);
        }

        protected override void UpdateBackgroundColor()
        {
            UpdateBackground(Control);


        }
        protected void UpdateBackground(FormsEditText control)
        {
            if (control == null) return;

            var gd = new GradientDrawable();
            gd.SetColor(Element.BackgroundColor.ToAndroid());
            gd.SetCornerRadius(Context.ToPixels(ElementV2.CornerRadius));
            gd.SetStroke((int)Context.ToPixels(ElementV2.BorderThickness), ElementV2.BorderColor.ToAndroid());
            control.SetBackground(gd);

            var padTop = (int)Context.ToPixels(ElementV2.Padding.Top);
            var padBottom = (int)Context.ToPixels(ElementV2.Padding.Bottom);
            var padLeft = (int)Context.ToPixels(ElementV2.Padding.Left);
            var padRight = (int)Context.ToPixels(ElementV2.Padding.Right);

            control.SetPadding(padLeft, padTop, padRight, padBottom);
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            // this.EditText.TextChanged += EditText_TextChanged;
            //this.EditText.FocusChange += EditText_FocusChange;
            //this.EditText.Touch += EditText_Touch;
            //this.EditText.Click += EditText_Click;



        }


    }
}