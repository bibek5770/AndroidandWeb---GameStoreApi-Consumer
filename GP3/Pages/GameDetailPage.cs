using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GP3
{
	public class GameDetailPage : ContentPage
	{
		public GameDetailPage (GameVM Game)
		{
			//Game Images
			var imgsrc1 = new UriImageSource{ Uri = new Uri ("http://i.imgur.com/eUn6eJq.jpg") };
			var ProductImage1 = new Image{ Source = imgsrc1 };

			var imgsrc2 = new UriImageSource{ Uri = new Uri ("http://i.imgur.com/QrFU7Nb.jpg") };
			var ProductImage2 = new Image{ Source = imgsrc2 };

			var imgsrc3 = new UriImageSource{ Uri = new Uri ("http://i.imgur.com/mcBeItO.jpg") };
			var ProductImage3 = new Image{ Source = imgsrc3 };

			//ProductImage Slideshow
			var imagecollection = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Children = {ProductImage2, ProductImage1, ProductImage3},
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness(0,7,0,0),
				HeightRequest = 200
			};
					
			ToolbarItem Cart = new ToolbarItem {
				Icon = "shoppingcartsm.png",
			};

			ToolbarItem Logout = new ToolbarItem{ 
				Icon = "logoutsm.png"
			};
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

			var scrollView = new ScrollView
			{
				HorizontalOptions = LayoutOptions.Fill,
				Orientation = ScrollOrientation.Horizontal,
			};
			scrollView.Content = imagecollection;

			Title = "GP3";

			Label title = new Label {
				Text= Game.GameName + "   $" + Game.Price,
				TextColor = Color.FromHex(Constants.White),
				FontSize = 25,
				BackgroundColor = Color.FromHex(Constants.Dark),
				FontAttributes = FontAttributes.Bold,
				YAlign = TextAlignment.Center,
				XAlign = TextAlignment.Center
			};


			StackLayout titlestack = new StackLayout() {};
			titlestack.Children.Add (title);

			StackLayout topLayout = new StackLayout ();
			topLayout.Children.Add (scrollView);
			topLayout.Children.Add (titlestack);

			StackLayout Tablestack = new StackLayout() { Padding = new Thickness(10,10,10,10)};
			StackLayout instock = new StackLayout ();
			Label instocklabel = new Label(){ Text = "In Stock : ", FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex(Constants.Dark)};
			Label instockvaluelabel = new Label (){ Text = Game.InventoryStock.ToString() };
			instock.Children.Add (instocklabel);
			instock.Children.Add (instockvaluelabel);


			StackLayout Releasedate = new StackLayout ();
			Label releasedatelabel = new Label(){ Text = "Release Date : ", FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex(Constants.Dark) };
			Label releasedatevaluelabel = new Label (){ Text = Game.ReleaseDate, FontSize = 20, TextColor = Color.Gray};
			Releasedate.Children.Add (releasedatelabel);
			Releasedate.Children.Add (releasedatevaluelabel);

			StackLayout Genres = new StackLayout ();
			Label genreslabel = new Label(){ Text = "Genres : " , FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex(Constants.Dark)};
			Label genresvaluelabel = new Label (){ Text = Game.Genres , FontSize = 20, TextColor = Color.Gray};
			Genres.Children.Add (genreslabel);
			Genres.Children.Add (genresvaluelabel);
				
			StackLayout Tags = new StackLayout ();
			Label tagslabel = new Label(){ Text = "Tags : ", FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex(Constants.Dark) };
			Label tagsvaluelabel = new Label(){Text = Game.Tags, FontSize = 20, TextColor = Color.Gray};
			Tags.Children.Add (tagslabel);
			Tags.Children.Add (tagsvaluelabel);


			Tablestack.Children.Add (instock);
			Tablestack.Children.Add (Releasedate);
			Tablestack.Children.Add (Genres);
			Tablestack.Children.Add (Tags);

			//Add to Cart Block
			Label OrderText = new Label {
				Text= "Order",
				TextColor = Color.FromHex(Constants.White),
				FontSize = 25,
				BackgroundColor = Color.FromHex(Constants.Dark),
				FontAttributes = FontAttributes.Bold,
				YAlign = TextAlignment.Center,
				XAlign = TextAlignment.Center
			};
					
			Button minus = new Button(){
				FontSize = 15,
				Text = "-",
				FontAttributes = FontAttributes.Bold,
				VerticalOptions= LayoutOptions.CenterAndExpand, 
				BackgroundColor = Color.FromHex(Constants.Dark)
			};

			Button plus = new Button(){
				FontSize = 15,
				Text = "+",
				FontAttributes = FontAttributes.Bold,
				VerticalOptions= LayoutOptions.CenterAndExpand, 
				BackgroundColor = Color.FromHex(Constants.Dark)
			};

			int quantityCount = 1;

			var quantitycontainer = new StackLayout{ Padding = new Thickness(0,15) };
			var quantity = new Entry { Placeholder = quantityCount.ToString(), TextColor = Color.FromHex(Constants.Dark), VerticalOptions= LayoutOptions.Center, BackgroundColor=Color.FromHex("#ffffff") };
			quantity.SetBinding(Entry.TextProperty, "Quantity");
			quantitycontainer.Children.Add (quantity);


			var addtocartbutton = new Button (){ 
				FontSize = 25,
				Text = "+ Add to Cart",
				FontAttributes = FontAttributes.Bold,
				VerticalOptions= LayoutOptions.Center, 
				BackgroundColor = Color.FromHex(Constants.Green)
			};

			plus.Clicked += (sender, e) => {
				if(quantityCount >= Game.InventoryStock){
					DisplayAlert("Quantity Error","We only have " + Game.InventoryStock + " units in stock.","OK");
				}
				else{
					quantityCount = quantityCount + 1;
					quantity.Text = quantityCount.ToString();
				}
			};

			minus.Clicked += (sender, e) => {
				if(quantityCount == 1){
					DisplayAlert("Quantity Error","Quantity cannot be less than one.","OK");
				}
				else{
					quantityCount = quantityCount - 1;
					quantity.Text = quantityCount.ToString();
				}
			};

			addtocartbutton.Clicked += (sender, e) => {
				if(quantityCount > Game.InventoryStock){
					DisplayAlert("Quantity Error","We only have " + Game.InventoryStock + " units in stock.","OK");
				}
				else{
					//Post Games to API
					if(AddGametoCart(Game)){
						Cart.Icon = "shoppingcartfull.png";
						DisplayAlert("Added to Cart", "Game has been added to cart.", "OK");
					}
					else{
						DisplayAlert("Failed", "Game could not be added to cart.", "OK");
					}
				}
			};

			StackLayout addtocartimagestack = new StackLayout () {
				
				Children = {addtocartbutton}
			};

			StackLayout addtocart = new StackLayout () {
				Padding = new Thickness(5,15,5,15),
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				Children = {
					minus, quantitycontainer, plus
				},
				Orientation = StackOrientation.Horizontal
			};
					
			StackLayout finallayout = new StackLayout () {
				BackgroundColor = Color.FromHex ("#fff"),
				Children = {
					topLayout,
					Tablestack,
					OrderText,
					addtocart,
					addtocartimagestack
				}	
			};

			var sv = new ScrollView {
				HorizontalOptions = LayoutOptions.Fill,
				Orientation = ScrollOrientation.Vertical,
				Content = finallayout
			};
			Content = sv;
		}

		public bool AddGametoCart(GameVM game){
			string gameURL = game.URL;

			//GetGames and Bind it to setgame DTO
			SetGameDTO tempGame = new SetGameDTO ();
			List<SetGameDTO> gamelist = new List<SetGameDTO> ();

			var client = new RestClient (gameURL);
			var request = APIGateway.CreateRequest("", Method.GET);
			var response = client.Execute (request);

			//BindItTo<SetGameDTO>
			if (response.StatusCode == HttpStatusCode.OK) {
				tempGame = JsonConvert.DeserializeObject<SetGameDTO>(response.Content);
				gamelist.Add (tempGame);
			}

			//GetGame to SetGame
			string fullURL = "http://dev.envocsupport.com/GameStore3/api/Carts";
			var postclient = new RestClient (fullURL);
			var postrequest = APIGateway.CreateRequest("",Method.POST);
			postrequest.AddBody (gamelist);
			var postresponse = postclient.Execute (postrequest);

			if(postresponse.StatusCode == HttpStatusCode.OK){
				//Added to cart
				//Redirect to cart page
				return true;
			}
			else if(postresponse.StatusCode == HttpStatusCode.BadRequest)
			{
				string responsemsg = postresponse.Content;

				if(responsemsg.Contains("active cart")){
					//Fetch existing cart
					var cartclient = new RestClient (fullURL);
					var userid = App.Current.Properties["UserId"];

					var cartrequest = APIGateway.CreateRequest("/" + userid, Method.GET);
					var cartresponse = cartclient.Execute (cartrequest);

					var newsetcart1 = new CartVm.SetCartDTO (){
						User_Id=int.Parse(userid.ToString()) ,
						Games=new List<Tuple<SetGameDTO,int>>()
					};
					int x = 0;

					var newsetcart = new CartVm.SetCartDTO ();
						
					if (cartresponse.StatusCode == HttpStatusCode.OK) {
						newsetcart = JsonConvert.DeserializeObject<CartVm.SetCartDTO>(cartresponse.Content);
						foreach(var item in newsetcart.Games){
							newsetcart1.Games.Add (new Tuple<SetGameDTO,int> (item.Item1, item.Item2));
							x = item.Item2;
						}
						newsetcart1.Games.Add (new Tuple<SetGameDTO,int> (tempGame, x));
					}
					newsetcart.Games.Add (new Tuple<SetGameDTO,int> (tempGame, x));

					//Post updated cart
					var xpostclient = new RestClient (fullURL);
					var xpostrequest = APIGateway.CreateRequest("/PutCartModel",Method.PUT);
					postrequest.AddBody (newsetcart);
					var xresponse = xpostclient.Execute (xpostrequest);

					if(xresponse.StatusCode == HttpStatusCode.OK){
						//Cart Success
						return true;
					}
				}
			}
			return false;
		}
	}

	public class KeyValueCell : ViewCell
	{
		public KeyValueCell (string key, string value)
		{
			View = new StackLayout () {
				Padding = new Thickness(15,10),
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.Center,
				Children = {
					new Label () { 
						Text = key,
						TextColor = Color.FromHex(Constants.Dark),
						FontSize = 15,
						HorizontalOptions = LayoutOptions.StartAndExpand
					},
					new Label () { 
						Text = value,
						TextColor = Color.Gray,
						FontSize = 15,
						HorizontalOptions = LayoutOptions.EndAndExpand
					}
				}
			};
		}
	}
}

