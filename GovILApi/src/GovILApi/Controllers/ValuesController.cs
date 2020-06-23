using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using GovILApi.ViewModels;
using Newtonsoft.Json;
using GovILApi.Helpers;
using GovILApi.Enums;
using GovILApi.Configuration;
using Microsoft.Extensions.Options;

namespace GovILApi.Controllers
{
    /// <summary>
    /// :Postman -נתיבים ב
    /// http://localhost:26736/api/values/getData - לוקאלי
    /// https://minhaldrprep.hit.loc/GovILApi/api/values/getData - פרהפרוד
    /// :Postman הגדות של
    /// postman.png ניתן להיעזר בתמונה המופיעה בקובץ
    /// </summary>
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<GlobalSettingsOptions> _optionsAccessor;
        public ValuesController(IConfiguration config, IOptions<GlobalSettingsOptions> options)
        {
            _configuration = config;
            _optionsAccessor = options;
        }

        [HttpPost]
        [Route("GetData")]
        public IActionResult GetData([FromBody]GetDataRequest request)
        {
            ResponseData res;
            try
            {

                var model = GetStudentsDataList(request.personId[0].idNum);

                if (model.Count == 0)
                {
                    res = GetNoResponseDataModel(eErrorCode.NoData);
                }
                else
                {
                    res = GetResponseDataModel(request.personId[0].idNum, model);
                }

                JsonSerializerSettings settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
                return Ok(JsonConvert.SerializeObject(res, settings));
            }
            catch (Exception ex)
            {
                AddToLogs(ex);

                res = GetNoResponseDataModel(eErrorCode.NoDataConnection);
                return Ok(JsonConvert.SerializeObject(res));
            }
        }

        [HttpGet]
        [Route("Health-Check")]
        public IActionResult HealthCheck()
        {
            var res = new StatusObj()
            {
                status = "success"
            };
            return Ok(JsonConvert.SerializeObject(res));
        }

        private void AddToLogs(Exception ex)
        {
            string sql = CreateQueryLog(ex);

            var connectionString = _configuration.GetConnectionString("DigitalRural");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var command = new SqlCommand(sql, connection) {CommandType = CommandType.Text};
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        private ResponseData GetResponseDataModel(string idNum, List<StudentModel> model)
        {
            var studentsData = GetAllStudents(model);
            return new ResponseData()
            {
                errorCode = new ErrorCode()
                {
                    errorCode = (int)eErrorCode.Ok,
                },
                userDataList = new[]
                {
                    new UserDataObj()
                    {
                        idNum = idNum,
                        errorCode = new ErrorCode()
                        {
                            errorCode = (int)eErrorCode.Ok,
                        },
                        dataList = studentsData
                    }
                }
            };
        }

        private DataObj[] GetAllStudents(List<StudentModel> students)
        {
            DataObj[] studentList = new DataObj[students.Count];
            for (int i = 0; i < students.Count; i++)
            {
                studentList[i] = new DataObj
                {
                    dataDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                    dataType = 2,
                    tags = { },
                    titleSentenceCode = GetTitleSentenceCode(i),
                    titleSentencePlaceHoldersValues = "",
                    urgent = 0,
                    descriptionDataList = GetStudentList(students[i])
                };
            }

            return studentList;
        }

        private int GetTitleSentenceCode(int studentIndex)
        {
            return studentIndex == 0
                ? _optionsAccessor.Value.TitleSentenceCode
                : _optionsAccessor.Value.TitleSentenceCodeFromSecondStudentOnwards;
        }

        private DescriptionDataObj[] GetStudentList(StudentModel student)
        {
            List<DescriptionDataObj> studentList = new List<DescriptionDataObj>();

            //studentName
            if (!string.IsNullOrEmpty(student.studentName))
            {
                AddDescriptionDataObj(student.studentName, studentList, _optionsAccessor.Value.DescriptionSentenceCode1);
            }

            //personID
            if (!string.IsNullOrEmpty(student.personID))
            {
             AddDescriptionDataObj(student.personID, studentList, _optionsAccessor.Value.DescriptionSentenceCode2);
            }

            //status
            if (student.status > 0)
            {
                AddDescriptionDataObj(student.status, studentList, _optionsAccessor.Value.DescriptionSentenceCode3);
            }

            //educationPlaceName
            if (!string.IsNullOrEmpty(student.educationPlaceName))
            {
                AddDescriptionDataObj(student.educationPlaceName, studentList, _optionsAccessor.Value.DescriptionSentenceCode5);
            }

            //contactName
            if (!string.IsNullOrEmpty(student.contactName))
            {
                AddDescriptionDataObj(student.contactName, studentList, _optionsAccessor.Value.DescriptionSentenceCode6);
            }

            return studentList.ToArray();
        }

        private static void AddDescriptionDataObj(object data, List<DescriptionDataObj> studentList, int descriptionSentenceCode)
        {
            var descriptionSentencePlaceHoldersValues = new DescriptionPlaceHoldersModel()
            {
                P0 = data
            };
            studentList.Add(new DescriptionDataObj()
            {
                urgent = 0,
                descriptionSentenceCode = descriptionSentenceCode,
                descriptionSentencePlaceHoldersValues = JsonConvert.SerializeObject(descriptionSentencePlaceHoldersValues),
                files = { },
                ctaList = { },
                dataPresentationType = { }
            });
        }


        private static ResponseData GetNoResponseDataModel(eErrorCode code)
        {
            return new ResponseData()
            {
                errorCode = new ErrorCode()
                {
                    errorCode = (int)code
                },
                userDataList = { }
            };
        }

       
        private List<StudentModel> GetStudentsDataList(string id)
        {
            List<StudentModel> model = new List<StudentModel>();
            string sql = CreateQuery(id);

            var connectionString = _configuration.GetConnectionString("DigitalRural");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        model.Add(CreateStudentViewModel(dataReader));
                    }
                }

