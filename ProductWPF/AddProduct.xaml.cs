using ProductWPF.API;
using ProductWPF.DTO;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ProductWPF
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window, INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<StatusDTO> Statuses { get; set; }
        public StatusDTO SelectedStatus { get; set; }

        public AddProduct()
        {
            InitializeComponent();
            DataContext = this;
            LoadStatuses();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private async Task LoadStatuses()
        {
            var client = new Client();
            Statuses = await client.GetStatuses();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Statuses)));
        }

        private async void Save(object sender, RoutedEventArgs e)
        {
            await Client.Instance.AddProductAsync(new ProductDTO
            {
                Name = Name,
                Description = Description,
                IdStatus = SelectedStatus.Id,
                Status = SelectedStatus.Name
            });
            Close();
        }

        private async void Back(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
