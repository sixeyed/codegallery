using System.Configuration;
using Amazon;
using Amazon.SimpleDB;
using Amazon.SQS;

namespace CloudServiceBus.Core.Aws
{
    public static class AwsFacade
    {
        private static string AccessKey
        {
            get { return ConfigurationManager.AppSettings["AWSAccessKey"]; }
        }

        private static string SecretKey
        {
            get { return ConfigurationManager.AppSettings["AWSSecretKey"]; }
        }

        public static AmazonSQS GetSqsClient()
        {
            return AWSClientFactory.CreateAmazonSQSClient(AccessKey, SecretKey);
        }        

        public static AmazonSimpleDB GetSimpleDBClient()
        {
            return AWSClientFactory.CreateAmazonSimpleDBClient(AccessKey, SecretKey);
        }
    }
}
