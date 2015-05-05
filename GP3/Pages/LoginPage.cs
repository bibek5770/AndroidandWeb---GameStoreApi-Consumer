using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

namespace GP3
{
	public class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			//BackgroundColor = Color.FromHex(Constants.Green);
			BackgroundImage = "bg.jpg";

			var layout = new StackLayout { Padding = 30 };
			NavigationPage.SetHasNavigationBar (this, false);

			var logo = new Label
			{
				Text = "GP3",
				FontSize = 50,
				TextColor = Color.White,
				FontAttributes = FontAttributes.Bold,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center
			};

			layout.Children.Add(logo);

			var usernameContainer = new StackLayout{ Padding = new Thickness(0,15) };
			var username = new Entry { Placeholder = "Username", TextColor = Color.FromHex(Constants.Dark), VerticalOptions= LayoutOptions.Center, BackgroundColor=Color.FromHex("#ffffff") };
			username.SetBinding(Entry.TextProperty, "Username");
			usernameContainer.Children.Add (username);
			layout.Children.Add(usernameContainer);

			var passwordContainer = new StackLayout{ };
			var password = new Entry { 
				Placeholder = "Password", 
				IsPassword = true, 
				TextColor = Color.FromHex(Constants.Dark), 
				VerticalOptions= LayoutOptions.Center, 
				BackgroundColor=Color.FromHex("#ffffff")
			};
			password.SetBinding(Entry.TextProperty, "Password");
			passwordContainer.Children.Add(password);
			layout.Children.Add (passwordContainer);

			var button = new Button { Text = "Sign In", TextColor = Color.White, BackgroundColor = Color.FromHex(Constants.Dark), 
			VerticalOptions= LayoutOptions.CenterAndExpand, FontSize = 25, FontAttributes = FontAttributes.Bold};
			button.SetBinding(Button.CommandProperty, "Login");

			layout.Children.Add(button);

			button.Clicked += async (sender, e) => {

				if(username.Text.Length > 0 && password.Text.Length > 0){
					LoginResultVM User = new LoginResultVM();
					User = Login(username.Text, password.Text);

					//If Logged In
					if(User.isValid()){
						//Save Login Credentials
						Application.Current.Properties["ApiKey"] = User.ApiKey;
						Application.Current.Properties["UserId"] = User.UserId;
						Application.Current.Properties["IsLoggedIn"] = true;

						Navigation.InsertPageBefore(new GamesPage(), Navigation.NavigationStack[0]);
						await Navigation.PopToRootAsync();
					}
					else{
						await DisplayAlert ("Login Details Incorrect", "Username/password incorrect. Please try again.", "OK");
					}
				}
				else{
					await DisplayAlert("Username/Password Error", "Username and Password fields are required.", "OK");
				}
			};
			Content = new ScrollView { Content = layout };
		}

		public LoginResultVM Login(string username, string password)
		{
			var client = new RestClient (Constants.ApiURL);
			var request = new RestRequest ("Apikey",Method.GET);
			request.AddParameter ("email",username);
			request.AddParameter ("password",password);

			var User = new LoginResultVM ();
			var response = client.Execute(request);

			if (response.StatusCode == HttpStatusCode.OK) {
				User = JsonConvert.DeserializeObject<LoginResultVM>(response.Content);
			} 
			return User;
		}
	}
}