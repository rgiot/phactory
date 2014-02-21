using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Project.View
{
    public partial class FindDialog : Form
    {
        private ListView listView;
        private int curIndex = 0;

        public FindDialog(ListView listView)
        {
            InitializeComponent();

            this.listView = listView;

            filter.Focus();
        }

        private void BtnFindNext_Click(object sender, EventArgs e)
        {
            StringComparison comparisonType = GetCase();

            int startIndex = curIndex;
            int iItem;

            for (iItem = startIndex + 1; iItem < listView.Items.Count; iItem++)
            {
                String column0 = listView.Items[iItem].Text;

                if (column0.IndexOf(filter.Text, comparisonType) >= 0)
                {
                    SelectItem(iItem);
                    return;
                }
            }

            for (iItem = 0; iItem < startIndex; iItem++)
            {
                String column0 = listView.Items[iItem].Text;

                if (column0.IndexOf(filter.Text, comparisonType) >= 0)
                {
                    if ( iItem != startIndex )
                    {
                        SelectItem(iItem);
                    }
                    
                    return;
                }
            }
        }

        private void SelectItem( int itemIndex )
        {
            listView.SelectedItems.Clear();

            listView.Items[itemIndex].Selected = true;
            listView.EnsureVisible(itemIndex);

            curIndex = itemIndex;                    
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private StringComparison GetCase()
        {
            StringComparison comparisonType = StringComparison.CurrentCultureIgnoreCase;
            if (ignoreCase.Checked)
            {
                comparisonType = StringComparison.CurrentCulture;
            }

            return comparisonType;
        }

        private void StartFind()
        {
            StringComparison comparisonType = GetCase();

            curIndex = 0;

            for (int iItem = 0; iItem < listView.Items.Count; iItem++)
            {
                String column0 = listView.Items[iItem].Text;

                if (column0.IndexOf(filter.Text, comparisonType) >= 0)
                {
                    SelectItem(iItem);

                    BtnFindNext.Enabled = true;

                    return;
                }
            }

            listView.SelectedItems.Clear();
            BtnFindNext.Enabled = false;
        }

        private void filter_TextChanged(object sender, EventArgs e)
        {
            StartFind();
        }

        private void ignoreCase_CheckedChanged(object sender, EventArgs e)
        {
            StartFind();
        }

        private void filter_KeyDown(object sender, KeyEventArgs e)
        {
            if ( e.KeyCode == Keys.Escape )
            {
                Close();
            }
        }
    }
}
