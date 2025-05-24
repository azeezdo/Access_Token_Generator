using System;
namespace AccessTokenDomain.Model.Response
{
	public class CustomResponse
	{
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object Data { get; set; }
    }
}

