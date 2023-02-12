using Android.App;
using Android.Widget;
using AppCompras.Droid.Implementation;
using AppCompras.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]
namespace AppCompras.Droid.Implementation
{
    public class ToastMessage : IToast
    {
        public void Show(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }
    }
}