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
    /// Interaction logic for UpdateSecondxaml.xaml
    /// </summary>
    public partial class UpdateSecond : Window
    {
        public int Votes1 { get; set; } = 0;
        public int Votes2 { get; set; } = 0;

        public UpdateSecond(int pvotes1, int pvotes2, string pcand1, string pcand2)
        {
            InitializeComponent();
            votes1.Text = "" + pvotes1;
            votes2.Text = "" + pvotes2;
            cand1.Text = "Number of votes for " + pcand1;
            cand2.Text = "Number of votes for " + pcand2;
        }

        private void Ok(object? sender, EventArgs? e)
        {
            if (votes1.Text != "0" && int.TryParse(votes1.Text, out _))
            {
                if (votes2.Text != "0" && int.TryParse(votes2.Text, out _))
                {
                    Votes1 = Math.Abs(int.Parse(votes1.Text));
                    Votes2 = Math.Abs(int.Parse(votes2.Text));
                    this.Close();
                }
                else
                {
                    votes2.Focus();
                }
            }
            else
            {
                votes1.Focus();
            }
        }

    }
}
