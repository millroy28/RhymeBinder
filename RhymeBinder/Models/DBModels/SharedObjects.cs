
using System;

namespace RhymeBinder.Models.DBModels
{
    public class SharedObjects
    {
        public int SharedObjectId { get; set; }
        public int ObjectId { get; set; }
        public int SharedObjectTypeId { get; set; }
        public int SharedObjectActionId { get; set; }
        public int Grantor { get; set; }
        public int Grantee { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual SharedObjectTypes SharedObjectType { get; set; }
        public virtual SharedObjectActions SharedObjectAction { get; set; }
        public virtual SimpleUser GrantorUser { get; set; }
        public virtual SimpleUser GranteeUser { get; set; }

    }
}
