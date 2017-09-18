using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using PaperLessPDI.Models;
using Java.Lang;
using Android.Text;
using System.Linq;
using Android.Graphics;

namespace PaperLessPDI.Adapter
{
    public class CheckListAdapter : BaseAdapter
    {

        Activity context;
        List<ApiModel> list;
        public CheckListAdapter(Activity _context, List<ApiModel> _list)
                : base()
        {
            this.context = _context;
            this.list = _list;                  
        }

        public override int Count
        {
            get
            {
                return list.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder;
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = context.LayoutInflater.Inflate(Resource.Layout.CkeckListItemList, null);

                holder.tvSrNo = convertView.FindViewById<TextView>(Resource.Id.lblChecklistSrNo);
                holder.tvCheckListItemName = convertView.FindViewById<TextView>(Resource.Id.lblChecklistItemName);
                holder.edRemark = convertView.FindViewById<EditText>(Resource.Id.txtRemark);
                holder.chkOk = convertView.FindViewById<CheckBox>(Resource.Id.chkOk);

                convertView.Tag = (holder);
                convertView.SetTag(Resource.Id.lblChecklistSrNo, holder.tvSrNo);
                convertView.SetTag(Resource.Id.lblChecklistItemName, holder.tvCheckListItemName);
                convertView.SetTag(Resource.Id.txtRemark, holder.edRemark);
                convertView.SetTag(Resource.Id.chkOk, holder.chkOk);
                    
            }

            else
            {
                holder = (ViewHolder)convertView.Tag;
            }

            holder.edRemark.Tag = position;
            holder.edRemark.Visibility = ViewStates.Visible;

            holder.chkOk.Tag = position;
            holder.chkOk.Visibility = ViewStates.Visible;

            holder.edRemark.AfterTextChanged += (sender, args) =>
            {
                if (holder.tvSrNo.Text == (position + 1).ToString())
                {
                    var s = holder.edRemark.Text.ToString();
                    list[position].Remark = args.Editable.ToString();
                }
            };

            if (list[position].ProcessCheckListId != 0)
            {
                holder.chkOk.SetOnCheckedChangeListener(new CheckChangeListner(list, convertView, this));
            }
            //if (list[position].Remark.ToString() != "")
            //    if (holder.tvSrNo.Text == (position + 1).ToString())
            //    {
            //        {
            //            convertView.SetBackgroundResource(Resource.Drawable.listview_selector_even);
            //        }
            //}

            holder.tvSrNo.Text = (position + 1).ToString();
            holder.tvCheckListItemName.Text = list[position].CheckListName == null ? "" : list[position].CheckListName.ToString();
            holder.edRemark.Text = list[position].Remark == null ? "" : list[position].Remark.ToString();
            holder.chkOk.Checked = list[position].isSelected();        
            return convertView;
        }

        /** Holds planet data. */

        private class ViewHolder : Java.Lang.Object
        {
            public TextView tvSrNo { get; set; }
            public TextView tvCheckListItemName { get; set; }
            public EditText edRemark { get; set; }
            public CheckBox chkOk { get; set; }
        }

    }
}