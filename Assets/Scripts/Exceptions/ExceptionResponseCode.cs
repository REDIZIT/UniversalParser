using System;
using System.Net;
using UnityEngine;

namespace InGame.Exceptions
{
	public class ExceptionResponseCode : Exception
	{
		public HttpStatusCode responseCode;

        public ExceptionResponseCode(string message, HttpStatusCode responseCode) : base(message)
        {
			this.responseCode = responseCode;
        }
	}
}