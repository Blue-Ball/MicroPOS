// Decompiled with JetBrains decompiler
// Type: MicroPOS.RestAPI.clsRestAPI
// Assembly: MicroPOS, Version=0.9.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E28C7A9-BD22-4D9E-9CDE-8AA00F0A6EEA
// Assembly location: C:\Users\Toten1\Documents\1219-2nd\MicroPOS.exe

using MicroPOS.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

namespace MicroPOS.RestAPI
{
    public class clsRestAPI : HttpWebClientProtocol
    {
        private string _hostUrl = "http://64.225.38.42/api";
        private string _email = "5dedb782c1125";
        private string _password = "1234";
        private DateTime m_loginDate;
        private RTLogin m_rtLogin;

        public bool isLogined()
        {
            return this.m_rtLogin != null && !string.IsNullOrEmpty(this.m_rtLogin.access_token) && (int)(DateTime.Now - this.m_loginDate).TotalSeconds <= this.m_rtLogin.expires_in;
        }

        public RTLogin Login()
        {
            string restEndPoint = this._hostUrl + "/" + "login";
            try
            {
                string body = this.Serialize<RTAccount>(new RTAccount()
                {
                    email = this._email,
                    password = this._password
                });
                using (HttpWebResponse response = (HttpWebResponse)this.CreatePostRequest(restEndPoint, (string)null, body).GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            PosHelper.Logger.Info("api : " + restEndPoint + ", reponse : " + end);
                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                PosHelper.Logger.Info("Login Failed " + response.StatusCode.ToString());
                                return (RTLogin)null;
                            }
                            this.m_rtLogin = this.Deserialize<RTLogin>(end);
                            this.m_loginDate = DateTime.Now;
                            return this.m_rtLogin;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PosHelper.Logger.Error("Exception - Login " + ex.Message);
            }
            return (RTLogin)null;
        }

        public async Task<RTNetworkArray> GetMobileNetwork()
        {
            string w_url = this._hostUrl + "/" + "mobile";
            try
            {
                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreateGetRequest(w_url, this.m_rtLogin.access_token).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            PosHelper.Logger.Info("api : " + w_url + ", reponse : " + end);
                            if (responseAsync.StatusCode == HttpStatusCode.OK)
                                return this.Deserialize<RTNetworkArray>(end);
                            PosHelper.Logger.Info("GetMobileNetwork Failed " + responseAsync.StatusCode.ToString());
                            return (RTNetworkArray)null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PosHelper.Logger.Error("GetMobileNetwork - Login " + ex.Message);
            }
            return (RTNetworkArray)null;
        }

        public async Task<List<double>> GetRechargeAmountOnMobile(
          string network,
          string phoneNumber)
        {
            string w_url = this._hostUrl + "/" + ("mobile/" + network + "/" + phoneNumber);
            try
            {
                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreateGetRequest(w_url, this.m_rtLogin.access_token).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            PosHelper.Logger.Info("api : " + w_url + ", reponse : " + end);
                            if (responseAsync.StatusCode != HttpStatusCode.OK)
                            {
                                PosHelper.Logger.Info("GetRechargeAmountOnMobile Failed " + responseAsync.StatusCode.ToString());
                                return (List<double>)null;
                            }
                            List<string> stringList = this.Deserialize<List<string>>(end);
                            if (stringList == null || stringList.Count <= 0)
                                return (List<double>)null;
                            List<double> doubleList = new List<double>();
                            foreach (string s in stringList)
                            {
                                double num = 0.0;
                                double.TryParse(s, out num);
                                doubleList.Add(num);
                            }
                            return doubleList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PosHelper.Logger.Error("GetRechargeAmountOnMobile - ex : " + ex.Message);
            }
            return (List<double>)null;
        }

