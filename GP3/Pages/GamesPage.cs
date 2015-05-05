using System;
using Xamarin.Forms;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GP3
{
	public class GamesPage : ContentPage
	{
		public GamesPage ()
		{
			Title = "GP3";
			BackgroundColor = Color.FromHex(Constants.White);

			ToolbarItem Cart = new ToolbarItem {
				Icon = "shoppingcartsm.png",
			};

			ToolbarItem Logout = new ToolbarItem{ 
				Icon = "logoutsm.png"
			};

			StackLayout titlestack = new StackLayout {Padding = new Thickness(0,0,0,10)};

			Label title = new Label {
				Text= "Available Games",
				TextColor = Color.FromHex(Constants.White),
				FontSize = 25,
				BackgroundColor = Color.FromHex(Constants.Dark),
				FontAttributes = FontAttributes.Bold,
				YAlign = TextAlignment.Center,
				XAlign = TextAlignment.Center
			};

			titlestack.Children.Add (title);

			Logout.Clicked += (sender, e) => {
				//
				Application.Current.Properties ["IsLoggedIn"] = false;
				Application.Current.Properties.Remove ("UserId");
				Application.Current.Properties.Remove ("ApiKey");

				Navigation.InsertPageBefore(new LoginPage(), Navigation.NavigationStack[0]);
				Navigation.PopToRootAsync();
			};

			this.ToolbarItems.Add (Cart);
			this.ToolbarItems.Add (Logout);

			var gameList = new ListView {
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate (typeof(GameView)),
				ItemsSource = GameData.GetGameData(),
				SeparatorColor = Color.FromHex ("#ddd"),
			};

			var layout = new StackLayout {
				Children = {
					titlestack,
					gameList
				}
			};

			gameList.ItemSelected += (sender,e) => {
				//Clear the selected item
				if(e.SelectedItem == null) return;

				//Open up the Product Detail Page
				if(e.SelectedItem != null){
					Navigation.PushAsync(new GameDetailPage(e.SelectedItem as GameVM));
				}

				((ListView)sender).SelectedItem = null;
			};

			Content = layout;
		}
	}

	public class GameView : ViewCell
	{
		public GameView()
		{
			//Game Title
			var GameTitle = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 18,
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Constants.Dark)
			};
			GameTitle.SetBinding (Label.TextProperty, "GameName");

			//var imagesourceurl = IconUrl (GameTitle.Text);

			//ImagePlaceholder
			var imgsrcurl = randomUrl(); 
			if(String.IsNullOrEmpty(imgsrcurl)){
				imgsrcurl = "http://i.imgur.com/QdOsylp.png";
			}
			var imgsrc = new UriImageSource{ Uri = new Uri (imgsrcurl) };
			var ProductImage = new Image{ Source = imgsrc };

			//GamePrice
			var GamePrice = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 18,
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex("#909090"),
			};
			GamePrice.SetBinding (Label.TextProperty, new Binding("Price", stringFormat : "${0}") );

			var labelsrc = new Image ();
			labelsrc.HorizontalOptions = LayoutOptions.End;
			labelsrc.Source = ImageSource.FromFile ("viewbutton.png");

			//New Layout
			var ProductContainer = new StackLayout {
				Spacing = 0,
				Padding = new Thickness (5, 5, 5, 5),
				Orientation = StackOrientation.Horizontal,
			};

			var eachgrid = new Grid {
				ColumnSpacing = 10,
				RowDefinitions = new RowDefinitionCollection {
					new RowDefinition{}
				},
				ColumnDefinitions = new ColumnDefinitionCollection {
					new ColumnDefinition{ Width = new GridLength (70) },
					new ColumnDefinition{ }
				}
			};
				
			var viewLabel = new Label {
				Text = "View",
				TextColor = Color.FromHex(Constants.White),
				BackgroundColor = Color.FromHex(Constants.Green),
				FontSize = 15,
				FontAttributes = FontAttributes.Bold,
				WidthRequest = 15,
				HeightRequest = 15
			};

			var ProductText = new StackLayout {
				Padding = new Thickness (5),
				Children = {GameTitle, GamePrice}
			};

			eachgrid.Children.Add(ProductImage,0,0);
			eachgrid.Children.Add(ProductText,1,0);


			ProductContainer.Children.Add (eachgrid);

			this.View = ProductContainer;
		}

		public string IconUrl(string game)
		{
			var querystring = game + "game icon";
			string iconurl = "";
			List<IconVm> images = new List<IconVm> ();

			const string url = Constants.BingURL;
			var client = new RestClient (url);
			var request = APIGateway.CreateIconImageRequest(querystring);
			var response = client.Execute (request);

			if (response.StatusCode == HttpStatusCode.OK) {
				images = JsonConvert.DeserializeObject<List<IconVm>>(response.Content);
			}
			return iconurl;
		}

		public string randomUrl(){
			String[] imgurl = new string[5];
			imgurl [0] = "http://i.imgur.com/RMHTdIO.png";
			imgurl [1] = "http://i.imgur.com/8EpiJcD.png";
			imgurl [2] = "http://i.imgur.com/Ji7vCCN.png";
			imgurl [3] = "http://i.imgur.com/Cv8Uto2.png";
			imgurl [4] = "http://i.imgur.com/QdOsylp.png";

			Random random = new Random();
			int randomNumber = random.Next(0, 5);
			return imgurl[randomNumber];
		}
	}
}