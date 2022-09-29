using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAppTest.Model
{
    internal class Model_MainWindow : ModelBase
    {
        #region Attributes
        ObservableCollection<User> list;
        string add_lastName;
        string add_firstName;
        #endregion

        #region Properties
        public ObservableCollection<User> List
        {
            get
            {
                return list;
            }
            set
            {
                list = value;
                OnPropertyChanged("");
            }
        }
        public string Add_LastName
        {
            get
            {
                return add_lastName;
            }
            set
            {
                add_lastName = value;
                OnPropertyChanged("");
            }
        }
        public string Add_FirstName
        {
            get
            {
                return add_firstName;
            }
            set
            {
                add_firstName = value;
                OnPropertyChanged("");
            }
        }
        #endregion

        #region Constructor
        public Model_MainWindow()
        {
            this.List = new ObservableCollection<User>();
            this.Add_LastName = String.Empty;
            this.Add_FirstName = String.Empty;
        }
        #endregion

    }

    internal class User
    {
        #region Attributes
        string fistName;
        string lastName;
        #endregion

        #region Properties
        [JsonProperty("firstName")]
        public string FirstName
        {
            get
            {
                return fistName;
            }
            set
            {
                fistName = value;
            }
        }
        [JsonProperty("lastName")]
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }

        public User()
        {

        }

        public User(string fistName,string lastName)
        {
            this.fistName = fistName;
            this.lastName = lastName;
        }


        #endregion
    }
}
