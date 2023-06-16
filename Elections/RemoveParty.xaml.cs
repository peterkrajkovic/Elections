using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ChangeParty.xaml
    /// </summary>
    public partial class RemoveParty : Window
    {
        public string Party { get; private set; } = string.Empty;
        public RemoveParty(string type, string[] parties)
        {
            InitializeComponent();
            foreach (var item in parties)
            {
                comboBox.Items.Add(item);
            }
            if (type == "parliamentary")
            {
                textBox.Text = "Choose party:";
                
            }
            else
            {
                textBox.Text = "Choose candidate:";
            }
        }
        private void Ok(object? sender, EventArgs? e)
        {
            if (comboBox.SelectedItem != null)
            {
                Party = (string)comboBox.SelectedItem;
                this.Close();
            }
        }
    }
}
