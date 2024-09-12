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
		console.log("Sucessfully login for user: ", username);
		baseurl = "https://prod-shiftattendance-api.huhoka.com";
		endpoint = "/api/Shift/CheckInShift";

		const checkinModel = { customTime: new Date().toISOString() };
		await makePostRequest(baseurl, endpoint, checkinModel, accessToken);
		console.log("Sucessfully Checkin for user: ", username);

		const employee = await getEmployeeByToken(accessToken);
		console.log("Sucessfully get employee: ", username);

		baseurl = "https://prod-shiftattendance-api.huhoka.com";
		endpoint = "/api/TimeSheet/GetTimeSheetsByEmployeeUId";

		const getTimesheetModel = {
			dateList: [new Date().toISOString()],
			employeeUId: employee.uId,
		};

		const timesheetResponse = await makePostRequest(baseurl, endpoint, getTimesheetModel, accessToken);
		console.log("Sucessfully fetch timesheet for user: ", username);

		const timesheetModelList = timesheetResponse;
		console.log(timesheetModelList[0]);
		const timesheetModel = timesheetModelList[0];
		if (timesheetModel.taskSlots === null) {
			timesheetModel.taskSlots = [];
		}
		let projectName = "";
		let projectUId = "";
		let MeetingTask = [];
		if (username.toLowerCase() === "gaurav.pekhale@centralogic.net") {
			projectName = "EXP Document AI";
			projectUId = "e0fa113c-e7ca-4cc4-a1f9-23b19b4c8c85";
			MeetingTask.push({
				projectUId: "e0fa113c-e7ca-4cc4-a1f9-23b19b4c8c85",
				taskUId: "",
				projectName: "EXP Document AI",
				tasks: "Daily Standup Call with Celia",
				type: "Free Task",
				from: "19:30",
				to: "20:00",
			});
			MeetingTask.push({
				projectUId: "e0fa113c-e7ca-4cc4-a1f9-23b19b4c8c85",
				taskUId: "",
				projectName: "EXP Document AI",
				tasks: "Task Center And Document AI Dev Interaction Call",
				type: "Free Task",
				from: "20:00",
				to: "20:30",
			});
		}

		timesheetModel.taskSlots.push(
			{
				projectName: projectName,
				projectUId: projectUId,
				taskUId: " ",
				type: "Free Task",
				timeSlots: " ",
				tasks: "                  ",
				from: "10:30",
				to: "11:30",
			},
			{
				projectName: projectName,
				projectUId: projectUId,
				taskUId: " ",
				type: "Free Task",
				timeSlots: " ",
				tasks: " .                 ",
				from: "11:30",
				to: "13:30",
			},
			{
				projectName: projectName,
				projectUId: projectUId,
				taskUId: " ",
				type: "Free Task",
				timeSlots: " ",
				tasks: "Lunch  Break  ",
				from: "13:30",
				to: "14:30",
			},
			{
				projectName: projectName,
				projectUId: projectUId,
				taskUId: " ",
				type: "Free Task",
				timeSlots: " ",
				tasks: ".      ",
				from: "14:30",
				to: "16:30",
			},
			{
				projectName: projectName,
				projectUId: projectUId,
				taskUId: " ",
				type: "Free Task",
				timeSlots: " ",
				tasks: " .            ",
				from: "16:30",
				to: "18:30",
			},
			{
				projectName: projectName,
				projectUId: projectUId,
				taskUId: " ",
				type: "Free Task",
				timeSlots: " ",
				tasks: " .            ",
				from: "18:30",
				to: "19:00",
			}
		);

		if (MeetingTask.length > 0) {
			MeetingTask.forEach((element) => {
				timesheetModel.taskSlots.push(element);
			});
		}
		endpoint = "/api/TimeSheet/UpdateTimeSheet";
		await makePostRequest(baseurl, endpoint, timesheetModel, accessToken);
		console.log("Sucessfully pdated timesheet for user: ", username);

		res.send(`
			<html>
		<head>
			<title>Timesheet Update</title>
		</head>
		<body>
			<h1>Timesheet Updated Successfully</h1>
			<p>Hello, <strong> ${username}</strong>! Your timesheet has been updated <span style="color: green;"> successfully.<span/></p>
		</body>
	</html>
	
		`);
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
