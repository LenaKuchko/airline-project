using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Vacation;

namespace Vacation.Objects
{
  public class Friend
  {
    private int _id;
    private string _name;
    private string _date;

    public Friend()
    {
      _id = 0;
      _name = null;
      _date = null;
    }

    public Friend(string Name, string Date, int Id = 0)
    {
      _id = Id;
      _name= Name;
      _date = Date;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }

    public string GetDate()
    {
      return _date;
    }
    public void SetDate(string newDate)
    {
      _date = newDate;
    }

    public override bool Equals(System.Object otherFriend)
    {
      if (!(otherFriend is Friend))
      {
        return false;
      }
      else
      {
        Friend newFriend = (Friend) otherFriend;
        return (this.GetId() == newFriend.GetId() &&
                this.GetName() == newFriend.GetName() &&
                this.GetDate() == newFriend.GetDate());
      }
    }

    public static List<Friend>GetAll()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT * FROM friends;", DB.GetConnection());
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Friend> allFriends = new List<Friend>{};
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string date = rdr.GetString(2);

        Friend newFriend = new Friend(name, date, id);
        allFriends.Add(newFriend);
      }

      if (rdr != null)
      {
        rdr.Close();
      }

      DB.CloseConnection();

      return allFriends;
    }

    public void Save()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("INSERT INTO friends (name, date) OUTPUT INSERTED.id VALUES (@FriendName, @FriendDate);", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@FriendName", this.GetName()));
      cmd.Parameters.Add(new SqlParameter("@FriendDate", this.GetDate()));


      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();
    }

    public static Friend Find(int searchId)
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT * FROM friends WHERE id = @FriendId;", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@FriendId", searchId));

      SqlDataReader rdr = cmd.ExecuteReader();

      Friend foundFriend = new Friend();
      while (rdr.Read())
      {
        foundFriend._id = rdr.GetInt32(0);
        foundFriend._name = rdr.GetString(1);
        foundFriend._date = rdr.GetString(2);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();

      return foundFriend;
    }

    public void AddCity(City newCity)
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("INSERT INTO friends_cities (friend_id, city_id) VALUES (@FriendId, @CityId);", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@CityId",newCity.GetId()));
      cmd.Parameters.Add(new SqlParameter("@FriendId", this.GetId()));

      cmd.ExecuteNonQuery();

      DB.CloseConnection();
    }

    public List<City> GetCities()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT cities.* FROM friends JOIN friends_cities ON (friends.id = friends_cities.friend_id) JOIN cities ON (friends_cities.city_id = cities.id) WHERE friends.id = @FriendId;", DB.GetConnection());


      cmd.Parameters.Add(new SqlParameter("@FriendId", this.GetId()));

      SqlDataReader rdr = cmd.ExecuteReader();

      List<City> cities = new List<City> {};

      while (rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        cities.Add(newCity);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      DB.CloseConnection();
      return cities;
    }
  }
}

// SELECT friends.* FROM
// cities JOIN friends_cities ON (cities.id = friends_cities.cities_id)
//         JOIN friends ON (friends_cities.friend_id = friends.id)
// WHERE cities.id = 3;
