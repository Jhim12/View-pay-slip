using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using View_pay_slip.Pages;
using Xamarin.Forms;

namespace View_pay_slip
{
    internal class UserInterface
    {
        public void removePageControls(ContentPage page)
        {
            //code to remove the arrow back button in navigation page
            NavigationPage.SetHasBackButton(page, false);
            NavigationPage.SetHasNavigationBar(page, false);
        }

        public void reusePassMethod(CheckBox cbPassName, Entry entryPassName)
        {
            if (cbPassName.IsChecked == true)
            {
                entryPassName.IsPassword = false;
            }
            else
            {
                entryPassName.IsPassword = true;
            }
        }


        // Previous and logout prompt
        public async Task<bool> backAndLogout(Page page, string title, string message)
        {
            bool yesOrNo = await page.DisplayAlert(title, message, "Yes", "No");

            if (yesOrNo)
            {
                // Navigate back to the previous page
                await page.Navigation.PopAsync();
            }

            return yesOrNo;
        }

    }
}
