using ProductWPF.API;
using ProductWPF.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window, INotifyPropertyChanged
    {
        private ProductDTO selectedProduct;
        private StatusDTO selectedStatus;

        public List<StatusDTO> Statuses { get; set; }
        public StatusDTO SelectedStatus
        {
            get => selectedStatus;
            set
            {
                selectedStatus = value;
                Signal();
            }
        }
        
        public ProductDTO SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;

            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Signal([CallerMemberName] string prop = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));


        public EditProduct(ProductDTO product)
        {
            InitializeComponent();
            SelectedProduct = product;
            LoadStatuses();
            DataContext = this;
        }

        private async Task LoadStatuses()
        {
            var client = new Client();
            Statuses = await client.GetStatuses();
            SelectedStatus = Statuses.FirstOrDefault(s => s.Id == SelectedProduct.IdStatus);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Statuses)));
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Сохранение завершено");
                return;
            }
            SelectedProduct.IdStatus = SelectedStatus.Id;
            SelectedProduct.Status = SelectedStatus.Name;
            Client.Instance.EditProduct(SelectedProduct, SelectedProduct.Id);
            Close();
        }

        private async void Back(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
