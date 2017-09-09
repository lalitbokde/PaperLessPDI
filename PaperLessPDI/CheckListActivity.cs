using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Newtonsoft.Json;
using PaperLessPDI.Adapter;
using PaperLessPDI.Models;
using System.Linq;

namespace PaperLessPDI
{
    [Activity(Label = "CheckListActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class CheckListActivity : AppCompatActivity
    {
        TextView tvBarcodeItem;
        Button btnSave, btnAccept, btnDecline;
        ProgressDialog progressDialog;
        WebHelper _objHelper = new WebHelper();
        List<GetAllDepartmentNameListModel> ResultGetAllDepartmentEmployeeNameModel;
        GetUserDetailsByTokenNoModel ResultGetUserDetailsByTokenNoModel;
        BarcodeReturnData ResultApiModel;
        List<ApiModel> _objlistApiModel;
        ApiModel _objApiModel = new ApiModel();
        GetAllDepartmentNameListModel CheckListNo;
        ListView mListView;
        String sBarcodeItem;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Checklistlayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);


            mListView = FindViewById<ListView>(Resource.Id.listViewCheckList);
            tvBarcodeItem = FindViewById<TextView>(Resource.Id.lblBarcodeItemNameCLL);
            btnAccept = FindViewById<Button>(Resource.Id.btnAcceptCLL);
            btnDecline = FindViewById<Button>(Resource.Id.btnDeclineCLL);

            tvBarcodeItem.Text = StatusModel.BarcodeItem;
            sBarcodeItem = tvBarcodeItem.Text.Trim();
            //sBarcodeItem = StatusModel.BarcodeItem.Substring(2,3);
            BarcodeCheck();
            mListView.ItemClick += MListView_ItemClick;
            btnAccept.Click += BtnAccept_Click;
            btnDecline.Click += BtnDecline_Click;
            mListView.ItemLongClick += MListView_ItemLongClick;
        }

        private void MListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            _objApiModel = (_objlistApiModel.ElementAt(e.Position));
            StatusModel.ChecklistID = _objApiModel.CheckListId;
            Intent intent = new Intent(this, typeof(ChecklistStatusActivity));
            this.StartActivity(intent);
        }

