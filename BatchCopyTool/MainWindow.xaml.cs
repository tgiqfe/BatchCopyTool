using MaterialDesignThemes.Wpf;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BatchCopyTool
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _passwordDebounceTimer;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Item.Setting;

            this.Text_Password.Password = Item.Setting.DefaultAccount.RawPassword;

            /*
            //  initialize password field.
            this.Text_Password.Password = Item.Setting.DefaultAccount?.GetRawPassword();
            _passwordDebounceTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _passwordDebounceTimer.Tick += (sender, e) =>
            {
                _passwordDebounceTimer?.Stop();
                Item.Setting.DefaultAccount.SetRawPassword(this.Text_Password.Password);
            };
            Text_Password.PasswordChanged += (sender, e) =>
            {
                _passwordDebounceTimer?.Stop();
                _passwordDebounceTimer?.Start();
            };
            */
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Item.Setting.DefaultAccount.SetRawPassword(this.Text_Password.Password);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
            }
        }


        private bool _isViewPassword = false;
        private void Label_ViewPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isViewPassword = !_isViewPassword;
            Icon_ViewPassword.Kind = _isViewPassword ?
                PackIconKind.EyeCheck :
                PackIconKind.EyeCheckOutline;
            Raw_Password.Visibility = _isViewPassword ?
                Visibility.Visible :
                Visibility.Hidden;
            Text_Password.Visibility = _isViewPassword ?
                Visibility.Hidden :
                Visibility.Visible;
            //Raw_Password.Text = Item.Setting.DefaultAccount.GetRawPassword();
        }

        private void Text_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Item.Setting.DefaultAccount.RawPassword = Text_Password.Password;
        }
    }
}