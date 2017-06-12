using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Vacation;

namespace Vacation.Objects
{
  public class City
  {
    private int _id;
    private string _name;

    public City()
    {
      _id = 0;
      _name = null;
    }

    public City(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
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

    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        return (this.GetId() == newCity.GetId() &&
                this.GetName() == newCity.GetName());
      }
    }

    public static List<City>GetAll()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities;", DB.GetConnection());
      SqlDataReader rdr = cmd.ExecuteReader();

      List<City> allCities = new List<City>{};
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);


        City newCity = new City(name, id);
        allCities.Add(newCity);
      }

      if (rdr != null)
      {
        rdr.Close();
      }

      DB.CloseConnection();

      return allCities;
    }

    public void Save()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities (name) OUTPUT INSERTED.id VALUES (@CityName)", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@CityName", this.GetName()));

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
    public static City Find(int searchId)
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities WHERE id = @CityId;", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@CityId", searchId));

      SqlDataReader rdr = cmd.ExecuteReader();

      City foundCity = new City();
      while (rdr.Read())
      {
        foundCity._id = rdr.GetInt32(0);
        foundCity._name = rdr.GetString(1);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();

      return foundCity;
    }

    public void AddFriend(Friend newFriend)
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("INSERT INTO friends_cities (friend_id, city_id) VALUES (@FriendId, @CityId);", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@FriendId",newFriend.GetId()));
      cmd.Parameters.Add(new SqlParameter("@CityId", this.GetId()));

      cmd.ExecuteNonQuery();

      DB.CloseConnection();
    }

    public List<Friend> GetFriends()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT friends.* FROM cities JOIN friends_cities ON (cities.id = friends_cities.city.id) JOIN friend ON (friends_cities.friend_id = friends.id) WHERE city.id = @CityId;", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@CityId", this.GetId()));

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Friend> friends = new List<Friend> {};

      while (rdr.Read())
      {
        int friendId = rdr.GetInt32(0);
        string friendName = rdr.GetString(1);
        string friendDate = rdr.GetString(2);
        Friend newFriend = new Friend(friendName, friendDate, friendId);
        friends.Add(newFriend);
      }

      if (rdr != null)
      {
        rdr.Close();
      }

      DB.CloseConnection();
      return friends;
    }
  }
}