                connection.Close();
            }
            return model;
        }

        private static StudentModel CreateStudentViewModel(SqlDataReader dataReader)
        {
            return new StudentModel()
            {
                studentName = Convert.ToString(dataReader["studentName"]),
                personID = Convert.ToString(dataReader["personID"]),
                status = Convert.ToInt32(dataReader["status"]),
                educationPlaceName = Convert.ToString(dataReader["educationPlaceName"]),
                contactName = Convert.ToString(dataReader["contactName"]),
            };
        }

        private static string CreateQuery(string id)
        {
            return $@"  SELECT distinct p.FirstName + ' ' + p.LastName AS studentName, p.PersonID AS personID, cs.StatusIdGovIl AS[status], w.Name AS educationPlaceName,
                        (SELECT TOP 1 jp.FirstName + ' ' + jp.LastName + ', ' + roles.RoleName +  COALESCE(', ' + ph.AreaCode + '-' + ph.PhoneNumber, '')
						 FROM dbo.UserRoleAndWorkPlaceAssociations urawpa
                        LEFT JOIN persons jp ON urawpa.RoleAndWorkPlaceUserID = jp.ID
						left join Roles on urawpa.RoleID = Roles.ID								
						left join Phones ph on jp.CommunicationID = ph.CommunicationID and (ph.PhoneTypeID = 3 or ph.PhoneTypeID = 4) and ph.IsActive = 1 --יביא נתוני איש קשר גם אם אין טלפון בעבודה
                        WHERE urawpa.roleid = 12 AND urawpa.WorkplaceID = w.id AND sfs.PrimaryStatusID = 3
						and jp.IsActive = 1 and roles.IsActive = 1 and urawpa.FromDate <= GETDATE() and urawpa.ToDate >= GETDATE() and urawpa.IsActive = 1
                        ) AS contactName 
                         FROM Students s

                        LEFT JOIN LegalResponsibilityOnStudent ON s.ID = LegalResponsibilityOnStudent.StudentID
                        left JOIN Persons lrp on lrp.ID = LegalResponsibilityOnStudent.LegalResponsibilityID
                        LEFT JOIN dbo.Persons p ON p.ID = s.ID
                        LEFT JOIN dbo.StudentEducations se ON s.ID = se.ID and se.isActive = 1 --כדי שיביא את התלמיד למרות שאין לו מקום חינוך. שם מקום החינוך יהיה ריק
                        LEFT JOIN dbo.Workplaces w ON se.EducationPlaceID = w.ID and w.isActive = 1 --כדי שיביא את התלמיד למרות שאין לו מקום חינוך. שם מקום החינוך יהיה ריק
                        LEFT JOIN dbo.StatusForStudents sfs ON s.ID = sfs.ID
                        LEFT JOIN dbo.ConversionDRToGovIlStatus cs ON sfs.PrimaryStatusID = cs.StatusIdDR
                        
                        WHERE LegalResponsibilityOnStudent.LegalResponsibilityTypeID IN (3, 5, 6)
                        AND LegalResponsibilityOnStudent.IsUpdatedFromMoe = 1
                        AND LegalResponsibilityOnStudent.isActive = 1
                        AND lrp.PersonID = '{id}'
                        And sfs.PrimaryStatusID not in (4, 5)
						and s.IsActive = 1 and lrp.IsActive = 1 and p.IsActive = 1 and sfs.IsActive = 1";
        }

        private string CreateQueryLog(Exception ex)
        {
            var query = $@"insert into Logs values
                           ('Exception has occurred in GovILApi',
                            'Error',
                            getdate(),
                            '{ex}',
                            'GovILApi',
                            '',
                            '111',
                            '',
                            '',
                            null)";

            return query;
        }
    }
}
