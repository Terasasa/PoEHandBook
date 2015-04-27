//  ------------------------------------------------------------------ 
//  PoEHandbook
//  App.xaml.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System.Windows;
using PoEHandbook.Data;

namespace PoEHandbook
{
    public partial class App
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            // Load data before main window pops up
            DataAccess.LoadData();
        }
    }
}