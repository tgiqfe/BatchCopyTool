using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace BatchCopyTool
{
    internal class MachineAccount : INotifyPropertyChanged
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DomainName { get; set; }

        private string _rawPassword = null;

        [JsonIgnore]
        public string RawPassword
        {
            get => _rawPassword;
            set
            {
                _rawPassword = value;
                OnPropertyChanged();
            }
        }

        #region Inotify change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
