using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace ENYSchedular_new
{
    class helper
    {
        CommFun objComm = new CommFun();
        public DataSet GetEmptyEyData()
        {
            DataSet dt = new DataSet();
            try
            {
                dt = objComm.GetEmptyEyData();
                return dt;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return dt;
        }
        public void GenerateEyScore(DataSet dt)
        {
            string applicationNo = string.Empty;
            try
            {

                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    EYRiskScoreRequest eny_Score = new EYRiskScoreRequest();
                    RequestSource resSoc = new RequestSource();
                    Requestbody1 resBody = new Requestbody1();
                    applicationNo = dt.Tables[0].Rows[i]["ApplicationNo"].ToString();
                    DataSet ds2 = objComm.Featch_SMARTAPIENYFLAG_Details(applicationNo);
                    if (ds2.Tables[0].Rows.Count != 0 && ds2.Tables[0].Rows.Count > 0)
                 {
                    resSoc.DataKeyName = "ApplicationNo";
                    resSoc.DataKeyValue = applicationNo;
                    resSoc.Source = "DEQC";
                    resSoc.APIDetails = "EYScoreDetails";
                    resBody.relation_nominee_la = (ds2.Tables[0].Rows[0]["NOMINEERELATION"].ToString());
                    resBody.PolicyID = (ds2.Tables[0].Rows[0]["PolicyNumber"].ToString());
                    resBody.AppNo2 = applicationNo;
                    resBody.PolicyTerm = (ds2.Tables[0].Rows[0]["PT"].ToString());
                    resBody.Agntnum = (ds2.Tables[0].Rows[0]["AgentCode"].ToString());
                    resBody.LA_name = (ds2.Tables[0].Rows[0]["LANAME"].ToString());
                    string date_of_birth = (ds2.Tables[0].Rows[0]["LA_DOB"].ToString());
                    string Dob = Convert.ToDateTime(date_of_birth).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                    resBody.LA_DOB = Dob;
                    resBody.LA_Annual_Income = (ds2.Tables[0].Rows[0]["LA_ANNUAL_INCOME"].ToString());
                    resBody.LA_Education = (ds2.Tables[0].Rows[0]["LA_EDUCATION_DESC"].ToString());
                    resBody.LA_Occupation = (ds2.Tables[0].Rows[0]["LA_OCCUPATION"].ToString());
                    resBody.LA_Occupation_Desc = (ds2.Tables[0].Rows[0]["LA_OCCUPATION"].ToString());
                        resBody.LA_State = (ds2.Tables[0].Rows[0]["LA_State"].ToString());
                        resBody.LA_PCODE = (ds2.Tables[0].Rows[0]["LA_PCODE"].ToString());
                        resBody.APE = (ds2.Tables[0].Rows[0]["APE"].ToString());
                        resBody.flag_la_not_equal_proposer = ds2.Tables[0].Rows[0]["flag_la_not_equal_proposer"].ToString();
                        resBody.SumAssured = (ds2.Tables[0].Rows[0]["sumAssured"].ToString());
                        resBody.PolicyID = (ds2.Tables[0].Rows[0]["PolicyID"].ToString());
                        string issue = ds2.Tables[0].Rows[0]["Issue_date"].ToString();
                        string issue_Date = Convert.ToDateTime(issue).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string Login_Date = ds2.Tables[0].Rows[0]["Login_Date"].ToString();
                        string myDate = Convert.ToDateTime(Login_Date).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        resBody.Login_Date = myDate;
                        resBody.issue_date = issue_Date;
                        resBody.Address_proof = ds2.Tables[0].Rows[0]["Address_proof"].ToString();
                        resBody.Product_Code = ds2.Tables[0].Rows[0]["ProductCode"].ToString();
                        resBody.Channel = ds2.Tables[0].Rows[0]["Channel"].ToString();
                        resBody.Sales_manager_id = ds2.Tables[0].Rows[0]["Sales_manager_id"].ToString();
                        resBody.covid_check = ds2.Tables[0].Rows[0]["covid_check"].ToString();
                        resBody.policy_decline_or_postponsed = ds2.Tables[0].Rows[0]["policy_decline_or_postponsed"].ToString();
                        resBody.distance_between_insurer_and_branch = ds2.Tables[0].Rows[0]["distance_between_insurer_and_branch"].ToString();
                        eny_Score.RequestSource = resSoc;
                        eny_Score.Requestbody = resBody;
                        string Eyscore = CallENYScore(eny_Score);
                        if (!string.IsNullOrEmpty(Eyscore))
                        {
                            if (Eyscore == "0")
                            {
                                objComm.Insert_Logs_RiskAndEY(applicationNo, "Api called successfully", "UW Saral", "EnyScoreDetails", "Success");
                            }
                            else
                            {
                                objComm.Insert_Logs_RiskAndEY(applicationNo, "Api call successfully-error", "UW Saral", "EnyScoreDetails", "Success");

                            }
                        }
                        else
                        {
                            objComm.Insert_Logs_RiskAndEY(applicationNo, "Api call fail", "UW Saral", "EnyScoreDetails", "Fail");
                        }

                    }
                    else
                    {
                        objComm.Insert_Logs_RiskAndEY(applicationNo, "Api data not found in database ", "UW Saral", "EnyScoreDetails", "Error");

                    }

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                objComm.Insert_Logs_RiskAndEY(applicationNo, ex.Message, "UW Saral", "EnyScoreDetails", "Error");

            }
        }


        public void GenerateEyScore_Deqc(DataSet ds)
        {
            string applicationNo = string.Empty;
            try
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    EYRiskScoreRequest eny_Score = new EYRiskScoreRequest();
                    RequestSource resSoc = new RequestSource();
                    Requestbody1 resBody = new Requestbody1();
                   applicationNo= ds.Tables[0].Rows[i]["ApplicationNo"].ToString();
    
                    DataSet ds2 = objComm.Featch_SMARTAPIENYFLAG_Details(applicationNo);
                    if (ds2.Tables[0].Rows.Count != 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        resSoc.DataKeyName = "ApplicationNo";
                        resSoc.DataKeyValue = ds.Tables[0].Rows[i]["ApplicationNo"].ToString();
                        resSoc.Source = "UWSaral";
                        resSoc.APIDetails = "EYScoreDetails";
                        resBody.relation_nominee_la = ds2.Tables[0].Rows[i]["NOMINEERELATION"].ToString(); 
                        resBody.PolicyID = ds2.Tables[0].Rows[i]["PolicyNumber"].ToString(); 
                        resBody.AppNo2 = ds.Tables[0].Rows[i]["ApplicationNo"].ToString();
                        resBody.PolicyTerm = (ds2.Tables[0].Rows[0]["PT"].ToString());
                        resBody.Agntnum = (ds2.Tables[0].Rows[0]["AgentCode"].ToString());
                        resBody.LA_name = (ds2.Tables[0].Rows[0]["LANAME"].ToString());
                        string date_of_birth = (ds2.Tables[0].Rows[0]["LA_DOB"].ToString());
                        string Dob = Convert.ToDateTime(date_of_birth).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        resBody.LA_DOB = Dob;
                        resBody.LA_Annual_Income = (ds2.Tables[0].Rows[0]["LA_ANNUAL_INCOME"].ToString());
                        resBody.LA_Education = (ds2.Tables[0].Rows[0]["LA_EDUCATION_DESC"].ToString());
                        resBody.LA_Occupation = (ds2.Tables[0].Rows[0]["LA_OCCUPATION"].ToString());
                        resBody.LA_Occupation_Desc = (ds2.Tables[0].Rows[0]["LA_OCCUPATION"].ToString());
                        resBody.LA_State = (ds2.Tables[0].Rows[0]["LA_State"].ToString());
                        resBody.LA_PCODE = (ds2.Tables[0].Rows[0]["LA_PCODE"].ToString());
                        resBody.APE = (ds2.Tables[0].Rows[0]["APE"].ToString());
                        resBody.flag_la_not_equal_proposer = ds2.Tables[0].Rows[0]["flag_la_not_equal_proposer"].ToString();
                        resBody.SumAssured = (ds2.Tables[0].Rows[0]["sumAssured"].ToString());
                        resBody.PolicyID = (ds2.Tables[0].Rows[0]["PolicyID"].ToString());
                        string issue = ds2.Tables[0].Rows[0]["Issue_date"].ToString();
                        string issue_Date = Convert.ToDateTime(issue).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string Login_Date = ds2.Tables[0].Rows[0]["Login_Date"].ToString();
                        string myDate = Convert.ToDateTime(Login_Date).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        resBody.Login_Date = myDate;
                        resBody.issue_date = issue_Date;
                        resBody.Address_proof = ds2.Tables[0].Rows[0]["Address_proof"].ToString();
                        resBody.Product_Code = ds2.Tables[0].Rows[0]["ProductCode"].ToString();
                        resBody.Channel = ds2.Tables[0].Rows[0]["Channel"].ToString();
                        resBody.Sales_manager_id = ds2.Tables[0].Rows[0]["Sales_manager_id"].ToString();
                        resBody.covid_check = ds2.Tables[0].Rows[0]["covid_check"].ToString();
                        resBody.policy_decline_or_postponsed = ds2.Tables[0].Rows[0]["policy_decline_or_postponsed"].ToString();
                        resBody.distance_between_insurer_and_branch = ds2.Tables[0].Rows[0]["distance_between_insurer_and_branch"].ToString();
                        eny_Score.RequestSource = resSoc;
                        eny_Score.Requestbody = resBody;
                        string Eyscore = CallENYScore(eny_Score);
                        if (!string.IsNullOrEmpty(Eyscore))
                        {
                            if (Eyscore == "0")
                            {
                                objComm.Insert_Logs_RiskAndEY(applicationNo, "Api called successfully", "UW Saral", "EnyScoreDetails", "Success");
                            }
                            else
                            {
                                objComm.Insert_Logs_RiskAndEY(applicationNo, "Api call successfully-error", "UW Saral", "EnyScoreDetails", "Success");

                            }
                        }
                        else
                        {
                            objComm.Insert_Logs_RiskAndEY(applicationNo, "Api call fail", "UW Saral", "EnyScoreDetails", "Fail");
                        }

                    }
                    else
                    {
                        objComm.Insert_Logs_RiskAndEY(applicationNo, "Api data not found in database ", "UW Saral", "EnyScoreDetails", "Error");

                    }

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                objComm.Insert_Logs_RiskAndEY(applicationNo, ex.Message, "UW Saral", "EnyScoreDetails", "Error");

            }
        }
        private string CallENYScore(EYRiskScoreRequest ey)
        {
            try
            {
                string rs = string.Empty;
                string inputJson = string.Empty;
                string result = string.Empty;
                string Authorization = string.Empty;
                string apiUrl = ConfigurationManager.AppSettings["ENYScoreApiURL"].ToString();
                //generateToken(ref Authorization);

                //if (!String.IsNullOrEmpty(Authorization))
                //{
                    inputJson = (new JavaScriptSerializer()).Serialize(ey);
                    result = SmartApi(apiUrl, "GetEYRiskScore", inputJson, Authorization, ey.Requestbody.AppNo2);
                    EnyScoreResponse1 objResponseClass = JsonConvert.DeserializeObject<EnyScoreResponse1>(result);
                    if (objResponseClass.ResponseResult.responseCode == "0")
                    {

                        String score = objResponseClass.ResponseBody[0].score;
                        if (!string.IsNullOrEmpty(score))
                        {
                            InsertEnyVal(objResponseClass.ResponseBody[0].appno2, objResponseClass.ResponseBody[0].score, objResponseClass.ResponseBody[0].Early_claim_risk_level);
                            //         txtENYScore.Text = score;
                            return objResponseClass.ResponseResult.responseCode;
                        }
                        else
                        {
                            objComm.Insert_Logs_RiskAndEY(ey.RequestSource.DataKeyValue, "EnyRiskScore is null", "UW Saral", "CallENYScore", "Fail");
                            return objResponseClass.ResponseResult.responseCode;
                        }
                    }
                    else
                    {
                        return objResponseClass.ResponseResult.responseCode;
                    }
              //  }
                //else
                //{
                 //   objComm.Insert_Logs_RiskAndEY(ey.RequestSource.DataKeyValue, "Token Not found", "UW Saral", "CallENYScore", "Fail");
                //}
            }
            catch (Exception ex)
            {
                objComm.Insert_Logs_RiskAndEY(ey.RequestSource.DataKeyValue, ex.Message, "UW Saral", "CallENYScore", "Fail");

            }
            return "";
        }
        private void generateToken(ref string token)
        {
            token = string.Empty;
            try
            {
                using (var client1 = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var postData = new List<KeyValuePair<string, string>>();

                    postData.Add(new KeyValuePair<string, string>("ClientID", ConfigurationManager.AppSettings["ClientID"].ToString()));
                    postData.Add(new KeyValuePair<string, string>("ClientSecret", ConfigurationManager.AppSettings["ClientSecret"].ToString()));
                    postData.Add(new KeyValuePair<string, string>("Source", ConfigurationManager.AppSettings["Source"].ToString()));
                    postData.Add(new KeyValuePair<string, string>("PartnerID", ConfigurationManager.AppSettings["PartnerID"].ToString()));

                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client1.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ConfigurationManager.AppSettings["TOKENSubscriptionKey"].ToString());

                    HttpContent content = new FormUrlEncodedContent(postData);

                    var responseResult = client1.PostAsync(ConfigurationManager.AppSettings["TOKENURL"].ToString(), content).Result;


                    string Result = string.Empty;
                    if (responseResult.IsSuccessStatusCode)
                    {
                        Result = responseResult.Content.ReadAsStringAsync().Result;
                        JObject json = JObject.Parse(Result);

                        token = json["access_token"].ToString();
                        //token = json["token_type"].ToString() + " " + json["access_token"].ToString();
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        public string SmartApi(string apiUrl, string Method, string inputJson, string Authorization, string Appno)
        {

            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                client.Headers["Content-type"] = "application/json";
                client.Headers["Ocp-Apim-Subscription-Key"] = ConfigurationManager.AppSettings["Ocp-Apim-Subscription-Key"].ToString();
                //client.Headers["Authorization"] = Authorization;
                client.Encoding = Encoding.UTF8;
                string Response = client.UploadString(apiUrl + Method, inputJson);

                return Response;
            }
            catch (Exception ex)
            {
                objComm.Insert_Logs_RiskAndEY(Appno, ex.Message, "Deqc", "CallENYScore", "Fail");
                return null;
            }
        }
        public void InsertEnyVal(string appno, string score, string early_claim_risk_level)
        {
            objComm.Insert_EnyFlag_Details(appno, score, early_claim_risk_level);
        }

    }
}
