using Nancy;
using Airline.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nancy.ViewEngines.Razor;

namespace Airline
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        Dictionary<string, object> model= new Dictionary<string, object>{};
        
        model.Add("flights", Flight.GetAll());
        model.Add("cities", City.GetAll());

        return View["index.cshtml", model];
      };
    }
  }
}
