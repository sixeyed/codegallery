using System;
using System.ServiceModel;
using System.Runtime.Serialization;


/// <summary>
/// 
/// </summary>
[ServiceContract()]
public interface IErroringService
{
    [OperationContract]
    void ThrowUnhandledException();

    [OperationContract]
    void LogHandledException();
}