        CheckListStatusModel objCheckListStatusModel = new CheckListStatusModel();
        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
          
        }

        private void BtnDecline_Click(object sender, EventArgs e)
        {
            bool AllChecked = false;
            int ti = mListView.Count;

            for (int i = 0; i < ti; i++)
            {
                if (ResultApiModel.checkListItem[i].isSelected())
                {
                    //AllChecked = false;

                }
                else
                {
                    if (ResultApiModel.checkListItem[i].Remark.ToString() == "")
                    {
                        string sMessage = "Enter First Remark For" + " SrNo: " + (i + 1) + "Checklist Name: " + ResultApiModel.checkListItem[i].CheckListName.ToString();
                        Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                        alert.SetTitle("PaperLess PDI Says:");
                        alert.SetMessage(sMessage);
                        alert.SetNeutralButton("OK", (senderAlert, args) =>
                        {
                            AllChecked = false;
                            return;
                        });

                        Dialog dialog = alert.Create();
                        dialog.Show();
                    }
                    else
                    {
                        AllChecked = true;
                    }

                }
            }


            if (AllChecked)
            {
                //Code For Update List
                progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Data Inserting...</font>"), true);

                List<ApiModel> _objApiModelList = new List<ApiModel>();
                for (int i = 0; i < ti; i++)
                {
                    bool Selected = false;

                    if (ResultApiModel.checkListItem[i].isSelected())
                    {
                        Selected = true;
                    }
                    else
                    {
                        Selected = false;
                    }

                    _objApiModel = new ApiModel();


                    _objApiModel.ProcessCheckListId = ResultApiModel.checkListItem[i].ProcessCheckListId;
                    _objApiModel.ProcessId = ResultApiModel.checkListItem[i].ProcessId;
                    _objApiModel.CheckListId = ResultApiModel.checkListItem[i].CheckListId;
                    _objApiModel.CheckListName = ResultApiModel.checkListItem[i].CheckListName;
                    _objApiModel.UserId = StatusModel.UserID;
                    _objApiModel.DepartmentId = ResultApiModel.checkListItem[i].DepartmentId;
                    _objApiModel.Status = Selected;
                    _objApiModel.Remark = ResultApiModel.checkListItem[i].Remark;
                    _objApiModel.Date = ResultApiModel.checkListItem[i].Date;
                    _objApiModel.PermanentDelete = ResultApiModel.checkListItem[i].PermanentDelete;
                    _objApiModelList.Add(_objApiModel);
                }

                BarcodeReturnData _objBarcodeReturnData = new BarcodeReturnData();
                if (ResultGetUserDetailsByTokenNoModel.DepartmentName == "Manufacturer")
                {
                    _objBarcodeReturnData.ProcessAcceptManufacturer = false;
                    _objBarcodeReturnData.ProcessAcceptQuality = ResultApiModel.ProcessAcceptQuality;
                }
                else if (ResultGetUserDetailsByTokenNoModel.DepartmentName == "Quality")
                {
                    _objBarcodeReturnData.ProcessAcceptManufacturer = ResultApiModel.ProcessAcceptManufacturer;
                    _objBarcodeReturnData.ProcessAcceptQuality = false;
                }

                _objBarcodeReturnData.checkListItem = _objApiModelList;
                UpdateCheckListItem(_objBarcodeReturnData);

                progressDialog.Hide();
            }

            else
            {
                //string sMessage = "Enter First Remark For" + " SrNo: " + (i + 1) + "Checklist Name: " + ResultApiModel.checkListItem[i].CheckListName.ToString();
                //Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                //alert.SetTitle("PaperLess PDI Says:");
                //alert.SetMessage(sMessage);
                //alert.SetNeutralButton("OK", (senderAlert, args) =>
                //{
                //    AllChecked = false;
                //    return;
                //});

                //Dialog dialog = alert.Create();
                //dialog.Show();
            }


        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            bool AllChecked = false;
            int ti = mListView.Count;
            int nti = 0;
            for (int i = 0; i < ti; i++)
            {
                if (ResultApiModel.checkListItem[i].isSelected())
                {
                    nti++;
                    if (ti == nti)
                    {
                        AllChecked = true;
                    }
                    else
                    { AllChecked = false; }
                }
                else
                {
                    AllChecked = false;
                }
            }


            if (AllChecked)
            {
                //Code For Update List
                progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Data Inserting...</font>"), true);


                List<ApiModel> _objApiModelList = new List<ApiModel>();
                for (int i = 0; i < ti; i++)
                {
                    bool Selected = false;

                    if (ResultApiModel.checkListItem[i].isSelected())
                    {
                        Selected = true;
                    }
                    else
                    {
                        Selected = false;
                    }

                    _objApiModel = new ApiModel();

                    _objApiModel.ProcessCheckListId = ResultApiModel.checkListItem[i].ProcessCheckListId;
                    _objApiModel.ProcessId = ResultApiModel.checkListItem[i].ProcessId;
                    _objApiModel.CheckListId = ResultApiModel.checkListItem[i].CheckListId;
                    _objApiModel.CheckListName = ResultApiModel.checkListItem[i].CheckListName;
                    _objApiModel.UserId = StatusModel.UserID;
                    _objApiModel.DepartmentId = ResultApiModel.checkListItem[i].DepartmentId;
                    _objApiModel.Status = Selected;
                    _objApiModel.Remark = ResultApiModel.checkListItem[i].Remark;
                    _objApiModel.Date = ResultApiModel.checkListItem[i].Date;
                    _objApiModel.PermanentDelete = ResultApiModel.checkListItem[i].PermanentDelete;
                    _objApiModelList.Add(_objApiModel);
                }

                BarcodeReturnData _objBarcodeReturnData = new BarcodeReturnData();
                if (ResultGetUserDetailsByTokenNoModel.DepartmentName == "Manufacturer")
                {
                    _objBarcodeReturnData.ProcessAcceptManufacturer = true;
                    _objBarcodeReturnData.ProcessAcceptQuality = ResultApiModel.ProcessAcceptQuality;
                }
                else if (ResultGetUserDetailsByTokenNoModel.DepartmentName == "Quality")
                {
                    _objBarcodeReturnData.ProcessAcceptManufacturer = ResultApiModel.ProcessAcceptManufacturer;
                    _objBarcodeReturnData.ProcessAcceptQuality = true;
                }
                _objBarcodeReturnData.checkListItem = _objApiModelList;
                UpdateCheckListItem(_objBarcodeReturnData);

                progressDialog.Hide();
            }

            else
            {
                string sMessage = "Check List not Eligible For Accept this Request";
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("PaperLess PDI Says:");
                alert.SetMessage(sMessage);
                alert.SetNeutralButton("OK", (senderAlert, args) =>
                {
                    return;
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }

        public async void UpdateCheckListItem(BarcodeReturnData _objBarcodeReturnData)
        {
            try
            {
                string Url = StatusModel.Url + "CheckListUpdateOnCheckBoxClickApi";

                var PostString = JsonConvert.SerializeObject(_objBarcodeReturnData);
                var requestTemp = await _objHelper.MakePostRequest(Url, PostString, true);
                var _objResponce = JsonConvert.DeserializeObject<ResponseMessege>(requestTemp);

                if (_objResponce.success == 1)
                {
                    string sMessage = _objResponce.msg.ToString();
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
                else
                {
                    string sMessage = _objResponce.msg.ToString();
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

            catch (Exception ex)
            {
                string sMessage = ex.ToString().ToString();
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


        public async void BarcodeCheck()
        {
            try
            {
                UserLoginModel _objUserLoginModel = new UserLoginModel();
                _objUserLoginModel.UserTokenNo = StatusModel.UserTokenNo;
                //_objUserLoginModel.UserTokenNo = 2;
                string Url = StatusModel.Url + "GetUserDetailsByTokenNo";

                var PostString = JsonConvert.SerializeObject(_objUserLoginModel);
                var requestTemp = await _objHelper.MakePostRequest(Url, PostString, true);
                ResultGetUserDetailsByTokenNoModel = new GetUserDetailsByTokenNoModel();
                ResultGetUserDetailsByTokenNoModel = JsonConvert.DeserializeObject<GetUserDetailsByTokenNoModel>(requestTemp);


                SendBarcodeAndUserIDModel _objSendBarcodeAndUserID = new SendBarcodeAndUserIDModel();
                _objSendBarcodeAndUserID.BarcodeItem = sBarcodeItem.Trim();
                _objSendBarcodeAndUserID.UserId = ResultGetUserDetailsByTokenNoModel.UserId;
                StatusModel.UserID = ResultGetUserDetailsByTokenNoModel.UserId;
                string Url1 = StatusModel.Url + "BarcodeCheck";

                var PostString1 = JsonConvert.SerializeObject(_objSendBarcodeAndUserID);
                var requestTemp1 = await _objHelper.MakePostRequest(Url1, PostString1, true);
                ResultApiModel = JsonConvert.DeserializeObject<BarcodeReturnData>(requestTemp1);

                // Toast.MakeText(this, ResultApiModel.msg.ToString(), ToastLength.Long).Show();
                if (ResultApiModel.checkListItem != null)
                {
                    _objlistApiModel = ResultApiModel.checkListItem;
                    mListView.Adapter = new CheckListAdapter(this, ResultApiModel.checkListItem);
                }
                else
                {
                    string sMessage = ResultApiModel.msg.ToString();
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

        public async void GetUserDetailsByTokenNo()
        {
            try
            {
                UserLoginModel _objUserLoginModel = new UserLoginModel();
                _objUserLoginModel.UserTokenNo = StatusModel.UserTokenNo;

                string Url = StatusModel.Url + "GetUserDetailsByTokenNo";

                progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Checking User Info...</font>"), true);

                var PostString = JsonConvert.SerializeObject(_objUserLoginModel);
                var requestTemp = await _objHelper.MakePostRequest(Url, PostString, true);
                ResultGetUserDetailsByTokenNoModel = new GetUserDetailsByTokenNoModel();
                ResultGetUserDetailsByTokenNoModel = JsonConvert.DeserializeObject<GetUserDetailsByTokenNoModel>(requestTemp);
                progressDialog.Hide();
            }
            catch (Exception ex)
            {
                progressDialog.Hide();
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
            SupportActionBar.SetTitle(Resource.String.CheckListDetails);
            base.OnResume();
        }
    }
}