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
    /// Interaction logic for AddParty.xaml
    /// </summary>
    public partial class AddParty : Window
    {
        public int NumberOfVotes { get; private set; }
        public string Party { get; private set; } = string.Empty;
        public AddParty(string type)
        {
            InitializeComponent();
            if (type == "parliamentary")
            {
                textBox.Text = "Name of the party:";
            }
            else
            {
                textBox.Text = "Name of the candidate:";
            }
        }
        private void Ok(object? sender, EventArgs? e)
        {
            int num;
            if (number.Text != null && int.TryParse(number.Text, out num))
            {
                if (party.Text != null && party.Text != string.Empty)
                {
                    NumberOfVotes = num;
                    Party = party.Text;
                    this.Close();
                } 
                else
                {
                    party.BorderBrush = Brushes.Red;
                    number.BorderBrush = Brushes.Black;
                }
            }
            else
            {
                number.BorderBrush = Brushes.Red;
            }
        }
    }
}
