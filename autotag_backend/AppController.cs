using System;
using AutoTagBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTagBackEnd
{
	public class AppController : Controller
	{
		protected Account CurrentAccount {
			get
            {
				if(HttpContext == null || HttpContext?.Items == null || HttpContext?.Items["Account"] == null)
                {
					throw new Exception("No existe el valor Account dentro de HttpContext");
                }
                return (Account)HttpContext?.Items["Account"]!;
			}
		}
	}
}