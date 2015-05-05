using System;
using RestSharp;
using Xamarin.Forms;

namespace GP3
{
	public class APIGateway : Application
	{
		public static string GetApiKey()
		{
			string ApiKey = "";
			if(Application.Current.Properties.ContainsKey("ApiKey")){
				ApiKey = Application.Current.Properties["ApiKey"] as string;
			}
			return ApiKey;
		}

		public static string GetUserId()
		{
			string UserId = "";
			if(Application.Current.Properties.ContainsKey("UserId")){
				UserId = Application.Current.Properties["UserId"].ToString();
			}
			return UserId;
		}

		public static RestRequest CreateRequest(string url, Method method)
		{
			var request = new RestRequest(url, method);
			request.AddHeader("xcmps383authenticationid", GetUserId());
			request.AddHeader("xcmps383authenticationkey", GetApiKey());
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			return request;
		}

		public static RestRequest CreateIconImageRequest(String query)
		{
			var request = new RestRequest ("/Image", Method.GET);
			request.AddHeader ("Authorization", "Basic cTNhNS84TTlKL0hFZTk3dE9oM1BjN0NURGkrcWZCT0VVeGxlSDd5TG5vUTpxM2E1LzhNOUovSEVlOTd0T2gzUGM3Q1REaStxZkJPRVV4bGVIN3lMbm9R");
			request.AddQueryParameter ("Query","\"game icon\"");
			request.AddQueryParameter ("Adult","Strict");
			request.AddQueryParameter ("$format","json");
			request.AddQueryParameter ("$top","1");
			request.RequestFormat = DataFormat.Json;
			return request;
		}

		public static string Base64Encode(string plainText) 
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}
	}
}

