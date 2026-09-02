using System.Collections.Generic;

namespace RhymeBinder.Models.DTOModels
{
    public class GroupMembershipUpdateRequest
    {
        public List<int> TextHeaderIds { get; set; }
        public Dictionary<int, bool> GroupChanges { get; set; }
    }
}
