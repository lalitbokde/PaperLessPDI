using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using PaperLessPDI.Models;

namespace PaperLessPDI.Adapter
{
    internal class CheckChangeListner : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
    {
        private CheckListAdapter checkListAdapter;
        private View convertView;
        private List<ApiModel> list;

        public CheckChangeListner(List<ApiModel> list, View convertView, CheckListAdapter checkListAdapter)
        {
            this.list = list;
            this.convertView = convertView;
            this.checkListAdapter = checkListAdapter;
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            int getPosition = (int)buttonView.Tag;
            list[getPosition].setSelected(buttonView.Checked);
        }
    }
}