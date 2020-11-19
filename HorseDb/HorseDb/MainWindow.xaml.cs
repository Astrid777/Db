using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HorseDb
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Выбрать все заезды
        private void Tb1SelectRaces(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            string sql = "SELECT Race.Id, Race.Name, Race.Date, Race.Time, RaceTrack.Name, RaceTrack.Address FROM HorseRaceDb.dbo.Race, HorseRaceDb.dbo.RaceTrack WHERE Race.RaceTrackId=RaceTrack.Id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();

            dt.Columns.Add("Id");
            dt.Columns.Add("Соревнование");
            dt.Columns.Add("Дата");
            dt.Columns.Add("Время");
            dt.Columns.Add("Ипподром");
            dt.Columns.Add("Адрес");

            while (reader.Read())
            {

                dt.Rows.Add(
                      reader.GetInt32(0),
                      reader.GetValue(1).ToString(),
                      Convert.ToDateTime(reader.GetValue(2)).ToShortDateString(),
                      reader.GetTimeSpan(3).ToString(),
                      reader.GetValue(4).ToString(),
                      reader.GetValue(5).ToString()
                      );

            }

            dgRaceTrack.ItemsSource = dt.DefaultView;
            dgRaceTrack.Columns[0].Visibility = Visibility.Hidden;

            reader.Close();
            connection.Close();
        }

        //Добавить звезд
        private void InsertRaceTb1(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            int n = 0;

            string s = RaceTrackList.SelectedItem.ToString();
            string sql = "SELECT Id FROM HorseRaceDb.dbo.RaceTrack WHERE Name='" + s + "'";
            SqlCommand command = new SqlCommand(sql, connection);

            SqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                n = Convert.ToInt32(reader.GetValue(0));
            }

            reader.Close();

            sql = "INSERT INTO HorseRaceDb.dbo.Race (Name, Date, Time, RaceTrackId) VALUES ('" + Tb1Name.Text + "','" + dp1.SelectedDate.Value.ToShortDateString() + "','" + Tb1Time.Text + "'," + n + ")";
            command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();

            connection.Close();

            MessageBox.Show("Добавлено соревнование: " + Tb1Name.Text);
        }

        //Открытие списка с ипподромами
        void OnDropDownOpened(object sender, EventArgs e)
        {

             var connection = new DbWork().Connect();
            connection.Open();


            SqlDataAdapter da = new SqlDataAdapter("SELECT Id, Name From HorseRaceDb.dbo.RaceTrack", connection);

            var table = new DataTable();
            da.Fill(table);

            RaceTrackList.ItemsSource = table.DefaultView;
        }

        //Удалить заезд
        private void Tb1Delete(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            List<DataRowView> str = new List<DataRowView>();

            foreach (var r in dgRaceTrack.SelectedItems)
                str.Add((DataRowView)r);

            SqlCommand command;
            string sql;

            foreach (var r in str)
            {
                sql = "DELETE FROM HorseRaceDb.dbo.Race WHERE Id=" + r.Row["Id"];
              
                command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }

            connection.Close();

            MessageBox.Show("Удалено");
        }

        //Вкладка ЖОКЕИ

        //Выбрать всех жокеев
        private void ButtonSelectJockeysTb2(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            String sql = "SELECT * FROM HorseRaceDb.dbo.Jockey";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();

            var jockeys = new List<Jockey>();

            while (reader.Read())
            {
                jockeys.Add(new Jockey(

                    Convert.ToInt32(reader.GetValue(0)),
                    reader.GetString(1),
                    Convert.ToDateTime(reader.GetValue(2)).ToShortDateString(),
                    Convert.ToDouble(reader.GetValue(3)),
                    reader.GetValue(4).ToString()
                    ));
            }

            dg2.ItemsSource = jockeys;

            dg2.Columns[1].Header = "ФИО";
            dg2.Columns[2].Header = "Дата рождения";
            dg2.Columns[3].Header = "Рейтинг";
            dg2.Columns[4].Header = "Телефон";

            dg2.Columns[0].Visibility = Visibility.Hidden;

            reader.Close();
            connection.Close();
        }

        //Добавить жокея
        private void btnInsertJockey(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

           string sql = "INSERT INTO HorseRaceDb.dbo.Jockey (Name, BirthDate, Rating, Telephone) VALUES ('" + Tb2Fio.Text + "','" + dp2.SelectedDate.Value.ToShortDateString() + "','" + Tb2Rating.Text + "','" + Tb2Telephone.Text + "')";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

            connection.Close();

            MessageBox.Show("Добавлен жокей: " + Tb2Fio.Text);
        }

        //Удалить жокея
        private void Tb2Delete(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            List<Jockey> str = new List<Jockey>();

            foreach (var r in dg2.SelectedItems)
                str.Add((Jockey)r);

            SqlCommand command;
            string sql;

            foreach (var r in str)
            {
                sql = "DELETE FROM HorseRaceDb.dbo.Jockey WHERE Id=" + r.Id;
                command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }

            connection.Close();

            MessageBox.Show("Удалено");
        }

        //Выбрать лошадей
        private void SelectTb3(object sender, RoutedEventArgs e)
        {
            
            var connection = new DbWork().Connect();
            connection.Open();

            string sql = "SELECT Horse.Id, Horse.Name, Horse.Gender, Horse.Age, Owner.Name FROM HorseRaceDb.dbo.Horse, HorseRaceDb.dbo.Owner WHERE Horse.OwnerId=Owner.Id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<Horse> horses = new List<Horse>();

            while (reader.Read())
            {
                horses.Add(new Horse(
                    Convert.ToInt32(reader.GetValue(0)),

                    reader.GetValue(1).ToString(),
                    reader.GetValue(2).ToString(),
                    Convert.ToInt32(reader.GetValue(3)),
                    reader.GetValue(4).ToString()
                    ));
            }

            connection.Close();

            dg3.ItemsSource = horses;

            dg3.Columns[1].Header = "Кличка";
            dg3.Columns[2].Header = "Пол";
            dg3.Columns[3].Header = "Возраст";
            dg3.Columns[4].Header = "Владелец";

            dg3.Columns[0].Visibility = Visibility.Hidden;
  

            reader.Close();
            connection.Close();
        }

        //Открыть список владельцев
        void cbOwnersOpened(object sender, EventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            SqlDataAdapter da = new SqlDataAdapter("Select Id, Name From HorseRaceDb.dbo.Owner", connection);
            
            var table = new DataTable();
            da.Fill(table);
           
            cbOwners.ItemsSource = table.DefaultView;
        }

        //Выбор пола лошади
        void cbGenderOpened(object sender, EventArgs e)
        {
            cbGender.Items.Clear();

            cbGender.Items.Add("\"Муж\"");
            cbGender.Items.Add("\"Жен\"");
        }

        //Добавить лошадь
        private void InsertTb3(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            string sql = "INSERT INTO HorseRaceDb.dbo.Horse (Name, Gender, Age, OwnerId) VALUES ('" + Tb31.Text + "','" + cbGender.SelectedItem.ToString()+ "','" + Tb33.Text + "','" + cbOwners.SelectedValue + "')";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

            connection.Close();

            MessageBox.Show("Добавлена лошадь: " + Tb31.Text);
        }
        
        //Удалить лошадь
        private void Tb3Delete(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            List<Horse> str = new List<Horse>();

            foreach (var r in dg3.SelectedItems)
                str.Add((Horse)r);

            SqlCommand command;
            string sql;

            foreach (var r in str)
            {
                sql = "DELETE FROM HorseRaceDb.dbo.Horse WHERE Id=" + r.Id;
                
                command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }

            connection.Close();

            MessageBox.Show("Удалена лошадь");
        }

        //Выбрать всех владельцев
        private void Tb4Select(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            string sql = "SELECT * FROM HorseRaceDb.dbo.Owner";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<Owner> owners = new List<Owner>();

            while (reader.Read())
            {
                owners.Add(new Owner(
                    Convert.ToInt32(reader.GetValue(0)),

                    reader.GetValue(1).ToString(),
                    Convert.ToDateTime(reader.GetValue(2)).ToShortDateString(),
                    reader.GetValue(3).ToString()
                    ));
            }

            dg4.ItemsSource = owners;

            dg4.Columns[1].Header = "Имя";
            dg4.Columns[2].Header = "Др";
            dg4.Columns[3].Header = "Телефон";

            dg4.Columns[0].Visibility = Visibility.Hidden;
            
            connection.Close();

            reader.Close();
            connection.Close();
        }

        //Добавить владельца
        private void Tb4Insert(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            string sql = "INSERT INTO HorseRaceDb.dbo.Owner (Name, BirthDate, Telephone) VALUES ('" + Tb41.Text + "','" + dp4.SelectedDate.Value.ToShortDateString()+ "'," + Tb43.Text + ")";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

            connection.Close();

            MessageBox.Show("Добавлен владелец: " + Tb41.Text);
        }


        //Удалить владельца
        private void Tb4Delete(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            List<Owner> str = new List<Owner>();

            foreach (var r in dg4.SelectedItems)
                str.Add((Owner)r);

            SqlCommand command;
            string sql;

            foreach (var r in str)
            {
                sql = "DELETE FROM HorseRaceDb.dbo.Owner WHERE Id=" + r.Id;

                command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }

            connection.Close();

            MessageBox.Show("Удален владелец");
        }

        //Вкладка результат соревнования
        private void Tb5Select(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            string sql = "SELECT Race.Name, Horse.Name, Jockey.Name, ResultTime, ResultPlace " +
                "FROM HorseRaceDb.dbo.RaceResult, HorseRaceDb.dbo.Race, HorseRaceDb.dbo.Horse, HorseRaceDb.dbo.Jockey " +
                "WHERE  RaceResult.RaceId=Race.Id AND RaceResult.HorseId = Horse.Id AND JockeyId = Jockey.Id";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<RaceResult> results = new List<RaceResult>();

            while (reader.Read())
            {   
                results.Add(new RaceResult(
                    reader.GetValue(0).ToString(),
                    reader.GetValue(1).ToString(),
                    reader.GetValue(2).ToString(),

                    reader.GetTimeSpan(3),
                    reader.GetInt32(4)
                    )); 
            }


            dg5.ItemsSource = results;

            dg5.Columns[0].Header = "Соревнование";
            dg5.Columns[1].Header = "Лошадь";
            dg5.Columns[2].Header = "Жокей";
            dg5.Columns[3].Header = "Время";
            dg5.Columns[4].Header = "Место";

            connection.Close();

            reader.Close();
            connection.Close();
        }

        //Добавить результат соревнования
        private void Tb5Insert(object sender, RoutedEventArgs e)
        {

            var connection = new DbWork().Connect();
            connection.Open();

            string sql = "INSERT INTO HorseRaceDb.dbo.RaceResult (ResultPlace, ResultTime, RaceId, HorseId, JockeyId) VALUES ('" + Tb5Place.Text + "','" + Tb5Time.Text + "'," + cbTb5Race.SelectedValue + ","+ cbTb5Horse.SelectedValue + "," + cbTb5Jockey.SelectedValue + ")";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

            connection.Close();

            MessageBox.Show("Добавлен результат соревнования: " + Tb41.Text);
        }

        //Открыть список жокеев
        private void cb5OpenedJockey(object sender, EventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            SqlDataAdapter da = new SqlDataAdapter("Select Id, Name From HorseRaceDb.dbo.Jockey", connection);

            var table = new DataTable();
            da.Fill(table);


            cbTb5Jockey.ItemsSource = table.DefaultView;
        }

        //Открыть список лошадей
        private void cb5OpenedHorse(object sender, EventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();


            SqlDataAdapter da = new SqlDataAdapter("SELECT Id, Name FROM HorseRaceDb.dbo.Horse", connection);

            var table = new DataTable();
            da.Fill(table);

            cbTb5Horse.ItemsSource = table.DefaultView;
        }

        //Открыть список заездов
        private void cb5OpenedRace(object sender, EventArgs e)
        {

            var connection = new DbWork().Connect();
            connection.Open();

            SqlDataAdapter da = new SqlDataAdapter("SELECT Id, Name FROM HorseRaceDb.dbo.Race", connection);

            var table = new DataTable();
            da.Fill(table);

            cbTb5Race.ItemsSource = table.DefaultView;
        }

        //удалить результат
        private void Tb5Delete(object sender, RoutedEventArgs e)
        {

            var connection = new DbWork().Connect();
            connection.Open();

            List<RaceResult> str = new List<RaceResult>();

            foreach (var r in dg5.SelectedItems)
                str.Add((RaceResult)r);

            SqlCommand command;
            string sql;

            foreach (var r in str)
            {
                sql = "DELETE FROM HorseRaceDb.dbo.RaceResult " +
                "WHERE RaceId = (Select Id from HorseRaceDb.dbo.Race Where Name = '" + r.Race + "') AND " +
                "JockeyId = (Select Id from HorseRaceDb.dbo.Jockey Where Name = '" + r.Jockey + "') AND " +
                "HorseId=(Select Id from HorseRaceDb.dbo.Horse Where Name = '" + r.Horse + "')";

                command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }

            connection.Close();

            MessageBox.Show("Удален результат");
        }

       
        //Выбрать все ипподромы
        private void Tb6Select(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            string sql = "SELECT * FROM HorseRaceDb.dbo.RaceTrack";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<RaceTrack> raceTracks = new List<RaceTrack>();

            while (reader.Read())
            {
                raceTracks.Add(new RaceTrack(
                    reader.GetInt32(0),
                    reader.GetValue(1).ToString(),
                    reader.GetValue(2).ToString(),
                    reader.GetValue(3).ToString()

                    ));
            }


            dg6.ItemsSource = raceTracks;

            dg6.Columns[1].Header = "Название";
            dg6.Columns[2].Header = "Адрес";
            dg6.Columns[3].Header = "Телефон";

            dg6.Columns[0].Visibility = Visibility.Hidden;

            connection.Close();

            reader.Close();
            connection.Close();
        }

        //Добавить ипподром
        private void Tb6Insert(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            string sql = "INSERT INTO HorseRaceDb.dbo.RaceTrack (Name, Address, Telephone) VALUES ('" + Tb6Name.Text + "','" + Tb6Address.Text + "'," + Tb6Telephone.Text + ")";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

            connection.Close();

            MessageBox.Show("Добавлен ипподром: " + Tb6Name.Text);
        }

        //Удалить ипподром
        private void Tb6Delete(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            List<RaceTrack> str = new List<RaceTrack>();

            foreach (var r in dg6.SelectedItems)
                str.Add((RaceTrack)r);

            SqlCommand command;
            string sql;

            foreach (var r in str)
            {
                sql = "DELETE FROM HorseRaceDb.dbo.RaceTrack WHERE Id=" + r.Id;

                command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }

            connection.Close();

            MessageBox.Show("Ипподром удален");
        }

        //Отчет 1
        private void Tb7_1(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            String sql = "Select Owner.Name, Owner.BirthDate, Owner.Telephone from HorseRaceDb.dbo.Owner where  EXISTS  (SELECT OwnerId, COUNT(OwnerId) FROM HorseRaceDb.dbo.Horse where Owner.Id=Horse.OwnerId group by OwnerId having COUNT(OwnerId)>1)";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();


            DataTable dt = new DataTable();
            dt.Columns.Add("Имя");
            dt.Columns.Add("Дата рождения");
            dt.Columns.Add("Телефон");


            while (reader.Read())
            {

                dt.Rows.Add(

                      reader.GetValue(0).ToString(),
                      Convert.ToDateTime(reader.GetValue(1)).ToShortDateString(),
                      reader.GetValue(2).ToString()

                      );

            }

            connection.Close();

            dg7.ItemsSource = dt.DefaultView;
        }

        //Отчет 2
        private void Tb7_2(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            string raiting = Tb7Raiting.Text;

            String sql = "select Name, BirthDate, Rating, Telephone from HorseRaceDb.dbo.Jockey where Rating>" + raiting;
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();


            DataTable dt = new DataTable();
            dt.Columns.Add("Имя");
            dt.Columns.Add("Возраст");
            dt.Columns.Add("Рейтинг");
            dt.Columns.Add("Телефон");

            while (reader.Read())
            {

                dt.Rows.Add(

                      reader.GetValue(0).ToString(),
                      Convert.ToDateTime(reader.GetValue(1)).ToShortDateString(),
                      reader.GetValue(2).ToString(),
                      reader.GetValue(3).ToString()
                      );

            }
            connection.Close();

            dg7.ItemsSource = dt.DefaultView;
        }


        //Отчет 3
        private void Tb7_3(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();

            string sql = "Select Horse.Name, RaceTrack.Name, Race.Date, RaceResult.ResultPlace " +
            "from HorseRaceDb.dbo.Race, HorseRaceDb.dbo.RaceResult, HorseRaceDb.dbo.Horse, HorseRaceDb.dbo.RaceTrack " +
            "Where Race.Id = RaceResult.RaceId " +
            "and Horse.Id = RaceResult.HorseId " +
            "and RaceTrack.Id = Race.RaceTrackId " +
            "and Date>'" + dp7_1.SelectedDate + "' AND Date<'" + dp7_2.SelectedDate +
            "' and RaceResult.ResultPlace IN(1,2)";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();


            DataTable dt = new DataTable();
            dt.Columns.Add("Лошадь");
            dt.Columns.Add("Соревнование");
            dt.Columns.Add("Дата");
            dt.Columns.Add("Место");

            while (reader.Read())
            {

                dt.Rows.Add(

                      reader.GetValue(0).ToString(),
                      reader.GetValue(1).ToString(),
                      Convert.ToDateTime(reader.GetValue(2)).ToShortDateString(),
                      reader.GetValue(3).ToString()
                      );

            }

            connection.Close();

            dg7.ItemsSource = dt.DefaultView;
        }


        //Отчет 4
        private void Tb7_4(object sender, RoutedEventArgs e)
        {
            var connection = new DbWork().Connect();
            connection.Open();


            string sql = "SELECT TOP 3 Jockey.Name, Jockey.Rating, Race.Name, Race.Date " +
            "FROM HorseRaceDb.dbo.Race, HorseRaceDb.dbo.Jockey, HorseRaceDb.dbo.RaceResult " +
            "WHERE RaceResult.RaceId=Race.Id AND " +
            "RaceResult.JockeyId=Jockey.Id AND " +
            "Race.RaceTrackId="+ cbTb7.SelectedValue + 
            " ORDER BY date desc, Rating desc";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();


            DataTable dt = new DataTable();
            dt.Columns.Add("Жокей");
            dt.Columns.Add("Рейтинг");
            dt.Columns.Add("Соревнование");
            dt.Columns.Add("Дата");

            while (reader.Read())
            {

                dt.Rows.Add(

                      reader.GetValue(0).ToString(),
                      reader.GetValue(1).ToString(),
                      reader.GetValue(2).ToString(),
                      Convert.ToDateTime(reader.GetValue(3)).ToShortDateString()

                      );
            }

            connection.Close();
            dg7.ItemsSource = dt.DefaultView;
        }

        //Выбрать все ипподромы для отчета
        void cb7OpenedReport1(object sender, EventArgs e)
        {

            var connection = new DbWork().Connect();
            connection.Open();


            SqlDataAdapter da = new SqlDataAdapter("Select Id, Name From HorseRaceDb.dbo.RaceTrack", connection);

            var table = new DataTable();
            da.Fill(table);

            cbTb7.ItemsSource = table.DefaultView;

        }

    }
}
