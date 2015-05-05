using System;

using Xamarin.Forms;

namespace GP3
{
	public class App : Application
	{
		public App ()
		{
			var isLoggedIn = Application.Current.Properties.ContainsKey ("IsLoggedIn")?(bool)Application.Current.Properties ["IsLoggedIn"] : false;

			if (isLoggedIn) {
				var MP = new NavigationPage (new GamesPage ());
				MP.BarBackgroundColor = Color.FromHex (Constants.Green);
				MP.BarTextColor = Color.FromHex (Constants.White);
				MainPage = MP;
			}
			else {
				var MP = new NavigationPage (new LoginPage ());
				MP.BarBackgroundColor = Color.FromHex (Constants.Green);
				MP.BarTextColor = Color.FromHex (Constants.White);
				MainPage = MP;
			}
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

