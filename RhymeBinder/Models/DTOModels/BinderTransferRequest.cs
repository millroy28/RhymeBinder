using System.Collections.Generic;

namespace RhymeBinder.Models.DTOModels
{
    public class BinderTransferRequest
    {
        public List<int> TextHeaderIds { get; set; }
        public int DestinationBinderId { get; set; }
    }
}
