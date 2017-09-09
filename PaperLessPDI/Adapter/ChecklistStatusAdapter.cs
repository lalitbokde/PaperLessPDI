using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using PaperLessPDI.Models;          

namespace PaperLessPDI.Adapter
{
    public class ChecklistStatusAdapter : BaseAdapter<CheckListStatusModel>
    {
        Activity context;
        List<CheckListStatusModel> list;
        int SrNo;

        public ChecklistStatusAdapter(Activity _context, List<CheckListStatusModel> _list)
                : base()
        {
            this.context = _context;
            this.list = _list;

        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override CheckListStatusModel this[int index]
        {
            get { SrNo = 1; return list[index]; }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.CkeckListItemStatusDetailsList, parent, false);

            CheckListStatusModel item = this[position];
            if (item != null)
            {
                string CheckListstatus = "", Department="";
                if (item.Status == false && item.Remark=="")
                {
                    CheckListstatus = "Not Attend";
                }
                else if (item.Status == false)
                {
                    CheckListstatus = "Not OK";
                }
                else if (item.Status == true)
                {
                    CheckListstatus = "OK";
                }

                if (item.DepartmentName == null)
                {
                    Department = "";
                }
                else if (item.DepartmentName == "Manufacturer")
                {
                    Department = "M";
                }
                else if (item.DepartmentName == "Quality")
                {
                    Department = "Q";
                }

                view.FindViewById<TextView>(Resource.Id.lblSSrNo).Text = (position + 1).ToString();
                view.FindViewById<TextView>(Resource.Id.lblSOperatorName).Text = item.UserName == null ? "" : item.UserName.ToString();
                view.FindViewById<TextView>(Resource.Id.lblSDepartmentName).Text = Department;
                view.FindViewById<TextView>(Resource.Id.lblStatus).Text = CheckListstatus.ToString();
                view.FindViewById<TextView>(Resource.Id.lblStatusRemark).Text = item.Remark == null ? "" : item.Remark.ToString(); 
       
            }
            return view;

        }
    }
}