using System;

namespace GP3
{
	public class LoginResultVM
	{
		public string ApiKey{ get; set; }
		public int UserId { get; set; }

		public Boolean isValid(){
			if(!String.IsNullOrEmpty(this.ApiKey) && this.UserId > 0){
				return true;
			}
			else{
				return false;
			}
		}
	}
}