using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using System.Net;


namespace iDashNet.Controllers
{
    public class AvailabilityController : Controller
    {
        private iDashNet.Models.AvailabilityModel model;


        public AvailabilityController(IOptions<List<AvailabilityApps>> availApps)
        {
            model = new Models.AvailabilityModel();
            model.AvailApps = new List<AvailabilityApps>();

        }




		// MyQL Specific Endpoints to check availability:
		//https://confluence/display/~BPatel1/MyQL+Midware+Sanity+Suite



		// BASE URLS:
		// On Prem:
		// TEST: https://apitest.rockfin.com/
		// BETA: https://apibeta.rockfin.com/
		// PROD: https://api.rockfin.com/

		// Cloud: 
		// https://apitest.quickenloans.com/
		// https://apibeta.quickenloans.com/
		// https://api.quickenloans.com/

		// END URLS:
		/*
            // post / application/json / message_body: '{"Property": {"Type": "noncondo","Value": 100000,"Address": {"AddressLine1": "109 MAIN","AddressLine2": "","City": "PLEVNA","State": "KS","Zip": "67568","ZipPlus4": "0000","County": "Reno"}}}'
            tax-assessment/assessments
            
            
            http://httpstat.us/200

        */







        private HttpResponseMessage LoansListTest()
        {
            var client = AvailabilityHelper.HttpClientSetup();

			var relativeUri = "https://myqlmwapi.rockfin.com/Loans/GetLoanListByAuthId/64671691";

			var Task = client.GetAsync(relativeUri);
			Task.Wait();
			var data = Task.Result;
			//var contentTask = data.Content.ReadAsStringAsync();
			//contentTask.Wait();
			//var jsonData = contentTask.Result;

            return data;
        }



