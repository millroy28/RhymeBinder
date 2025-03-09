using System.Collections;
using System.Collections.Generic;

namespace RhymeBinder.Models.DBModels
{
    public class SharedObjectTypes
    {
        public SharedObjectTypes() 
        {
            SharedObjects = new HashSet<SharedObjects>();
        }
        public int SharedObjectTypeId { get; set; }
        public string ObjectType { get; set; }
        public virtual ICollection<SharedObjects> SharedObjects { get; set; }
    }
}
