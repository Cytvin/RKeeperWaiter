using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WaiterMobile.Controls;
using Android.Content;
using Android.Graphics.Drawables;
using WaiterMobile.Droid.Renderers;

[assembly: ExportRenderer(typeof(StandartEntry), typeof(StandartEntryRenderer))]
namespace WaiterMobile.Droid.Renderers
{
    public class StandartEntryRenderer : EntryRenderer
    {
        public StandartEntryRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            }
        }
    }
}