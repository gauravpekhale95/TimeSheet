using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TimeSheetByPass.Models;

namespace TimeSheetByPass.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScriptController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> CheckIn(string username, string password)
        {
            var baseurl = "https://prod-usermgmt-api.huhoka.com";
            var endpoint = "/api/User/LogIn";

            var loginRequest = JsonConvert.SerializeObject(new LoginModel() { UserName = username , Password=password});
            var loginResponseString  = await MakePostRequest(baseurl, endpoint, loginRequest,"","");
            var loginResponse  = JsonConvert.DeserializeObject<LoginResponse>(loginResponseString);

            baseurl = "https://prod-shiftattendance-api.huhoka.com";
            endpoint = "/api/Shift/CheckInShift";

            var checkinModel = new  { customTime = DateTime.Today };
            var checkinRequest = JsonConvert.SerializeObject(checkinModel);
            var checkinResponse = await MakePostRequest(baseurl, endpoint, checkinRequest, loginResponse.access_token, "");
            //var resp = JsonConvert.DeserializeObject<LoginResponse>(loginResponseString);

            var employe  = await GetEmployeeByToken(loginResponse.access_token);

            endpoint = "/api/TimeSheet/GetTimeSheetsByEmployeeUId";

            var getTimesheetModel = new TimeSheetDateModel
            {
                DateList = new List<string> { DateTime.Today.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
                EmployeeUId = employe.UId,

            };

            var timesheetRequest = JsonConvert.SerializeObject(getTimesheetModel);
            var timesheetResponse = await MakePostRequest(baseurl, endpoint, timesheetRequest, loginResponse.access_token, "");
            var timesheetModelList = JsonConvert.DeserializeObject<List<TimeSheet>>(timesheetResponse);
            var timesheetModel = timesheetModelList.First();
            //timesheetModel.TaskSlots.Clear();


           
            timesheetModel.TaskSlots.Add(new Slots { ProjectName = "", ProjectUId = "", TaskUId = " ", Type = "", TimeSlots = " ", Tasks = "                  ", From = "11:30", To = "13:30" });
            timesheetModel.TaskSlots.Add(new Slots { ProjectName = "", ProjectUId = "", TaskUId = " ", Type = "", TimeSlots = " ", Tasks = "Lunch  Break  ", From = "13:30", To = "14:30" });
            timesheetModel.TaskSlots.Add(new Slots { ProjectName = "", ProjectUId = "", TaskUId = " ", Type = "", TimeSlots = " ", Tasks = "      ", From = "14:30", To = "16:30" });
            timesheetModel.TaskSlots.Add(new Slots { ProjectName = "", ProjectUId = "", TaskUId = " ", Type = "", TimeSlots = " ", Tasks = "             ", From = "16:30", To = "18:30" });
            timesheetModel.TaskSlots.Add(new Slots { ProjectName = "", ProjectUId = "", TaskUId = " ", Type = "", TimeSlots = " ", Tasks = "             ", From = "18:30", To = "19:00" });


            endpoint = "/api/TimeSheet/UpdateTimeSheet";
 
            timesheetRequest = JsonConvert.SerializeObject(timesheetModel);
            timesheetResponse = await MakePostRequest(baseurl, endpoint, timesheetRequest, loginResponse.access_token, "");
             

            return Ok(timesheetResponse);

        }

        private async Task<EmployeeAPIModel> GetEmployeeByToken(string token)
        {

                 var apiRequestData = JsonConvert.SerializeObject(new object());
                var HttpResponsee = await MakePostRequest("https://prod-employee-api.huhoka.com", "/api/Employee/GetEmployeeByToken", apiRequestData, token);

                var employeeAPiModel = JsonConvert.DeserializeObject<EmployeeAPIModel>(HttpResponsee);


                return employeeAPiModel;

            

        }

        public static async Task<string> MakePostRequest(string baseUrl, string endPoint, string apiRequestData, string token = "", string orgUId = "")
        {
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 10
            };


            using (HttpClient httpClient = new HttpClient(socketsHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                httpClient.BaseAddress = new Uri(baseUrl);
                StringContent apiRequestContent = new StringContent(apiRequestData, Encoding.UTF8, "application/json");

                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                if (!string.IsNullOrEmpty(orgUId))
                {
                    httpClient.DefaultRequestHeaders.Add("OrganizationUId", orgUId);
                }
                 
                var httpResponse = httpClient.PostAsync(endPoint, apiRequestContent).Result;
                var httpResponseString = httpResponse.Content.ReadAsStringAsync().Result;

                if (!httpResponse.IsSuccessStatusCode)
                {
                    // todo log failure response 
                    throw new Exception(httpResponseString);
                }
                return httpResponseString;
            }
        }

    }
}
