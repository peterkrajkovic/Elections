using System;
using System.Collections.Generic;
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

namespace Elections
{
    /// <summary>
    /// Interaction logic for NewElection.xaml
    /// </summary>
    public partial class NewElection : Window
    {
        public DateTime? SelectedDate { get; private set; }
        public string? Type { get; private set; }
        public NewElection()
        {
            InitializeComponent();
        }
        private void ok(object? sender, EventArgs? e)
        {
            if (datePicker.SelectedDate != null)
            {
                SelectedDate = datePicker.SelectedDate.Value;
                if(comboBox.SelectedIndex == 0)
                {
                    Type = "presidential";
                }
                else
                {
                    Type = "parliamentary";
                }
                this.Close();
            }
            else
            {
                datePicker.BorderBrush = Brushes.Red;
                datePicker.Focus();
            }
        }
    }
}
