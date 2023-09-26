using System.ComponentModel;
using System.Windows.Input;

namespace TCPExample
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string selectedMode;
        private string localIP;
        private string localPort;
        private string remoteIP;
        private string remotePort;
        private string logText;
        private string messageText;

        public MainViewModel()
        {
            this.SelectedMode = "TCP Server"; // Set default mode
            this.LogText = "Log messages will appear here.";
            this.ConnectCommand = new RelayCommand(this.Connect);
            this.SendCommand = new RelayCommand(this.Send);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string SelectedMode
        {
            get => this.selectedMode;
            set
            {
                if (this.selectedMode != value)
                {
                    this.selectedMode = value;
                    this.OnPropertyChanged(nameof(this.SelectedMode));
                }
            }
        }

        public string LocalIP
        {
            get => this.localIP;
            set
            {
                if (this.localIP != value)
                {
                    this.localIP = value;
                    this.OnPropertyChanged(nameof(this.LocalIP));
                }
            }
        }

        public string LocalPort
        {
            get => this.localPort;
            set
            {
                if (this.localPort != value)
                {
                    this.localPort = value;
                    this.OnPropertyChanged(nameof(this.LocalPort));
                }
            }
        }

        public string RemoteIP
        {
            get => this.remoteIP;
            set
            {
                if (this.remoteIP != value)
                {
                    this.remoteIP = value;
                    this.OnPropertyChanged(nameof(this.RemoteIP));
                }
            }
        }

        public string RemotePort
        {
            get => this.remotePort;
            set
            {
                if (this.remotePort != value)
                {
                    this.remotePort = value;
                    this.OnPropertyChanged(nameof(this.RemotePort));
                }
            }
        }

        public string LogText
        {
            get => this.logText;
            set
            {
                if (this.logText != value)
                {
                    this.logText = value;
                    this.OnPropertyChanged(nameof(this.LogText));
                }
            }
        }

        public string MessageText
        {
            get => this.messageText;
            set
            {
                if (this.messageText != value)
                {
                    this.messageText = value;
                    this.OnPropertyChanged(nameof(this.MessageText));
                }
            }
        }

        public ICommand ConnectCommand { get; private set; }
        public ICommand SendCommand { get; private set; }

        private void Connect(object parameter)
        {
            // Implement your connection logic here based on the selected mode (TCP Server or TCP Client)
            // Update the LogText property with connection status and messages
        }

        private void Send(object parameter)
        {
            // Implement your send message logic here
            // Update the LogText property with sent messages
        }
    }
}
