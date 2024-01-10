using Duly.UI.Flourish.GeneratePDF.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace Duly.UI.Flourish.GeneratePDF.Data
{
    public class DBHelper
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private readonly string constring;

        public DBHelper(IConfiguration _configuration)
        {
            constring = _configuration.GetSection("ConnectionStrings:DBConnection").Value;
            con = new SqlConnection(constring);
        }

        public void LogException(string jobName, int? afterVisitPdfId, string appointmentId, string errDesc, string methodName, string shortErr)
        {
            using (con = new SqlConnection(constring))
            {
                using (cmd = new SqlCommand("uspInsertJobProcessingDetail", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@JobName", jobName);
                    cmd.Parameters.AddWithValue("@AfterVisitPDFId", afterVisitPdfId);
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                    cmd.Parameters.AddWithValue("@ErrorDescription", errDesc);
                    cmd.Parameters.AddWithValue("@MethodsName", methodName);
                    cmd.Parameters.AddWithValue("@ShortError", shortErr);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public List<SitesDetailsViewModel> FollowUpLocation()
        {
            List<SitesDetailsViewModel> sitesDetails = new List<SitesDetailsViewModel>();

            using (con = new SqlConnection(constring))
            {
                using (cmd = new SqlCommand("[GetSites]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        sitesDetails.Add(new SitesDetailsViewModel
                        {
                            Id = dr["Id"].ToString(),
                            Line = dr["Line"].ToString(),
                            PostalCode = dr["PostalCode"].ToString(),
                            City = dr["City"].ToString(),
                            State = dr["State"].ToString(),
                        });
                    }

                    con.Close();
                }
                return sitesDetails;
            }
        }

        public void InsertJobDetail(string jobName, int totalRecordsCount, int sccessfulProcessedRecordsCount, int failedRecordsCount)
        {
            using (con = new SqlConnection(constring))
            {
                using (cmd = new SqlCommand("uspInsertJobDetail", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@JobName", jobName);
                    cmd.Parameters.AddWithValue("@TotalRecordsCount", totalRecordsCount);
                    cmd.Parameters.AddWithValue("@SuccessfulProcessedRecordsCount", sccessfulProcessedRecordsCount);
                    cmd.Parameters.AddWithValue("@FailedRecordsCount", failedRecordsCount);
                    cmd.Parameters.AddWithValue("@LastJobRunExecutedTime", DateTime.Now);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public List<AppointmentDetailsBySMSStatusViewModel> GetPendingAppointmentsBySMSStatus(string status)
        {
            List<AppointmentDetailsBySMSStatusViewModel> Listappt = new List<AppointmentDetailsBySMSStatusViewModel>();

            using (con = new SqlConnection(constring))
            {
                using (cmd = new SqlCommand("[uspGetAppointmentsBySMSStatus]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Status", status);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Listappt.Add(new AppointmentDetailsBySMSStatusViewModel
                        {
                            AppointmentId = dr["AppointmentId"].ToString(),
                            AppointmentTime = dr["AppointmentTime"].ToString(),
                            Id = (long)dr["Id"],
                            PatientId = dr["PatientId"].ToString(),
                            ProviderId = dr["ProviderId"].ToString(),
                            SiteId = dr["SiteId"].ToString(),
                            PhoneNumber = dr["PhoneNumber"].ToString(),
                        });

                    }

                    con.Close();
                }
                return Listappt;
            }
        }

        public int UpdateAppointmentSMSStatus(string _appointmentId, string _smsStatus)
        {
            int response = 0;
            using (con = new SqlConnection(constring))
            {
                using (cmd = new SqlCommand("[uspPostSMSStatusForAfterVisitPdf]", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Appointment_ID", _appointmentId);
                    cmd.Parameters.AddWithValue("@SMSStatus", _smsStatus);
                    con.Open();
                    response = cmd.ExecuteNonQuery();
                    con.Close();
                }
                return response;
            }
        }

        public int PostAfterVisitPDF(AfterVisitPdf objParam)
        {
            int _response = 0;
            using (con = new SqlConnection(constring))
            {
                using (cmd = new SqlCommand("uspInsertAfterVisitPdf", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Patient_ID", objParam.PatientId);
                    cmd.Parameters.AddWithValue("@Appointment_ID", objParam.AppointmentId);
                    cmd.Parameters.AddWithValue("@PhoneNumber", objParam.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Provider_ID", objParam.ProviderId);
                    cmd.Parameters.AddWithValue("@AfterVisitPDF", Convert.FromBase64String(objParam.AfterVisitPDF));
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        _response = Convert.ToInt32(dr["Id"].ToString());
                    }
                    con.Close();
                }
            }
            return _response;
        }
    }
}