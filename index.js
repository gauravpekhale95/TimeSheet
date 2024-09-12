const express = require("express");
const axios = require("axios");
const bodyParser = require("body-parser");

const app = express();
app.use(bodyParser.json());

app.get("/api/Script", async (req, res) => {
	const { username, password } = req.query;

	try {
		let baseurl = "https://prod-usermgmt-api.huhoka.com";
		let endpoint = "/api/User/LogIn";

		const loginRequest = {
			UserName: username,
			Password: password,
		};

		const loginResponse = await makePostRequest(baseurl, endpoint, loginRequest);
		const accessToken = loginResponse.access_token;

		baseurl = "https://prod-shiftattendance-api.huhoka.com";
		endpoint = "/api/Shift/CheckInShift";

		const checkinModel = { customTime: new Date().toISOString() };
		await makePostRequest(baseurl, endpoint, checkinModel, accessToken);

		const employee = await getEmployeeByToken(accessToken);

		baseurl = "https://prod-shiftattendance-api.huhoka.com";
		endpoint = "/api/TimeSheet/GetTimeSheetsByEmployeeUId";

		const getTimesheetModel = {
			DateList: [new Date().toISOString()],
			EmployeeUId: employee.UId,
		};

		const timesheetResponse = await makePostRequest(baseurl, endpoint, getTimesheetModel, accessToken);
		const timesheetModelList = timesheetResponse;
		console.log(timesheetModelList[0]);
		const timesheetModel = timesheetModelList[0];
		timesheetModel.TaskSlots.push(
			{ ProjectName: "", ProjectUId: "", TaskUId: " ", Type: "", TimeSlots: " ", Tasks: "                  ", From: "11:30", To: "13:30" },
			{ ProjectName: "", ProjectUId: "", TaskUId: " ", Type: "", TimeSlots: " ", Tasks: "Lunch  Break  ", From: "13:30", To: "14:30" },
			{ ProjectName: "", ProjectUId: "", TaskUId: " ", Type: "", TimeSlots: " ", Tasks: "      ", From: "14:30", To: "16:30" },
			{ ProjectName: "", ProjectUId: "", TaskUId: " ", Type: "", TimeSlots: " ", Tasks: "             ", From: "16:30", To: "18:30" },
			{ ProjectName: "", ProjectUId: "", TaskUId: " ", Type: "", TimeSlots: " ", Tasks: "             ", From: "18:30", To: "19:00" }
		);

		endpoint = "/api/TimeSheet/UpdateTimeSheet";
		await makePostRequest(baseurl, endpoint, timesheetModel, accessToken);

		res.json(timesheetResponse);
	} catch (error) {
		console.error(error);
		res.status(500).send(error.message);
	}
});

const makePostRequest = async (baseUrl, endpoint, apiRequestData, token = "", orgUId = "") => {
	try {
		const headers = { "Content-Type": "application/json" };
		if (token) headers["Authorization"] = `Bearer ${token}`;
		if (orgUId) headers["OrganizationUId"] = orgUId;

		const response = await axios.post(`${baseUrl}${endpoint}`, apiRequestData, { headers });
		console.log(response.data);
		return response.data;
	} catch (error) {
		console.error(`Error making POST request to ${baseUrl}${endpoint}:`, error.message);
		throw error;
	}
};

const getEmployeeByToken = async (token) => {
	const apiRequestData = {};
	const employeeResponse = await makePostRequest("https://prod-employee-api.huhoka.com", "/api/Employee/GetEmployeeByToken", apiRequestData, token);
	return employeeResponse;
};

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
	console.log(`Server is running on port ${PORT}`);
});
