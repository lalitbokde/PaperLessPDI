using System;
using Java.Lang;
using System.Collections.Generic;

namespace PaperLessPDI.Models
{
    public static class StatusModel
    {
        public static string Url = "http://192.168.0.5:8080/APIForMobile/";

        //public static string Url = "http://outletdealer.suvarnapp.com/APIForMobile/";
        public static int UserTokenNo;
        public static int UserID;
        public static string BarcodeItem = "";
        public static int ChecklistID;
    }

    public class ResponseMessege
    {
        public int success { get; set; }
        public string msg { get; set; }
    }

    public class UserLoginModel
    {
        public long UserTokenNo { get; set; }
        public string Password { get; set; }
    }

    public class GetAllDepartmentNameListModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public bool OK { get; set; }
        public bool NOTOK { get; set; }
    }

    public class GetUserDetailsByTokenNoModel
    {
        public int UserId { get; set; }
        public long UserTokenNo { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public System.DateTime Date { get; set; }
        public bool PermanentDelete { get; set; }
    }


    public class SendBarcodeAndUserIDModel
    {
        public int UserId { get; set; }
        public string BarcodeItem { get; set; }
    }

    public class BarcodeReturnData
    {
        public int success { get; set; }
        public string msg { get; set; }
        public bool? ProcessAcceptManufacturer { get; set; }
        public bool? ProcessAcceptQuality { get; set; }
        public List<ApiModel> checkListItem { get; set; }
    }

    public class ApiModel
    {
        public int ProcessCheckListId { get; set; }
        public int ProcessId { get; set; }
        public int CheckListId { get; set; }
        public string CheckListName { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public bool Status { get; set; }
        public string Remark { get; set; }
        public System.DateTime Date { get; set; }
        public bool PermanentDelete { get; set; }

        //private bool selected;
        public bool isSelected()
        {
            return Status;
        }
        public void setSelected(bool selected)
        {
            this.Status = selected;
        }

    }
    public class CheckListStatusModel
    {
        public string UserName { get; set; }
        public string DepartmentName { get; set; }
        public bool Status { get; set; }
        public string Remark { get; set; }
        public string Barcode { get; set; }
        public long CheckListId { get; set; }
        public int success { get; set; }
        public string msg { get; set; }
    }
}
