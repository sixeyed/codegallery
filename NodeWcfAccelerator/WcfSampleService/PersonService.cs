using System;
using System.Linq;
using System.ServiceModel.Web;
using System.Threading;

namespace WcfSampleService
{
    public class PersonService : IPersonService
    {
        public void Update(string personId, string lastName, string firstName)
        {
            Guid oldETag;
            var newETag = Guid.NewGuid();

            using (var context = new SampleEntities())
            {
                var iPersonId = int.Parse(personId);
                var guest = context.People.Single(x => x.PersonId == iPersonId);
                oldETag = Guid.Parse(guest.ETag);
                guest.LastName = lastName;
                guest.FirstName = firstName;
                guest.ETag = newETag.ToString();
                context.SaveChanges();
            }

            ETagCache.Replace(oldETag, newETag);
        }

        public Person Fetch(string personId)
        {
            if (ETagCache.ExistsForCurrent())
            {
                var request = WebOperationContext.Current.IncomingRequest;
                //make sure you check this, otherwise standard WCF POST connections will fail:
                if (request.Method == "GET" || request.Method == "HEAD")
                {
                    request.CheckConditionalRetrieve(ETagCache.GetForCurrent());
                }
            }

            //fake wait - e.g. going off to a cloud service or moderate processing load:
            Thread.Sleep(250);

            Person person = null;
            var iPersonId = int.Parse(personId);
            using (var context = new SampleEntities())
            {
                person = context.People.Single(x => x.PersonId == iPersonId);
            }
            var eTag = Guid.Parse(person.ETag);
            ETagCache.SetForCurrent(eTag);

            var response = WebOperationContext.Current.OutgoingResponse;
            if (response != null)
            {
                response.SetETag(eTag);
            }
            return person;
        }
    }
}