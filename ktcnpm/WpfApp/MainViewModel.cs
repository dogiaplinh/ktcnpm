using Core.Models;
using System;
using System.Linq;

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
    }
}