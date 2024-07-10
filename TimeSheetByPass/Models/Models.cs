using Newtonsoft.Json;

namespace TimeSheetByPass.Models
{

    public class EmployeeAPIModel
    {
        [JsonProperty(PropertyName = "organizationUId", NullValueHandling = NullValueHandling.Ignore)]
        public string OrganizationUId { get; set; }

        // Employee UId
        [JsonProperty(PropertyName = "uId", NullValueHandling = NullValueHandling.Ignore)]
        public string UId { get; set; }

        [JsonProperty(PropertyName = "userUId", NullValueHandling = NullValueHandling.Ignore)]
        public string UserUId { get; set; }


        [JsonProperty(PropertyName = "firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }


        [JsonProperty(PropertyName = "lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }



    }

    public class TimeSheet 
    {
        public TimeSheet()
        {
            TaskSlots = new List<Slots>();
        }

        [JsonProperty(PropertyName = "organizationUId", NullValueHandling = NullValueHandling.Ignore)]
        public string OrganizationUId { get; set; }


        [JsonProperty(PropertyName = "uId", NullValueHandling = NullValueHandling.Ignore)]
        public string UId { get; set; }


        [JsonProperty(PropertyName = "employeeUId", NullValueHandling = NullValueHandling.Ignore)]

        public string EmployeeUId { get; set; }

        [JsonProperty(PropertyName = "employeeId", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeId { get; set; }


        [JsonProperty(PropertyName = "shiftUId", NullValueHandling = NullValueHandling.Ignore)]
        public string ShiftUId { get; set; }


        [JsonProperty(PropertyName = "employeeName", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeName { get; set; }

        [JsonProperty(PropertyName = "shiftStartTime", NullValueHandling = NullValueHandling.Ignore)]
        public string ShiftStartTime { get; set; }

        [JsonProperty(PropertyName = "shiftEndTime", NullValueHandling = NullValueHandling.Ignore)]
        public string ShiftEndTime { get; set; }

        [JsonProperty(PropertyName = "firstCheckedInTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime FirstCheckedInTime { get; set; }

        [JsonProperty(PropertyName = "lastCheckedOutTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastCheckedOutTime { get; set; }

        [JsonProperty(PropertyName = "date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }


        [JsonProperty(PropertyName = "day", NullValueHandling = NullValueHandling.Ignore)]
        public string Day { get; set; }

        [JsonProperty(PropertyName = "shiftDateString", NullValueHandling = NullValueHandling.Ignore)]
        public string ShiftDateString { get; set; }


        [JsonProperty(PropertyName = "submittedOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime SubmittedOn { get; set; }


        [JsonProperty(PropertyName = "taskSlots", NullValueHandling = NullValueHandling.Ignore)]
        public List<Slots> TaskSlots { get; set; }

        [JsonProperty(PropertyName = "dayStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string DayStatus { get; set; }

        [JsonProperty(PropertyName = "apiSecurityKey", NullValueHandling = NullValueHandling.Ignore)]
        public string ApiSecurityKey { get; set; }
    }


    public class TimeSheetDateModel
    {
        [JsonProperty(PropertyName = "employeeUId", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeUId { get; set; }

        [JsonProperty(PropertyName = "dateList", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> DateList { get; set; }
    }



    public class Slots
    {
        [JsonProperty(PropertyName = "projectUId", NullValueHandling = NullValueHandling.Ignore)]
        public string ProjectUId { get; set; }


        [JsonProperty(PropertyName = "projectName", NullValueHandling = NullValueHandling.Ignore)]
        public string ProjectName { get; set; }


        [JsonProperty(PropertyName = "taskUId", NullValueHandling = NullValueHandling.Ignore)]
        public string TaskUId { get; set; }


        [JsonProperty(PropertyName = "timeSlots", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeSlots { get; set; }

        [JsonProperty(PropertyName = "from", NullValueHandling = NullValueHandling.Ignore)]
        public string From { get; set; }

        [JsonProperty(PropertyName = "to", NullValueHandling = NullValueHandling.Ignore)]
        public string To { get; set; }

        [JsonProperty(PropertyName = "tasks", NullValueHandling = NullValueHandling.Ignore)]
        public string Tasks { get; set; }


        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

    }


    public class LoginModel
    {
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "refreshToken")]
        public string RefreshToken { get; set; }
    }

    public class LoginResponse
    {
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public string token_type { get; set; }
        public string message { get; set; }
        public string refresh_token { get; set; }
    }

}
