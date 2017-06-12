using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Airline;

namespace Airline.Objects
{
  public class City
  {
    private int _id;
    private string _name;

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
          int rating = rdr.GetInt32(2);

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
  }
}
