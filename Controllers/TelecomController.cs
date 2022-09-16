using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VerficationApi.Data;
using VerficationApi.Model;

namespace VerficationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelecomController : ControllerBase
    {
        private readonly ApiContext db;
        private readonly UpdateDatabase _UpdateDatabase;
        public TelecomController(ApiContext _db, UpdateDatabase updateDatabase)
        {
            this.db = _db;
            _UpdateDatabase = updateDatabase;
            _UpdateDatabase.SaveEntries();
        }

        [HttpPost("VerifyPhoneNumber")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel))]
        public IActionResult VerifyPhoneNumber(RequestModel Model)
        {
            DateTime processedAt = DateTime.Now;
            ResponseData responseData = new ResponseData();
            RequestData requestData = new RequestData()
            {
                referenceId = Model.referenceId,
                nin = Model.nin,
                phoneNumber = Model.phoneNumber,
                processedAt = processedAt,
            };
            int mid = 1;
            var mlast = db.RequestData?.OrderBy(o => o.Id).ToList().LastOrDefault();
            if (mlast != null)
            {
                mid = (mlast.Id + 1);
            }
            requestData.Id = mid;
            db.RequestData?.Add(requestData);
            db.SaveChanges();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage);
                responseData = new ResponseData()
                {
                    phoneNumber = Model.phoneNumber,
                    errorCode = 400,
                    errorMessage = string.Join(',', errors.ToArray()),
                    PhoneNumberValidity = "Invalid",
                    nin = Model.nin,
                    referenceId = Model.referenceId,
                    processedAt = processedAt
                };
                int id = 1;
                var last = db.ResponseData?.OrderBy(o => o.ResponseId).ToList().LastOrDefault();
                if (last != null)
                {
                    id = (last.ResponseId + 1);
                }
                responseData.ResponseId = id;
                db.ResponseData?.Add(responseData);
                db.SaveChanges();
                //return BadRequest(ModelState);
                return BadRequest(new ResponseModel()
                {
                    phoneNumber = Model.phoneNumber,
                    errorCode = 400,
                    errorMessage = string.Join(',', errors.ToArray()),
                    PhoneNumberValidity = "Invalid",
                    nin = Model.nin,
                    referenceId = Model.referenceId,
                    processedAt = processedAt
                });
            }

            var phoneNumberExists = db.TelecomData?.FirstOrDefault(o => o.phoneNumber == Model.phoneNumber);
            if (phoneNumberExists == null)
            {
                responseData = new ResponseData()
                {
                    phoneNumber = Model.phoneNumber,
                    errorCode = 404,
                    errorMessage = "Phone number was not found",
                    PhoneNumberValidity = "Invalid",
                    nin = Model.nin,
                    referenceId = Model.referenceId,
                    processedAt = processedAt
                };
                int id = 1;
                var last = db.ResponseData?.OrderBy(o => o.ResponseId).ToList().LastOrDefault();
                if (last != null)
                {
                    id = (last.ResponseId + 1);
                }
                responseData.ResponseId = id;
                db.ResponseData?.Add(responseData);
                db.SaveChanges();

                return BadRequest(new ResponseModel()
                {
                    phoneNumber = Model.phoneNumber,
                    errorCode = 404,
                    errorMessage = "Phone number was not found",
                    PhoneNumberValidity = "Invalid",
                    nin = Model.nin,
                    referenceId = Model.referenceId,
                    processedAt = processedAt
                });
            }
            if (Model.nin != phoneNumberExists.nin)
            {
                responseData = new ResponseData()
                {
                    phoneNumber = Model.phoneNumber,
                    errorCode = 400,
                    errorMessage = "Invalid Phone Number, The nin number provided with the phone number doesnot match with the one provided by the telecom company.",
                    PhoneNumberValidity = "Invalid",
                    nin = Model.nin,
                    referenceId = Model.referenceId,
                    processedAt = processedAt
                };
                int id = 1;
                var last = db.ResponseData?.OrderBy(o => o.ResponseId).ToList().LastOrDefault();
                if (last != null)
                {
                    id = (last.ResponseId + 1);
                }
                responseData.ResponseId = id;
                db.ResponseData?.Add(responseData);
                db.SaveChanges();

                return BadRequest(new ResponseModel()
                {
                    phoneNumber = Model.phoneNumber,
                    errorCode = 400,
                    errorMessage = "Invalid Phone Number, The nin number provided with the phone number doesnot match with the one provided by the telecom company.",
                    PhoneNumberValidity = "Invalid",
                    nin = Model.nin,
                    referenceId = Model.referenceId,
                    processedAt = processedAt
                });
            }

            responseData = new ResponseData()
            {
                phoneNumber = Model.phoneNumber,
                firstName = phoneNumberExists.firstName,
                middleName = phoneNumberExists.middleName,
                surname = phoneNumberExists.surname,
                gender = phoneNumberExists.gender,
                status = phoneNumberExists.status,
                PhoneNumberValidity = "Valid",
                nin = Model.nin,
                id = phoneNumberExists.id,
                createdAt = phoneNumberExists.createdAt,
                referenceId = Model.referenceId,
                processedAt = processedAt
            };
            int _id = 1;
            var _last = db.ResponseData?.OrderBy(o => o.ResponseId).ToList().LastOrDefault();
            if (_last != null)
            {
                _id = (_last.ResponseId + 1);
            }
            responseData.ResponseId = _id;
            db.ResponseData?.Add(responseData);
            db.SaveChanges();

            return Ok(new ResponseModel()
            {
                phoneNumber = Model.phoneNumber,
                firstName = phoneNumberExists.firstName,
                middleName = phoneNumberExists.middleName,
                surname = phoneNumberExists.surname,
                gender = phoneNumberExists.gender,
                status = phoneNumberExists.status,
                PhoneNumberValidity = "Valid",
                nin = Model.nin,
                id = phoneNumberExists.id,
                createdAt = phoneNumberExists.createdAt,
                referenceId = Model.referenceId,
                processedAt = processedAt
            });
        }

        [HttpPost("GetVerificationList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetVerificationList(ListRequestModel Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = db.ResponseData?.OrderBy(o => o.ResponseId).ToList().Where(o => (Model.Statuses == null || Model.Statuses.Split(",").Any(x => x == o.status)) && (Model.Genders == null || Model.Genders.Split(",").Any(x => x == o.gender)) && (o.processedAt >= Model.FromDate && o.processedAt <= Model.ToDate)).ToList();
            ListResponseModel listResponseModel = new ListResponseModel();
            if (data != null)
            {
                listResponseModel.X_totalcount = data.Count;
                int pages = (data.Count / Model.PageSize.Value);
                listResponseModel.x_totalpages = (pages + 1);
                listResponseModel.x_pagesize = Model.PageSize.Value;
                int previouspageNo = Model.PageNumber.Value - 1;
                int startRecord = (Model.PageSize.Value * previouspageNo);
                var _data = data.Skip(startRecord)
                        .Take(Model.PageSize.Value)
                        .ToList();
                listResponseModel.ResponseModel = new List<ResponseModel>();
                if (_data.Count > 0)
                {
                    foreach (var n in _data)
                    {
                        listResponseModel.ResponseModel.Add(new ResponseModel()
                        {
                            phoneNumber = n.phoneNumber,
                            firstName = n.firstName,
                            middleName = n.middleName,
                            surname = n.surname,
                            gender = n.gender,
                            status = n.status,
                            PhoneNumberValidity = n.PhoneNumberValidity,
                            nin = n.nin,
                            id = n.id,
                            createdAt = n.createdAt,
                            referenceId = n.referenceId,
                            processedAt = n.processedAt
                        });
                    }
                }
            }
            else
            {
                listResponseModel.X_totalcount = 0;
                listResponseModel.x_totalpages = 0;
                listResponseModel.x_pagesize = Model.PageSize.Value;
            }

            return Ok(listResponseModel);
        }
        [HttpGet("GetRequestByReferenceId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestData))]
        public IActionResult GetRequestByReferenceId(string ReferenceId)
        {
            RequestData requestData = new RequestData();
            var Data = db.RequestData?.FirstOrDefault(o => o.referenceId == ReferenceId);
            if (Data != null)
            {
                requestData = Data;
            }
            return Ok(requestData);
        }
    }
}

public class Rootobject
{
    public string type { get; set; }
    public string title { get; set; }
    public int status { get; set; }
    public string traceId { get; set; }
    public Errors errors { get; set; }
}

public class Errors
{
    public string[] nin { get; set; }
    public string[] phoneNumber { get; set; }
    public string[] referenceId { get; set; }
}
