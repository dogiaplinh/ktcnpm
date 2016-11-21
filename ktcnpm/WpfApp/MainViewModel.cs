﻿using Core;
using Core.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace WpfApp
{
    public class MainViewModel : BindableBase
    {
        private double discountRate = 10; // tính theo đơn vị %
        private Node root;
        private object selected;
        private ObservableCollection<RouteItem> routes = new ObservableCollection<RouteItem>();
        private int selectedRoute = -1;

        public int SelectedRoute
        {
            get { return selectedRoute; }
            set
            {
                ClearSelectedRoute(root);
                if (SetProperty(ref selectedRoute, value) && selectedRoute >= 0)
                {
                    Selected = null;
                    int index = 0;
                    SelectRoute(Routes[selectedRoute].Paths.Reverse<int>().ToList(), root, ref index);
                }
            }
        }

        private void SelectRoute(List<int> list, Node node, ref int index)
        {
            node.Select = true;
            if (node.Type == NodeType.Decision)
            {
                int current = index;
                if (node.Paths.Count == 0)
                    return;
                int currentBranch = current < list.Count ? list[current] : -1;
                int i;
                for (i = 0; i < node.Paths.Count; i++)
                {
                    if (node.Paths[i].Id == currentBranch)
                        break;
                }
                node.Paths[i].Select = true;
                index++;
                SelectRoute(list, node.Paths[i].Target, ref index);
            }
            else
            {
                foreach (var item in node.Paths)
                {
                    item.Select = true;
                    SelectRoute(list, item.Target, ref index);
                }
            }
        }

        private void ClearSelectedRoute(Node root)
        {
            root.Select = false;
            foreach (var item in root.Paths)
            {
                item.Select = false;
                ClearSelectedRoute(item.Target);
            }
        }

        public ObservableCollection<RouteItem> Routes
        {
            get { return routes; }
        }

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
            set
            {
                if(SetProperty(ref selected, value) && value!=null)
                {
                    SelectedRoute = -1;
                }
            }
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
        }

        private void CalculateNpv()
        {
            // Doan nay de test
            List<RouteItem> routes = Npv.ListAllPaths(root);
            Routes.Clear();
            foreach (RouteItem r in routes)
            {
                double npv = Npv.CalculateNpv(root, r);
                Console.WriteLine(r.ToString() + ": " + npv);
                r.Npv = npv;
                Routes.Add(r);
            }
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