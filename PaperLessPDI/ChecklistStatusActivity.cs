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
using static PaperLessPDI.Models.ApiModel;
using PaperLessPDI.Models;
using Newtonsoft.Json;
using PaperLessPDI.Adapter;
using Android.Support.V7.App;

namespace PaperLessPDI
{
    [Activity(Label = "ChecklistStatusActivity")]
    public class ChecklistStatusActivity : AppCompatActivity
    {
        TextView tvBarcodeItem;
        ListView mListView;
        WebHelper _objHelper = new WebHelper();
        List<CheckListStatusModel> objCheckListStatusModel = new List<CheckListStatusModel>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ChecklistStatuslayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            tvBarcodeItem = FindViewById<TextView>(Resource.Id.lblBarcodeNameRemark);
            mListView = FindViewById<ListView>(Resource.Id.listViewCheckListStatus);
            tvBarcodeItem.Text = StatusModel.BarcodeItem;
        }


        public async void GetCheckListStatus()
        {
            CheckListStatusModel _objCheckListStatusModel = new CheckListStatusModel();
            try
            {
                
                _objCheckListStatusModel.CheckListId = StatusModel.ChecklistID;
                _objCheckListStatusModel.Barcode = StatusModel.BarcodeItem;

                string Url = StatusModel.Url + "GetChecklistItemStatusByBarcode";

                var PostString = JsonConvert.SerializeObject(_objCheckListStatusModel);
                var requestTemp = await _objHelper.MakePostRequest(Url, PostString, true);

                objCheckListStatusModel = JsonConvert.DeserializeObject<List<CheckListStatusModel>>(requestTemp);

           
                // Toast.MakeText(this, ResultApiModel.msg.ToString(), ToastLength.Long).Show();
                if (objCheckListStatusModel != null)
                {
                    mListView.Adapter = new ChecklistStatusAdapter(this, objCheckListStatusModel);
                }
                else
                {
                    string sMessage = _objCheckListStatusModel.msg.ToString();
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                    alert.SetTitle("PaperLess PDI Says:");
                    alert.SetMessage(sMessage);
                    alert.SetNeutralButton("OK", (senderAlert, args) =>
                    {
                        this.Finish();
                        Intent intent = new Intent(this, typeof(HomeActivity));
                        this.StartActivity(intent);
                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
                }

            }
            catch (Exception ex)
            {
                string sMessage = ex.ToString();
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("PaperLess PDI Says:");
                alert.SetMessage(sMessage);
                alert.SetNeutralButton("OK", (senderAlert, args) =>
                {
                });

                Dialog dialog = alert.Create();
                dialog.Show();
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
            SupportActionBar.SetTitle(Resource.String.CheckListItemRemarkDetails);
            GetCheckListStatus();
            base.OnResume();
        }
    }
}