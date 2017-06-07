using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using TemsParser.Models.Config;

namespace TemsParser.ViewModels.Config
{
    public class TreeViewItemViewModel: ViewModelBase
    {
        private bool _isSelected;

        private bool _isExpanded;

        private string _name;

        public readonly ObjectTypes ObjectType;

        public readonly bool ContainsFixedValues;

        private readonly Dictionary<ObjectTypes, string> ObjectTypesToDisplay = new Dictionary<ObjectTypes, string>()
        {
            {ObjectTypes.Region, "Pегион"},
            {ObjectTypes.Operator, "Оператор"},
            {ObjectTypes.Technology, "Технология"},
            {ObjectTypes.Freq, "Частоты"}
        };


        public TreeViewItemViewModel(ObjectTypes objectType)
        {
            Children = new ObservableCollection<TreeViewItemViewModel>();
            ObjectType = objectType;
        }


        //public ObjectTypes Type { get; set; }

        public int Id { get; set; }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string TypeToDisplay
        {
            get { return ObjectTypesToDisplay[ObjectType]; }
        }

        public ObservableCollection<TreeViewItemViewModel> Children { get; set; }

        public TreeViewItemViewModel Parent { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { _isExpanded = value; OnPropertyChanged(); }
        }
    }
}