        public async Task<RTNetworkArray> GetGiftCard()
        {
            string w_url = this._hostUrl + "/" + "giftcard";
            try
            {
                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreateGetRequest(w_url, this.m_rtLogin.access_token).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            PosHelper.Logger.Info("api : " + w_url + ", reponse : " + end);
                            if (responseAsync.StatusCode == HttpStatusCode.OK)
                                return this.Deserialize<RTNetworkArray>(end);
                            PosHelper.Logger.Info("GetGiftCard Failed " + responseAsync.StatusCode.ToString());
                            return (RTNetworkArray)null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PosHelper.Logger.Error("Exception - GetGiftCard " + ex.Message);
            }
            return (RTNetworkArray)null;
        }

        public async Task<List<double>> GetGiftCardAmount(string network)
        {
            string w_url = this._hostUrl + "/" + ("giftcard/" + network);
            try
            {
                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreateGetRequest(w_url, this.m_rtLogin.access_token).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            PosHelper.Logger.Info("api : " + w_url + ", reponse : " + end);
                            if (responseAsync.StatusCode != HttpStatusCode.OK)
                            {
                                PosHelper.Logger.Info("GetGiftCardAmount Failed " + responseAsync.StatusCode.ToString());
                                return (List<double>)null;
                            }
                            List<string> stringList = this.Deserialize<List<string>>(end);
                            if (stringList == null || stringList.Count <= 0)
                                return (List<double>)null;
                            List<double> doubleList = new List<double>();
                            foreach (string s in stringList)
                            {
                                double num = 0.0;
                                double.TryParse(s, out num);
                                doubleList.Add(num);
                            }
                            return doubleList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PosHelper.Logger.Error("GetGiftCardAmount - ex " + ex.Message);
            }
            return (List<double>)null;
        }

        public async Task<RTIpsInvoice> GetOpenInvoice(string cpf)
        {
            string w_url = this._hostUrl + "/" + ("ips/" + cpf);
            try
            {
                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreateGetRequest(w_url, this.m_rtLogin.access_token).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            PosHelper.Logger.Info("api : " + w_url + ", reponse : " + end);
                            if (responseAsync.StatusCode == HttpStatusCode.OK)
                                return this.Deserialize<RTIpsInvoice>(end);
                            PosHelper.Logger.Info("GetOpenInvoice Failed " + responseAsync.StatusCode.ToString());
                            return (RTIpsInvoice)null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PosHelper.Logger.Error("GetOpenInvoice - ex " + ex.Message);
            }
            return (RTIpsInvoice)null;
        }

        public async Task<bool> PostRechargeOnMobile(RTMobileRecharge rechargeParam)
        {
            string w_url = this._hostUrl + "/" + "recharge";
            string w_body = this.Serialize<RTMobileRecharge>(rechargeParam);
            try
            {
                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreatePostRequest(w_url, this.m_rtLogin.access_token, w_body).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            PosHelper.Logger.Info("post_api : " + w_url + ", body : " + w_body + ", reponse : " + end);
                            if (responseAsync.StatusCode != HttpStatusCode.OK)
                            {
                                PosHelper.Logger.Info("PostRechargeOnMobile Failed " + responseAsync.StatusCode.ToString());
                                return false;
                            }
                            this.Deserialize<RTRechargeResponse>(end);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PosHelper.Logger.Error("[PostRechargeOnMobile_EX] url : " + w_url + ", body : " + w_body + ", error : " + ex.Message);
            }
            return false;
        }

