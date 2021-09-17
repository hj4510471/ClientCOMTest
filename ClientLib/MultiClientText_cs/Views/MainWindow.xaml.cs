using MultiClientText_cs.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MultiClientText_cs.Views
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

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //https://blog.naver.com/luckyboyphs/20203583182
            var listViewItem = (ListViewItem)ClientListView.ItemContainerGenerator.ContainerFromItem(((Button)sender).DataContext);

            ClientData testItemModel = listViewItem.Content as ClientData;


        }
    }
}
