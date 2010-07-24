using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.SQS.Model;
using CloudServiceBus.Core;
using CloudServiceBus.Core.DataStores;
using CloudServiceBus.Core.Extensions;
using CloudServiceBus.Core.Serialization;
using CloudServiceBus.Entities;
using CloudServiceBus.Entities.Enums;
using CloudServiceBus.Entities.ServiceRequests;
using CloudServiceBus.Entities.ServiceResponses;

namespace CloudServiceBus.ServiceProvider.ServiceHandlers
{
    /// <summary>
    /// Sample query service handler - returns a random list of
    /// employee entities changed since the given date
    /// </summary>
    public class GetChangedEmployees : ServiceHandlerBase<GetChangedEmployeesRequest, QueryServiceResponse>
    {
        public const string StoreIdentifier = "Sixeyed-CloudServiceBus-ResponseData";

        public override void HandleRequest(Message request)
        {
            var response = new QueryServiceResponse();

            if (ServiceRequest == null)
            {
                response.IsRequestValid = false;
            }
            else
            {
                Console.WriteLine("GetChangedEmployees - Request received, LastChangeDate: {0}", ServiceRequest.LastChangeDate);

                var requestId = ServiceRequest.ToString().ToDeterministicGuid();

                //check if request already stored:
                if (!DataStore.Current.Exists(StoreIdentifier, requestId))
                {
                    Console.WriteLine("Loading Employees to Data Store");
                    //load up a list of employees::
                    var employees = GetEmployees(ServiceRequest.LastChangeDate);
                    Console.WriteLine("Built Employee list");
                    //place in storage:
                    var serializedEmployees = new List<string>(employees.Count());
                    foreach (var employee in employees)
                    {
                        var serializedEmployee = Serializer.Current.Serialize(typeof(Employee), employee).ToString();
                        serializedEmployees.Add(serializedEmployee);
                    }
                    Console.WriteLine("Serialized Employee list");
                    DataStore.Current.Add(StoreIdentifier, requestId, serializedEmployees.ToArray());
                    Console.WriteLine("Stored Employee list");
                }
                else
                {
                    Console.WriteLine("Employees already in Data Store");
                }

                response.IsRequestValid = true;
                response.StoreIdentifier = StoreIdentifier;
                response.State = ServiceResponseState.Completed;
                response.ItemKey = requestId;
            }

            SendResponse(response);
        }

        private IEnumerable<Employee> GetEmployees(DateTime dateTime)
        {
            //stub implementation only:
            var resultCount = RandomValueGenerator.GetRandomInt(1, 100);
            var employees = new List<Employee>(resultCount);
            for (int index = 0; index < resultCount; index++)
            {
                var employee = new Employee();
                employee.DepartmentName = RandomValueGenerator.GetRandomString();
                employee.DirectDialNumber = RandomValueGenerator.GetRandomInt(1000000000).ToString();
                employee.EmailAddress = string.Format("{0}@sixeyed.com", RandomValueGenerator.GetRandomString(10, " "));
                employee.EmployeeId = RandomValueGenerator.GetRandomInt().ToString();
                employee.Status = RandomValueGenerator.GetRandomEnumValue<EmployeeStatus>();
                employee.ExtensionNumber = RandomValueGenerator.GetRandomInt(1000).ToString();
                employee.FaxNumber = RandomValueGenerator.GetRandomInt(1000000000).ToString();
                employee.FullName = RandomValueGenerator.GetRandomString(20);
                employee.LatestHireDate = RandomValueGenerator.GetRandomDateTime(2000);
                employee.LastChangeDate = RandomValueGenerator.GetRandomDateTime(dateTime.Year-1);
                employee.MobileNumber = RandomValueGenerator.GetRandomInt(1000000000).ToString();
                employee.OfficeCountry = RandomValueGenerator.GetRandomString(15);
                employee.OfficeLocation = RandomValueGenerator.GetRandomString(30);
                employee.RoleDescription = RandomValueGenerator.GetRandomString(30);
                employee.RoleTitle = RandomValueGenerator.GetRandomString(20);
                employees.Add(employee);
            }
            return employees;
        }
    }
}
