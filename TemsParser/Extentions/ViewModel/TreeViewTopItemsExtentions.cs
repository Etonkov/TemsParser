using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using TemsParser.ViewModels.Config;

namespace TemsParser.Extentions.ViewModel
{
    public static class TreeViewTopItemsExtentions
    {
        /// <summary>
        /// Selects treeViewTopItem in treeViewTopItems collection and expands all of its parents.
        /// </summary>
        /// <param name="treeViewTopItems"></param>
        /// <param name="itemId">Id of the treeViewTopItem.</param>
        /// <param name="expandSelected">Determines whether expand the selected treeViewTopItem.</param>
        public static void FocusOn(this ObservableCollection<TreeViewItemViewModel> treeViewTopItems, int itemId, bool expandSelected = false)
        {
            var itemInTree = treeViewTopItems
                                    .Traverse(item => item.Children)
                                    .First(item => (item.Id == itemId));

            itemInTree.IsSelected = true;
            itemInTree.ExpandAllParents();

            if (expandSelected)
            {
                itemInTree.IsExpanded = true;
            }
        }

        /// <summary>
        /// Expands all parents of treeViewItem.
        /// </summary>
        /// <param name="treeViewItemViewModel"></param>
        private static void ExpandAllParents(this TreeViewItemViewModel treeViewItem)
        {
            if (treeViewItem.Parent != null)
            {
                treeViewItem.Parent.IsExpanded = true;
                treeViewItem.Parent.ExpandAllParents();
            }
        }
    }
}
