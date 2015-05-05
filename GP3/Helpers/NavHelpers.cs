using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace GP3
{
	public static class NavHelpers
	{
		private static NavigationPage currentNavigationPage
		{
			get
			{
				return ((App)Application.Current).MainPage as NavigationPage;
			}
		}

		public static Page GetGamesPage(){
			return new NavigationPage(new GamesPage());
		}

		public async static Task ReplaceRoot(Page page)
		{
			var root = currentNavigationPage.Navigation.NavigationStack[0];
			currentNavigationPage.Navigation.InsertPageBefore(page, root);
			await PopToRootAsync();
		}

		private static async Task PopToRootAsync()
		{
			while (currentNavigationPage.Navigation.ModalStack.Count > 0)
			{
				await currentNavigationPage.Navigation.PopModalAsync(false);
			}
			while (currentNavigationPage.CurrentPage != currentNavigationPage.Navigation.NavigationStack[0])
			{
				await currentNavigationPage.PopAsync(false);
			}
		}
	}
}