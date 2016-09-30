using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.TestBase.SampleApplication
{
    [Table("Messages")]
    public class Message : FullAuditedEntity, IMayHaveTenant
    {
        public Message()
        {
        }

        public Message(int? tenantId, string text)
        {
            TenantId = tenantId;
            Text = text;
        }

        public string Text { get; set; }
        public int? TenantId { get; set; }
    }
}