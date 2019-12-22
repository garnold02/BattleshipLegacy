using System.Threading.Tasks;
using System.Windows;
using BattleshipClient.Engine;

namespace BattleshipClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            Root.Start(TbxName.Text, TbxHostname.Text, TbxPort.Text);
            Close();
        }
    }
}
