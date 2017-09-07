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
using PaperLessPDI.Models;
using Newtonsoft.Json;
using Android.Graphics.Drawables;
using Android.Views.InputMethods;

namespace PaperLessPDI
{
    [Activity(Label = "PaperLess PDI", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LogInActivity : Activity
    {
        Button btnLogIN, btnExit;
        EditText txt_UserName, txt_Password;
        ProgressDialog progressDialog;
        WebHelper _objHelper = new WebHelper();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.LogInlayout);

            btnLogIN = FindViewById<Button>(Resource.Id.btnLogIn);
            btnExit = FindViewById<Button>(Resource.Id.btnExit);

            txt_UserName = FindViewById<EditText>(Resource.Id.txtUsername);
            txt_Password = FindViewById<EditText>(Resource.Id.txtPassword);

            btnLogIN.Click += BtnLogIN_Click;
            btnExit.Click += BtnExit_Click;
        }

        private async void BtnLogIN_Click(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);

            try
            {
                Drawable icon_error = Resources.GetDrawable(Resource.Drawable.alert);
                icon_error.SetBounds(0, 0, 40, 30);
                if (txt_UserName.Text != "")
                {
                    if (txt_Password.Text != "")
                    {
                        UserLoginModel _objUserLoginModel = new UserLoginModel();
                        StatusModel.UserTokenNo = Convert.ToInt32(txt_UserName.Text.Trim());
                        _objUserLoginModel.UserTokenNo = Convert.ToInt32(txt_UserName.Text.Trim());
                        _objUserLoginModel.Password = txt_Password.Text.Trim();

                        string Url = StatusModel.Url + "UserLogIn";

                        progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Checking User Info...</font>"), true);

                        var PostString = JsonConvert.SerializeObject(_objUserLoginModel);
                        var requestTemp = await _objHelper.MakePostRequest(Url, PostString, true);
                        ResponseMessege ResultgetRequest = JsonConvert.DeserializeObject<ResponseMessege>(requestTemp);

                        if (ResultgetRequest.success == 1)
                        {
                            clear();
                            progressDialog.Hide();
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetTitle("PaperLess PDI Says:");
                            alert.SetMessage(ResultgetRequest.msg.ToString());
                            alert.SetNeutralButton("OK", (senderAlert, args) =>
                            {
                                Intent intent = new Intent(this, typeof(HomeActivity));
                                this.StartActivity(intent);
                            });

                            Dialog dialog = alert.Create();
                            dialog.Show();
                            clear();
                            return;
                        }

                        else
                        {
                            progressDialog.Hide();
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetTitle("PaperLess PDI Says:");
                            alert.SetMessage(ResultgetRequest.msg.ToString());
                            alert.SetNeutralButton("OK", (senderAlert, args) =>
                            {

                            });

                            Dialog dialog = alert.Create();
                            dialog.Show();
                            clear();
                            return;
                        }

                    }
                    else
                    {
                        txt_Password.RequestFocus();
                        txt_Password.SetError("Please Enter Password First", icon_error);
                    }
                }
                else
                {

                    txt_UserName.RequestFocus();
                    txt_UserName.SetError("Please Enter UserName First", icon_error);
                }

            }
            catch (Exception ex)
            {
                progressDialog.Hide();
                string ErrorMsg = ex.ToString();
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("PaperLess PDI Says:");
                alert.SetMessage(ex.ToString());
                alert.SetNeutralButton("OK", (senderAlert, args) =>
                {
                   
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }

        public void AlertCreate(string sMessage)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("PaperLess PDI Says:");
            alert.SetMessage(sMessage);
            alert.SetNeutralButton("OK", (senderAlert, args) =>
            {
                Intent intent = new Intent(this, typeof(HomeActivity));
                this.StartActivity(intent);
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }


        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Finish();
        }


        //protected override void OnResume()
        //{
        //    SupportActionBar.SetTitle(Resource.String.home);
        //    base.OnResume();
        //}

        public void clear()
        {
            txt_Password.Text = "";
            txt_UserName.Text = "";
            txt_UserName.RequestFocus();
        }
    }
}