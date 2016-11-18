using Core.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Core;

namespace WpfApp
{
    public class MainViewModel : BindableBase
    {
        private double discountRate = 10; // tính theo đơn vị %
        private Node root;
        private object selected;

        public MainViewModel()
        {
            Root = new Node
            {
                Type = NodeType.Decision
            };
            SaveTreeCommand = new DelegateCommand<object>(SaveTree);
            LoadTreeCommand = new DelegateCommand<object>(LoadTree);
            HidePaneCommand = new DelegateCommand<object>(HidePane);
            CalculateNpvCommand = new DelegateCommand<object>(CalculateNpv);
        }

        public ICommand CalculateNpvCommand { get; set; }

        public ICommand HidePaneCommand { get; set; }

        public ICommand LoadTreeCommand { get; private set; }

        public double DiscountRate
        {
            get { return discountRate; }
            set { SetProperty(ref discountRate, value); }
        }

        public Node Root
        {
            get { return root; }
            set { SetProperty(ref root, value); }
        }

        public ICommand SaveTreeCommand { get; private set; }

        public object Selected
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

        public void LoadTree()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "save"; // Default file name
            dlg.DefaultExt = ".json"; // Default file extension
            dlg.Filter = "JavaScript Object Notation File (.json)|*.json"; // Filter files by extension
            dlg.InitialDirectory = Environment.CurrentDirectory;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                string str;
                using (var reader = new StreamReader(dlg.FileName))
                {
                    str = reader.ReadToEnd();
                }
                var node = JsonConvert.DeserializeObject<Node>(str);
                Root = node;
            }
            Npv.ListAllPaths(Root);
        }

        private void CalculateNpv()
        {
        }

        private void HidePane()
        {
            Selected = null;
        }

        private void SaveTree()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "save"; // Default file name
            dlg.DefaultExt = ".json"; // Default file extension
            dlg.Filter = "JavaScript Object Notation File (.json)|*.json"; // Filter files by extension
            dlg.InitialDirectory = Environment.CurrentDirectory;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                string s = JsonConvert.SerializeObject(Root);
                using (var writer = new StreamWriter(filename))
                {
                    writer.Write(s);
                }
            }
        }
    }
}