        public IActionResult AvailabilityIndex()
        {
            /*
            // Intentional Failure:
            AvailabilityApps FailApp = new AvailabilityApps();
            FailApp.Name = "Intentional Failure";
            FailApp.AppStatus = HttpStatusCode.InternalServerError;
            model.AvailApps.Add(FailApp);
            */

            // RM BETA: Tax Assessment Example
            AvailabilityApps TaxAssApp = new AvailabilityApps();
            HttpResponseMessage TaxAssess = AvailabilityHelper.TaxAssessmentCheck();
            TaxAssApp.Name = "Dependency: Tax Assessment";
            TaxAssApp.AppStatus = TaxAssess.StatusCode;
            model.AvailApps.Add(TaxAssApp);

            // RM BETA: Tax Estimate Example
            AvailabilityApps TaxEstApp = new AvailabilityApps();
            HttpResponseMessage TaxEst = AvailabilityHelper.TaxAssessmentCheck();
            TaxEstApp.Name = "Dependency: Tax Estimate";
            TaxEstApp.AppStatus = TaxEst.StatusCode;
            model.AvailApps.Add(TaxEstApp);

			// RM BETA: Auth Example
			AvailabilityApps AuthApp = new AvailabilityApps();
			HttpResponseMessage Auth = AvailabilityHelper.AuthCheck();
            AuthApp.Name = "Dependency: Auth";
			AuthApp.AppStatus = Auth.StatusCode;
			model.AvailApps.Add(AuthApp);


            // RocketDyne
            AvailabilityApps RDyneApp = new AvailabilityApps();
            HttpResponseMessage RDyne = AvailabilityHelper.RocketDyneCheck();
            RDyneApp.Name = "Dependency: Rocket Dyne";
            RDyneApp.AppStatus = RDyne.StatusCode;
            model.AvailApps.Add(RDyneApp);

            // PiR
            AvailabilityApps PiRApp = new AvailabilityApps();
            HttpResponseMessage PiR = AvailabilityHelper.PiRCheck();
            PiRApp.Name = "Dependency: Price Is Right";
            PiRApp.AppStatus = PiR.StatusCode;
            model.AvailApps.Add(PiRApp);


			// RM BETA: AVS
			AvailabilityApps AVSApp = new AvailabilityApps();
			HttpResponseMessage AVS = AvailabilityHelper.AVSCheck();
            AVSApp.Name = "Dependency: AVS";
			AVSApp.AppStatus = AVS.StatusCode;
			model.AvailApps.Add(AVSApp);


            // MyQL Loan List Example
            AvailabilityApps LoanListApp = new AvailabilityApps();
            HttpResponseMessage LoansList = AvailabilityHelper.LoansListCheck();
            LoanListApp.Name = "MyQL: Loans List";
			LoanListApp.AppStatus = LoansList.StatusCode;
            model.AvailApps.Add(LoanListApp);

			// MyQL PendingActionCheck
			AvailabilityApps PendActApp = new AvailabilityApps();
			HttpResponseMessage PendAct = AvailabilityHelper.PendingActionCheck();
            PendActApp.Name = "MyQL: Pending Actions";
			PendActApp.AppStatus = PendAct.StatusCode;
			model.AvailApps.Add(PendActApp);

			// MyQL ServicingActivity
			AvailabilityApps SvcActApp = new AvailabilityApps();
			HttpResponseMessage SvcAct = AvailabilityHelper.SvcActCheck();
            SvcActApp.Name = "MyQL: Servicing Activity";
			SvcActApp.AppStatus = SvcAct.StatusCode;
			model.AvailApps.Add(SvcActApp);

            // MyQLQTweetConsumer
            AvailabilityApps MyQLQTweetConsumerApp = new AvailabilityApps();
            HttpResponseMessage MyQLQTweetConsumer = AvailabilityHelper.MyQLQTweetConsumerCheck();
            MyQLQTweetConsumerApp.Name = "MyQL: MyQLQTweetConsumer";
            MyQLQTweetConsumerApp.AppStatus = MyQLQTweetConsumer.StatusCode;
            model.AvailApps.Add(MyQLQTweetConsumerApp);

            // MyQLFrontendInterfaceCheck
            AvailabilityApps MyQLFrontendInterfaceCheckApp = new AvailabilityApps();
            HttpResponseMessage MyQLFrontendInterface = AvailabilityHelper.MyQLFrontendInterfaceCheck();
            MyQLFrontendInterfaceCheckApp.Name = "MyQL: MyQLFrontendInterface";
            MyQLFrontendInterfaceCheckApp.AppStatus = MyQLFrontendInterface.StatusCode;
            model.AvailApps.Add(MyQLFrontendInterfaceCheckApp);


            /* TODO: NOT WORKING CURRENTLY - INVESTIGATE
            // Token Generator
            AvailabilityApps TGenApp = new AvailabilityApps();
            HttpResponseMessage TGen = AvailabilityHelper.TGenCheck();
            TGenApp.Name = "Token Generator";
            TGenApp.AppStatus = TGen.StatusCode;
            model.AvailApps.Add(TGenApp);

            // Einstein Hub
            AvailabilityApps EHubApp = new AvailabilityApps();
            HttpResponseMessage EHub = AvailabilityHelper.EHubCheck();
            EHubApp.Name = "Einstein Hub";
            EHubApp.AppStatus = EHub.StatusCode;
            model.AvailApps.Add(EHubApp);
            */




            /*
            // Message Board
            AvailabilityApps XXXApp = new AvailabilityApps();
            HttpResponseMessage XXXAct = AvailabilityHelper.XXXActCheck();
            XXXApp.Name = "XXX";
            XXXApp.AppStatus = XXXAct.StatusCode;
            model.AvailApps.Add(XXXApp);

            // Submission Engine
            AvailabilityApps XXXApp = new AvailabilityApps();
            HttpResponseMessage XXXAct = AvailabilityHelper.XXXActCheck();
            XXXApp.Name = "XXX";
            XXXApp.AppStatus = XXXAct.StatusCode;
            model.AvailApps.Add(XXXApp);

            // DOC Conversion
            AvailabilityApps XXXApp = new AvailabilityApps();
            HttpResponseMessage XXXAct = AvailabilityHelper.XXXActCheck();
            XXXApp.Name = "XXX";
            XXXApp.AppStatus = XXXAct.StatusCode;
            model.AvailApps.Add(XXXApp);

            // UM Queue
            AvailabilityApps XXXApp = new AvailabilityApps();
            HttpResponseMessage XXXAct = AvailabilityHelper.XXXActCheck();
            XXXApp.Name = "XXX";
            XXXApp.AppStatus = XXXAct.StatusCode;
            model.AvailApps.Add(XXXApp);

            // Person Hub
            AvailabilityApps XXXApp = new AvailabilityApps();
            HttpResponseMessage XXXAct = AvailabilityHelper.XXXActCheck();
            XXXApp.Name = "XXX";
            XXXApp.AppStatus = XXXAct.StatusCode;
            model.AvailApps.Add(XXXApp);

            // QKS
            AvailabilityApps XXXApp = new AvailabilityApps();
            HttpResponseMessage XXXAct = AvailabilityHelper.XXXActCheck();
            XXXApp.Name = "XXX";
            XXXApp.AppStatus = XXXAct.StatusCode;
            model.AvailApps.Add(XXXApp);

            // Eternia
            AvailabilityApps XXXApp = new AvailabilityApps();
            HttpResponseMessage XXXAct = AvailabilityHelper.XXXActCheck();
            XXXApp.Name = "XXX";
            XXXApp.AppStatus = XXXAct.StatusCode;
            model.AvailApps.Add(XXXApp);

            // Rocket Accounts
            AvailabilityApps XXXApp = new AvailabilityApps();
            HttpResponseMessage XXXAct = AvailabilityHelper.XXXActCheck();
            XXXApp.Name = "XXX";
            XXXApp.AppStatus = XXXAct.StatusCode;
            model.AvailApps.Add(XXXApp);
            */

			return View(model);
        }
    }
}



