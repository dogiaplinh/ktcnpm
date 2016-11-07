using Core.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace WpfApp
{
    public class MainViewModel : BindableBase
    {
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
        }

        public Node Root
        {
            get { return root; }
            set { SetProperty(ref root, value); }
        }

        public object Selected
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

        public ICommand SaveTreeCommand { get; private set; }

        public ICommand LoadTreeCommand { get; private set; }

        private void SaveTree()
        {
            string s = JsonConvert.SerializeObject(Root);
            using (var writer = new StreamWriter("save.json"))
            {
                writer.Write(s);
            }
        }

        public void LoadTree()
        {
            string str;
            using (var reader = new StreamReader("save.json"))
            {
                str = reader.ReadToEnd();
            }
            var node = JsonConvert.DeserializeObject<Node>(str);
            Root = node;
        }
    }
}