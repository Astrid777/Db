using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HorseDb
{
    //Заезд
    class Race
    {
        private int v1;
        private string v2;
        private string v3;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public string Time { get; set; }
        public int TrackId { get; set; }


        public Race(int Id, string Name, string Data, string Time, int TrackId)
        {
            this.Id = Id;
            this.Name = Name;
            this.Data = Data;
            this.Time = Time;
            this.TrackId = TrackId;

        }

        public Race(int v1, string v2, string v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }

    //Ипподром
    class RaceTrack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string Telephone { get; set; }


        public RaceTrack(int Id, string Name, string Adress, string Telephone)
        {
            this.Id = Id;
            this.Name = Name;
            this.Adress = Adress;
            this.Telephone = Telephone;
        }

    }

    //Результат заезда
    class RaceResult
    {
        public string Race { get; set; }
        public string Horse { get; set; }
        public string Jockey { get; set; }
        public TimeSpan ResultTime { get; set; }
        public int ResultPlace { get; set; }

        public RaceResult(string Race, string Horse, string Jockey, TimeSpan ResultTime, int ResultPlace)
        {
            this.Race = Race;
            this.Horse = Horse;
            this.Jockey = Jockey;
            this.ResultTime = ResultTime;
            this.ResultPlace = ResultPlace;
        }
    }

    //Лошадь
    public class Horse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Owner { get; set; }

        public Horse(int Id, string Name, string Gender, int Age, string Owner)
        {
            this.Id = Id;
            this.Name = Name;

            this.Gender = Gender;
            this.Age = Age;
            this.Owner = Owner;
        }

    }

    //Жокей
    class Jockey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public double Rating { get; set; }
        public string Telephone { get; set; }

        public Jockey(int Id, string Name, string BirthDate, double Rating, string Telephone)
        {
            this.Id = Id;
            this.Name = Name;
            this.BirthDate = BirthDate;
            this.Rating = Rating;
            this.Telephone = Telephone;
        }
    }

    //Владелец
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Telephone { get; set; }


        public Owner(int Id, string Name, string BirthDate, string Telephone)
        {
            this.Id = Id;
            this.Name = Name;
            this.BirthDate = BirthDate;
            this.Telephone = Telephone;
        }
    }
}