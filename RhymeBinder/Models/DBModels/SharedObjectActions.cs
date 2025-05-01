using System.Collections.Generic;

namespace RhymeBinder.Models.DBModels
{
    public class SharedObjectActions
    {
        public SharedObjectActions()
        {
            SharedObjects = new HashSet<SharedObjects>();
        }
        public int SharedObjectActionId { get; set; }
        public string Action {  get; set; }

        public virtual ICollection<SharedObjects> SharedObjects { get; set; }
    }
}
