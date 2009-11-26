using System;
using System.ServiceModel;
using System.Runtime.Serialization;
using ESBGuidanceErrorHandler.ErrorHandlers;
using ESBGuidanceErrorHandler.Enums;


/// <summary>
/// 
/// </summary>
public class ErroringService : IErroringService
{
    public void ThrowUnhandledException()
    {
        DivideByZero();
    }

    public void LogHandledException()
    {
        try
        {
            DivideByZero();
        }
        catch (Exception ex)
        {
            ServiceProviderErrorHandler.SubmitFault("ErroringService", "LogHandledException", "Caught exception", FaultSeverity.Error, ex);
        }
    }

    private static void DivideByZero()
    {
        int zero = 0;
        int dbz = 10 / zero;
    }
}