        public async Task<string> PostRechargeOnGift(RTGiftRecharge rechargeParam)
        {
            string w_url = this._hostUrl + "/" + "recharge";
            string w_body = this.Serialize<RTGiftRecharge>(rechargeParam);
            try
            {
                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreatePostRequest(w_url, this.m_rtLogin.access_token, w_body).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            PosHelper.Logger.Info("post_api : " + w_url + ", body : " + w_body + ", reponse : " + end);
                            if (responseAsync.StatusCode == HttpStatusCode.OK)
                                return this.Deserialize<RTRechargeResponse>(end).pin;
                            PosHelper.Logger.Info("PostRechargeOnGift Failed " + responseAsync.StatusCode.ToString());
                            return (string)null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PosHelper.Logger.Error("[PostRechargeOnGift_EX] url : " + w_url + ", body : " + w_body + ", error : " + ex.Message);
            }
            return (string)null;
        }
        public async Task<Model.boleto> GetBoleto(string cdgBarrasBoleto)
        {
            string w_url = this._hostUrl + "/" + ("boleto/" + cdgBarrasBoleto);
            string Result = "";
            int countRetry = 0;
            retry:

            try
            {
                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreateGetRequest(w_url, this.m_rtLogin.access_token).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            Result = end;
                            countRetry++;
                            if (countRetry <= 5 && end.Contains("API offline"))
                            {
                                Thread.Sleep(5000);
                                goto retry;
                            }
                               
                            PosHelper.Logger.Info("api : " + w_url + ", reponse : " + end);
                            if (responseAsync.StatusCode == HttpStatusCode.OK)
                                return this.Deserialize<Model.boleto>(end);
                            PosHelper.Logger.Info("GetBoleto Failed " + responseAsync.StatusCode.ToString());
                            return (Model.boleto)null;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Message == "The request was aborted: The operation has timed out.")
                {
                    countRetry++;
                    goto retry;

                }

                if (ex.Response != null)
                {
                    //RV: Removed Encoding.Unicode to fix encoding issue(chinnese text)
                    //using (StreamReader r = new StreamReader(ex.Response.GetResponseStream(), Encoding.Unicode))
                    using (StreamReader r = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string responseContent = r.ReadToEnd();
                        if (responseContent.Contains("API offline"))
                        {
                            countRetry++;
                            Thread.Sleep(5000);
                            goto retry;
                        }
                        return new Model.boleto { failed = true, failedMsg = responseContent };
                    }

                }
                else
                {
                    countRetry++;
                    Thread.Sleep(5000);
                    goto retry;
                }



                PosHelper.Logger.Error("GetOpenInvoice - ex " + ex.Message);
            }


            return (Model.boleto)null;
        }

        public async Task<Model.tarifa> GetTarifa(string Data)
        {
            string w_url = this._hostUrl + "/fee/" + Data;
            string Result = "";
            int countRetry = 0;
        retry:
            try
            {
                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreateGetRequest(w_url).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            Result = end;
                            countRetry++;
                            if (countRetry <= 5 && end.Contains("API offline"))
                            {
                                Thread.Sleep(5000);
                                goto retry;
                            }

                            PosHelper.Logger.Info("api : " + w_url + ", reponse : " + end);
                            if (responseAsync.StatusCode == HttpStatusCode.OK)
                                return this.Deserialize<Model.tarifa>(end);
                            PosHelper.Logger.Info("GetBoleto Failed " + responseAsync.StatusCode.ToString());
                            return null;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Message.ToLower().Contains("the operation has timed out"))
                {
                    countRetry++;
                    Thread.Sleep(2000);
                    goto retry;
                }
                    PosHelper.Logger.Error("GetOpenInvoice - ex " + ex.Message);
            }


