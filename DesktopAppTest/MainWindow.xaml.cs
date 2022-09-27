using DesktopAppTest.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
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

namespace DesktopAppTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Attributes
        private Model_MainWindow mod_MainWindow;
        private Command.Command readUsers_Command;
        private Command.Command addUser_Command;
        #endregion


        #region Properties
        internal Model_MainWindow Mod_MainWindow { get => mod_MainWindow; set => mod_MainWindow = value; }
        public Command.Command ReadUsers_Command { get => readUsers_Command; set => readUsers_Command = value; }
        public Command.Command AddUser_Command { get => addUser_Command; set => addUser_Command = value; }
        #endregion


        public MainWindow()
        {
            InitializeComponent();

            this.Mod_MainWindow = new Model_MainWindow();
            this.ReadUsers_Command = new Command.Command(ReadUsers_Executed, ReadUsers_CanExecute);
            this.AddUser_Command = new Command.Command(AddUser_Executed, AddUser_CanExecute);

            DataContext = new
            {
                mod_mw = this.Mod_MainWindow,
                list = this.Mod_MainWindow.List,
                read_users_cmd = this.ReadUsers_Command,
                add_user_cmd = this.AddUser_Command
            };
        }

        #region Gui_commands

        private void Commands_Evaluate()
        {
            this.ReadUsers_Command.RaiseCanExecuteChanged();
            this.AddUser_Command.RaiseCanExecuteChanged();
        }

        private bool AddUser_CanExecute(object obj)
        {
            if (this.Mod_MainWindow.Add_FirstName == string.Empty || this.Mod_MainWindow.Add_LastName == string.Empty)
            {

                return false;
            }
            else
            {
                return true;
            }
        }

        private async void AddUser_Executed(object obj)
        {
            bool new_user_result = await CreateNewUser(); 
            if(new_user_result)
            {
                ReadUsers();
            }
        }

        private bool ReadUsers_CanExecute(object obj)
        {
            return true;
        }

        private async void ReadUsers_Executed(object obj)
        {
            await Task.Run(() => { ReadUsers(); });
        }

        #endregion

        #region Operations

        private async void ReadUsers()
        {
            string result_users_json = await GetRequestDataAsync(new Uri($"http://localhost:9000/api/users"));
            this.Mod_MainWindow.List = JsonConvert.DeserializeObject<List<User>>(result_users_json);
        }

        private async Task<bool> CreateNewUser()
        {
            HttpStatusCode result_code =  await PostRequestDataAsync(new Uri($"http://localhost:9000/api/users"), this.Mod_MainWindow.Add_FirstName, this.Mod_MainWindow.Add_LastName);
            if(result_code == HttpStatusCode.OK)
            {
                MessageBox.Show("User Correctly Inserted", "New User", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else
            {
                MessageBox.Show("Error During Insert", "Insert Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<string> GetRequestDataAsync(Uri fromUri)
        {
            using (var _client = new HttpClient())
            using (var msg = new HttpRequestMessage(HttpMethod.Get, fromUri))
            using (var resp = await _client.SendAsync(msg))
            {
                resp.EnsureSuccessStatusCode();
                return await resp.Content.ReadAsStringAsync();
            }
        }

        public async Task<HttpStatusCode> PostRequestDataAsync(Uri fromUri, string firstName, string lastName)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("firstName", firstName),
                new KeyValuePair<string, string>("lastName", lastName)
            });

            var myHttpClient = new HttpClient();
            var response = await myHttpClient.PostAsync(fromUri, formContent);
            return response.StatusCode;
        }

        #endregion
    }
}
