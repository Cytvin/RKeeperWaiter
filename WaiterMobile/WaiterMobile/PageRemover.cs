using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WaiterMobile
{
    public class PageRemover
    {
        public static void Remove(int pageCount)
        {
            while (pageCount > 0)
            {
                IReadOnlyList<Page> pages = Shell.Current.Navigation.NavigationStack;
                Page previousPage = Shell.Current.Navigation.NavigationStack[pages.Count - 1];
                Shell.Current.Navigation.RemovePage(previousPage);

                pageCount--;
            }
        }
    }
}
