using ProductWPF.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using ProductWPF.DTO;

namespace ProductWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public List<StatusDTO> Status { get; set; }
        public ObservableCollection<ProductDTO> Products { get; set; }
        public ProductDTO SelectedProduct { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadStatuses();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async Task LoadStatuses()
        {
            Client client = new Client();
            await LoadStatuses(client);
        }

        private async Task LoadStatuses(Client client)
        {
            Products = new ObservableCollection<ProductDTO>(await client.GetProducts());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Products)));
        }

        private async void Edit(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            SelectedProduct = b.Tag as ProductDTO;
            new EditProduct(SelectedProduct).ShowDialog();
            LoadStatuses();

        }

        private async void Delete(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            SelectedProduct = b.Tag as ProductDTO;
            await Client.Instance.DeleteProduct(SelectedProduct.Id);
            await LoadStatuses();
        }

        private async void Add(object sender, RoutedEventArgs e)
        {
            new AddProduct().ShowDialog();
            Close();
        }
    }
}