/*
 * //Tax-Assessment
    newService = {
        name: 'Tax Assessment',
        monitor_prod: true,
        url_ending_onprem: 'tax-assessment/assessments',
        url_ending_cloud: 'tax-assessment/assessments',
        request_type: 'post',
        content_type: 'application/json',
        message_body: '{"Property": {"Type": "noncondo","Value": 100000,"Address": {"AddressLine1": "109 MAIN","AddressLine2": "","City": "PLEVNA","State": "KS","Zip": "67568","ZipPlus4": "0000","County": "Reno"}}}'
    };
    services.push(newService);

    //Tax-Estimate
    newService = {
        name: 'Tax Estimate',
        monitor_prod: true,
        url_ending_onprem: 'tax-estimate/estimates?property_type=noncondo&property_value=100000&city=WATERFORD&county=OAKLAND&state=MI&zip=48328',
        url_ending_cloud: 'tax-estimate/estimates?property_type=noncondo&property_value=100000&city=WATERFORD&county=OAKLAND&state=MI&zip=48328',
        request_type: 'get',
        content_type: 'application/json',
        message_body: ''
    };
    services.push(newService);

    //Authentication
    newService = {
        name: 'Auth',
        monitor_prod: true,
        url_ending_onprem: '',
        url_ending_cloud: 'auth-proxy/',
        request_type: 'get',
        content_type: 'text/xml',
        message_body: ''
    };
    services.push(newService);

    //AVS
    newService = {
        name: 'AVS',
        monitor_prod: true,
        url_ending_onprem: 'avs/zip_codes?city=Detroit&state=MI',
        url_ending_cloud: 'avs/zip_codes?city=Detroit&state=MI',
        request_type: 'get',
        content_type: 'application/json',
        message_body: ''
    };
    services.push(newService);

    //AVS
    newService = {
        name: 'AVS+Satori',
        monitor_prod: true,
        url_ending_onprem: 'avs/address/validation?primary_address=1050%20woodward&city=detroit&state=mi&zip_code=48080',
        url_ending_cloud: 'avs/address/validation?primary_address=1050%20woodward&city=detroit&state=mi&zip_code=48080',
        request_type: 'get',
        content_type: 'application/json',
        message_body: ''
    };
    services.push(newService);

    //Token Generator
    newService = {
        name: 'Token Generator',
        monitor_prod: true,
        url_ending_onprem: 'token-generator/token',
        url_ending_cloud: '',
        request_type: 'post',
        content_type: 'application/json',
        message_body: '{"DurationInMinutes": 5,"Data": "testing from hexagon alley"}'
    };
    services.push(newService);
    
    //Advanced Income
    newService = {
        name: 'Advanced Income',
        monitor_prod: false,
        url_ending_onprem: 'advanced-income/AdvancedIncomeReport',
        url_ending_cloud: '',
        request_type: 'post',
        content_type: 'application/json',
        message_body: '{"AdvancedIncomeRequest":[{"RequestSource":"Hexagon Alley","AIRequestID":"TEST","LoanIdentifier":"BIGTEST23456","LoanPurpose":"Refinance","NumberOfBorrowers":1,"ProductCode":"631","TotalLiabilities":2310.29,"SubjectPropertyAddress":{"StreetAddress":"765 N Anywhere Street","City":"Nixa","State":"MO","ZipCode":"65714"},"Clients":[{"Name":"Michael Hines","ClientIdentifier":"99883776d6df55","MaritalStatus":"Married","IncludeInAssetReport":true,"CurrentResidentialAddress":{"StreetAddress":"455 N Banker St ","ApartmentNumber":"#456","City":"Nixa","State":"MO","ZipCode":"65714"},"Incomes":[{"CurrentEmployerName":"Springfield Catholic Schools","IsCurrentEmployer":"yes","IncomeType":"Employment","JobTitle":"Teacher","JobType":"FullTime","StateEmployedIn":"MO","ZipCode":"65809","ApplicationIncome":{"TotalMonthlyBase":13614.52,"TotalMonthlyBonus":0.0,"TotalMonthlyOvertime":0.0,"TotalMonthlyCommission":0.0}},{"CurrentEmployerName":"Republic R-III Schools","IsCurrentEmployer":"no","IncomeType":"Employment","JobTitle":"Teacher","JobType":"FullTime","StateEmployedIn":"MO","ZipCode":"65738","ApplicationIncome":{"TotalMonthlyBase":0.0,"TotalMonthlyBonus":0.0,"TotalMonthlyOvertime":0.0,"TotalMonthlyCommission":0.0}}]}]}]}'
    };
    services.push(newService);

    //Mortgage Lookup
    newService = {
        name: 'Mortgage Lookup',
        monitor_prod: false,
        url_ending_onprem: '',
        url_ending_cloud: 'mortgage-lookup/subject_property',
        request_type: 'post',
        content_type: 'application/json',
        message_body: '{"Borrower": {"FirstName": "Randy","LastName": "Lei"},"Property": {"Street": "29722 Rosebriar st","City": "St Clair Shores","State": "MI","Zip": "48082"}}'
    };
    services.push(newService);

    //Submission Engine
    newService = {
        name: 'Submission Engine',
        monitor_prod: true,
        url_ending_onprem: 'submissionengine/?wsdl',
        url_ending_cloud: '',
        request_type: 'get',
        content_type: 'text/xml',
        message_body: ''
    };
    services.push(newService);

    //Rocketdyne
    newService = {
        name: 'Rocketdyne',
        monitor_prod: false,
        url_ending_onprem: '',
        url_ending_cloud: 'rocketdyne/asset_status?maximumAgeInDays=0&loanNumber=1235678&gcid=123457',
        request_type: 'get',
        content_type: 'application/json',
        message_body: ''
    };
    services.push(newService);

    //Einstein Hub
    newService = {
        name: 'Einstein Hub',
        monitor_prod: true,
        url_ending_onprem: 'solution-engine/mmc',
        url_ending_cloud: 'solution-engine/mmc',
        request_type: 'post',
        content_type: 'application/json',
        message_body: '{"ClientApplicationName": "HexagonAlley","LoanPurpose": "Refinance","RefinanceInfo": {"LoanPurposeGoalSelected": "LowerPayment","CurrentLoanBalanceAmount": 180000,"CurrentLoanPaymentAmount": 1000},"ClientList": [{"CreditScore": 750}],"PropertyInfo": {"Zip": "48328","HomeValueAmount": 300000, "NumberOfUnits": 5},"ProductSelectionOptions": {"IncludeFixed": true}}'
    };
    services.push(newService);

    //PiR Admin
    newService = {
        name: 'PiR Admin',
        monitor_prod: true,
        url_ending_onprem: '',
        url_ending_cloud: 'price-is-right-admin/health',
        request_type: 'get',
        content_type: 'text/xml',
        message_body: ''
    };
    services.push(newService);

    //ORB Runtime
    newService = {
        name: 'ORB Runtime',
        monitor_prod: true,
        url_ending_onprem: '',
        url_ending_cloud: 'orb/?wsdl',
        request_type: 'get',
        content_type: 'application/soap+xml',
        message_body: ''
    };
*/