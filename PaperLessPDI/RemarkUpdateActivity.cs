using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using PaperLessPDI.Models;
using Newtonsoft.Json;

namespace PaperLessPDI
{
    [Activity(Label = "RemarkUpdateActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RemarkUpdateActivity : AppCompatActivity
    {
        public ApiModel ListData;
        Button btnSaveUpdate;
        EditText edUpdateRemark;
        WebHelper _objHelper = new WebHelper();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RemarkUpdatelayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            btnSaveUpdate = FindViewById<Button>(Resource.Id.btnCheckListUpdate);
            edUpdateRemark = FindViewById<EditText>(Resource.Id.txt_UpdateRemark);
            

            ListData = JsonConvert.DeserializeObject<ApiModel>(Intent.GetStringExtra("RemarkUpdate") ?? "Data not available");
            btnSaveUpdate.Click += BtnSaveUpdate_Click;
        }

        private async void BtnSaveUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ApiModel datafalse = new ApiModel();
                datafalse.ProcessCheckListId = ListData.ProcessCheckListId;
                datafalse.ProcessId = ListData.ProcessId;
                datafalse.CheckListId = ListData.CheckListId;
                datafalse.UserId = StatusModel.UserID;
                datafalse.DepartmentId = ListData.DepartmentId;
                datafalse.Status = false;
                datafalse.Remark = edUpdateRemark.Text.Trim();
                datafalse.Date = ListData.Date;
                datafalse.PermanentDelete = ListData.PermanentDelete;

                string UrlFalse = StatusModel.Url + "CheckListUpdateOnCheckBoxClickApi";

                var PostStringFalse = JsonConvert.SerializeObject(datafalse);
                var requestTempfalse = await _objHelper.MakePostRequest(UrlFalse, PostStringFalse, true);
                var _objResponceFalse = JsonConvert.DeserializeObject<ResponseMessege>(requestTempfalse);

                Finish();
            }

            catch (Exception ex)
            {
                string ErrorMsg = ex.ToString();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.UpdateRemark);
            edUpdateRemark.Text = ListData.Remark.ToString();
            edUpdateRemark.RequestFocus();
            base.OnResume();
        }
    }
}