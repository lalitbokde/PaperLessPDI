using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;            
using Android.Graphics.Drawables;
using Newtonsoft.Json;
using Android.Views.InputMethods;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using PaperLessPDI.Models;
using ZXing.Mobile;

namespace PaperLessPDI
{
    [Activity(Label = "HomeActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class HomeActivity : AppCompatActivity
    {
        MobileBarcodeScanner scanner;
        ImageButton btnBarCodeScan;
        DrawerLayout drawerLayout;
        String sUserName;
        TextView tvUserName;
        EditText edEnterBarcode;
        NavigationView navigationView;
        string ErorMsg = "";
        ListView mListView;

        WebHelper _objHelper = new WebHelper();
        List<GetAllDepartmentNameListModel> ResultGetAllDepartmentEmployeeNameModel;
        GetUserDetailsByTokenNoModel ResultGetUserDetailsByTokenNoModel;
        static List<ApiModel> ResultApiModel;
        String sBarcodeItem;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            MobileBarcodeScanner.Initialize(Application);
            SetContentView(Resource.Layout.Homelayout);

            scanner = new MobileBarcodeScanner();

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            edEnterBarcode = FindViewById<EditText>(Resource.Id.txt_EnterBarcode);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            // Create ActionBarDrawerToggle button and add it to the toolbar
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            View header = navigationView.GetHeaderView(0);

            btnBarCodeScan = FindViewById<ImageButton>(Resource.Id.imgbtnbarcodeScan);
            btnBarCodeScan.Click += BtnBarCodeScan_Click;
        }
        

        private async void BtnBarCodeScan_Click(object sender, EventArgs e)
        {
            if (edEnterBarcode.Text == "")
            {
                scanner.UseCustomOverlay = false;

                scanner.TopText = "Above the text";
                scanner.BottomText = "The following text";

                var result = await scanner.Scan();
                HandleScanResult(result);
            }
            else
            {
                ScanResult(edEnterBarcode.Text.Trim());
            }
        }


        void HandleScanResult(ZXing.Result result)
        {
            if (result == null || string.IsNullOrEmpty(result.Text))
            {
                this.RunOnUiThread(() =>
                {
                    Toast.MakeText(this, "Scan canceled！", ToastLength.Short).Show();
                });
                return;
            }
            else
            {
                this.RunOnUiThread(() =>
                {
                    Toast.MakeText(this, result.Text, ToastLength.Short).Show();
                    ScanResult(result.Text.Trim());
                });
            }
        }

        public void ScanResult(string BARCODEITEMNO)
        {
            try
            {
                this.Finish();
                StatusModel.BarcodeItem = BARCODEITEMNO;
                Intent intent = new Intent(this, typeof(CheckListActivity));
                this.StartActivity(intent);
            }

            catch (Exception e)
            {
                ErorMsg = e.ToString();
                Toast.MakeText(this, ErorMsg, ToastLength.Short).Show();
            }
        }


        void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            var ft = FragmentManager.BeginTransaction();
            switch (e.MenuItem.ItemId)
            {
                //case (Resource.Id.nav_about_us):

                //    StartActivity(new Intent(this, typeof(AboutUsActivity)));
                //    break;

                //case (Resource.Id.nav_contact):
                //    StartActivity(new Android.Content.Intent(this, typeof(ContactActivity)));
                //    break;
                //case (Resource.Id.nav_shareit):
                //    ShareToBrowser("https://play.google.com/store/search?q=appsthentic&hl=en");
                //    break;
                //case (Resource.Id.nav_settings):
                //    StartActivity(new Intent(this, typeof(ItemRateMasterActivity)));
                //    break;
            }
            // Close drawer
            drawerLayout.CloseDrawers();
        }

        //add custom icon to tolbar


        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount != 0)
            {
                FragmentManager.PopBackStack();// fragmentManager.popBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }


        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.home);
            base.OnResume();
        }
    }
}