using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
namespace Elections
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly  string[] SK_ELECTIONS_PATHS = { "SK_elections/pres2014.csv", "SK_elections/pres2019.csv", "SK_elections/pres2004.csv", "SK_elections/pres2009.csv", "SK_elections/pres1999.csv", "SK_elections/parl1994.csv", "SK_elections/parl1998.csv", "SK_elections/parl2002.csv", "SK_elections/parl2006.csv", "SK_elections/parl2010.csv", "SK_elections/parl2012.csv", "SK_elections/parl2016.csv", "SK_elections/parl2020.csv" };

        List<Election> elections = new();
        List<Election> displayed = new();
        private bool about = false;
        private Election? selectedElection;

        public MainWindow()
        {
            InitializeComponent();
            for (int i = DateTime.Now.Year; i > 1992; i--)
            {
                comboBox.Items.Add(i);
            }
            comboBox.SelectedIndex = 0;
            results.Items.Clear();
        }
        private void New(object sender, RoutedEventArgs e)
        {

            if (elections.Count != 0)
            {
                if (MessageBox.Show("Do you really want to drop loaded elections?", "New", MessageBoxButton.OKCancel, MessageBoxImage.None, MessageBoxResult.Cancel) == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            comboBox.SelectedIndex = 0;
            results.Items.Clear();
            elections.Clear();
            displayed.Clear();
            about = false; 
            selectedElection = null;

        }
        private void Load(object? sender, RoutedEventArgs? e)
        {
            int success = 0;
            int not = 0;
            foreach(string s in SK_ELECTIONS_PATHS)
            {
                if (Open(null, null, "SK_elections/pres2014.csv"))
                {
                    success++;
                }
                else
                {
                    not++;
                }
            }
            editBox.Text = "Elections loaded: " + success + "\n" + "not loaded: " + not;
            
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            if (selectedElection != null)
            {
                SaveFileDialog saveFileDialog = new()
                {
                    Title = "Save CSV File",
                    Filter = "CSV files (*.csv)|*.csv"
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        string path = saveFileDialog.FileName;
                        string text = selectedElection.Date.ToString("dd.MM.yyyy");
                        if (selectedElection.GetType() == typeof(ParliamentaryElection))
                        {
                            text += ",parliamentary\n";
                        }
                        else
                        {
                            text += ",presidential\n";
                        }
                        for (int i = 0; i < selectedElection.Results.Keys.Count; i++)
                        {
                            text += $"{selectedElection.Results.ElementAt(i).Key},{selectedElection.Results.ElementAt(i).Value}\n";
                        }
                        if (selectedElection.GetType() == typeof(PresidentialElection))
                        {
                            text += "second round\n";
                            PresidentialElection elect = ((PresidentialElection)selectedElection);
                            for (int i = 0; i < elect.Second.Keys.Count; i++)
                            {
                                text += $"{elect.Second.ElementAt(i).Key},{elect.Second.ElementAt(i).Value}\n";
                            }
                        }
                        File.WriteAllText(path, text);
                        editBox.Text = "Election saved.";
                    }
                    catch (Exception)
                    {
                        editBox.Text = "An error occurred while saving file";
                        editBox.Text = "Election not saved.";
                    }
                }
            }
        }
        private void Open(object sender, EventArgs e)
        {
            Open(sender, e, null);
        }
        private bool Open(object? sender, EventArgs? e, string? filePath)
        {
            
            string path = string.Empty;
            if (filePath == null)
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "CSV files (*.csv)|*.csv"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    path = openFileDialog.FileName;
                }
            }
            else
            {
                path = filePath;
            }
            try
            {
                StreamReader sr = new(path);
                string? line;
                string[] row = new string[2];
                List<string> parties = new();
                List<int> votes = new();
                List<string> parties2 = new();
                List<int> votes2 = new();
                string type = String.Empty;
                DateTime date = DateTime.Now;
                line = sr.ReadLine();
                if (line != null)
                {
                    row = line.Split(',');
                    date = DateTime.Parse(row[0]);
                    type = row[1];
                }
                int c = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    row = line.Split(',');
                    if (row[0] == "second round")
                    {
                        c = 1;
                    }
                    else
                    {
                        switch (c)
                        {
                            case 0:
                                {
                                    parties.Add(row[0]);
                                    votes.Add(int.Parse(row[1]));
                                }
                                break;
                            default:
                                {
                                    parties2.Add(row[0]);
                                    votes2.Add(int.Parse(row[1]));
                                }
                                break;
                        }
                    }
                }
                if (type == "presidential")
                {
                    elections.Add(new PresidentialElection(date, parties.ToArray(), votes.ToArray(), parties2.ToArray(), votes2.ToArray()));
                }
                else
                {
                    elections.Add(new ParliamentaryElection(date, parties.ToArray(), votes.ToArray()));
                }
                editBox.Text = "Election loaded.";
                return true;
            }
            catch (Exception)
            {
                editBox.Text = "Election not loaded.";
                return false;
            }

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Help(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Elections\n\nVersion 1.0\nCopyright (c) Peter Krajkovič\n\nThis is an app for editing and viewing election results.", "About application");
        }


        private void Remove(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to remove selected election?", "Remove", MessageBoxButton.OKCancel, MessageBoxImage.None, MessageBoxResult.Cancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            foreach (var el in elections)
            {
                if (el == selectedElection)
                {
                    elections.Remove(el);
                    YearChanged(null, null);
                    editBox.Text = "Election removed";
                    return;
                }

            }
        }

        private void Coalitions(object sender, RoutedEventArgs e)
        {
            if ((string)coalitionsButton.Content == "First round")
            {
                if ((selectedElection != null && displayed.Count > 0))
                {
                    results.Items.Clear();
                    foreach (var item in ((PresidentialElection)selectedElection).FirstRound())
                    {
                        results.Items.Add(item);
                    }
                    backButton.IsEnabled = true;
                    about = true;
                }

            }
            else
            {
                if (selectedElection != null)
                {
                    Coalitions dialog = new(selectedElection.Results);
                    dialog.ShowDialog();
                }
            }
        }

        private void AboutElection(object sender, RoutedEventArgs e)
        {
            if (selectedElection != null)
            {
                results.Items.Clear();
                foreach(var item in selectedElection.About())
                {
                    results.Items.Add(item);
                }
                backButton.IsEnabled = true;
                about = true;
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            YearChanged(null, null);
            backButton.IsEnabled = false;
            selectedElection = null;
            about = false;
            ShowAll(null, null);
        }

        private void Clear(object? sender, EventArgs? e)
        {
            results.Items.Clear();
            aboutButton.IsEnabled = false;
            addVotesButton.IsEnabled = false;
            subVotesButton.IsEnabled = false;
            addPartyButton.IsEnabled = false;
            removePartyButton.IsEnabled = false;
            removeButton.IsEnabled = false;
            saveButton.IsEnabled = false;
            coalitionsButton.IsEnabled = false;
            editBox.Text = null;
            backButton.IsEnabled = false;
            about = false;
            selectedElection = null;
            displayed.Clear();
        }

        private void NewElection(object sender, EventArgs e)
        {
            NewElection dialog = new();
            dialog.ShowDialog();
            if (dialog.SelectedDate != null && dialog.Type != null)
            {
                if (dialog.Type == "presidential")
                {
                    elections.Add(new PresidentialElection((DateTime)dialog.SelectedDate));
                }
                else
                {
                    elections.Add(new ParliamentaryElection((DateTime)dialog.SelectedDate));
                }
                YearChanged(null, null);
                editBox.Text = "Election created.";
            }
            else
            {
                editBox.Text = "Election not created.";
            }
        }

        private void YearChanged(object? sender, EventArgs? e)
        {
            Clear(null, null);
            int count = 0;
            for (int i = 0; i < elections.Count; i++)
            {

                if (elections.ElementAt(i).Date.Year == DateTime.Now.Year - comboBox.SelectedIndex)
                {
                    count++;
                    results.Items.Add(elections.ElementAt(i).ToString());
                    displayed.Add(elections.ElementAt(i));
                }
            }
            if (count == 0)
            {
                results.Items.Add("No elections associated \nwith this year were found.");
            }
        }

        private void ElectionSelected(object? sender, EventArgs? e)
        {
            if (about)
            {
                results.SelectedItem = null;
                return;
            }
            int index = results.SelectedIndex;
            if (index > -1 && index < displayed.Count && displayed.Count > 0)
            {
                aboutButton.IsEnabled = true;
                addVotesButton.IsEnabled = true;
                subVotesButton.IsEnabled = true;
                addPartyButton.IsEnabled = true;
                removePartyButton.IsEnabled = true;
                removeButton.IsEnabled = true;
                saveButton.IsEnabled = true;
                selectedElection = displayed[index];
                if (selectedElection != null)
                {
                    coalitionsButton.IsEnabled = true;
                    if (selectedElection.GetType() == typeof(ParliamentaryElection))
                    {
                        coalitionsButton.Content = "Parliament";
                        addPartyButton.Content = "Add party";
                        removePartyButton.Content = "Remove party";
                    }
                    else
                    {
                        coalitionsButton.Content = "First round";
                        addPartyButton.Content = "Add candidate";
                        removePartyButton.Content = "Remove candidate";
                    }
                }
            }
        }

        private void ShowAll(object? sender, EventArgs? e)
        {
            Clear(null, null);
            List<Election> sortedElections = elections.OrderByDescending(el => el.Date).ToList();
            foreach (var el in sortedElections)
            {
                results.Items.Add(el);
                displayed.Add(el);
            }
        }

        private void AddVotes(object sender, RoutedEventArgs e)
        {
            if (selectedElection != null)
            {
                VotesChange dialog = new(selectedElection.Type, selectedElection.Parties());
                dialog.ShowDialog();
                if (dialog.NumberOfVotes > 0 && dialog.Party != string.Empty)
                {
                    selectedElection.AddVotes(dialog.NumberOfVotes, dialog.Party);
                    if (selectedElection.GetType() == typeof(PresidentialElection))
                    {
                        ((PresidentialElection)selectedElection).Update();
                    }
                    YearChanged(null, null);
                    editBox.Text = "Votes were added.";
                }
                else
                {
                    editBox.Text = "Votes were not added.";
                }
            }
        }

        private void SubVotes(object sender, RoutedEventArgs e)
        {
            if (selectedElection != null)
            {
                VotesChange dialog = new(selectedElection.Type, selectedElection.Parties());
                dialog.ShowDialog();
                if (dialog.NumberOfVotes > 0 && dialog.Party != string.Empty)
                {
                    if (selectedElection.SubVotes(dialog.NumberOfVotes, dialog.Party) == false)
                    {
                        editBox.Text = "Votes were not subtracted.\nNegative votes occurred.";
                        return;
                    }
                    if (selectedElection.GetType() == typeof(PresidentialElection))
                    {
                        ((PresidentialElection)selectedElection).Update();
                    }
                    YearChanged(null, null);
                    editBox.Text = "Votes were subtracted.";
                }
                else
                {
                    editBox.Text = "Votes were not subtracted.";
                }
            }
        }

        private void AddParty(object sender, RoutedEventArgs e)
        {
            if (selectedElection != null)
            {
                AddParty dialog = new(selectedElection.Type);
                dialog.ShowDialog();
                if (dialog.Party != string.Empty)
                {
                    if (selectedElection.Results.ContainsKey(dialog.Party))
                    {
                        MessageBox.Show("Name " + dialog.Party + " already exists in current election.");
                        editBox.Text = "Add not successful.";
                        return;
                    }
                    selectedElection.Results.Add(dialog.Party, dialog.NumberOfVotes);
                    if (selectedElection.GetType() == typeof(PresidentialElection))
                    {
                        ((PresidentialElection)selectedElection).Update();
                    }
                    YearChanged(null, null);
                    editBox.Text = "Add successful.";
                }
                else
                {
                    editBox.Text = "Add not successful.";
                }
            }
        }

        private void RemoveParty(object sender, RoutedEventArgs e)
        {
            if (selectedElection != null)
            {
                RemoveParty dialog = new(selectedElection.Type, selectedElection.Parties());
                dialog.ShowDialog();
                if (dialog.Party != string.Empty)
                {
                    selectedElection.Results.Remove(dialog.Party);
                    if (selectedElection.GetType() == typeof(PresidentialElection))
                    {
                        ((PresidentialElection)selectedElection).Update();
                    }
                    YearChanged(null, null);
                    editBox.Text = "Remove successful.";
                    if (selectedElection.GetType() == typeof(PresidentialElection))
                    {
                        if (((PresidentialElection)selectedElection).Second.ContainsKey(dialog.Party))
                        {
                            ((PresidentialElection)selectedElection).Second.Remove(dialog.Party);
                            ((PresidentialElection)selectedElection).Update();
                        }
                    }
                }
                else
                {
                    editBox.Text = "Remove not successful.";
                }
            }
        }
        
    }
}