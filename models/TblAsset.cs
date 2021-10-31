using System;
using System.Collections.Generic;

#nullable disable

namespace Asset.models
{
    public partial class TblAsset
    {
        public Guid AssetId { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string CreatedBy { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
    }
}