            return null;
        }


        public async Task<string> PostBoletoConfirm(Model.boleto boletoInfo)
        {
            string w_url = this._hostUrl + "/" + ("boleto");
            string Result = "";
            int countRetry = 0;
        retry:

            int counter = 0;
            if (counter >= 5)
                return "ERRO 400  ";

            try
            {
                string BodyToSend= "{"+
                "\"digitable\": \"[digitable]\"," +
                "\"due_date\": \"[due_date]\"," +
                "\"doc_payer\": \"[doc_payer]\"," +
                "\"type\": [type]," +
                "\"original_value\": [original_value]," +
                "\"discount_value\": [discount_value]," +
                "\"total_updated\": [total_updated]," +
                "\"total_discount\": [total_discount]," +
                "\"total_additional\": [total_additional]," +
                "\"transaction_id\": [transaction_id]" +
                "}";

                BodyToSend = BodyToSend
                .Replace("[digitable]", boletoInfo.digitable)
                .Replace("[due_date]", boletoInfo.due_date)
                .Replace("[doc_payer]", "75786192091")
                .Replace("[type]", boletoInfo.type.ToString().Replace(",","."))
                .Replace("[original_value]", boletoInfo.original_value.ToString().Replace(",", "."))
                .Replace("[discount_value]", boletoInfo.discount_value.ToString().Replace(",", "."))
                .Replace("[total_updated]", boletoInfo.total_updated.ToString().Replace(",", "."))
                 .Replace("[total_discount]", boletoInfo.total_discount.ToString().Replace(",", "."))
                    .Replace("[total_additional]", boletoInfo.total_additional.ToString().Replace(",", "."))
                    .Replace("[transaction_id]", boletoInfo.transaction_id.ToString().Replace(",", "."));


              //string resp=  HtmlPost(w_url, BodyToSend);


                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreatePostRequest(w_url, this.m_rtLogin.access_token, BodyToSend).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            return end;
       
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                PosHelper.Logger.Error("GetOpenInvoice - ex " + ex.Message);
                if (ex.Message.ToLower().Contains("the operation has timed out"))
                {
                    counter++;
                    Thread.Sleep(2000);
                    goto retry;

                }

     
                    if (ex.Response != null)
                    {
                        using (StreamReader r = new StreamReader(ex.Response.GetResponseStream()))
                        {
                            string responseContent = r.ReadToEnd();
                            return "ERRO 400 " + responseContent;
                        }
                    }
                    counter++;
                    Thread.Sleep(2000);
                    goto retry;
                    return "ERRO 400  ";




            }


            return null;
        }

        /// <summary>
        /// //RV - Cancel transaction by transaction_id
        /// </summary>
        /// <param name="boletoInfo"></param>
        /// <returns></returns>
        public async Task<string> PostBoletoCancel(Model.boleto boletoInfo)
        {
            string w_url = this._hostUrl + "/boleto/cancel";

            string Result = "";
            int countRetry = 0;
        retry:

            int counter = 0;
            if (counter >= 5)
                return "ERRO 400  ";

            try
            {
                string BodyToSend = "{" +
                "\"transaction_id\": [transaction_id]" +
                "}";

                BodyToSend = BodyToSend
                    .Replace("[transaction_id]", boletoInfo.transaction_id.ToString().Replace(",", "."));


                //string resp=  HtmlPost(w_url, BodyToSend);


                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreatePostRequest(w_url, this.m_rtLogin.access_token, BodyToSend).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string end = streamReader.ReadToEnd();
                            return end;

                        }
                    }
                }
            }
            catch (WebException ex)
            {
                PosHelper.Logger.Error("PostBoletoCancel - ex " + ex.Message);
                if (ex.Message.ToLower().Contains("the operation has timed out"))
                {
                    counter++;
                    Thread.Sleep(2000);
                    goto retry;

                }


                if (ex.Response != null)
                {
                    using (StreamReader r = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string responseContent = r.ReadToEnd();
                        return "ERRO 400 " + responseContent;
                    }
                }
                counter++;
                Thread.Sleep(2000);
                goto retry;
                return "ERRO 400  ";

            }

            return null;
        }

        public async Task<string> PostBoletoPaymentConfirm(string digitable, string transaction_id, string tef_id, string total_paid, string boleto_amount)
        {
            string w_url = this._hostUrl + "/boleto/capture";
            string Result = "";
            int countRetry = 0;
        retry:

            int counter = 0;
            if (counter >= 5)
                return "ERRO 400  ";

            try
            {
                string BodyToSend = "{" +
                "\"digitable\": \"[digitable]\"," +
                "\"transaction_id\": \"[transaction_id]\"," +
                "\"tef_id\": \"[tef_id]\"," +
                "\"total_paid\": \"[total_paid]\"," +
                 "\"boleto_amount\": \"[boleto_amount]\"" +
                "}";


                BodyToSend = BodyToSend
                .Replace("[digitable]", digitable)
                .Replace("[transaction_id]", transaction_id)
                .Replace("[tef_id]", tef_id)
                .Replace("[total_paid]", total_paid)
                .Replace("[boleto_amount]", boleto_amount);


                using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreatePostRequest(w_url, this.m_rtLogin.access_token, BodyToSend).GetResponseAsync())
                {
                    using (Stream responseStream = responseAsync.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream, Encoding.Default))
                        {
                            string end = streamReader.ReadToEnd();
                            return end;

                        }
                    }
                }
            }
            catch (WebException ex)
            {
                PosHelper.Logger.Error("GetOpenInvoice - ex " + ex.Message);
                if (ex.Message.ToLower().Contains("the operation has timed out"))
                {
                    counter++;
                    Thread.Sleep(2000);
                    goto retry;

                }



                if (ex.Response != null)
                {
                    using (StreamReader r = new StreamReader(ex.Response.GetResponseStream(), Encoding.Default ))
                    {
                        string responseContent = r.ReadToEnd();
                        return "ERRO 400 " + responseContent;
                    }
                }
                counter++;
                Thread.Sleep(2000);
                goto retry;
                return "ERRO 400  ";




            }


            return null;
        }





        public async Task<bool> PostPaidInvoice(List<RTPaidFatura> p_invoiceList)
        {
            string w_url = this._hostUrl + "/" + "ips";
            if (p_invoiceList == null || p_invoiceList.Count <= 0)
                return true;
            try
            {
                foreach (RTPaidFatura pInvoice in p_invoiceList)
                {
                    string w_body = this.Serialize<RTPaidFatura>(pInvoice);
                    using (HttpWebResponse responseAsync = (HttpWebResponse)await this.CreatePostRequest(w_url, this.m_rtLogin.access_token, w_body).GetResponseAsync())
                    {
                        using (Stream responseStream = responseAsync.GetResponseStream())
                        {
                            using (StreamReader streamReader = new StreamReader(responseStream))
                            {
                                string end = streamReader.ReadToEnd();
                                PosHelper.Logger.Info("post_api : " + w_url + ", body : " + w_body + ", reponse : " + end);
                                if (responseAsync.StatusCode != HttpStatusCode.OK)
                                {
                                    PosHelper.Logger.Info("PostPaidInvoice Failed : " + responseAsync.StatusCode.ToString());
                                    continue;
                                }
                                this.Deserialize<RTRechargeResponse>(end);
                            }
                        }
                    }
                    w_body = (string)null;
                }
            }
            catch (Exception ex)
            {
                PosHelper.Logger.Error("[PostPaidInvoice_EX] url : " + w_url + ", body : " + this.Serialize<List<RTPaidFatura>>(p_invoiceList) + ", error : " + ex.Message);
                return false;
            }
            return true;
        }

        protected HttpWebRequest CreatePostRequest(
          string restEndPoint,
          string accessToken,
          string body)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(restEndPoint);
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            httpWebRequest.Timeout = 10000;
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.Accept = "*/*";
            if (!string.IsNullOrEmpty(accessToken))
            {
                httpWebRequest.PreAuthenticate = true;
                httpWebRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            }
            if (!string.IsNullOrEmpty(body))
            {
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    streamWriter.Write(body);
            }
            return httpWebRequest;
        }

        protected HttpWebRequest CreateGetRequest(string restEndPoint, string accessToken = null)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(restEndPoint);
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            httpWebRequest.Timeout = 10000;
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.Accept = "application/json";
            if (!string.IsNullOrEmpty(accessToken))
            {
                httpWebRequest.PreAuthenticate = true;
                httpWebRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            }
            return httpWebRequest;
        }

        private string Serialize<T>(T value)
        {
            JsonSerializerSettings jsonSerializerSetting = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(value, jsonSerializerSetting);
        }

        private T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
