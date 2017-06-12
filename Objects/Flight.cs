using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Airline;

namespace Airline.Objects
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
        allCities.Add(newFriend);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO friends (name, date) OUTPUT INSERTED.id VALUES (@FriendName, @FriendDate)", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@FriendName", this.GetName()));
      cmd.Parameters.Add(new SqlParameter("@Frienddate", this.Getdate()));


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

      SqlCommand cmd = new SqlCommand("INSERT INTO friends_cities (flight_id, city_id) VALUES (@FriendId, @CityId);", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@CityId",newCity.GetId()));
      cmd.Parameters.Add(new SqlParameter("@FriendId", this.GetId()));

      cmd.ExecuteNonQuery();

      DB.CloseConnection();
    }

    public List<City> GetCities()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT city_id FROM friends_cities WHERE friend_id = @FriendId;", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@FriendId", this.GetId()));

      SqlDataReader rdr = cmd.ExecuteReader();

      List<int> cityIds = new List<int> {};

      while (rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        cityIds.Add(cityId);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      List<City> cities = new List<City> {};

      foreach (int cityId in cityIds)
      {
        SqlCommand cityQuery = new SqlCommand("SELECT * FROM cities WHERE id = @CityId;", DB.GetConnection());

        cityQuery.Parameters.Add(new SqlParameter("@CityId", cityId));

        SqlDataReader queryReader = cityQuery.ExecuteReader();
        while (queryReader.Read())
        {
          int thisCityId = queryReader.GetInt32(0);
          string cityName = queryReader.GetString(1);
          cities.Add(new City(cityName, thisCityId));
        }
        if (queryReader != null)
        {
          queryReader.Close();
        }
      }
      DB.CloseConnection();
      return cities;
    }
  }
}
