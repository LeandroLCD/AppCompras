using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.CurrentActivity;
using Android.Views;
using AppCompras.Controls;
using Xamarin.Forms;
using AppCompras.Droid.Implementation;

namespace AppCompras.Droid
{
    [Activity(Label = "AppCompras", 
        Icon = "@mipmap/icon", 
        Theme = "@style/MainTheme",
        WindowSoftInputMode = SoftInput.StateAlwaysVisible,
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize,
        ScreenOrientation = ScreenOrientation.Portrait)]
   
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        public bool DefaultFocusHighlightEnabled
        {
            [Android.Runtime.Register("getDefaultFocusHighlightEnabled", "()Z", "", ApiSince = 26)]
            get; [Android.Runtime.Register("setDefaultFocusHighlightEnabled", "(Z)V", "GetSetDefaultFocusHighlightEnabled_ZHandler", ApiSince = 26)]
            set;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
          
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}