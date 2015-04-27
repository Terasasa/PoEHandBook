//  ------------------------------------------------------------------ 
//  PoEHandbook
//  MainWindow.xaml.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System.Windows;
using PoEHandbook.Properties;

namespace PoEHandbook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FrmMain_Loaded(object sender, RoutedEventArgs e)
        {
            // Restore previous window position and size
            var pos = Settings.Default.WinPosition;
            var size = Settings.Default.WinSize;

            if (pos != System.Drawing.Point.Empty)
            {
                FrmMain.Left = pos.X;
                FrmMain.Top = pos.Y;
            }

            if (size != System.Drawing.Size.Empty)
            {
                FrmMain.Width = size.Width;
                FrmMain.Height = size.Height;
            }
        }

        private void FrmMain_Closed(object sender, System.EventArgs e)
        {
            // Save window position and size
            Settings.Default.WinPosition = new System.Drawing.Point((int) FrmMain.Left, (int) FrmMain.Top);
            Settings.Default.WinSize = new System.Drawing.Size((int) FrmMain.Width, (int) FrmMain.Height);
            Settings.Default.Save();
        }
    }
}