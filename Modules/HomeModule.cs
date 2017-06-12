using Nancy;
using Vacation.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nancy.ViewEngines.Razor;

namespace Vacation
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        model.Add("friends", Friend.GetAll());
        model.Add("cities", City.GetAll());
        model.Add("display", null);
        return View["index.cshtml", model];
      };
      // Get["/friends/{id}/info"] = parameter => {
      //
      // };
      Get["/friends/add_new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        model.Add("friends", Friend.GetAll());
        model.Add("cities", City.GetAll());
        model.Add("display", "friend-new");
        return View["index.cshtml", model];
      };
      Post["/friends/add_new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        Friend newFriend = new Friend(Request.Form["friend-name"], Request.Form["friend-date"]);
        newFriend.Save();
        newFriend.AddCity(City.Find(Request.Form["city"]));
        model.Add("friends", Friend.GetAll());
        model.Add("cities", City.GetAll());
        model.Add("display", "friend-added");
        return View["index.cshtml", model];
      };
      Get["/friends/{id}/info"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        Friend selectedFriend = Friend.Find(parameters.id);
        model.Add("selected", selectedFriend);
        model.Add("friendCities", selectedFriend.GetCities());
        model.Add("friends", Friend.GetAll());
        model.Add("cities", City.GetAll());
        model.Add("display", "friend-info");
        return View["index.cshtml", model];
      };
    }
  }
}
